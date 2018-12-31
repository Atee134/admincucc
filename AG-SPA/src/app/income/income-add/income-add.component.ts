import { Component, OnInit } from '@angular/core';
import { IncomeEntryAddDto, Site, IncomeChunkAddDto, IncomeEntryForReturnDto } from 'src/app/_models/generatedDtos';
import { AuthService } from 'src/app/_services/auth.service';
import { IncomeService } from 'src/app/_services/income.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-income-add',
  templateUrl: './income-add.component.html',
  styleUrls: ['./income-add.component.css']
})
export class IncomeAddComponent implements OnInit {
  public incomeEntry: IncomeEntryAddDto;
  public responseEntry: IncomeEntryForReturnDto;

  constructor(private authService: AuthService, private incomeService: IncomeService, private router: Router) { }

  ngOnInit() {
    // TODO if we come from an ADD route, we make a new incomeEntry, otherwise we fetch it from the server
    // if we fetch it from the server, we have to make sure to include new dtos to newly assigned sites
    // !!!!!probably better to make a new, edit component, and rename this to add!!!!!!!
    this.incomeEntry = this.createIncomeEntry();
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
    });
  }
}
