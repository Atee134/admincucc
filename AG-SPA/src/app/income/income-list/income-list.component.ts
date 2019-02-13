import { Component, OnInit, Input } from '@angular/core';
import { IncomeService } from 'src/app/_services/income.service';
import { IncomeEntryForReturnDto,
  Site,
  IncomeChunkForReturnDto,
  Role,
  IncomeListDataReturnDto,
  IncomeStatisticsSiteSumDto,
  IncomeListFilterParams
} from 'src/app/_models/generatedDtos';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-income-list',
  templateUrl: './income-list.component.html',
  styleUrls: ['./income-list.component.css']
})
export class IncomeListComponent implements OnInit {
  public incomeList: IncomeListDataReturnDto;

  constructor(private authService: AuthService, private alertify: AlertifyService, private incomeService: IncomeService) { }

  ngOnInit() {
  }

  public getIncomeEntries(incomeFilters: IncomeListFilterParams): void {
    this.incomeService.getIncomeEntries(incomeFilters).subscribe(resp => {
      this.incomeList = resp;
    }, error => {
      this.alertify.error(error);
    });
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

  public getIncomeChunk(entry: IncomeEntryForReturnDto, site: Site): IncomeChunkForReturnDto {
    return entry.incomeChunks.find(i => i.site === site);
  }

  public getSiteStatistics(site: Site): IncomeStatisticsSiteSumDto {
    return this.incomeList.siteStatistics.find(s => s.site === site);
  }

  public isCurrentUser(role: string): boolean {
    return this.authService.currentUser.role.toLowerCase() === role.toLowerCase();
  }

  public onLock(userId: number, incomeId: number): void {
    this.incomeService.lockIncomeEntry(userId, incomeId).subscribe(resp => {
      if (resp) {
        this.alertify.success('Bevétel lezárva');
        const incomeEntry = this.incomeList.incomeEntries.find(i => i.id === incomeId);
        incomeEntry.locked = true;
      } else {
        this.alertify.message('Bevétel már le van zárva');
      }
    }, error => {
      this.alertify.error(error);
    });
  }

  public onUnlock(userId: number, incomeId: number): void {
    this.incomeService.unlockIncomeEntry(userId, incomeId).subscribe(resp => {
      if (resp) {
        this.alertify.success('Bevétel feloldva');
        const incomeEntry = this.incomeList.incomeEntries.find(i => i.id === incomeId);
        incomeEntry.locked = false;
      } else {
        this.alertify.message('Bevétel már fel van oldva');
      }
    }, error => {
      this.alertify.error(error);
    });
  }
}
