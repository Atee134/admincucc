import { Component, OnInit } from '@angular/core';
import { IncomeService } from 'src/app/_services/income.service';
import { IncomeEntryForReturnDto, Site, IncomeChunkForReturnDto } from 'src/app/_models/generatedDtos';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-income-list',
  templateUrl: './income-list.component.html',
  styleUrls: ['./income-list.component.css']
})
export class IncomeListComponent implements OnInit {
  public incomeEntries: IncomeEntryForReturnDto[];

  constructor(private authService: AuthService, private incomeService: IncomeService) { }

  ngOnInit() {
    this.getIncomeEntries();
  }

  getIncomeEntries(): void {
    this.incomeService.getIncomeEntries(this.authService.currentUser.id).subscribe(resp => {
      this.incomeEntries = resp;
    });
  }

  get uniqueSites(): Site[] {
    const incomeChunks = this.incomeEntries.map(e => e.incomeChunks);

    const incomeChnksFlattened = <IncomeChunkForReturnDto[]>[].concat(...incomeChunks);
    const uniqueSites: Site[] = [];
    for (const incomeChunk of incomeChnksFlattened) {
      if (!uniqueSites.includes(incomeChunk.site)) {
        uniqueSites.push(incomeChunk.site);
      }
    }

    return uniqueSites;
  }

  getIncomeChunk(entry: IncomeEntryForReturnDto, site: Site): IncomeChunkForReturnDto {
    return entry.incomeChunks.find(i => i.site === site);
  }
}
