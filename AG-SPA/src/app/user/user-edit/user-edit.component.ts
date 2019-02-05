import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { UserDetailDto, Role, UserForListDto, Shift, UserForEditDto, Site } from 'src/app/_models/generatedDtos';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
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

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private alertify: AlertifyService,
    private authService: AuthService,
    private staticDataService: StaticdataService
    ) { }

  ngOnInit() {
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
      userName: userDetail.userName,
      password: undefined,
      color: userDetail.color,
      sites: userDetail.sites,
      minPercent: userDetail.minPercent,
      maxPercent: userDetail.maxPercent
    });
  }

  get sites(): Site[] {
    return Object.keys(Site).map(k => Site[k]);
  }

  public onCancel() {
    this.initUser();
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

      if (this.userForEdit.userName !== this.user.userName) {
        changedProperties += ('</br>- Felhasználónév');
      }
      if (this.userForEdit.color !== this.user.color) {
        changedProperties += ('</br>- Szín');
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
        self.user.colleagues.push(new UserForListDto({id: linkedUserId, userName: 'dummy', shift: Shift.Afternoon, role: Role.Operator}));
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
    return user.role === role;
  }
}
