import { Component, OnInit, Input, SimpleChanges, OnChanges } from '@angular/core';
import { IncomeEntryAddDto,
  Site,
  IncomeChunkAddDto,
  UserForListDto,
  Shift,
  Role
} from 'src/app/_models/generatedDtos';
import { AuthService } from 'src/app/_services/auth.service';
import { IncomeService } from 'src/app/_services/income.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-income-add',
  templateUrl: './income-add.component.html',
  styleUrls: ['./income-add.component.css']
})
export class IncomeAddComponent implements OnInit, OnChanges {
  @Input() userId: number;

  public incomeEntry: IncomeEntryAddDto;
  public colleagues: UserForListDto[];
  private operatorSites: Site[];
  private performerSites: Site[];
  public availableSites: Site[];

  constructor(
    private authService: AuthService,
    private incomeService: IncomeService,
    private userService: UserService,
    private alertify: AlertifyService,
    private router: Router
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
        this.operatorSites = user.sites;

        this.incomeEntry = entry;
      }, error => {
        this.alertify.error(error);
      });
    } else {
      this.operatorSites = this.authService.currentUser.sites;
      this.incomeEntry = entry;
    }
  }

  public getPerformerSites() {
    // tslint:disable-next-line:triple-equals for some reason the HTML select component sets string to incomeEntry.performerId
    const performer = this.colleagues.find(c => c.id == this.incomeEntry.performerId);

    this.performerSites = performer.sites;

    this.calculateAvailableSites();
  }

  private calculateAvailableSites() {
    this.availableSites = this.operatorSites.filter(x => this.performerSites.includes(x));
    this.recreateIncomeChunks();
  }

  private recreateIncomeChunks() {
    this.incomeEntry.incomeChunks = [];

    for (const site of this.availableSites) {
      this.incomeEntry.incomeChunks.push(this.createIncomeChunk(site));
    }
  }

  private createIncomeChunk(site: Site) {
    const chunk = new IncomeChunkAddDto();
    chunk.site = site;
    return chunk;
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
      this.alertify.success('Bevétel hozzáadva.');
      this.router.navigate(['/incomes']);
    }, error => {
      this.alertify.error(error);
    });
  }
}
