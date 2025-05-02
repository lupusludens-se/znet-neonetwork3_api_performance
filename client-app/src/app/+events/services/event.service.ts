import { Observable, Subject, take } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/core/services/http.service';
import { EventsApiEnum } from 'src/app/shared/enums/api/events-api.enum';
import { EventInterface } from 'src/app/shared/interfaces/event/event.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { EventUserInterface } from 'src/app/shared/interfaces/event/event-user.interface';
import { EventAttendanceInterface, EventListRequestInterface } from '../component/events-list/events-list.component';
import * as dayjs from 'dayjs';
import { EventOccurrenceInterface } from 'src/app/shared/interfaces/event/event-occurrence.interface';
import { map } from 'rxjs/operators';
import { EventCalendarRequestInterface } from '../../shared/interfaces/event/event-calendar-request-interface.interface';

@Injectable()
export class EventService {
  attendanceChange$: Subject<EventAttendanceInterface> = new Subject<EventAttendanceInterface>();
  private apiRoutes = EventsApiEnum;

  constructor(private httpService: HttpService) {}

  getEventDetails(id: string): Observable<EventInterface> {
    return this.httpService.get<EventInterface>(
      `${this.apiRoutes.Events}/${id}?expand=timezone,categories,moderators.followers,moderators.company,moderators.image,occurrences,links,attendees,attendees.followers,attendees.company,attendees.image`
    );
  }

  getCalendarEvents(requestData: EventCalendarRequestInterface): Observable<PaginateResponseInterface<EventInterface>> {
    return this.httpService.get<PaginateResponseInterface<EventInterface>>(`${this.apiRoutes.Events}`, {
      expand: 'moderators.company,moderators.image,occurrences,links,attendees.image',
      ...requestData
    });
  }

  getListEvents(requestData: EventListRequestInterface): Observable<PaginateResponseInterface<EventInterface>> {
    return this.httpService.get<PaginateResponseInterface<EventInterface>>(`${this.apiRoutes.Events}`, {
      expand: 'occurrences,attendees',
      ...requestData
    });
  }

  getPastEvents(requestData: EventListRequestInterface): Observable<PaginateResponseInterface<EventInterface>> {
    return this.httpService.get<PaginateResponseInterface<EventInterface>>(
      `${this.apiRoutes.Events}` + '/past-events',
      {
        expand: 'occurrences,attendees',
        ...requestData
      }
    );
  }

  getHighlightedEvents(): Observable<EventInterface[]> {
    return this.httpService
      .get<PaginateResponseInterface<EventInterface>>(`${this.apiRoutes.Events}`, {
        expand: 'moderators.image,moderators.company,attendees',
        from: dayjs().toISOString(),
        take: 3,
        highlightedOnly: true
      })
      .pipe(
        map((events: PaginateResponseInterface<EventInterface>) => {
          events.dataList.forEach(event => this.convertEvent(event));
          return events.dataList;
        })
      );
  }

  getAttendees(eventId: string): Observable<PaginateResponseInterface<EventUserInterface>> {
    return this.httpService.get<PaginateResponseInterface<EventUserInterface>>(
      `${this.apiRoutes.Events}/${eventId}/attendees?expand=company,image,followers`
    );
  }

  addEventToCalendar(id: string, occurrenceIds: number[]): void {
    this.httpService
      .download(`${this.apiRoutes.Events}/${id}/occurrences/export?skipActivities=true`, {
        eventOccurrenceIds: occurrenceIds
      })
      .pipe(take(1))
      .subscribe((file: File) => {
        if (file) {
          const blob = new Blob([file]);
          const url = window.URL.createObjectURL(blob);
          const link = document.createElement('a');

          link.href = url;
          link.download = `Event ${id}.ics`;
          link.click();
        }
      });
  }

  changeAttendance(id: string, isAttending: boolean): void {
    this.httpService
      .put<{ isAttending: boolean }>(`${this.apiRoutes.Events}/${id}/attendees/current`, {
        isAttending: isAttending
      })
      .subscribe(() => {
        this.attendanceChange$.next({ eventId: id, isAttending });
      });
  }

  getEventTimezoneDate(dateString: string, timeZoneUtcOffset: number): Date {
    const fullOffset =
      dayjs(dateString).get('hours') * 60 +
      dayjs(dateString).get('minutes') -
      new Date().getTimezoneOffset() -
      Math.trunc(timeZoneUtcOffset) * 60 -
      EventService.convertFractionalToMinutes(timeZoneUtcOffset);

    const offsetHours = Math.trunc(fullOffset / 60);
    const offsetMinutes = fullOffset - offsetHours * 60;

    return dayjs(dateString).set('hours', offsetHours).set('minutes', offsetMinutes)?.toDate();
  }

  convertEvent(event: EventInterface, filterBefore: boolean = true): void {
    event.occurrences = event.occurrences.map(o => ({
      id: o.id,
      fromDate: o.fromDate,
      toDate: o.toDate,
      fromDateBrowser: dayjs(this.getEventTimezoneDate(o.fromDate, o.timeZoneUtcOffset)).format('YYYY-MM-DDTHH:mm:ss'),
      toDateBrowser: dayjs(this.getEventTimezoneDate(o.toDate, o.timeZoneUtcOffset)).format('YYYY-MM-DDTHH:mm:ss'),
      isToday: dayjs(o.fromDate).isSame(dayjs(), 'date'),
      timeZoneAbbr: o.timeZoneAbbr,
      timeZoneName: o.timeZoneName,
      timeZoneUtcOffset: o.timeZoneUtcOffset
    }));

    this.addShortLabels(event);

    if (filterBefore) {
      event.occurrences = event.occurrences.filter(oe => new Date(oe.fromDateBrowser) > new Date());
    }
  }

  addShortLabels(event: EventInterface, addCounts = true): void {
    const firstOccurrence: EventOccurrenceInterface = event?.occurrences[0];

    if (firstOccurrence) {
      event.shortDates = dayjs(firstOccurrence.fromDate).format('MMM D, YYYY');

      event.shortTimes = `${dayjs(firstOccurrence.fromDate).format(
        EventService.getTimeFormat(firstOccurrence.fromDate)?.toLowerCase()
      )}-${dayjs(firstOccurrence.toDate)
        .format(EventService.getTimeFormat(firstOccurrence.toDate))
        ?.toLowerCase()} ${firstOccurrence.timeZoneAbbr?.toUpperCase()}`;

      if (addCounts) {
        this.addEventLabelsCounts(event);
      }
    }
  }

  addEventLabelsCounts(event: EventInterface): void {
    let datesCount = 1;
    let uniqueOccurrenceTimes = [event.occurrences[0]];

    for (let i = 1; i < event.occurrences.length; i++) {
      if (
        dayjs(event.occurrences[i - 1].fromDate).format('YYYY-dd-MM') !==
        dayjs(event.occurrences[i].fromDate).format('YYYY-dd-MM')
      ) {
        datesCount++;
      }

      if (
        !uniqueOccurrenceTimes.some(
          uot => dayjs(uot.fromDate).format('HH:mm') === dayjs(event.occurrences[i].fromDate).format('HH:mm')
        )
      ) {
        uniqueOccurrenceTimes.push(event.occurrences[i]);
      }
    }

    event.moreDatesCount = datesCount - 1;
    event.moreOccurrencesCount = uniqueOccurrenceTimes.length - 1;
  }

  private static getTimeFormat(dateTime: string): string {
    return dateTime.split(':')[1] === '00' ? 'ha' : 'h:mma';
  }

  private static convertFractionalToMinutes(value: number): number {
    const fractionalOffsetNumber = Math.abs(value) - Math.floor(Math.abs(value));
    return fractionalOffsetNumber * 60 * (value > 0 ? 1 : -1);
  }
}
