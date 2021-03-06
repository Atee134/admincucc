import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Role } from '../_models/generatedDtos';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  onLogout() {
    this.alertify.message('Kijelentkezve');
    this.authService.logout();
  }

  isCurrentUser(role: string): boolean {
    return this.authService.currentUser.role.toLowerCase() === role.toLowerCase();
  }
}
