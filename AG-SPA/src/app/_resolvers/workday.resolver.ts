import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { WorkdayService } from "../_services/workday.service";

@Injectable()
export class WorkdayResolver implements Resolve<Date[]> {
  constructor(
     private workDayService: WorkdayService,
    private router: Router
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Date[]> {
    return this.workDayService.getAvailableDates().pipe(
      catchError(error => {
        //this.alertify.error("Problem retrieving data"); //TODO add alertify
        console.log(error);
       // this.router.navigate(["/home"]); // TODO add home route and navigate there
        return of(null);
      })
    );
  }
}
