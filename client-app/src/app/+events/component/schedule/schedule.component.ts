/* eslint-disable */
import { Component, EventEmitter, HostListener, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { createElement } from '@syncfusion/ej2-base';
import {
  ActionEventArgs,
  EventSettingsModel,
  PopupOpenEventArgs,
  RenderCellEventArgs
} from '@syncfusion/ej2-angular-schedule';

import { nextIcon, prevIcon } from '../../../shared/svg-icons/events.icons';
import { Router } from '@angular/router';
import * as dayjs from 'dayjs';
import { map, Observable, Subject, switchMap, takeUntil } from 'rxjs';
import { EventService } from '../../services/event.service';
import { EventInterface } from 'src/app/shared/interfaces/event/event.interface';
import { EventOccurrenceInterface } from 'src/app/shared/interfaces/event/event-occurrence.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { PermissionService } from 'src/app/core/services/permission.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { EventNavigatingInterface } from '../../interfaces/event-navigating.interface';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { DatePipe } from '@angular/common';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { BULLET_SYMBOL } from '../../../shared/constants/symbols.const';
import { MAX_HIGHLIGHTS_LENGTH, MIN_HIGHLIGHT_ITEM_LENGTH } from '../../../shared/constants/maxlength.const';
import { EventCalendarRequestInterface } from '../../../shared/interfaces/event/event-calendar-request-interface.interface';

@Component({
  selector: 'neo-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
  providers: [DatePipe]
})
export class SchedulerComponent implements OnInit, OnDestroy {
  @Output() onEventsLoaded: EventEmitter<number> = new EventEmitter();

  calendarEvents: EventInterface[];
  today: Date = new Date(new Date().setHours(0, 0, 0, 0));
  currentDate: Date = new Date();
  fromDate: Date;
  toDate: Date;
  weekFirstDay: number = 1;
  eventSettingsObject: EventSettingsModel = {
    allowAdding: false,
    allowEditing: false,
    allowDeleting: false
  };
  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  readonly userStatuses = UserStatusEnum;

  isEventActual: boolean;
  isFirefoxBrowser = navigator?.userAgent?.includes('Firefox');

  private requestData: EventCalendarRequestInterface;
  private flag = true;

  private loadData$: Subject<void> = new Subject<void>();
  private unsubscribe$: Subject<void> = new Subject<void>();

  @HostListener('window:resize', ['$event'])
  onResize(): void {
    setTimeout(() => {
      this.onDataBound();
    });
  }

  @ViewChild('schedule') scheduleObj;

  constructor(
    private readonly router: Router,
    private readonly eventService: EventService,
    private readonly authService: AuthService,
    private readonly permissionService: PermissionService,
    private readonly sanitizer: DomSanitizer,
    private readonly datePipe: DatePipe
  ) {}

  private static replaceIcon(iconElementClassName: string, outerHTML: string): void {
    let iconToReplace = document.getElementsByClassName(iconElementClassName)[0];

    if (iconToReplace) {
      iconToReplace.outerHTML = outerHTML;
    }
  }

  ngOnInit(): void {
    this.listenToLoadData();
    this.listenToAttendanceChange();
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    this.loadData$.next();
    this.loadData$.complete();
  }

  onActionBegin(args: ActionEventArgs): void {
    if (args.requestType === 'toolbarItemRendering') {
      const arg = args as any;
      const leftButton = arg.items[0];
      const rightButton = arg.items[1];
      rightButton.align = 'right';
      // Getting date range text
      let originDateRangeText = arg.items[2];
      const dateRangeText = {
        align: 'center',
        text: originDateRangeText.text,
        cssClass: 'e-custom'
      };
      // Rearranging the toolbar items
      arg.items[0] = leftButton;
      arg.items[1] = dateRangeText;
      arg.items[2] = rightButton;
    }
  }

  onDataBinding(): void {
    SchedulerComponent.replaceIcon('e-icon-prev', prevIcon.data);
    SchedulerComponent.replaceIcon('e-icon-next', nextIcon.data);
    const appointments = document.getElementsByClassName('e-appointment');

    for (let i = 0; i < appointments.length; i++) {
      let workCell = appointments.item(i).parentNode.parentNode as HTMLElement;
      if (!workCell.classList.contains('has-appointment')) {
        workCell.classList.add('has-appointment');
      }
    }

    if (this.flag) {
      this.updateDateRangeText();
      this.flag = false;
    }
  }

  onDataBound(): void {
    this.rebuildSingleOccurrence();
    this.replaceMoreIndicators();
    this.setAttendingEventsLabel();
    this.restyleTimeLabels();
  }

  onMoreEventsClick(ev): void {
    setTimeout(() => {
      this.setAttendingEventsLabel();
      this.rebuildMoreDialogHeader(ev?.startTime);
      this.addStartTime();
    });
  }

  rebuildSingleOccurrence(): void {
    const cellsElements = document.querySelectorAll('.e-appointment-wrapper') || [];
    const scheduleEvents = this.scheduleObj.getEvents();

    cellsElements.forEach(el => {
      if (el.querySelectorAll('.e-appointment')?.length === 1) {
        const appointment = el.querySelector('.e-appointment');

        const calendarEventIndex = scheduleEvents.findIndex(e => e.Guid.toString() === appointment?.dataset?.guid);

        if (calendarEventIndex >= 0) {
          appointment.classList += ' e-single-event';
          appointment.innerHTML = '';

          const singleEventHeaderElement = createElement('div', {
            className: 'e-single-event-header',
            innerHTML: scheduleEvents[calendarEventIndex].ShortTimes?.split(' ')[0]
          });

          const singleEventSubjectElement = createElement('div', {
            className: 'e-single-event-subject',
            innerHTML: scheduleEvents[calendarEventIndex].Subject
          });

          appointment.appendChild(singleEventHeaderElement);
          appointment.appendChild(singleEventSubjectElement);
        }
      }
    });
  }

  replaceMoreIndicators(): void {
    const moreLabelElements = document.querySelectorAll('.e-more-indicator');

    if (moreLabelElements?.length) {
      for (let i = 0; i < moreLabelElements.length; i++) {
        if (!moreLabelElements[i].innerHTML.includes('Events')) {
          moreLabelElements[i].innerHTML += ' Events';
        }
      }
    }
  }

  appendLastEvent(eventsWrapper: Element): void {
    const lastEventElementCopy = eventsWrapper.lastChild.cloneNode(true) as HTMLElement;
    const dayNumber = eventsWrapper.parentElement?.getElementsByClassName('e-date-header')?.[0]?.innerHTML;
    const presentedEventsIds = Array.from(eventsWrapper.querySelectorAll('.e-appointment')).map(
      el => +el.getAttribute('data-id')?.replace(/\D/g, '')
    );

    const currentMonthDate: Date = this.scheduleObj.activeView?.monthDates?.start;
    const currentDate = new Date(currentMonthDate.setDate(+dayNumber));
    const eventsByDate = this.calendarEvents.filter(
      el => dayjs(el.occurrences[0]?.fromDate).format('YYYY-MM-DD') === dayjs(currentDate).format('YYYY-MM-DD')
    );
    let lastEvent = eventsByDate.find(el => !presentedEventsIds.includes(el.id));

    if (!lastEvent && eventsByDate.length === 3) {
      lastEvent = eventsByDate.sort((ebd1: EventInterface, ebd2: EventInterface) =>
        ebd1.occurrences[0].fromDate > ebd2.occurrences[0].fromDate ? 1 : -1
      )?.[3];
    }

    if (lastEvent) {
      const timeElement = lastEventElementCopy.querySelector('.e-time');
      const subjectElement = lastEventElementCopy.querySelector('.e-subject');

      if (timeElement && subjectElement) {
        timeElement.innerHTML = lastEvent.shortTimes.split('-')[0]?.slice(0, -1);
        subjectElement.innerHTML = lastEvent.subject;

        const separatorElement = createElement('div', {
          className: 'e-time-separator'
        });

        timeElement.append(separatorElement);

        const record = this.scheduleObj
          .getEvents()
          .find(
            el =>
              el.Id === lastEvent.id &&
              this.equalDates(dayjs(el.StartTime).toDate(), dayjs(lastEvent.occurrences[0]?.fromDate).toDate()) &&
              dayjs(el.StartTime).format('hh:mm') === dayjs(lastEvent.occurrences[0]?.fromDate).format('hh:mm')
          );

        if (record) {
          record.Id += '_appended';

          lastEventElementCopy.dataset.id = 'Appointment_' + record.EventId;
          lastEventElementCopy.dataset.guid = record.Guid;

          setTimeout(() => {
            eventsWrapper.appendChild(lastEventElementCopy);

            lastEventElementCopy.addEventListener('click', () => {
              if (this.scheduleObj?.openQuickInfoPopup) {
                this.scheduleObj.openQuickInfoPopup(record);
              }
            });
          });
        }
      }
    }
  }

  setAttendingEventsLabel(): void {
    const appointments = document.getElementsByClassName('e-appointment');

    for (let i = 0; i < appointments.length; i++) {
      let appointment = appointments.item(i);

      const calendarEventIndex = this.calendarEvents.findIndex(
        e => e.id.toString() === appointment.getAttribute('data-id').replace(/\D/g, '')
      );

      if (calendarEventIndex >= 0) {
        const subjectElement = appointment.querySelector('.e-subject');

        if (subjectElement) {
          subjectElement.innerHTML = this.calendarEvents[calendarEventIndex].subject;
        }

        if (this.calendarEvents[calendarEventIndex].isAttending) {
          appointment.classList.add('attend');
        }
      }

      let workCell = this.getParentWorkCell(appointment as HTMLElement);

      if (workCell && !workCell.classList?.contains('has-appointment')) {
        workCell.classList.add('has-appointment');

        if (workCell.classList.contains('e-disable-cell')) {
          appointment.classList.add('past');
        }
      }
    }
  }

  addStartTime(): void {
    const moreEventsPopupElement: Element = document.querySelector('.e-more-popup-wrapper');

    if (moreEventsPopupElement) {
      const appointments = moreEventsPopupElement.getElementsByClassName('e-appointment');

      for (let i = 0; i < appointments.length; i++) {
        let appointment = appointments.item(i);
        const timeSplits = appointment.getAttribute('aria-label')?.split('at ')[1]?.split(':');

        if (timeSplits) {
          const timeElement = createElement('div', {
            innerHTML:
              timeSplits[0] +
              (timeSplits[1] !== '00' ? `:${timeSplits[1]}` : '') +
              timeSplits[2]?.split(' ')[1]?.slice(0, 1).toLowerCase(),
            className: 'e-time'
          });

          const separatorElement = createElement('div', {
            className: 'e-time-separator'
          });

          appointment.prepend(separatorElement);
          appointment.prepend(timeElement);
        }
      }
    }
  }

  rebuildMoreDialogHeader(date: Date): void {
    const moreDialogHeaderElement = document.querySelector('.e-more-popup-wrapper .e-more-event-date-header');
    const formattedDate = this.datePipe.transform(date, 'MMM d, EEEE');

    if (moreDialogHeaderElement) {
      const moreDateLabel = document.querySelector('.e-more-popup-wrapper .e-more-date-label');

      if (moreDialogHeaderElement.contains(moreDateLabel)) {
        const todayLabel = moreDialogHeaderElement.querySelector('.e-more-popup-wrapper .e-today-label');
        const dateLabel = moreDialogHeaderElement.querySelector('.e-more-popup-wrapper .e-header-date');

        moreDateLabel.innerHTML = '';

        if (!todayLabel && this.equalDates(date, this.today)) {
          moreDialogHeaderElement.appendChild(this.getTodayLabelElement());
          dateLabel.innerHTML = '';
        } else if (moreDialogHeaderElement.contains(todayLabel) && !this.equalDates(date, this.today)) {
          moreDialogHeaderElement.removeChild(todayLabel);
        }

        moreDateLabel.innerHTML = formattedDate;
      } else {
        const headerLabelElement = createElement('div', {
          className: 'e-more-date-label',
          innerHTML: formattedDate
        });

        moreDialogHeaderElement.prepend(headerLabelElement);

        if (this.equalDates(date, this.today)) {
          moreDialogHeaderElement.appendChild(this.getTodayLabelElement());
        }
      }
    }
  }

  restyleTimeLabels(): void {
    const timeElements = document.querySelectorAll('.e-time');

    if (timeElements?.length) {
      timeElements.forEach(el => {
        const innerTextSplit = el.innerHTML.split(':') || [];

        if (innerTextSplit[1]) {
          innerTextSplit[1] = innerTextSplit[1].startsWith('00') ? innerTextSplit[1].slice(2) : ':' + innerTextSplit[1];
        }

        el.innerHTML = innerTextSplit.join('').replace(' ', '').toLowerCase().slice(0, -1);

        const separatorElement = createElement('div', {
          className: 'e-time-separator'
        });

        el.append(separatorElement);
      });
    }
  }

  onCurrentViewChange(ev: string): void {
    if (ev === 'Month') {
      this.loadDataForCurrentRange();
    }
  }

  onActionComplete(args: ActionEventArgs): void {
    if (args.requestType === 'dateNavigate') {
      this.updateDateRangeText();
    }
  }

  onRenderCell(args: RenderCellEventArgs): void {
    if (args.elementType === 'monthCells') {
      if (args.date < this.today && !args.element.classList.contains('e-disable-dates')) {
        args.element.classList.add('e-disable-dates');
        args.element.classList.add('e-disable-cell');
      }

      if (dayjs(args.date).isSame(this.today, 'date')) {
        const todayNumberElement = createElement('div', {
          innerHTML: `${this.today.getDate()}`,
          className: 'e-today-number-custom'
        });

        const flexDiv = createElement('div', {          
          className: 'd-flex fw-100'
        });
        flexDiv.appendChild(this.getTodayLabelElement());
        flexDiv.appendChild(todayNumberElement);
        //
        args.element.children[0].innerHTML = '';
        args.element.children[0].appendChild(flexDiv);
        //args.element.children[0].appendChild(this.getTodayLabelElement());
        //args.element.children[0].appendChild(todayNumberElement);
      }
    }
  }

  onAppointmentClick(): void {
    const popupInstance = (document.querySelector('.e-quick-popup-wrapper') as any).ej2_instances[0];
    popupInstance.open = () => {
      popupInstance.refreshPosition();
    };
  }

  onPopupOpen(args: PopupOpenEventArgs): void {
    if (args?.type !== 'QuickInfo') {
      return;
    }

    this.isEventActual = this.calendarEvents
      .filter(ce => ce.id === args.data?.Id)
      .some(fce => dayjs(fce.occurrences[0].toDateBrowser).toDate() > new Date());

    if (!args.data?.Attenders?.length) {
      const footer = args.element.querySelector('.e-popup-footer') as HTMLElement;

      if (footer) {
        footer.classList?.add('hidden');
      }
    }

    const popupHeaderDateElement = args.element.querySelector('.e-header-date');
    const popupCurrentDateElement = args.element.querySelector('.e-header-date.e-current-date');

    if (popupHeaderDateElement) {
      popupHeaderDateElement.innerHTML +=
        ', ' + ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'][args.data.date.getDay()];
    }

    if (popupCurrentDateElement) {
      popupCurrentDateElement.append(this.getTodayLabelElement());
    }

    const subjectElements = args.element.querySelectorAll('.e-appointment');

    if (subjectElements?.length) {
      subjectElements.forEach((el: Element, index: number) => {
        el.innerHTML = '';

        const currentOccurrence = args.data.event[index];
        const timeElement = createElement('div', {
          innerHTML: currentOccurrence.ShortTimes.split('-')[0]?.slice(0, -1),
          className: 'e-time'
        });
        const timeSeparatorElement = createElement('div', { className: 'e-time-separator' });
        const subjectElement = createElement('div', {
          innerHTML: currentOccurrence.Subject,
          className: 'e-subject'
        });

        el.append(timeElement);
        el.append(timeSeparatorElement);
        el.append(subjectElement);
      });
    }
  }

  onPopupClose(ev): void {
    if (ev?.target) {
      setTimeout(() => ev.target.blur());
    }
  }

  onCloseClick(): void {
    this.scheduleObj.closeQuickInfoPopup();
  }

  getAppointmentTime(date: Date): string {
    let hours: number = date.getHours();
    const ap: string = hours >= 12 ? 'p' : 'a';

    hours = hours % 12;
    hours = hours ? hours : 12;

    return `${hours}${ap}`;
  }

  openDetails(eventId: number): void {
    this.router.navigate([`events/${eventId}`]);
  }

  openUserProfile(userId: string): void {
    if (userId) {
      this.router.navigate([`user-profile/${userId}`]);
    }
  }

  addToCalendar(eventId: string): void {
    const actualEventOccurrencesIds = this.calendarEvents
      .filter(ev => ev.id === +eventId)
      .map(ev => ev.occurrences.filter(occ => new Date(occ.toDateBrowser) > new Date()).map(o => o.id))
      .reduce((a, b) => a.concat(b), []);

    this.eventService.addEventToCalendar(eventId, actualEventOccurrencesIds);
  }

  changeAttendance(eventId: string, isAttending: boolean): void {
    this.eventService.changeAttendance(eventId, isAttending);
  }

  hasPermission(currentUser: UserInterface): boolean {
    return this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.EventManagement);
  }

  onNavigating(ev: EventNavigatingInterface): void {
    if (ev?.action === 'date') {
      this.currentDate = ev.currentDate;
      this.loadDataForCurrentRange();
    }
  }

  getHighlightsArray(content: string = ''): string[] {
    let highlightsArray = content.split(`\n${BULLET_SYMBOL}`);

    // line with bullet is not cleared by split method
    if (highlightsArray[0]) {
      highlightsArray[0] = highlightsArray[0].slice(1);
    }

    if (this.isFirefoxBrowser) {
      const highlightsWholeLength = highlightsArray.join('').length;

      if (highlightsWholeLength > MAX_HIGHLIGHTS_LENGTH) {
        highlightsArray = this.reduceHighlightsArray(highlightsArray, highlightsWholeLength - MAX_HIGHLIGHTS_LENGTH);

        if (highlightsArray[highlightsArray.length - 1].length < MIN_HIGHLIGHT_ITEM_LENGTH) {
          highlightsArray.pop();
        }

        highlightsArray[highlightsArray.length - 1] = highlightsArray[highlightsArray.length - 1]?.trimEnd() + '...';
      }
    }

    return highlightsArray;
  }

  getHighlightsHTML(highlightsString: string = ''): SafeHtml {
    return this.sanitizer.bypassSecurityTrustHtml(
      this.getHighlightsArray(highlightsString)
        ?.map(el => `<p class="highlight-item pl-12">${el}</p>`)
        .join('')
    );
  }

  private getParentWorkCell(element: HTMLElement): HTMLElement {
    if (element) {
      if (element.classList?.contains('e-work-cells')) {
        return element;
      }

      return this.getParentWorkCell(element.parentNode as HTMLElement);
    }
  }

  private updateDateRangeText(): void {
    const monthNames = [
      'January',
      'February',
      'March',
      'April',
      'May',
      'June',
      'July',
      'August',
      'September',
      'October',
      'November',
      'December'
    ];
    const currentMonthDate = this.scheduleObj.getCurrentViewDates()[10] as Date;
    (document.querySelector('.e-toolbar-item.e-custom') as HTMLElement).innerText = `${
      monthNames[currentMonthDate.getMonth()]
    } ${currentMonthDate.getFullYear()}`;
  }

  private listenToLoadData(): void {
    this.loadData$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => this.eventService.getCalendarEvents(this.requestData)),
        map(events => {
          events.dataList?.forEach(event => {
            this.eventService.convertEvent(event, false);
          });

          return events.dataList;
        })
      )
      .subscribe((events: EventInterface[]) => {
        this.calendarEvents = [];

        events?.forEach((event: EventInterface) => {
          event.occurrences.forEach((eventOccurrence: EventOccurrenceInterface) => {
            let calendarEvent = { ...event };
            this.eventService.addEventLabelsCounts(calendarEvent);
            calendarEvent.occurrences = [eventOccurrence];
            this.eventService.addShortLabels(calendarEvent, false);
            this.calendarEvents.push(calendarEvent);
          });
        });

        this.scheduleObj.eventSettings.dataSource = this.calendarEvents.map((e: EventInterface) => ({
          // should be Pascal case here, duplicate id to allow open events occurrences for different dates
          Id: e.id,
          EventId: e.id,
          Subject: e.subject,
          Location: e.locationType === 0 ? 'Virtual' : e.location,
          Description: e.description,
          IsAttending: e.isAttending,
          StartTime: new Date(e.occurrences[0].fromDate),
          EndTime: this.calculateOccurrenceEndTime(e.occurrences[0]),
          Occurrences: e.occurrences,
          Moderators: e.moderators.slice(0, 3),
          Attenders: e.attendees,
          ShortDates: e.shortDates,
          ShortTimes: e.shortTimes,
          MoreDatesCount: e.moreDatesCount,
          MoreOccurrencesCount: e.moreOccurrencesCount,
          Highlights: e.highlights
        }));

        this.onEventsLoaded.emit(this.calendarEvents?.length);
        this.scheduleObj.refresh();
      });
  }

  private calculateOccurrenceEndTime(occurrence: EventOccurrenceInterface): Date {
    if (dayjs(occurrence.fromDate).format('YYYY-MM-DD') !== dayjs(occurrence.toDate).format('YYYY-MM-DD')) {
      return new Date(occurrence.fromDate.split('T')[0] + 'T23:59:59');
    }

    return new Date(occurrence.toDate);
  }

  private listenToAttendanceChange(): void {
    this.eventService.attendanceChange$.pipe(takeUntil(this.unsubscribe$)).subscribe(() => {
      this.loadData$.next();
    });
  }

  private loadDataForCurrentRange(): void {
    setTimeout(() => {
      const datesOfCalendarView = Array.from(document.querySelectorAll('.e-date-header.e-navigate')).map(
        el => +el.innerHTML.replace(/\D/g, '')
      );

      this.fromDate = new Date(this.currentDate.setDate(1));
      this.toDate = new Date(this.fromDate.getFullYear(), this.fromDate.getMonth() + 1, 0);

      if (datesOfCalendarView?.length) {
        if (datesOfCalendarView[0] > 1) {
          this.fromDate = new Date(
            this.currentDate.getFullYear(),
            this.currentDate.getMonth() - 1,
            datesOfCalendarView[0]
          );
        }

        if (datesOfCalendarView[datesOfCalendarView.length - 1] < 27) {
          this.toDate = new Date(
            this.currentDate.getFullYear(),
            this.currentDate.getMonth() + 1,
            datesOfCalendarView[datesOfCalendarView.length - 1]
          );
        }
      }

      this.requestData = {
        from: `${dayjs(this.fromDate).format('YYYY-MM-DD')}T00:00:00Z`,
        to: `${dayjs(this.toDate).format('YYYY-MM-DD')}T00:00:00Z`
      };

      this.loadData$.next();
    });
  }

  private reduceHighlightsArray(highlightsArray: string[], maxLength: number): string[] {
    if (highlightsArray[highlightsArray.length - 1]) {
      if (maxLength >= highlightsArray[highlightsArray.length - 1].length) {
        maxLength -= highlightsArray[highlightsArray.length - 1].length;
        highlightsArray.pop();
        this.reduceHighlightsArray(highlightsArray, maxLength);
      } else {
        const diff = maxLength - highlightsArray[highlightsArray.length - 1].length;
        highlightsArray[highlightsArray.length - 1] = highlightsArray[highlightsArray.length - 1].slice(0, -diff);
      }
    }

    return highlightsArray;
  }

  private getTodayLabelElement(): HTMLElement {
    return createElement('div', {
      className: 'e-today-label',
      innerHTML: 'Today'
    });
  }

  private equalDates(d1: Date, d2: Date): boolean {
    return dayjs(d1.toISOString()).format('YYYY-MM-DD') === dayjs(d2.toISOString()).format('YYYY-MM-DD');
  }
}
