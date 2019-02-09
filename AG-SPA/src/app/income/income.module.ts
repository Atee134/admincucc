import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IncomeListComponent } from './income-list/income-list.component';
import { IncomeEditComponent } from './income-edit/income-edit.component';
import { FormsModule } from '@angular/forms';
import { IncomeAddComponent } from './income-add/income-add.component';
import { RouterModule } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    BsDatepickerModule.forRoot(),
  ],
  declarations: [IncomeListComponent, IncomeEditComponent, IncomeAddComponent],
  exports: [
    IncomeListComponent,
    IncomeEditComponent
  ]
})
export class IncomeModule { }
