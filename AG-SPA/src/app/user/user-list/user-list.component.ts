import { Component, OnInit } from '@angular/core';
import { UserForListDto } from 'src/app/_models/generatedDtos';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  public users: UserForListDto[];

  constructor(private userService: UserService, private alertify: AlertifyService) {
  }

  ngOnInit() {
    this.userService.getUsers().subscribe(resp => {
      this.users = resp;
    }, error => {
      this.alertify.error(error);
    });
  }
}
