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
      this.incomeEntryUpdateDto = this.createIncomeEntryUpdateDto();
      this.getColleagues(this.incomeEntry);
    }, error => {
      this.alertify.error(error);
    });
  }

  private getColleagues(incomeEntry: IncomeEntryForReturnDto): void {
    const userId = this.authService.currentUser.role === Role.Admin ? incomeEntry.operatorId : this.authService.currentUser.id;

    this.userService.getColleagues(userId).subscribe(resp => {
      this.colleagues = resp;
    }, error => {
      this.alertify.error(error);
    });
  }

  private createIncomeEntryUpdateDto(): IncomeEntryUpdateDto {
    const incomeEntryUpdateDto = new IncomeEntryUpdateDto();
    incomeEntryUpdateDto.date = new Date(this.incomeEntry.date);
    incomeEntryUpdateDto.performerId = this.incomeEntry.performerId;
    incomeEntryUpdateDto.incomeChunks = [];

    for (const incomeChunk of this.incomeEntry.incomeChunks) {
      incomeEntryUpdateDto.incomeChunks.push(this.createIncomeChunkUpdateDto(incomeChunk));
    }

    const newSites = this.authService.currentUser.sites.filter(s => !incomeEntryUpdateDto.incomeChunks.map(i => i.site).includes(s));

    for (const site of newSites) {
      incomeEntryUpdateDto.incomeChunks.push(this.createNewIncomeChunkUpdateDto(site));
    }

    return incomeEntryUpdateDto;
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
           this.incomeEntryUpdateDto = this.createIncomeEntryUpdateDto();
        }, error => {
           this.alertify.error(error);
        });
  }
}
