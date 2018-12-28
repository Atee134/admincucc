import { Component } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  get loggedIn$(): BehaviorSubject<boolean> {
    return this.authService.loggedIn$;
  }

  constructor(private authService: AuthService) {}
}
