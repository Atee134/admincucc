import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IncomeListComponent } from './income-list/income-list.component';
import { IncomeEditComponent } from './income-edit/income-edit.component';
import { FormsModule } from '@angular/forms';
import { IncomeAddComponent } from './income-add/income-add.component';
import { RouterModule } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { IncomeFilterComponent } from './income-filter/income-filter.component';
import { IncomeAddUserselectorComponent } from './income-add-userselector/income-add-userselector.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    BsDatepickerModule.forRoot(),
  ],
  declarations: [IncomeListComponent, IncomeEditComponent, IncomeAddComponent, IncomeFilterComponent, IncomeAddUserselectorComponent],
  exports: [
    IncomeListComponent,
    IncomeEditComponent
  ]
})
export class IncomeModule { }
