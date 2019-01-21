import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { UserDetailDto, Role, UserForListDto } from 'src/app/_models/generatedDtos';
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
    this.getUser();
    this.getAssigneableUsers();
  }

  private getUser() {
    const id = +this.route.snapshot.paramMap.get('id');
    this.userService.getUser(id)
      .subscribe(user => {
        this.user = user;
      }, error => {
        this.alertify.error(error);
      });
  }

  private getAssigneableUsers() {
    const role = this.authService.currentUser.role === Role.Operator ? Role.Performer : Role.Operator;

    this.userService.getUsers(role)
    .subscribe(users => {
      this.users = users;
    }, error => {
      this.alertify.error(error);
    });
  }

  public isUserColleague(user: UserForListDto): boolean {
    return false;
  }
}
