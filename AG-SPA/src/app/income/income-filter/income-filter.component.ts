import { Component, OnInit, ViewChild } from '@angular/core';
import { IncomeListFilterParams } from 'src/app/_models/generatedDtos';
import { IncomeListComponent } from '../income-list/income-list.component';
import { AuthService } from 'src/app/_services/auth.service';
import { OrderDirection } from './income-order-enum';

@Component({
  selector: 'app-income-filter',
  templateUrl: './income-filter.component.html',
  styleUrls: ['./income-filter.component.css']
})
export class IncomeFilterComponent implements OnInit {
  @ViewChild(IncomeListComponent) incomeList: IncomeListComponent;
  public incomeListReady = false;
  private orderDirections: OrderDirection[] = Object.keys(OrderDirection).map(k => OrderDirection[k]);

  public incomeFilters: IncomeListFilterParams = new IncomeListFilterParams({
    hideLocked: false,
    orderDescending: true,
    orderByColumn: 'date',
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

  public getOrderDirection(order: string): OrderDirection {
    return this.orderDirections.find(o => o.toLowerCase() === order.toLowerCase());
  }

  public onOrderDirectionChanged(newOrderDirection: OrderDirection) {
    switch (newOrderDirection) {
      case OrderDirection.DateAscending:
        this.incomeFilters.orderByColumn = 'date';
        this.incomeFilters.orderDescending = false;
        break;
      case OrderDirection.DateDescending:
        this.incomeFilters.orderByColumn = 'date';
        this.incomeFilters.orderDescending = true;
        break;
      case OrderDirection.IncomeAscending:
        this.incomeFilters.orderByColumn = 'total';
        this.incomeFilters.orderDescending = false;
        break;
      case OrderDirection.IncomeDescending:
        this.incomeFilters.orderByColumn = 'total';
         this.incomeFilters.orderDescending = true;
        break;
      case OrderDirection.OperatorNameAscending:
        this.incomeFilters.orderByColumn = 'operator';
        this.incomeFilters.orderDescending = false;
        break;
      case OrderDirection.OperatorNameDescending:
        this.incomeFilters.orderByColumn = 'operator';
        this.incomeFilters.orderDescending = true;
        break;
      case OrderDirection.PerformerNameAscending:
        this.incomeFilters.orderByColumn = 'performer';
        this.incomeFilters.orderDescending = false;
        break;
      case OrderDirection.PerformerNameDescending:
        this.incomeFilters.orderByColumn = 'performer';
        this.incomeFilters.orderDescending = true;
        break;
    }
  }

  onGetIncomes(): void {
    this.incomeList.getIncomeEntries(this.incomeFilters);
  }

  public isCurrentUser(role: string): boolean {
    return this.authService.currentUser.role.toLowerCase() === role.toLowerCase();
  }
}
