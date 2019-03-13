import { Component, OnInit } from '@angular/core';
import { UserForListDto, Role } from 'src/app/_models/generatedDtos';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  public users: UserForListDto[];

  constructor(private userService: UserService, private alertify: AlertifyService, private authService: AuthService) {
  }

  ngOnInit() {
    this.userService.getUsers().subscribe(resp => {
      this.users = resp;
    }, error => {
      this.alertify.error(error);
    });
  }

  public isUserEditable(user: UserForListDto) {
    if (user.role === Role.Admin && user.id !== this.authService.currentUser.id) {
      return false;
    }

    return true;
  }

  public isCurrentUser(user: UserForListDto) {
    return user.id === this.authService.currentUser.id;
  }
}
