import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { WorkdayComponent } from './workday/workday.component';

const routes: Routes = [
  { path: '', redirectTo: '/workdays', pathMatch: 'full' },
  { path: 'workdays', component: WorkdayComponent },
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
