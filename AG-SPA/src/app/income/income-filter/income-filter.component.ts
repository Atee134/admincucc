import { Component, OnInit, ViewChild } from '@angular/core';
import { IncomeListFilterParams } from 'src/app/_models/generatedDtos';
import { IncomeListComponent } from '../income-list/income-list.component';

@Component({
  selector: 'app-income-filter',
  templateUrl: './income-filter.component.html',
  styleUrls: ['./income-filter.component.css']
})
export class IncomeFilterComponent implements OnInit {
  @ViewChild(IncomeListComponent) incomeList: IncomeListComponent;
  public incomeListReady = false;

  public incomeFilters: IncomeListFilterParams;

  constructor() { }

  // tslint:disable-next-line:use-life-cycle-interface
  ngAfterViewInit(): void {
    this.incomeListReady = true;
  }

  ngOnInit() {
  }

  onGetIncomes(): void {
    this.incomeList.getIncomeEntries();
  }
}
