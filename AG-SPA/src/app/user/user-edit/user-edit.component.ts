import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { UserDetailDto, Role, UserForListDto, Shift, UserForEditDto, Site } from 'src/app/_models/generatedDtos';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { StaticdataService } from 'src/app/_services/staticdata.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  public user: UserDetailDto;
  public userForEdit: UserForEditDto;
  public rePassword: string;
  public users: UserForListDto[];
  public colors: string[];
  public userColors: string[];
  public sites: Site[];

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private alertify: AlertifyService,
    private staticDataService: StaticdataService
    ) { }

  ngOnInit() {
    this.sites = Object.keys(Site).map(k => Site[k]).sort();
    this.initUser();
    this.getColors();
  }

  private initUser(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.userService.getUser(id)
      .subscribe(user => {
        this.user = user;
        this.initUserForEditDto(user);
        this.initAssigneableUsers();
      }, error => {
        this.alertify.error(error);
      });
  }

  private getColors(): void {
    this.staticDataService.getColors().subscribe(colors => {
      this.colors = colors;
    });
  }

  private initUserForEditDto(userDetail: UserDetailDto): void {
    this.userForEdit = new UserForEditDto({
      id: userDetail.id,
      password: undefined,
      sites: userDetail.sites,
      minPercent: userDetail.minPercent,
      maxPercent: userDetail.maxPercent
    });
  }

  public onCancel() {
    this.initUser();
  }

  public onSiteListChanged(event: any) {
    this.userForEdit.sites = this.userForEdit.sites.filter(s => s !== event.currentTarget.value);

    if (event.currentTarget.checked) {
      this.userForEdit.sites.push(event.currentTarget.value);
    }
  }

  public isSiteChecked(site: Site): boolean {
    if (this.userForEdit.sites.includes(site)) {
      return true;
    }
    return false;
  }

  public onSubmit() {
    if (this.userForEdit.password === '') {
      this.userForEdit.password = undefined;
    }
    if (this.rePassword === '') {
      this.rePassword = undefined;
    }

    if (this.userForEdit.password !== this.rePassword) {
      this.alertify.error('A két jelszó nem egyezik');
    } else {
      let changedProperties = '';

      if (this.userForEdit.password) {
        changedProperties += ('</br>- Jelszó');
      }
      if (this.userForEdit.sites !== this.user.sites) {
        changedProperties += ('</br>- Honlapok');
      }
      if (this.userForEdit.minPercent !== this.user.minPercent) {
        changedProperties += ('</br>- Minimum százalék');
      }
      if (this.userForEdit.maxPercent !== this.user.maxPercent) {
        changedProperties += ('</br>- Maximum százalék');
      }

      // if some change occured which the code knows nothing about, we save anyway.
      if (changedProperties === '') {
        this.saveChanges();
      } else {
        const self = this;
        this.alertify.confirm('Mented a változtatásokat? Az alábbi mezők módosultak: ' + changedProperties, function() {
          self.saveChanges();
        });
      }
    }
  }

  private saveChanges(): void {
    this.userService.updateUser(this.userForEdit).subscribe(resp => {
      this.alertify.success('Változtatások mentve.');
    }, error => {
      this.alertify.error(error);
    });
  }

  // TODO refactor methods below this, probably into a seperate component (add\remove performer component)
  private initAssigneableUsers(): void {
    const role = this.user.role === Role.Operator ? Role.Performer : Role.Operator;

    this.userService.getUsers(role)
    .subscribe(users => {
      this.users = users;
      this.initUserColors(users);
    }, error => {
      this.alertify.error(error);
    });
  }

  public isUserColleague(user: UserForListDto): boolean {
    if (this.user.colleagues) {
      return !!this.user.colleagues.find(u => u.id === user.id);
    }

    return false;
  }

  public initUserColors(users: UserForListDto[]) {
    this.userColors = [];

    for (const user of users) {
      if (this.isUserColleague(user)) {
        this.userService.getColor(this.user.id, user.id).subscribe(color => {
          this.userColors[user.id] = color.color;
        });
      }
    }
  }

  public isCurrentColor(user: UserForListDto, color: string) {
    return this.userColors[user.id] === color;
  }

  public onUserColorChanged(event: any, user: UserForListDto) {
    const newColor = event.currentTarget.value;

    this.userColors[user.id] = newColor;

    this.userService.changeColor(this.user.id, user.id, newColor).subscribe(resp => {
    }, error => {
      this.alertify.error(error);
    });
  }

  // TODO after refactor this shit can be cleaned, as IDs will be interchangeable
  private getIds(linkedUserId: number): [number, number] {
    let operatorId;
    let performerId;
    if (this.user.role === Role.Operator) {
      operatorId = this.user.id;
      performerId = linkedUserId;
    } else {
      operatorId = linkedUserId;
      performerId = this.user.id;
    }

    return [operatorId, performerId];
  }

  public addLink(linkedUserId: number): void {
    const self = this;
    this.alertify.confirm('Biztos, hogy egymáshoz akarod rendelni a felhasználókat?', function() {
      const ids = self.getIds(linkedUserId);
      self.userService.addPerformer(ids[0], ids[1]).subscribe(resp => {
        self.user.colleagues.push(new UserForListDto({
          id: linkedUserId,
          userName: 'dummy',
          shift: Shift.Afternoon,
          role: Role.Operator,
          lastPercent: 0
        }));
        self.alertify.success('Felhasználók sikeresen összerendelve.');
      }, error => {
        self.alertify.error(error);
      });
    });
  }

  public removeLink(linkedUserId: number): void {
    const self = this;
    this.alertify.confirm('Biztos, hogy törölni akarod a felhasználók közti összerendelést?', function() {
      const ids = self.getIds(linkedUserId);
      self.userService.removePerformer(ids[0], ids[1]).subscribe(resp => {
        self.user.colleagues = self.user.colleagues.filter(c => c.id !== linkedUserId);
        self.alertify.success('Összerendelés sikeresen törölve.');
      }, error => {
        self.alertify.error(error);
      });
    });
  }

  public hasRole(user: UserForListDto, role: string): boolean {
    return user.role.toLowerCase() === role.toLowerCase();
  }
}
