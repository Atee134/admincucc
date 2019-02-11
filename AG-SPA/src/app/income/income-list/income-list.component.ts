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
  @Input() incomeFilters: IncomeListFilterParams;
  public incomeList: IncomeListDataReturnDto;

  constructor(private authService: AuthService, private alertify: AlertifyService, private incomeService: IncomeService) { }

  ngOnInit() {
  }

  public getIncomeEntries(): void {
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
        this.alertify.success('Income locked');
        const incomeEntry = this.incomeList.incomeEntries.find(i => i.id === incomeId);
        incomeEntry.locked = true;
      } else {
        this.alertify.message('Income already locked');
      }
    }, error => {
      this.alertify.error(error);
    });
  }

  public onUnlock(userId: number, incomeId: number): void {
    this.incomeService.unlockIncomeEntry(userId, incomeId).subscribe(resp => {
      if (resp) {
        this.alertify.success('Income unlocked');
        const incomeEntry = this.incomeList.incomeEntries.find(i => i.id === incomeId);
        incomeEntry.locked = false;
      } else {
        this.alertify.message('Income already unlocked');
      }
    }, error => {
      this.alertify.error(error);
    });
  }
}
