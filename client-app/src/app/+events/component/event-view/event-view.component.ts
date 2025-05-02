import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as dayjs from 'dayjs';
import { catchError, map, Subject, switchMap, takeUntil, throwError } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { CoreService } from 'src/app/core/services/core.service';
import { FollowingService } from 'src/app/core/services/following.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { EventModeratorInterface } from 'src/app/shared/interfaces/event/event-moderator.interface';
import { EventOccurrenceInterface } from 'src/app/shared/interfaces/event/event-occurrence.interface';
import { EventUserInterface } from 'src/app/shared/interfaces/event/event-user.interface';
import { EventInterface } from 'src/app/shared/interfaces/event/event.interface';
import { UserAvatarInterface } from 'src/app/shared/interfaces/user-avatar.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { MemberInterface } from 'src/app/shared/modules/members-list/interfaces/member.interface';
import { EventService } from '../../services/event.service';
import { TaxonomyTypeEnum } from '../../../shared/enums/taxonomy-type.enum';
import { PermissionTypeEnum } from '../../../core/enums/permission-type.enum';
import { PermissionService } from '../../../core/services/permission.service';
import { TitleService } from 'src/app/core/services/title.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { HttpErrorResponse } from '@angular/common/http';
import { URL_REGEXP } from '../../../shared/validators/custom.validator';
import { CommonService } from 'src/app/core/services/common.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';

@UntilDestroy()
@Component({
  selector: 'neo-event-view',
  templateUrl: './event-view.component.html',
  styleUrls: ['./event-view.component.scss']
})
export class EventViewComponent implements OnInit, OnDestroy {
  type = TaxonomyTypeEnum;
  eventData: EventInterface;
  currentUser: UserInterface;
  readonly userStatuses = UserStatusEnum;
  sortedOccurrences = [];
  isEventActual: boolean;
  private eventId: string = this.activatedRoute.snapshot.paramMap.get('id');
  private loadData$: Subject<void> = new Subject<void>();
  private attendanceChange$: Subject<boolean> = new Subject<boolean>();
  private unsubscribe$: Subject<void> = new Subject<void>();
  subscription: any;
  activityTypeEnum: any;
  auth = AuthService;
  isUserLoggedIn = false;
  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly eventService: EventService,
    private readonly coreService: CoreService,
    private readonly snackbarService: SnackbarService,
    private readonly router: Router,
    private readonly followingService: FollowingService,
    public readonly authService: AuthService,
    private readonly permissionService: PermissionService,
    private commonService: CommonService,
    private titleService: TitleService
  ) { }

  ngOnInit(): void {
    if (this.auth.isLoggedIn() || this.auth.needSilentLogIn()) {
      this.isUserLoggedIn = true;
    }
    this.activityTypeEnum = ActivityTypeEnum.EventRegistration;
    this.subscription = this.authService
      .currentUser()
      .pipe(untilDestroyed(this))
      .subscribe((user: UserInterface) => {
        this.currentUser = user;
        this.activatedRoute.params.subscribe(() => {
          this.eventId = this.activatedRoute.snapshot.paramMap.get('id');
          this.listenToLoadData();
          if (user?.id) {
            this.listenToUserFollowing();
            this.listenToAttendanceChange();
          }
          this.loadData$.next();
        });
      });

    this.titleService.setTitle('events.viewEventLabel');
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    this.loadData$.next();
    this.loadData$.complete();

    this.attendanceChange$.next(null);
    this.attendanceChange$.complete();
  }

  addToCalendar(occurrences: EventOccurrenceInterface[]): void {
    let occIds = occurrences.filter(occ => new Date(occ.toDateBrowser) > new Date())?.map(fo => fo.id) || [];
    this.eventService.addEventToCalendar(this.eventId, occIds);
  }

  copyLink(): void {
    this.coreService.copyTextToClipboard(window.location.href);
    this.snackbarService.showSuccess('general.linkCopiedLabel');
  }

  openUserProfile(userId: number): void {
    if (userId) {
      this.router.navigate([`user-profile/${userId}`]);
    }
  }

  startNewMessageWithUser(userId: number): void {
    if (userId) {
      this.router.navigate([`messages/new-message`], {
        queryParams: { userId: userId }
      });
    }
  }

  getUserAvatar(moderator: EventModeratorInterface): UserAvatarInterface {
    if (moderator.user) {
      let user: EventUserInterface = moderator.user;

      return {
        firstName: user.statusId === UserStatusEnum.Deleted ? 'Deleted' : user.firstName,
        lastName: user.statusId === UserStatusEnum.Deleted ? 'User' : user.lastName,
        image: user.image
      };
    }

    let names = moderator.name?.split(' ');

    return {
      firstName: names?.[0],
      lastName: names?.[1]
    };
  }

  followUser(user: MemberInterface, moderator: EventModeratorInterface): void {
    this.followingService.followUser(user?.id, user?.isFollowed);
    this.followingService.followedUser().subscribe((result) => {
      moderator.user.isFollowed = !result.unFollow;
    });
  }

  changeAttendance(isAttending?: boolean): void {
    this.eventService.changeAttendance(this.eventData.id.toString(), isAttending);
  }

  openLink(url: string): void {
    location.href = url;
  }

  hasPermission(currentUser: UserInterface): boolean {
    return this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.EventManagement);
  }

  isLink(str: string): boolean {
    return URL_REGEXP.test(str);
  }

  goBack() {
    this.commonService.goBack();
  }

  private listenToLoadData() {
    this.loadData$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => this.eventService.getEventDetails(this.eventId)),
        map(event => {
          this.eventService.convertEvent(event, false);

          this.sortedOccurrences = Object.values(
            [...(event.occurrences as EventOccurrenceInterface[])].reduce((rv, x) => {
              (rv[dayjs(x.fromDate).format('YYYY-MM-DD')] = rv[dayjs(x.fromDate).format('YYYY-MM-DD')] || []).push(x);
              return rv;
            }, {})
          ).map(el => ({ ...el[0], timeRanges: el }));

          const lastOcc = this.sortedOccurrences[this.sortedOccurrences.length - 1];
          this.isEventActual =
            dayjs(lastOcc.timeRanges[lastOcc.timeRanges.length - 1].toDateBrowser).toDate() > new Date();

          return event;
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            this.router.navigate(['/events']);
            this.coreService.elementNotFoundData$.next({
              iconKey: 'event-calendar',
              mainTextTranslate: 'events.notFoundText',
              buttonTextTranslate: 'events.notFoundButton',
              buttonLink: '/events'
            });
          }

          return throwError(error);
        })
      )
      .subscribe(event => {
        this.eventData = event;
      });
  }

  private listenToUserFollowing(): void {
    this.followingService
      .followedUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(followInfo => {
        this.eventData.attendees.forEach(at => {
          if (at.id === followInfo.userId) at.isFollowed = !followInfo.unFollow;
        });
      });
  }

  private listenToAttendanceChange(): void {
    this.eventService.attendanceChange$.pipe(takeUntil(this.unsubscribe$)).subscribe(event => {
      this.eventData.isAttending = event.isAttending;
      const attendeeUser: EventUserInterface = {
        id: this.currentUser.id,
        firstName: this.currentUser.firstName,
        lastName: this.currentUser.lastName,
        name: this.currentUser.email,
        company: this.currentUser.company.name,
        image: this.currentUser.image,
        statusId: this.currentUser.statusId
      };

      if (event.isAttending) {
        this.eventData.attendees.push(attendeeUser);
        this.eventData.attendees = [...this.eventData.attendees];
      } else {
        this.eventData.attendees = this.eventData.attendees.filter(at => at.id !== attendeeUser.id);
      }

      this.eventData.attendees = [...new Map(this.eventData.attendees.map(item => [item['id'], item])).values()];
    });
  }
}
