import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ExchangeRateService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public getEurToUsdExchangeRate(): Observable<number> {
    return this.http.get('https://api.exchangeratesapi.io/latest?symbols=USD', { observe: 'response'}).pipe(
      map(res => {
        return (res as any).body.rates.USD as number;
      })
    );
  }
}
