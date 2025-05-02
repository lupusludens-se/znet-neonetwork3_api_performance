import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import * as dayjs from 'dayjs';
import { map, Observable, Subject, switchMap, takeUntil } from 'rxjs';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { EventInterface } from 'src/app/shared/interfaces/event/event.interface';
import { EventService } from '../../services/event.service';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { AuthService } from '../../../core/services/auth.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CoreService } from 'src/app/core/services/core.service';

export interface EventAttendanceInterface {
  eventId: string;
  isAttending?: boolean;
}

export interface EventListRequestInterface {
  from: string;
  search: string;
  skip: number;
  take: number;
  total: number;
  includeCount: boolean;
}

@UntilDestroy()
@Component({
  selector: 'neo-events-list',
  templateUrl: './events-list.component.html',
  styleUrls: ['./events-list.component.scss']
})
export class EventsListComponent implements OnChanges, OnInit, OnDestroy {
  @Input() searchInput: string;
  @Output() eventsLoad: EventEmitter<number> = new EventEmitter<number>();
  eventsData: PaginateResponseInterface<EventInterface>;
  defaultItemPerPage = 18;
  requestData: EventListRequestInterface = {
    from: dayjs().toISOString(),
    search: '',
    skip: 0,
    take: this.defaultItemPerPage,
    total: 0,
    includeCount: true
  };
  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  private loadData$: Subject<void> = new Subject<void>();
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly eventService: EventService,
    private readonly router: Router,
    private readonly authService: AuthService,
    private readonly coreService: CoreService
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.searchInput.currentValue !== changes.searchInput.previousValue) {
      this.requestData.search = changes.searchInput.currentValue;
      this.requestData.skip = 0;
      this.loadData$.next();
    }
  }

  ngOnInit(): void {
    this.authService.currentUser$.pipe(untilDestroyed(this)).subscribe((user: UserInterface) => {
      if (user?.id) {
        this.listenToAttendanceChange();
        this.listenToLoadData();
        this.loadData$.next();
      }
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    this.loadData$.next();
    this.loadData$.complete();
  }

  changeAttendance(eventId: string, isAttending: boolean): void {
    this.eventService.changeAttendance(eventId, isAttending);
  }

  openDetails(eventId: string): void {
    if (eventId) {
      this.router.navigate([`events/${eventId}`]);
    }
  }

  changePage(page: number): void {
    this.requestData.skip = (page - 1) * this.defaultItemPerPage;
    this.loadData$.next();
  }

  private listenToLoadData(): void {
    this.loadData$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => this.eventService.getListEvents(this.requestData)),
        map(events => {
          events.dataList.forEach(event => {
            this.eventService.convertEvent(event);
          });

          events.dataList = events.dataList.filter(ev =>
            ev.occurrences.some(occ => new Date(occ.fromDateBrowser) > new Date())
          );

          this.eventsLoad.emit(events.dataList.length);
          return events;
        })
      )
      .subscribe(events => {
        this.eventsData = events;
        this.requestData.total = events.count;
      });
  }

  private listenToAttendanceChange(): void {
    this.eventService.attendanceChange$.pipe(takeUntil(this.unsubscribe$)).subscribe(event => {
      this.eventsData.dataList.forEach(ev => {
        if (ev.id === +event.eventId) ev.isAttending = event.isAttending;
      });
    });
  }
}
