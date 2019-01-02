import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { UserForLoginDto, UserAuthResponseDto, UserDetailDto, UserForRegisterDto, UserForListDto } from '../_models/generatedDtos';
import { tap, catchError, map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public loggedIn$: BehaviorSubject<boolean>;
  public decodedToken;
  public currentUser: UserDetailDto;
  private baseUrl = environment.apiUrl;
  private jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient, private router: Router) {
    this.loggedIn$ = new BehaviorSubject<boolean>(false);

    const token = localStorage.getItem('token');
    if (token) {
      if (!this.jwtHelper.isTokenExpired(token)) {
        this.decodedToken = this.jwtHelper.decodeToken(token);
        this.currentUser = JSON.parse(localStorage.getItem('user'));
        this.loggedIn$.next(true);
      }
    }
  }

  login(user: UserForLoginDto): Observable<string> {
    return this.http.post<UserAuthResponseDto>(this.baseUrl + 'auth/login', user, { observe: 'response'}).pipe(
      map(res => {
        const token = res.body.token;
        localStorage.setItem('token', token);
        this.decodedToken = this.jwtHelper.decodeToken(token);
        this.currentUser = res.body.user;
        localStorage.setItem('user', JSON.stringify(res.body.user));
        this.loggedIn$.next(true);
        return res.body.user.userName;
      })
    );
  }

  register(user: UserForRegisterDto): Observable<UserForListDto> {
    return this.http.post<UserForListDto>(this.baseUrl + 'auth/register', user);
  }

  logout(): void {
    localStorage.removeItem('token');
    this.decodedToken = null;
    localStorage.removeItem('user');
    this.currentUser = null;
    this.loggedIn$.next(false);
  }
}
