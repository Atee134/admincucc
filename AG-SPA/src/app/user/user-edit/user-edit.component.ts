import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { UserDetailDto, Role, UserForListDto, Shift } from 'src/app/_models/generatedDtos';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  public user: UserDetailDto;
  public users: UserForListDto[];

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private alertify: AlertifyService,
    private authService: AuthService
    ) { }

  ngOnInit() {
    this.initUser();
  }

  private initUser(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.userService.getUser(id)
      .subscribe(user => {
        this.user = user;
        this.initAssigneableUsers();
      }, error => {
        this.alertify.error(error);
      });
  }

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
