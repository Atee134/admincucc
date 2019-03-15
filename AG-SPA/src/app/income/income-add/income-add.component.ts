import { Component, OnInit, Input, SimpleChanges, OnChanges } from '@angular/core';
import { IncomeEntryAddDto,
  Site,
  IncomeChunkAddDto,
  IncomeEntryForReturnDto,
  UserForListDto,
  Shift,
  Role
} from 'src/app/_models/generatedDtos';
import { AuthService } from 'src/app/_services/auth.service';
import { IncomeService } from 'src/app/_services/income.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-income-add',
  templateUrl: './income-add.component.html',
  styleUrls: ['./income-add.component.css']
})
export class IncomeAddComponent implements OnInit, OnChanges {
  @Input() userId: number;

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
    this.initialize(this.userId);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.userId.previousValue && changes.userId.previousValue !== changes.userId.currentValue) {
      this.initialize(changes.userId.currentValue);
    }
  }

  private initialize(userId: number) {
    this.getColleagues(userId);
    this.createIncomeEntry(userId);
  }

  private getColleagues(userId: number): void {
      this.userService.getColleagues(userId).subscribe(resp => {
        this.colleagues = resp;
      }, error => {
        this.alertify.error(error);
      });
  }

  private createIncomeEntry(userId: number) {
    const entry = new IncomeEntryAddDto();
    entry.date = this.getInitialDate();

    if (this.authService.currentUser.role === Role.Admin) {
      this.userService.getUser(userId).subscribe(user => {
        for (const site of user.sites) {
          entry.incomeChunks.push(this.createIncomeChunk(site));
        }

        this.incomeEntry = entry;
      }, error => {
        this.alertify.error(error);
      });
    } else {
      for (const site of this.authService.currentUser.sites) {
        entry.incomeChunks.push(this.createIncomeChunk(site));
      }

      this.incomeEntry = entry;
    }
  }

  private createIncomeChunk(site: Site) {
    const chunk = new IncomeChunkAddDto();
    chunk.site = site;
    return chunk;
  }

  get sites(): Site[] {
    const sites: Site[] = [];

    for (const chunk of this.incomeEntry.incomeChunks) {
      sites.push(chunk.site);
    }

    return sites;
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
    this.incomeService.addIncomeEntry(this.userId, this.incomeEntry).subscribe(resp => {
      this.responseEntry = resp;
      this.alertify.success('Bevétel hozzáadva.');
    }, error => {
      this.alertify.error(error);
    });
  }
}
