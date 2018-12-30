import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IncomeListComponent } from './income-list/income-list.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [IncomeListComponent],
  exports: [
    IncomeListComponent
  ]
})
export class IncomeModule { }
