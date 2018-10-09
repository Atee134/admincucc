import { Routes } from '@angular/router';
import { WorkdayComponent } from './workday/workday.component';
import { WorkdayResolver } from './_resolvers/workday.resolver';
import { AvailableDatesResolver } from './_resolvers/availabledates.resolver';

export const appRoutes: Routes = [
   { path: 'workdays', component: WorkdayComponent, resolve: {workdays: WorkdayResolver, availableDates: AvailableDatesResolver} }, 
];