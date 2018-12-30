import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { UserForLoginDto } from '../_models/generatedDtos';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  user: UserForLoginDto = new UserForLoginDto();

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.authService.loggedIn$.subscribe(next => {
      if (next) {
        this.router.navigate(['/earnings']);
      }
    });
  }

  onSubmit(): void {
    console.log('submitted');

    this.authService.login(this.user).subscribe(next => {
        console.log('Logged in as: ' + next);
        this.router.navigate(['/earnings']);
    }, error => {
      console.log(error);
    });
  }
}
