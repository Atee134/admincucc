import { Component, OnInit } from '@angular/core';
import {
  IncomeEntryForReturnDto,
  IncomeEntryUpdateDto,
  UserForListDto,
  Site,
  IncomeChunkUpdateDto,
  IncomeChunkForReturnDto,
  Role
} from 'src/app/_models/generatedDtos';
import { AuthService } from 'src/app/_services/auth.service';
import { IncomeService } from 'src/app/_services/income.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-income-edit',
  templateUrl: './income-edit.component.html',
  styleUrls: ['./income-edit.component.css']
})
export class IncomeEditComponent implements OnInit {
  public incomeEntry: IncomeEntryForReturnDto;
  public incomeEntryUpdateDto: IncomeEntryUpdateDto;
  public colleagues: UserForListDto[];

  constructor(private incomeService: IncomeService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private alertify: AlertifyService,
    private userService: UserService
    ) { }

  ngOnInit() {
    this.getIncomeEntry();
  }

  private getIncomeEntry(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.incomeService.getIncomeEntry(this.authService.currentUser.id, id).subscribe(resp => {
      this.incomeEntry = resp;
      this.getColleagues(this.incomeEntry);
    }, error => {
      this.alertify.error(error);
    });
  }

  private getColleagues(incomeEntry: IncomeEntryForReturnDto): void {
    const userId = this.authService.currentUser.role === Role.Admin ? incomeEntry.operatorId : this.authService.currentUser.id;

    this.userService.getColleagues(userId).subscribe(resp => {
      this.colleagues = resp;
      this.initIncomeEntryUpdateDto();
    }, error => {
      this.alertify.error(error);
    });
  }

  private initIncomeEntryUpdateDto() {
    const userId = this.authService.currentUser.role === Role.Admin ? this.incomeEntry.operatorId : this.authService.currentUser.id;

    if (this.authService.currentUser.role === Role.Admin) {
      this.userService.getUser(userId).subscribe(user => {
        const sites = user.sites.filter(s => !this.incomeEntry.incomeChunks.map(i => i.site).includes(s));
        this.createIncomeEntryUpdateDto(sites);
      });
    } else {
      const sites = this.authService.currentUser.sites.filter(s => !this.incomeEntry.incomeChunks.map(i => i.site).includes(s));
      this.createIncomeEntryUpdateDto(sites);
    }
  }

  private createIncomeEntryUpdateDto(operatorSites: Site[]) {
    const incomeEntryUpdateDto = new IncomeEntryUpdateDto();
    incomeEntryUpdateDto.date = new Date(this.incomeEntry.date);
    incomeEntryUpdateDto.performerId = this.incomeEntry.performerId;
    incomeEntryUpdateDto.incomeChunks = [];

    // site list consists of the already existing sites (sites of incomeChunks)
    // and the intersection of the operator and performer current sites.
    for (const incomeChunk of this.incomeEntry.incomeChunks) {
      incomeEntryUpdateDto.incomeChunks.push(this.createIncomeChunkUpdateDto(incomeChunk));
    }

    const performer = this.colleagues.find(c => c.id === this.incomeEntry.performerId);
    const performerSites = performer.sites;

    const availableSites = operatorSites.filter(x => performerSites.includes(x));

    for (const site of availableSites) {
      incomeEntryUpdateDto.incomeChunks.push(this.createNewIncomeChunkUpdateDto(site));
    }

    this.incomeEntryUpdateDto = incomeEntryUpdateDto;
  }

  private createIncomeChunkUpdateDto(incomeChunk: IncomeChunkForReturnDto): IncomeChunkUpdateDto {
    const incomeChunkDto = new IncomeChunkUpdateDto();

    incomeChunkDto.id = incomeChunk.id;
    incomeChunkDto.income = incomeChunk.sum;
    incomeChunkDto.site = incomeChunk.site;

    return incomeChunkDto;
  }

  private createNewIncomeChunkUpdateDto(site: Site): IncomeChunkUpdateDto {
    const incomeChunkDto = new IncomeChunkUpdateDto();

    incomeChunkDto.income = 0;
    incomeChunkDto.site = site;

    return incomeChunkDto;
  }

  public isCurrentUser(role: string): boolean {
    return this.authService.currentUser.role.toLowerCase() === role.toLowerCase();
  }

  public onDelete(): void {
    this.alertify.confirm('Biztos, hogy véglegesen törlöd a bevételt?', () => {
      this.incomeService.deleteIncomeEntry(this.incomeEntry.id).subscribe(resp => {
        this.alertify.success('Bevétel törölve');
        this.router.navigate(['/incomes']);
      }, error => {
        this.alertify.error(error);
      });
    });
  }

  public onSubmit(): void {
    this.incomeService.updateIncomeEntry(
      this.authService.currentUser.id,
      this.incomeEntry.id,
      this.incomeEntryUpdateDto).subscribe(
        resp => {
           this.incomeEntry = resp;
           this.alertify.success('Változtatások mentve.');
           this.initIncomeEntryUpdateDto();
        }, error => {
           this.alertify.error(error);
        });
  }
}
