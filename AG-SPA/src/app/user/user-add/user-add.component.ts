import { Component, OnInit } from '@angular/core';
import { UserForRegisterDto, Role } from 'src/app/_models/generatedDtos';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-user-add',
  templateUrl: './user-add.component.html',
  styleUrls: ['./user-add.component.css']
})
export class UserAddComponent implements OnInit {
  public user: UserForRegisterDto;
  public rePassword: string;

  constructor(private alertify: AlertifyService, private authService: AuthService) {
    this.user = new UserForRegisterDto();
    this.rePassword = '';
  }

  ngOnInit() {
  }

  get roles(): Role[] {
    return Object.keys(Role).map(k => Role[k]);
  }

  onSubmit() {
    if (this.user.password !== this.rePassword) {
      this.alertify.error('The passwords don\'t match.');
    } else {
      this.authService.register(this.user).subscribe(resp => {
        this.alertify.success('User successfully added.');
      }, error => {
        this.alertify.error(error);
      });
    }
  }

}
