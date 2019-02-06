import { Injectable } from '@angular/core';
import { Site, IncomeEntryForReturnDto, IncomeEntryAddDto, IncomeListDataReturnDto } from '../_models/generatedDtos';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class IncomeService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private authService: AuthService) { }

  getCurrentSites(): Site[] {
    return this.authService.currentUser.sites;
  }

  getIncomeEntry(userId: number, incomeId: number): Observable<IncomeEntryForReturnDto> {
    return this.http.get<IncomeEntryForReturnDto>(this.baseUrl + 'users/' + userId + '/incomes/' + incomeId);
  }

  getIncomeEntries(userId: number): Observable<IncomeListDataReturnDto> {
    return this.http.get<IncomeListDataReturnDto>(this.baseUrl + 'users/' + userId + '/incomes');
  }

  getAllIncomeEntries(): Observable<IncomeListDataReturnDto> {
    return this.http.get<IncomeListDataReturnDto>(this.baseUrl + 'incomes');
  }

  addIncomeEntry(userId: number, incomeEntry: IncomeEntryAddDto): Observable<IncomeEntryForReturnDto> {
    return this.http.post<IncomeEntryForReturnDto>(this.baseUrl + 'users/' + userId + '/incomes', incomeEntry);
  }
}
