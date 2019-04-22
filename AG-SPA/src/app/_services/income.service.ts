import { Injectable } from '@angular/core';
import { Site,
  IncomeEntryForReturnDto,
  IncomeEntryAddDto,
  IncomeListDataReturnDto,
  IncomeEntryUpdateDto,
  IncomeListFilterParams,
  Role } from '../_models/generatedDtos';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class IncomeService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private authService: AuthService) { }

  private createHttpParamsFromFilterParams(incomeFilters: IncomeListFilterParams): HttpParams {
    let params = new HttpParams();

    if (incomeFilters != null) {
      if (incomeFilters.userId) {
        params = params.append('userId', incomeFilters.userId.toString());
      }
      if (incomeFilters.userName) {
        params = params.append('userName', incomeFilters.userName);
      }
      if (incomeFilters.fromToFilter) {
        params = params.append('fromToFilter', 'true');
      } else {
        params = params.append('fromToFilter', 'false');
      }
      if (incomeFilters.month) {
        params = params.append('month', incomeFilters.month.toISOString());
      }
      if (incomeFilters.period) {
        params = params.append('period', incomeFilters.period.toString());
      }
      if (incomeFilters.from) {
        params = params.append('from', incomeFilters.from.toISOString());
      }
      if (incomeFilters.to) {
        params = params.append('to', incomeFilters.to.toISOString());
      }
      if (incomeFilters.hideLocked) {
        params = params.append('hideLocked', 'true');
      } else {
        params = params.append('hideLocked', 'false');
      }
      if (incomeFilters.minTotal) {
        params = params.append('minTotal', incomeFilters.minTotal.toString());
      }
      if (incomeFilters.maxTotal) {
        params = params.append('maxTotal', incomeFilters.maxTotal.toString());
      }
      if (incomeFilters.orderByColumn) {
        params = params.append('orderByColumn', incomeFilters.orderByColumn);
      }
      if (incomeFilters.orderDescending) {
        params = params.append('orderDescending', 'true');
      } else {
        params = params.append('orderDescending', 'false');
      }
    }

    return params;
  }

  getCurrentSites(): Site[] {
    return this.authService.currentUser.sites;
  }

  getIncomeEntry(userId: number, incomeId: number): Observable<IncomeEntryForReturnDto> {
    return this.http.get<IncomeEntryForReturnDto>(this.baseUrl + 'users/' + userId + '/incomes/' + incomeId);
  }

  getIncomeEntries(incomeFilters: IncomeListFilterParams): Observable<IncomeListDataReturnDto> {
    if (this.authService.currentUser.role === Role.Admin) {
      const params = this.createHttpParamsFromFilterParams(incomeFilters);
      return this.http.get<IncomeListDataReturnDto>(this.baseUrl + 'incomes', {params});
    } else {
      incomeFilters.userId = this.authService.currentUser.id;
      const params = this.createHttpParamsFromFilterParams(incomeFilters);
      return this.http.get<IncomeListDataReturnDto>(this.baseUrl + 'users/' + incomeFilters.userId + '/incomes', {params});
    }
  }

  updateIncomeEntry(userId: number, incomeId: number, incomeEntry: IncomeEntryUpdateDto): Observable<IncomeEntryForReturnDto> {
    return this.http.put<IncomeEntryForReturnDto>(this.baseUrl + 'users/' + userId + '/incomes/' + incomeId, incomeEntry);
  }

  addIncomeEntry(userId: number, incomeEntry: IncomeEntryAddDto): Observable<IncomeEntryForReturnDto> {
    return this.http.post<IncomeEntryForReturnDto>(this.baseUrl + 'users/' + userId + '/incomes', incomeEntry);
  }

  deleteIncomeEntry(incomeId: number) {
    return this.http.delete(this.baseUrl + 'incomes/' + incomeId);
  }

  lockIncomeEntry(userId: number, incomeId: number): Observable<boolean> {
    return this.http.put<boolean>(this.baseUrl + 'users/' + userId + '/incomes/' + incomeId + '/lock', {});
  }

  unlockIncomeEntry(userId: number, incomeId: number): Observable<boolean> {
    return this.http.put<boolean>(this.baseUrl + 'users/' + userId + '/incomes/' + incomeId + '/unlock', {});
  }
}
