import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WorkdayService {
  baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) { }

  addWorkDay(date: Date) {
    return this.http.post(this.baseUrl + 'users/1/workdays/' + date.toString(), {}); // TODO tokenből userid
  }

  removeWorkDay(date: Date) {
    return this.http.delete(this.baseUrl + 'users/1/workdays/' + date.toString()); // TODO tokenből userid
  }

  getAvailableDates(): Observable<Date[]> {
    return this.http.get<Date[]>(this.baseUrl + 'workdays/available');
  }

  getWorkdaysOfCurrentUser(): Observable<Date[]> {
    return this.http.get<Date[]>(this.baseUrl + 'workdays/1'); // TODO token-ből userId ide
  }
}


