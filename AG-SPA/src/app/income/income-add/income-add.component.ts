import { Component, OnInit } from '@angular/core';
import { IncomeEntryAddDto, Site, IncomeChunkAddDto, IncomeEntryForReturnDto, UserForListDto, Shift } from 'src/app/_models/generatedDtos';
import { AuthService } from 'src/app/_services/auth.service';
import { IncomeService } from 'src/app/_services/income.service';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-income-add',
  templateUrl: './income-add.component.html',
  styleUrls: ['./income-add.component.css']
})
export class IncomeAddComponent implements OnInit {
  public incomeEntry: IncomeEntryAddDto;
  public responseEntry: IncomeEntryForReturnDto;
  public colleagues: UserForListDto[];

  constructor(
    private authService: AuthService,
    private incomeService: IncomeService,
    private userService: UserService,
    private alertify: AlertifyService
    ) { }

  ngOnInit() {
    this.getColleagues();
    this.incomeEntry = this.createIncomeEntry();
  }

  private getColleagues(): void {
      this.userService.getColleagues(this.authService.currentUser.id).subscribe(resp => {
        this.colleagues = resp;
      }, error => {
        this.alertify.error(error);
      });
  }

  private createIncomeEntry(): IncomeEntryAddDto {
    const entry = new IncomeEntryAddDto();
    entry.date = this.getInitialDate();

    for (const site of this.authService.currentUser.sites) {
      entry.incomeChunks.push(this.createIncomeChunk(site));
    }

    return entry;
  }

  private createIncomeChunk(site: Site) {
    const chunk = new IncomeChunkAddDto();
    chunk.site = site;
    return chunk;
  }

  get sites(): Site[] {
    return this.authService.currentUser.sites;
  }

  private getInitialDate(): Date {
    const date = new Date();
    if (this.authService.currentUser.shift === Shift.Night) {
       date.setDate(date.getDate() - 1);
    }
    return date;
  }

  public getIncomeChunk(site: Site): IncomeChunkAddDto {
    return this.incomeEntry.incomeChunks.find(i => i.site === site);
  }

  public onSubmit(): void {
    this.incomeService.addIncomeEntry(this.authService.currentUser.id, this.incomeEntry).subscribe(resp => {
      this.responseEntry = resp;
      this.alertify.success('Bevétel hozzáadva.');
    }, error => {
      this.alertify.error(error);
    });
  }
}
