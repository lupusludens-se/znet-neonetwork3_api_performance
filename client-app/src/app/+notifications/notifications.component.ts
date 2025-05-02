import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';

import { TitleService } from '../core/services/title.service';

import { NotificationInterface } from '../shared/interfaces/notification.interface';
import { PaginateResponseInterface } from '../shared/interfaces/common/pagination-response.interface';

import { debounceTime, fromEvent } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CommonApiEnum } from '../core/enums/common-api.enum';
import { HttpService } from '../core/services/http.service';
import { NotificationsService } from '../core/services/notifications.service';

@UntilDestroy()
@Component({
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit, AfterViewInit {
  @ViewChild('notificationsWrapper') notificationsWrapper: ElementRef;

  loading: boolean;
  notifications: PaginateResponseInterface<NotificationInterface> = {
    count: 0,
    dataList: null as NotificationInterface[]
  } as PaginateResponseInterface<NotificationInterface>;

  private scrollState: Record<string, number> = {
    skip: 0,
    take: 15
  };

  constructor(
    private readonly titleService: TitleService,
    private readonly httpService: HttpService,
    private readonly notificationsService: NotificationsService
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle('notification.notificationsLabel');
    this.loadNotifications();
  }

  ngAfterViewInit(): void {
    fromEvent(this.notificationsWrapper.nativeElement, 'scroll')
      .pipe(debounceTime(600), untilDestroyed(this))
      .subscribe((event: Record<string, HTMLElement>) => {
        const scrolled =
          (event.target?.offsetHeight + Math.floor(event.target?.scrollTop)) / (event.target?.scrollHeight / 100);

        if (Math.floor(scrolled) >= 99 && this.notifications.dataList.length < this.notifications.count) {
          this.loadNotifications();
        }
      });
  }

  markAsRead(notification: NotificationInterface): void {
    this.httpService
      .put<unknown>(CommonApiEnum.Notifications, null, {
        id: notification.id,
        readSeen: 1
      })
      .subscribe(() => (notification.isRead = true));
  }

  clearNotifications(): void {
    this.notificationsService
      .markNotificationsAsRead(1)
      .subscribe(() => this.notifications.dataList.forEach(n => (n.isRead = true)));
  }

  private loadNotifications(): void {
    this.loading = true;

    this.httpService
      .get<PaginateResponseInterface<NotificationInterface>>(CommonApiEnum.Notifications, {
        ...this.scrollState,
        includeCount: true
      })
      .subscribe(response => {
        if (!this.scrollState.skip) {
          this.notifications = response;
        } else {
          const existingIds = new Set(this.notifications.dataList.map(item => item.id));

          this.notifications.dataList = [
            ...this.notifications.dataList,
            ...response.dataList.filter(item => !existingIds.has(item.id))
          ];

          this.notifications.count = response.count;
        }

        this.scrollState.skip += response.dataList.length;
        this.loading = false;
      });
  }
}
