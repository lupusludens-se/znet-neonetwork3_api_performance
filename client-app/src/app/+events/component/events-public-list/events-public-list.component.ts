import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, map, switchMap, takeUntil } from 'rxjs';
import { EventService } from '../../services/event.service';
import { EventListRequestInterface } from '../events-list/events-list.component';
import * as dayjs from 'dayjs';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { EventInterface } from 'src/app/shared/interfaces/event/event.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { CoreService } from 'src/app/core/services/core.service';
import { Router } from '@angular/router';

@Component({
  selector: 'neo-events-public-list',
  templateUrl: './events-public-list.component.html',
  styleUrls: ['./events-public-list.component.scss']
})
export class EventsPublicListComponent implements OnInit, OnDestroy {
  currentUser: UserInterface = null;
  defaultItemPerPage = 24;
  paging: PaginationInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null
  };
  requestData: EventListRequestInterface = {
    from: dayjs().toISOString(),
    search: '',
    skip: 0,
    take: 9,
    total: 0,
    includeCount: true
  };
  upcomingEventsList: PaginateResponseInterface<EventInterface>;
  pastEventsList: EventInterface[] = [];
  private unsubscribe$: Subject<void> = new Subject<void>();
  private loadUpcomingEvents$: Subject<void> = new Subject<void>();
  private loadPastEvents$: Subject<void> = new Subject<void>();
  constructor(private eventService: EventService, private coreService: CoreService, private router: Router) {}
  ngOnInit(): void {
    this.loadUpcomingEvents();
    this.loadPastEvents();

    this.loadUpcomingEvents$.next();
    this.loadPastEvents$.next();
  }

  loadUpcomingEvents() {
    this.loadUpcomingEvents$
      .pipe(
        takeUntil(this.unsubscribe$),
        map(() => {
          let requestData = { ...this.requestData };
          delete requestData['skip'];
          delete requestData['take'];
          return requestData;
        }),
        switchMap(data => this.eventService.getListEvents(data)),
        map(events => {
          if (events?.dataList) {
            events.dataList.forEach(event => {
              this.eventService.convertEvent(event);
            });
          }
          return events;
        })
      )
      .subscribe(val => {
        this.upcomingEventsList = val;
      });
  }

  loadPastEvents() {
    this.loadPastEvents$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => this.eventService.getPastEvents(this.requestData)),
        map(events => {
          if (events?.dataList) {
            events.dataList.forEach(event => {
              this.eventService.convertEvent(event);
            });
          }
          return events;
        })
      )
      .subscribe(val => {
        this.paging.total = val.count;
        let dataList = val.dataList;
        this.pastEventsList.push(...dataList);
      });
  }
  onLoadMoreData(page: number) {
    this.defaultItemPerPage = 9;
    this.paging.skip = page * this.defaultItemPerPage;
    var isLastPage = this.isLastPage(page, Math.floor(this.paging.total / this.defaultItemPerPage));
    if (!isLastPage) this.loadMorePastEvents();
  }

  loadMorePastEvents() {
    this.requestData.includeCount = true;
    this.requestData.search = null;
    this.requestData.skip = this.paging.skip;
    this.requestData.take = this.defaultItemPerPage;
    this.loadPastEvents$.next();
  }

  private isLastPage(page: number, total: number): boolean {
    return page > total ? true : false;
  }

  openDetails(eventId: string): void {
    if (eventId) {
      this.router.navigate([`events/${eventId}`]);
    }
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this.loadUpcomingEvents$.next();
    this.loadUpcomingEvents$.complete();
    this.loadPastEvents$.next();
    this.loadPastEvents$.complete();
  }
}
