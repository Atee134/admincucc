import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { UserForLoginDto } from '../_models/generatedDtos';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  user: UserForLoginDto = new UserForLoginDto();

  constructor(private authService: AuthService, private router: Router, private alertify: AlertifyService) { }

  ngOnInit() {
    this.authService.loggedIn$.subscribe(next => {
      if (next) {
        this.router.navigate(['/incomes']);
      }
    });
  }

  onSubmit(): void {
    this.authService.login(this.user).subscribe(resp => {
      this.alertify.success('Successfully logged in');
      this.router.navigate(['/incomes']);
    }, error => {
      this.alertify.error('Invalid username or password.');
    });
  }
}
