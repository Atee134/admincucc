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

  getAvailableDates(): Observable<Date[]> {
    return this.http.get<Date[]>(this.baseUrl + 'workdays/available');
  }
}


