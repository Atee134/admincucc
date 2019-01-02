import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { WorkdayComponent } from './workday/workday.component';
import { LoginComponent } from './login/login.component';
import { IncomeListComponent } from './income/income-list/income-list.component';
import { IncomeAddComponent } from './income/income-add/income-add.component';
import { UserAddComponent } from './user/user-add/user-add.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'incomes', component: IncomeListComponent },
  { path: 'incomes/add', component: IncomeAddComponent },
  { path: 'workdays', component: WorkdayComponent },
  { path: 'users/add', component: UserAddComponent },
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
