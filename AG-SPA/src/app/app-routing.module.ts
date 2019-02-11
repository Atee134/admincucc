import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { WorkdayComponent } from './workday/workday.component';
import { LoginComponent } from './login/login.component';
import { IncomeAddComponent } from './income/income-add/income-add.component';
import { UserAddComponent } from './user/user-add/user-add.component';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { IncomeEditComponent } from './income/income-edit/income-edit.component';
import { IncomeFilterComponent } from './income/income-filter/income-filter.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'incomes', component: IncomeFilterComponent },
  { path: 'incomes/add', component: IncomeAddComponent },
  { path: 'incomes/:id', component: IncomeEditComponent },
  { path: 'workdays', component: WorkdayComponent },
  { path: 'users', component: UserListComponent },
  { path: 'users/add', component: UserAddComponent },
  { path: 'users/:id', component: UserEditComponent },
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
