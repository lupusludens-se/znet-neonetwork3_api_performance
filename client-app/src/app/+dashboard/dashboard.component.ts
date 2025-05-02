import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { TitleService } from '../core/services/title.service';
import { Observable, of, Subject } from 'rxjs';
import { AnnouncementInterface } from '../+admin/modules/announcements/interfaces/announcement.interface';
import { HttpService } from '../core/services/http.service';
import { PaginateResponseInterface } from '../shared/interfaces/common/pagination-response.interface';
import { EventInterface } from '../shared/interfaces/event/event.interface';
import { EventsApiEnum } from '../shared/enums/api/events-api.enum';
import { UserProfileApiEnum } from '../shared/enums/api/user-profile-api.enum';
import { UserSuggestionInterface } from '../shared/interfaces/user/user-suggestion.intarface';
import { ApiRoutes } from '../+admin/modules/announcements/enums/announcement.enum';
import { AuthService } from '../core/services/auth.service';
import { UserInterface } from '../shared/interfaces/user/user.interface';
import { RolesEnum } from '../shared/enums/roles.enum';
import * as dayjs from 'dayjs';
import { UserStatusEnum } from '../user-management/enums/user-status.enum';
import { map, switchMap, takeUntil } from 'rxjs/operators';
import { EventService } from '../+events/services/event.service';
import { ActivityTypeEnum } from '../core/enums/activity/activity-type.enum';
import { ActivityService } from '../core/services/activity.service';
import { FirstClickInfoActivityDetailsInterface } from '../core/interfaces/activity-details/first-click-info-activity-details.interface';
import { DashboardClickElementActionTypeEnum } from '../core/enums/activity/dashboard-click-element-action-type.enum';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  providers: [EventService]
})
export class DashboardComponent implements OnInit, OnDestroy, AfterViewInit {
  roles = RolesEnum;
  status = UserStatusEnum;
  isSkeletonHidden: boolean = false;
  isUserAvailable: boolean = false;
  title$: Observable<AnnouncementInterface>;
  announcement$: Observable<AnnouncementInterface> = this.httpService.get<AnnouncementInterface>(
    ApiRoutes.AnnouncementLatest,
    {
      expand: 'backgroundimage,audience'
    }
  );
  auth = AuthService;

  events$: Observable<PaginateResponseInterface<EventInterface>> = this.httpService
    .get<PaginateResponseInterface<EventInterface>>(EventsApiEnum.Events, {
      expand: 'occurrences',
      skip: 0,
      take: 3,
      from: dayjs().toISOString()
    })
    .pipe(
      map((events: PaginateResponseInterface<EventInterface>) => {
        events.dataList.forEach(event => this.eventService.convertEvent(event));
        return events;
      })
    );

  suggestions$: Observable<UserSuggestionInterface> = this.httpService.get<UserSuggestionInterface>(
    UserProfileApiEnum.Suggestions
  );

  private unsubscribe$: Subject<void> = new Subject<void>();

  elementClick$: Subject<FirstClickInfoActivityDetailsInterface> =
    new Subject<FirstClickInfoActivityDetailsInterface>();

  constructor(
    private readonly httpService: HttpService,
    private readonly titleService: TitleService,
    private readonly eventService: EventService,
    private readonly activityService: ActivityService,
    public readonly authService: AuthService
  ) {}

  public ngOnInit(): void {
    this.title$ = this.announcement$;
    this.authService.currentUser$.subscribe(user => {
      this.isUserAvailable = !!user;
    });
    setTimeout(() => {
      this.isSkeletonHidden = true;
    }, 1000);

    this.titleService.setTitle('title.dashboardLabel');

    if (!this.wasFirstClickFired()) {
      this.elementClick$
        .pipe(
          takeUntil(this.unsubscribe$),
          switchMap((data: FirstClickInfoActivityDetailsInterface) => {
            if (!this.wasFirstClickFired()) {
              return this.activityService.trackElementInteractionActivity(ActivityTypeEnum.FirstDashboardClick, data);
            }
            return of(null);
          })
        )
        .subscribe(() => {
          if (!this.wasFirstClickFired()) {
            localStorage.setItem('firstDashboardClickFired', 'true');
          }
        });
    }
  }

  ngAfterViewInit(): void {
    // Validate the Initial Load Performance based on path
    let currentDate = new Date();
  }

  public ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  private wasFirstClickFired(): boolean {
    return !!localStorage.getItem('firstDashboardClickFired');
  }

  isInRole(currentUser: UserInterface, roleId: RolesEnum): boolean {
    if (currentUser != null) {
      return currentUser.roles.some(role => role.id === roleId && role.isSpecial);
    }
  }

  onEventAllClick(): void {
    this.elementClick$.next({ actionType: DashboardClickElementActionTypeEnum.EventsViewAll });
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ViewAllClick, { viewAllType: 'Event' })
      ?.subscribe();
  }

  onEventClick(id: number): void {
    this.elementClick$.next({ actionType: DashboardClickElementActionTypeEnum.EventView, id: id });
  }

  onAnnouncementClick(id: number): void {
    this.elementClick$.next({ actionType: DashboardClickElementActionTypeEnum.AnnouncementButtonClick, id: id });
  }

  onSuggestionSkipClick(): void {
    this.elementClick$.next({ actionType: DashboardClickElementActionTypeEnum.SuggestionSkip });
  }

  onSuggestionHideClick(): void {
    this.elementClick$.next({ actionType: DashboardClickElementActionTypeEnum.SuggestionHide });
  }

  onSuggestionYesClick(): void {
    this.elementClick$.next({ actionType: DashboardClickElementActionTypeEnum.SuggestionTake });
  }
  isNotRoles(currentUser: UserInterface): boolean {
    var isAlloftheRoles =
      !this.isInRole(currentUser, this.roles.SolutionProvider) &&
      !this.isInRole(currentUser, this.roles.SPAdmin) &&
      !this.isInRole(currentUser, this.roles.Corporation);
    return isAlloftheRoles;
  }
  isEitherOfTheRoles(currentUser: UserInterface): boolean {
    var isEitheroftheRoles =
      this.isInRole(currentUser, this.roles.SolutionProvider) ||
      this.isInRole(currentUser, this.roles.SPAdmin) ||
      this.isInRole(currentUser, this.roles.Corporation);
    return isEitheroftheRoles;
  }
}
