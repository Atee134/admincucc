import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { Role, UserForListDto } from 'src/app/_models/generatedDtos';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-income-add-userselector',
  templateUrl: './income-add-userselector.component.html',
  styleUrls: ['./income-add-userselector.component.css']
})
export class IncomeAddUserselectorComponent implements OnInit {

  public operators: UserForListDto[];
  public selectedUser = new UserForListDto();

  constructor(private userService: UserService, private alertify: AlertifyService, private authService: AuthService) { }

  ngOnInit() {
    if (this.authService.currentUser.role === Role.Admin) {
      this.getOperators();
    } else {
      this.selectedUser = new UserForListDto();
      this.selectedUser.id = this.authService.currentUser.id;
    }
  }

  private getOperators() {
    this.userService.getUsers(Role.Operator).subscribe(resp => {
      this.operators = resp;
    }, error => {
      this.alertify.error(error);
    });
  }

  public isUserSelectable() {
    if (this.authService.currentUser.role === Role.Admin) {
      return true;
    }

    return false;
  }
}
