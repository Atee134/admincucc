import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { UserForLoginDto } from '../_models/generatedDtos';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @Output() loggedIn = new EventEmitter<boolean>();
  user: UserForLoginDto = new UserForLoginDto();

  constructor() { }

  ngOnInit() {
  }

  onSubmit(): void {
    console.log('submitted');
    const success = true; // add http login here

    if (success) {
      this.loggedIn.emit(true);
    }
  }
}
