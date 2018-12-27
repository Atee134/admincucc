import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { WorkdayComponent } from './workday/workday.component';
import { WorkdayResolver } from './_resolvers/workday.resolver';
import { AvailableDatesResolver } from './_resolvers/availabledates.resolver';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { ButtonsModule } from 'ngx-bootstrap';
import { LoginComponent } from './login/login.component';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule } from '@angular/forms';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      WorkdayComponent,
      LoginComponent
   ],
   imports: [
      BrowserModule,
      FormsModule,
      HttpClientModule,
      ButtonsModule.forRoot(),
      AppRoutingModule
   ],
   providers: [WorkdayResolver, AvailableDatesResolver],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
