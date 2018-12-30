import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IncomeListComponent } from './income-list/income-list.component';
import { IncomeEditComponent } from './income-edit/income-edit.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [IncomeListComponent, IncomeEditComponent],
  exports: [
    IncomeListComponent,
    IncomeEditComponent
  ]
})
export class IncomeModule { }
