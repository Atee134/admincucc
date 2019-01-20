import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserForListDto, UserDetailDto } from '../_models/generatedDtos';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsers(): Observable<UserForListDto[]> {
    return this.http.get<UserForListDto[]>(this.baseUrl + 'users');
  }

  getUser(userId: number): Observable<UserDetailDto> {
    return this.http.get<UserDetailDto>(this.baseUrl + 'users/' + userId);
  }
}
