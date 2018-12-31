import { Injectable } from '@angular/core';
import { Site, IncomeEntryForReturnDto, IncomeEntryAddDto } from '../_models/generatedDtos';
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

  getIncomeEntries(userId: number): Observable<IncomeEntryForReturnDto[]> {
    return this.http.get<IncomeEntryForReturnDto[]>(this.baseUrl + 'users/' + userId + '/incomes');
  }

  addIncomeEntry(userId: number, incomeEntry: IncomeEntryAddDto): Observable<IncomeEntryForReturnDto> {
    return this.http.post<IncomeEntryForReturnDto>(this.baseUrl + 'users/' + userId + '/incomes', incomeEntry);
  }
}
