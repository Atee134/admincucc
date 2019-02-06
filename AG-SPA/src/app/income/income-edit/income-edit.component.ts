import { Component, OnInit } from '@angular/core';
import { IncomeEntryAddDto, IncomeChunkAddDto, Site, IncomeEntryForReturnDto } from 'src/app/_models/generatedDtos';
import { AuthService } from 'src/app/_services/auth.service';
import { IncomeService } from 'src/app/_services/income.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-income-edit',
  templateUrl: './income-edit.component.html',
  styleUrls: ['./income-edit.component.css']
})
export class IncomeEditComponent implements OnInit {
  public incomeEntry: IncomeEntryForReturnDto;

  constructor(private incomeService: IncomeService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private alertify: AlertifyService
    ) { }

  ngOnInit() {
    this.getIncomeEntry();
  }

  private getIncomeEntry(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.incomeService.getIncomeEntry(this.authService.currentUser.id, id).subscribe(resp => {
      this.incomeEntry = resp;
    }, error => {
      this.alertify.error(error);
    });
  }
}
