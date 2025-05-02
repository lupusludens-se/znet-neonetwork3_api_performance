import { Component, Input } from '@angular/core';

@Component({
  selector: 'neo-event-date-time',
  templateUrl: './event-date-time.component.html',
  styleUrls: ['./event-date-time.component.scss']
})
export class EventDateTimeComponent {
  @Input() dates: string;
  @Input() times: string;
  @Input() datesCount: number;
  @Input() timesCount: number;
}
