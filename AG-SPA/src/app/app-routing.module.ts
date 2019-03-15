import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { IncomeAddComponent } from './income/income-add/income-add.component';
import { UserAddComponent } from './user/user-add/user-add.component';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { IncomeEditComponent } from './income/income-edit/income-edit.component';
import { IncomeFilterComponent } from './income/income-filter/income-filter.component';
import { AuthGuardService } from './_guards/auth-guard.service';
import { Role } from './_models/generatedDtos';
import { RoleGuardService } from './_guards/role-guard.service';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'incomes', canActivate: [AuthGuardService], component: IncomeFilterComponent },
  {
    path: 'incomes/add',
    canActivate: [AuthGuardService, RoleGuardService],
    data: { allowedRoles: [Role.Admin, Role.Operator]},
    component: IncomeAddComponent
  },
  {
    path: 'incomes/:id',
    canActivate: [AuthGuardService, RoleGuardService],
    data: { allowedRoles: [Role.Admin, Role.Operator]},
    component: IncomeEditComponent
  },
  {
    path: 'users',
    canActivate: [AuthGuardService, RoleGuardService],
    data: { allowedRoles: [Role.Admin]},
    component: UserListComponent
  },
  {
    path: 'users/add',
    canActivate: [AuthGuardService, RoleGuardService],
    data: { allowedRoles: [Role.Admin]},
    component: UserAddComponent
 },
  {
    path: 'users/:id',
    canActivate: [AuthGuardService, RoleGuardService],
    data: { allowedRoles: [Role.Admin]},
    component: UserEditComponent
 },
];

@NgModule({
  imports: [ RouterModule.forRoot(routes, { enableTracing: false, useHash: true}) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
