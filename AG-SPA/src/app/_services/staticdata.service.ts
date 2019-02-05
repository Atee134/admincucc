import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StaticdataService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getColors(): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + 'staticdata/colors');
  }
}
