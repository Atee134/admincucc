import { Routes } from '@angular/router';
import { WorkdayComponent } from './workday/workday.component';
import { WorkdayResolver } from './_resolvers/workday.resolver';

export const appRoutes: Routes = [
   { path: 'workdays', component: WorkdayComponent, resolve: {availableDates: WorkdayResolver} }, 
];