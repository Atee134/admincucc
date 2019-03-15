import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { Role } from '../_models/generatedDtos';

@Injectable({
  providedIn: 'root'
})
export class RoleGuardService implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router,
  ) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const roles: Role[] = route.data.allowedRoles;

    if (roles.includes(this.authService.currentUser.role)) {
      return true;
    }

    this.router.navigate(['/incomes']);
    return false;
  }
}
