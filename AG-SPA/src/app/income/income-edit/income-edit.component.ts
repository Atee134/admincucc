import { Component, OnInit } from '@angular/core';
import { IncomeEntryAddDto, IncomeChunkAddDto, Site } from 'src/app/_models/generatedDtos';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-income-edit',
  templateUrl: './income-edit.component.html',
  styleUrls: ['./income-edit.component.css']
})
export class IncomeEditComponent implements OnInit {
  public testNumber: number;
  constructor() { }

  ngOnInit() {
     }

     get inputRegex(): RegExp {
      return new RegExp('');
    }
}
