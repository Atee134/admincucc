import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from "@angular/router";
import { HttpClientModule } from '@angular/common/http';
import { WorkdayComponent } from './workday/workday.component';
import { WorkdayResolver } from './_resolvers/workday.resolver';
import { AvailableDatesResolver } from './_resolvers/availabledates.resolver';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { appRoutes } from "./routes";
import { ButtonsModule } from 'ngx-bootstrap';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      WorkdayComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      RouterModule.forRoot(appRoutes),
      ButtonsModule.forRoot()
   ],
   providers: [WorkdayResolver, AvailableDatesResolver],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
