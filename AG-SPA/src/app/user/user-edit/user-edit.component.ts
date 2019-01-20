import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { UserDetailDto } from 'src/app/_models/generatedDtos';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  public user: UserDetailDto;

  constructor(private route: ActivatedRoute, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.getUser();
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

}
