import { Component, OnInit, ViewChild } from '@angular/core';
import { IncomeListFilterParams } from 'src/app/_models/generatedDtos';
import { IncomeListComponent } from '../income-list/income-list.component';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-income-filter',
  templateUrl: './income-filter.component.html',
  styleUrls: ['./income-filter.component.css']
})
export class IncomeFilterComponent implements OnInit {
  @ViewChild(IncomeListComponent) incomeList: IncomeListComponent;
  public incomeListReady = false;

  public incomeFilters: IncomeListFilterParams = new IncomeListFilterParams({
    hideLocked: false,
    orderDescending: true,
    fromToFilter: false,
  });

  constructor(private authService: AuthService) { }

  // tslint:disable-next-line:use-life-cycle-interface
  ngAfterViewInit(): void {
    this.incomeListReady = true;
  }

  ngOnInit() {
    this.incomeFilters.month = new Date();

    if (this.incomeFilters.month.getDate() <= 15) {
      this.incomeFilters.period = 1;
    } else {
      this.incomeFilters.period = 2;
    }
    this.incomeFilters.month.setDate(1);
    this.onGetIncomes();
  }

  onGetIncomes(): void {
    this.incomeList.getIncomeEntries(this.incomeFilters);
  }

  public isCurrentUser(role: string): boolean {
    return this.authService.currentUser.role.toLowerCase() === role.toLowerCase();
  }
}
