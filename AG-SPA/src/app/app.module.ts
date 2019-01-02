import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { JwtModule } from '@auth0/angular-jwt';
import { WorkdayComponent } from './workday/workday.component';
import { WorkdayResolver } from './_resolvers/workday.resolver';
import { AvailableDatesResolver } from './_resolvers/availabledates.resolver';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { ButtonsModule } from 'ngx-bootstrap';
import { LoginComponent } from './login/login.component';
import { AppRoutingModule } from './app-routing.module';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { IncomeModule } from './income/income.module';
import { environment } from 'src/environments/environment';
import { UserModule } from './user/user.module';

export function tokenGetter() {
   return localStorage.getItem('token');
 }

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
      IncomeModule,
      UserModule,
      ButtonsModule.forRoot(),
      AppRoutingModule,
      JwtModule.forRoot({
         config: {
            tokenGetter: tokenGetter,
            whitelistedDomains: [environment.domainUrl],
            blacklistedRoutes: [environment.domainUrl + 'api/auth/login']
         }
      })
   ],
   providers: [
      WorkdayResolver,
      AvailableDatesResolver,
      ErrorInterceptorProvider
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
