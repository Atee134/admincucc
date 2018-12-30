import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { WorkdayComponent } from './workday/workday.component';
import { LoginComponent } from './login/login.component';
import { IncomeListComponent } from './income/income-list/income-list.component';
import { IncomeEditComponent } from './income/income-edit/income-edit.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'incomes', component: IncomeListComponent },
  { path: 'incomes/add', component: IncomeEditComponent },
  { path: 'workdays', component: WorkdayComponent },
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
