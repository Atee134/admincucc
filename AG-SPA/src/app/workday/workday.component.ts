import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { WorkdayService } from '../_services/workday.service';

@Component({
  selector: 'app-workday',
  templateUrl: './workday.component.html',
  styleUrls: ['./workday.component.css']
})
export class WorkdayComponent implements OnInit {
  availableDates: Date[];
  workingDays: Date[] = [];

  constructor(private route: ActivatedRoute, private workdayService: WorkdayService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.availableDates = data['availableDates'];
      if (data['workingDays'] != null){
        this.workingDays = data['workingDays'];
      }
    })
  }

  addWorkDay(date: Date) {
    this.workdayService.addWorkDay(date).subscribe(() => {
      this.workingDays.push(date);
      console.log('added workday with date: ' + date);
    }, error => {
      console.log('error occured: ' + error);
    });
  }

  removeWorkDay(date: Date) {
    this.workdayService.removeWorkDay(date).subscribe(() => {
      this.workingDays = this.workingDays.filter(day => day != date);
      console.log('removed workday with date: ' + date);
    }, error => {
      console.log('error occured: ' + error);
    });
  }

  isWorkDay(date: Date): boolean {
    return this.workingDays.indexOf(date) > -1;
  }
}
