import { Component, OnInit } from '@angular/core';
import { IncomeEntryAddDto, Site, IncomeChunkAddDto, IncomeEntryForReturnDto, UserForListDto } from 'src/app/_models/generatedDtos';
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
  public colleagues: UserForListDto;
  public date: Date;

  constructor(
    private authService: AuthService,
    private incomeService: IncomeService,
    private userService: UserService,
    private router: Router,
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
    entry.date = new Date();

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

  /**
   * gets the union of the dto's current sites, and the user's assigned sites.
   * In case of editing an income which has not yet had the newly assigned site,
   * or editing an old income, which still as a site, but it's not assigned to the user anymore.
   */
  get uniqueSites(): Site[] {
    const uniqueSites: Site[] = [];
    for (const incomeChunk of this.incomeEntry.incomeChunks) {
      if (!uniqueSites.includes(incomeChunk.site)) {
        uniqueSites.push(incomeChunk.site);
      }
    }

    for (const site of this.authService.currentUser.sites) {
      if (!uniqueSites.includes(site)) {
        uniqueSites.push(site);
      }
    }

    return uniqueSites;
  }

  public getIncomeChunk(site: Site): IncomeChunkAddDto {
    return this.incomeEntry.incomeChunks.find(i => i.site === site);
  }

  public onSubmit(): void {
    this.incomeService.addIncomeEntry(this.authService.currentUser.id, this.incomeEntry).subscribe(resp => {
      this.responseEntry = resp;
      this.alertify.success('Successfully added icome.');
    });
  }
}
