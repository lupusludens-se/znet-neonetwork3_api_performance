import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  Renderer2,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import { NavigationEnd, Router, RouterEvent } from '@angular/router';

import { debounceTime, filter, fromEvent, take } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { NotificationInterface } from '../../../shared/interfaces/notification.interface';
import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';

import { NotificationTypeEnum } from '../../../shared/enums/notification-type.enum';
import { NotificationsService } from '../../services/notifications.service';

@UntilDestroy()
@Component({
  selector: 'neo-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit, OnChanges {
  @Input() badgeCount: number;
  @Input() shortVersion: boolean;

  @Output() badgeCountRefresh: EventEmitter<void> = new EventEmitter<void>();

  defaultNotificationsObj = {
    count: 0,
    dataList: null as NotificationInterface[]
  } as PaginateResponseInterface<NotificationInterface>;

  loading: boolean;
  notifications: PaginateResponseInterface<NotificationInterface> = Object.assign({}, this.defaultNotificationsObj);

  menuOpen: boolean;
  notificationType = NotificationTypeEnum;

  isNotificationPage: boolean;

  private scrollState: Record<string, number> = {
    skip: 0,
    take: 10
  };
  private menuBtnClick: boolean;

  @ViewChild('notificationsWrapper') notificationsWrapper: ElementRef;

  constructor(
    private readonly renderer: Renderer2,
    private readonly notificationsService: NotificationsService,
    private readonly router: Router
  ) {}

  get isActive(): boolean {
    return this.menuOpen || location.pathname === '/notifications';
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.badgeCount.currentValue > 0 && this.menuOpen) {
      this.scrollState.skip = 0;
      this.loadNotifications();
    }
  }

  ngOnInit(): void {
    this.listenForWindowClick();
    this.listenForNavigationEnd();

    this.router.events.pipe(untilDestroyed(this)).subscribe((val: RouterEvent) => {
      if (val instanceof NavigationEnd) {
        this.isNotificationPage = this.router.url === '/notifications';
      }
    });

    this.isNotificationPage = this.router.url === '/notifications';
  }

  toggleMenu(): void {
    if (!this.isNotificationPage) {
      this.menuOpen = !this.menuOpen;

      if (this.menuOpen) {
        this.loadNotifications();

        setTimeout(() => {
          this.listenToNotificationsScroll();
        }, 500);

        this.notificationsService
          .markNotificationsAsRead(2)
          .pipe(take(1))
          .subscribe(() => {
            this.badgeCountRefresh.emit();
          });
      }
    }
  }

  clearNotifications(): void {
    this.notificationsService
      .markNotificationsAsRead(1)
      .subscribe(() => this.notifications.dataList.forEach(n => (n.isRead = true)));
  }

  preventCloseOnClick(): void {
    this.menuBtnClick = true;
  }

  markAsRead(notification: NotificationInterface): void {
    this.notificationsService
      .markNotificationAsRead(notification.id)
      .pipe(take(1))
      .subscribe(() => (notification.isRead = true));
  }

  private listenForWindowClick(): void {
    this.renderer.listen('window', 'click', () => {
      if (!this.menuBtnClick) {
        this.menuOpen = false;
        this.notifications = null;
        this.scrollState.skip = 0;
      }

      this.menuBtnClick = false;
    });
  }

  private listenForNavigationEnd(): void {
    this.router.events
      .pipe(
        untilDestroyed(this),
        filter(event => event instanceof NavigationEnd)
      )
      .subscribe(() => {
        if (!this.menuBtnClick) {
          this.menuOpen = false;
          this.notifications = null;
          this.scrollState.skip = 0;
        }

        this.menuBtnClick = false;
      });
  }

  private loadNotifications(): void {
    this.loading = true;

    this.notificationsService
      .getNotifications(this.scrollState, true)
      .pipe(take(1))
      .subscribe(response => {
        if (!this.scrollState.skip) {
          this.notifications = response ?? Object.assign({}, this.defaultNotificationsObj);
        } else {
          const existingIds = new Set(this.notifications.dataList?.map(item => item.id));
          const filteredDataList = response?.dataList?.filter(item => !existingIds.has(item.id)) || [];

          this.notifications.dataList = [...this.notifications.dataList, ...filteredDataList];
          this.notifications.count = response.count;
        }

        this.scrollState.skip += response.dataList.length;
        this.loading = false;
      });
  }

  private listenToNotificationsScroll(): void {
    if (this.notificationsWrapper?.nativeElement) {
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
  }
}
