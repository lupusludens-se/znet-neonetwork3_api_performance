import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { EventInterface } from '../../../../shared/interfaces/event/event.interface';
import * as dayjs from 'dayjs';
import * as isBetween from 'dayjs/plugin/isBetween';
import { EventLocationType } from '../../../../shared/enums/event/event-location-type.enum';

dayjs.extend(isBetween);

@Component({
  selector: 'neo-upcoming-events',
  templateUrl: './upcoming-events.component.html',
  styleUrls: ['./upcoming-events.component.scss']
})
export class UpcomingEventsComponent implements OnInit {
  @Input() cssClasses: string;
  @Input() event: EventInterface;
  @Input() isPublicUser: boolean = false;
  @Output() eventClick: EventEmitter<void> = new EventEmitter<void>();

  isFromDate: boolean;
  isToDate: boolean;
  isTodayBetween: boolean;
  eventDate: string;
  eventLocationType: string;

  ngOnInit(): void {
    this.isTodayBetween = dayjs().isBetween(
      this.event.occurrences[0].fromDate,
      this.event.occurrences[0].toDate,
      'date'
    );
    this.isFromDate = dayjs(this.event.occurrences[0].fromDate).isSame(dayjs(), 'date');
    this.isToDate = dayjs(this.event.occurrences[0].toDate).isSame(dayjs(), 'date');
    this.eventDate = dayjs(this.event.occurrences[0].fromDate).format('MMM D, YYYY');

    this.eventLocationType =
      this.event.locationType === EventLocationType.Virtual ? 'events.virtualLabel' : 'events.inPersonLabel';
  }
}
