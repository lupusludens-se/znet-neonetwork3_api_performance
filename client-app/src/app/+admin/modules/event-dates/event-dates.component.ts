import { ChangeDetectorRef, Component, ElementRef, HostListener, Input, OnInit, ViewChild } from '@angular/core';
import { ControlContainer, FormArray, FormBuilder, FormGroup, FormGroupDirective } from '@angular/forms';
import { filter, take } from 'rxjs';
import * as utc from 'dayjs/plugin/utc';
import * as dayjs from 'dayjs';
dayjs.extend(utc);

import { EventOccurrenceInterface } from 'src/app/shared/interfaces/event/event-occurrence.interface';
import { TimezoneInterface } from '../../../shared/interfaces/common/timezone.interface';
import { EventTimeInterface } from '../+create-event/interfaces/event-time.interface';
import { HttpService } from '../../../core/services/http.service';
import { createElement } from '@syncfusion/ej2-base';
import { CalendarComponent } from '@syncfusion/ej2-angular-calendars';
import { EventsService } from '../../services/events.service';

interface EventDateInterface {
  eventDate: string;
  eventTime: EventTimeInterface[];
}
interface ChangedEventArgs {
  element: unknown;
  event: PointerEvent;
  isInteracted: boolean;
  name: string;
  value: unknown;
  values: Date[];
}

@Component({
  selector: 'neo-event-dates',
  templateUrl: 'event-dates.component.html',
  styleUrls: ['../+create-event/create-event.component.scss', 'event-dates.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class EventDatesComponent implements OnInit {
  @Input() formSubmitted: boolean;
  @Input() selectedTimezoneId: number = 1;

  timezonesList: TimezoneInterface[];
  selectedTimezone: TimezoneInterface;
  datepickerDatesList: Date[] = []; // * dates inside calendar
  pastDates: Date[] = [];
  eventDates: EventDateInterface[] = [];
  minDate = new Date();
  showCalendar: boolean;
  clickOutside: boolean;

  calendarAllowedClasses = ['e-header-month-text', 'e-header-year-text', 'e-day', 'e-cell'];

  @ViewChild('calendarWrapper') calendarWrapper: ElementRef;
  @ViewChild('toggleCalendarButton') toggleCalendarButton: ElementRef;
  @ViewChild('ejsCalendar') ejsCalendar: CalendarComponent;

  constructor(
    private formBuilder: FormBuilder,
    private httpService: HttpService,
    public controlContainer: ControlContainer,
    private eventsService: EventsService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  get occurrences(): FormArray {
    return this.controlContainer.control.get('occurrences') as FormArray;
  }

  @HostListener('document:click', ['$event.target'])
  documentClick(elem: HTMLElement) {
    if (
      this.calendarWrapper &&
      !this.calendarWrapper.nativeElement.contains(elem) &&
      !this.calendarAllowedClasses.includes(elem.className)
    ) {
      this.showCalendar = false;
    }
  }

  ngOnInit(): void {
    this.httpService.get<TimezoneInterface[]>('timezones').subscribe(tz => {
      this.timezonesList = tz;

      if (this.eventsService.currentFormValue.value?.timeZoneId) {
        this.selectedTimezoneId = this.eventsService.currentFormValue.value?.timeZoneId;
      }

      this.setTimezone();
    });

    this.eventsService.currentFormValue$
      .pipe(
        filter(v => !!v?.occurrences),
        take(1)
      )
      .subscribe(formVal => {
        this.datepickerDatesList = [];
        this.eventDates = [];
        this.pastDates = [];

        if (formVal.timeZoneId) {
          this.selectedTimezoneId = formVal.timeZoneId;
          this.setTimezone();
        }

        let dateIndex = 0;

        formVal.occurrences.forEach(oc => {
          const from = dayjs(oc.fromDate);
          const to = dayjs(oc.toDate);

          const eventTime: EventTimeInterface = {
            start: {
              time: from.format('h:mm'),
              dayPart: from.hour() >= 12 ? 'pm' : 'am',
              // get minutes for time items comparison at create occurrences validation (create/edit event)
              minutes: from.get('hours') * 60 + from.get('minutes') - (from.hour() >= 12 ? 12 * 60 : 0)
            },
            end: {
              time: to.format('h:mm'),
              dayPart: to.hour() >= 12 ? 'pm' : 'am',
              // get minutes for am/pm comparison at create occurrences validation (create/edit event)
              minutes: to.get('hours') * 60 + to.get('minutes') - (to.hour() >= 12 ? 12 * 60 : 0)
            }
          };

          let timeIndex = 0;

          let repeatedDateIndex = this.datepickerDatesList.findIndex(
            el => dayjs(el).format('MM/DD/YYYY') === dayjs(oc.fromDate).format('MM/DD/YYYY')
          );

          if (repeatedDateIndex === -1) {
            repeatedDateIndex = this.pastDates.findIndex(
              el => dayjs(el).format('MM/DD/YYYY') === dayjs(oc.fromDate).format('MM/DD/YYYY')
            );
          }

          if (repeatedDateIndex > -1) {
            this.eventDates[repeatedDateIndex].eventTime.push(eventTime);
            timeIndex = this.eventDates[repeatedDateIndex].eventTime.length - 1;
          } else {
            if (dayjs(this.minDate).format('YYYY-MM-DD') > dayjs(oc.fromDate).format('YYYY-MM-DD')) {
              this.pastDates.push(new Date(oc.fromDate));
            } else {
              this.datepickerDatesList.push(new Date(oc.fromDate));
            }

            this.eventDates.push({
              eventDate: dayjs(oc.fromDate).format('YYYY-MM-DD'),
              eventTime: [eventTime]
            });
          }

          this.addOccurrenceToForm(oc, repeatedDateIndex > -1 ? repeatedDateIndex : dateIndex, timeIndex);

          if (repeatedDateIndex === -1) {
            dateIndex++;
          }
        });
      });
  }

  isEventTimeOutdated(date: string, time: EventTimeInterface): boolean {
    let dayObj = dayjs(date);

    if (time?.start?.time) {
      const startTimeSplit = time.start.time.split(':');
      const startDayPart = time.start.dayPart;
      const hours =
        startDayPart === 'am' ? +startTimeSplit[0] : +startTimeSplit[0] + 12 === 24 ? 0 : +startTimeSplit[0] + 12;

      dayObj = dayObj.set('hour', hours).set('minute', +startTimeSplit[1]);
    }

    return this.minDate.getTime() > dayObj.toDate().getTime();
  }

  afterNavigated(ev): void {
    if (ev?.view === 'Month') {
      this.createCalendarHeaderLabel();
    }
  }

  afterValueChanged(): void {
    setTimeout(() => this.createCalendarHeaderLabel());
  }

  afterCalendarCreated(): void {
    setTimeout(() => {
      this.createCalendarHeaderLabel();

      const contentElement = document.querySelector('.e-content.e-month');
      const closeButtonElement = createElement('button', {
        className: 'e-button-done',
        innerHTML: 'Done'
      });

      contentElement.append(closeButtonElement);
      closeButtonElement.addEventListener('click', () => this.toggleCalendarButton.nativeElement.click());
    });
  }

  createCalendarHeaderLabel(): void {
    setTimeout(() => {
      const headerElement = document.querySelector('.e-day.e-title');
      const monthTextElement = document.querySelector('.e-header-month-text');

      if (headerElement && !monthTextElement) {
        const headerTextSplit = headerElement.innerHTML.split(' ');
        headerElement.innerHTML = '';

        const monthNameElement = createElement('div', {
          className: 'e-header-month-text',
          innerHTML: headerTextSplit[0]
        });

        const yearNumberElement = createElement('div', {
          className: 'e-header-year-text',
          innerHTML: headerTextSplit[1]
        });

        headerElement.appendChild(monthNameElement);
        headerElement.appendChild(yearNumberElement);
      }
    });
  }

  toggleCalendarDropdown(): void {
    this.showCalendar = !this.showCalendar;
    this.cdr.detectChanges();
  }

  changeEventDate(dates: ChangedEventArgs): void {
    if (!dates.isInteracted) {
      return;
    }

    const shouldBeRemoved = this.eventDates.length > dates.values.length + this.pastDates.length;
    this.datepickerDatesList = [];
    this.occurrences.clear();

    if (dates.values?.length > 0) {
      dates.values.forEach(dateValue => this.datepickerDatesList.push(new Date(dateValue)));

      if (shouldBeRemoved) {
        const allExcludedDates: EventDateInterface[] = this.eventDates.filter(
          el => !dates.values.map(el => dayjs(el).format('YYYY-MM-DD')).includes(el.eventDate)
        );
        const newExcludedDates: EventDateInterface[] = allExcludedDates.filter(
          el => el.eventDate >= dayjs(this.minDate).format('YYYY-MM-DD')
        );

        this.eventDates = this.eventDates.filter(
          el => !newExcludedDates.map(el => el.eventDate).includes(el.eventDate)
        );
      } else {
        dates.values
          .filter(el => !this.eventDates.map(event => event.eventDate).includes(dayjs(el).format('YYYY-MM-DD')))
          .forEach(el => this.generateOccurrenceDate(el));
      }

      this.eventDates = this.eventDates.sort(
        (a: EventDateInterface, b: EventDateInterface) => dayjs(a.eventDate).valueOf() - dayjs(b.eventDate).valueOf()
      );

      this.eventDates
        .map((date: EventDateInterface) =>
          date.eventTime.map((time: EventTimeInterface) => {
            const startTime: string = this.formatTime(time.start);
            const endTime: string = this.formatTime(time.end);

            return {
              fromDate: `${dayjs(date.eventDate).format('YYYY-MM-DD')}T${startTime}:00`,
              toDate: `${dayjs(date.eventDate).format('YYYY-MM-DD')}T${endTime}:00`
            };
          })
        )
        .reduce((acc, currentValue) => acc.concat(currentValue), [])
        .forEach(el => this.occurrences.push(this.formBuilder.group({ ...el })));
    } else {
      this.eventDates.pop();
    }
  }

  removeDate(dateIndex: number, date: EventDateInterface): void {
    this.datepickerDatesList = this.datepickerDatesList?.filter(d => dayjs(d).format('YYYY-MM-DD') !== date.eventDate);

    if (this.datepickerDatesList?.length < 1) {
      this.datepickerDatesList = [];
    }

    const lastOccurrenceIndexToRemove = this.getOccurrenceIndex(dateIndex);

    if (lastOccurrenceIndexToRemove > -1) {
      for (
        let i = lastOccurrenceIndexToRemove;
        i > lastOccurrenceIndexToRemove - this.eventDates[dateIndex].eventTime.length;
        i--
      ) {
        this.occurrences.removeAt(i);
      }
    }

    this.eventDates = this.eventDates.filter((eventDate, index) => {
      if (index !== dateIndex) return eventDate;
    });
  }

  addTimeToDate(dateIndex: number) {
    const timeRange = {
      start: {
        time: '12:00',
        dayPart: 'am',
        minutes: 0
      },
      end: {
        time: '1:00',
        dayPart: 'am',
        minutes: 60
      }
    };

    this.eventDates[dateIndex].eventTime.push(timeRange);

    const newDate = dayjs(this.eventDates[dateIndex].eventDate).toDate();

    const newOccurrence: FormGroup = this.formBuilder.group({
      fromDate: `${dayjs(newDate).format('YYYY-MM-DD')}T00:00:00`,
      toDate: `${dayjs(newDate).format('YYYY-MM-DD')}T01:00:00`
    });

    const occurrenceIndex = this.getOccurrenceIndex(dateIndex);

    if (occurrenceIndex > -1) {
      this.occurrences.insert(occurrenceIndex, newOccurrence);
    }
  }

  removeTimeFromDate(dateIndex: number, timeIndex: number): void {
    const occurrenceIndex = this.getOccurrenceIndex(dateIndex, timeIndex);
    this.eventDates[dateIndex].eventTime.splice(timeIndex, 1);

    if (occurrenceIndex > -1) {
      this.occurrences.removeAt(occurrenceIndex);
    }
  }

  updateTime(newTime: EventTimeInterface, dateIndex: number, timeIndex: number): void {
    this.eventDates[dateIndex].eventTime[timeIndex] = newTime;

    const startTime: string = this.formatTime(newTime.start);
    const endTime: string = this.formatTime(newTime.end);

    const occurrenceIndex = this.getOccurrenceIndex(dateIndex, timeIndex);

    if (occurrenceIndex > -1) {
      const updatedOccurrenceTime = {
        fromDate: `${this.occurrences
          .at(occurrenceIndex)
          .value.fromDate.slice(0, this.occurrences.at(occurrenceIndex).value.fromDate.indexOf('T'))}T${startTime}:00`,
        toDate: `${this.occurrences
          .at(occurrenceIndex)
          .value.toDate.slice(0, this.occurrences.at(occurrenceIndex).value.toDate.indexOf('T'))}T${endTime}:00`
      };

      this.occurrences.at(occurrenceIndex).patchValue(updatedOccurrenceTime);
    }
  }

  changeOccurrenceTimezone(timezoneId: number): void {
    this.controlContainer.control.get('timeZoneId').patchValue(timezoneId);
  }

  private addOccurrenceToForm(occurrence: EventOccurrenceInterface, dateIndex: number, timeIndex: number): void {
    const occurrenceFormItem: FormGroup = this.formBuilder.group({
      fromDate: occurrence.fromDate,
      toDate: occurrence.toDate
    });

    const occurrenceIndex = this.getOccurrenceIndex(dateIndex, timeIndex);

    if (occurrenceIndex > -1) {
      this.occurrences.push(occurrenceFormItem);
    }
  }

  private generateOccurrenceDate(date: Date): void {
    const eventOccurrence: EventDateInterface = {
      eventDate: dayjs(date).format('YYYY-MM-DD'),
      eventTime: [
        {
          start: {
            time: '12:00',
            dayPart: 'am',
            minutes: 0
          },
          end: {
            time: '1:00',
            dayPart: 'am',
            minutes: 60
          }
        }
      ]
    };

    if (this.eventDates.some(ev => ev.eventDate === eventOccurrence.eventDate)) {
      return;
    }

    this.eventDates.push(eventOccurrence);
  }

  private getOccurrenceIndex(dateIndex: number, timeIndex?: number): number {
    let occurrenceIndex: number = -1;

    if (!this.eventDates[dateIndex].eventTime.length) {
      return occurrenceIndex;
    }

    for (let i = 0; i < dateIndex; i++) {
      occurrenceIndex += this.eventDates[i].eventTime.length;
    }

    occurrenceIndex += (timeIndex !== undefined ? timeIndex : this.eventDates[dateIndex].eventTime.length - 1) + 1;

    return occurrenceIndex;
  }

  private setTimezone(): void {
    this.selectedTimezone = this.timezonesList?.filter(tz => tz.id === this.selectedTimezoneId)[0];

    if (this.controlContainer.control.get('timeZoneId').value != this.selectedTimezoneId) {
      this.changeOccurrenceTimezone(this.selectedTimezoneId);
    }
  }

  private formatTime(dayTime: { time: string; dayPart: string }): string {
    const hoursNumber: number =
      dayTime.dayPart === 'am'
        ? +dayTime.time.slice(0, dayTime.time.indexOf(':'))
        : +dayTime.time.slice(0, dayTime.time.indexOf(':')) + 12;
    const minutesNumber = dayTime.time.split(':')[1];
    const formattedHours =
      hoursNumber < 10 ? `0${hoursNumber}` : hoursNumber % 12 === 0 ? `${hoursNumber - 12 || '00'}` : `${hoursNumber}`;

    return `${formattedHours}:${minutesNumber}`;
  }
}
