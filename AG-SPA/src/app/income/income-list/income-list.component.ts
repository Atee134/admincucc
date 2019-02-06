import { Component, OnInit } from '@angular/core';
import { IncomeService } from 'src/app/_services/income.service';
import { IncomeEntryForReturnDto,
  Site,
  IncomeChunkForReturnDto,
  Role,
  IncomeListDataReturnDto,
  IncomeStatisticsSiteSumDto
} from 'src/app/_models/generatedDtos';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-income-list',
  templateUrl: './income-list.component.html',
  styleUrls: ['./income-list.component.css']
})
export class IncomeListComponent implements OnInit {
  public incomeList: IncomeListDataReturnDto;

  constructor(private authService: AuthService, private incomeService: IncomeService) { }

  ngOnInit() {
    this.getIncomeEntries();
  }

  getIncomeEntries(): void {
    // TODO add a filter component maybe which handles admin/operator lisings, as well as filters

    if (this.authService.currentUser.role === Role.Admin) {
      this.incomeService.getAllIncomeEntries().subscribe(resp => {
        this.incomeList = resp;
      });
    } else {
      this.incomeService.getIncomeEntries(this.authService.currentUser.id).subscribe(resp => {
        this.incomeList  = resp;
      });
    }
  }

  get uniqueSites(): Site[] {
    const incomeChunks = this.incomeList.incomeEntries.map(e => e.incomeChunks);

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

  getSiteStatistics(site: Site): IncomeStatisticsSiteSumDto {
    return this.incomeList.siteStatistics.find(s => s.site === site);
  }

  isCurrentUser(role: string): boolean {
    return this.authService.currentUser.role.toLowerCase() === role.toLowerCase();
  }
}
