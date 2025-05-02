import { Component, Input, OnInit } from '@angular/core';
import { EventService } from '../../services/event.service';
import { take } from 'rxjs';
import { EventInterface } from '../../../shared/interfaces/event/event.interface';
import { Router } from '@angular/router';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { PermissionTypeEnum } from '../../../core/enums/permission-type.enum';
import { PermissionService } from '../../../core/services/permission.service';
import { AuthService } from '../../../core/services/auth.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { EventLocationType } from '../../../shared/enums/event/event-location-type.enum';
import { EventModeratorInterface } from '../../../shared/interfaces/event/event-moderator.interface';
import { EventAttendanceInterface } from '../events-list/events-list.component';

@UntilDestroy()
@Component({
  selector: 'neo-events-slider',
  templateUrl: './events-slider.component.html',
  styleUrls: ['./events-slider.component.scss']
})
export class EventsSliderComponent implements OnInit {
  @Input() marginRight = 0;
  @Input() autoplayDelay = 10_000;
  canEdit: boolean;
  index = 0;
  swiperBullets = [];
  EventLocationType = EventLocationType;
  highlightedEvents: EventInterface[] = [];
  autoplayInterval;
  isUserLoaded: boolean;
  displayedModerators: EventModeratorInterface[][] = [];
  omittedModerators: string[] = [];

  constructor(
    private readonly authService: AuthService,
    private readonly eventService: EventService,
    private readonly permissionService: PermissionService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.pipe(untilDestroyed(this)).subscribe((user: UserInterface) => {
      if (user?.id) {
        this.getEditStatement();
        this.getHighlightedEvents();
        this.listenToAttendanceChange();
      }
    });
  }

  saveEventAttendingStatus(event: EventInterface, isAttending: boolean): void {
    const updatedAttending = event.isAttending !== isAttending ? isAttending : null;

    this.eventService.changeAttendance(event.id.toString(), updatedAttending);
    event.isAttending = updatedAttending === null ? null : isAttending;
  }

  openEditEventDetails(id: number): void {
    this.router.navigateByUrl('admin/events/edit-event/' + id);
  }

  getSlideTranslate(index: number): string {
    return `(-100% / 1 * ${index}) - ${this.marginRight * index}px`;
  }

  startAutoplay(): void {
    if (this.autoplayInterval) {
      this.stopAutoplay();
    }

    this.autoplayInterval = setInterval(() => {
      this.index === this.swiperBullets.length - 1 ? (this.index = 0) : this.index++;
    }, this.autoplayDelay);
  }

  stopAutoplay(): void {
    clearTimeout(this.autoplayInterval);
  }

  openEventDetails(event: EventInterface): void {
    this.router.navigateByUrl('events/' + event.id);
  }

  openModeratorProfile(user: UserInterface): void {
    if (user?.id) {
      this.router.navigateByUrl('user-profile/' + user.id);
    }
  }

  private getEditStatement(): void {
    this.authService
      .currentUser()
      .pipe(untilDestroyed(this))
      .subscribe((currentUser: UserInterface) => {
        if (currentUser?.roles?.length) {
          this.canEdit = this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.EventManagement);
          this.isUserLoaded = true;
        }
      });
  }

  private getHighlightedEvents(): void {
    this.eventService
      .getHighlightedEvents()
      .pipe(take(1))
      .subscribe((data: EventInterface[]) => {
        this.highlightedEvents = data;

        this.highlightedEvents.forEach((el: EventInterface, i: number) => {
          this.displayedModerators.push(el.moderators.sort((m1, m2) => (m1.name > m2.name ? 1 : -1)).slice(0, 2));
          this.displayedModerators[i].forEach(
            el =>
              (el.name = `${el.name}${el.company || el.user?.company ? ', ' + (el.company || el.user?.company) : ''}`)
          );

          this.omittedModerators.push(
            el.moderators
              .sort((m1, m2) => (m1.name > m2.name ? 1 : -1))
              .slice(2, el.moderators.length)
              .map(
                el => `${el.name}${el.company || el.user?.company ? ' (' + (el.company || el.user?.company) + ')' : ''}`
              )
              .join(', ')
          );
        });

        this.swiperBullets = Array(data.length)
          .fill(null)
          .map((el, index) => index);

        this.startAutoplay();
      });
  }

  private listenToAttendanceChange(): void {
    this.eventService.attendanceChange$.pipe(untilDestroyed(this)).subscribe((data: EventAttendanceInterface) => {
      const currentEvent = this.highlightedEvents.find(el => el.id === +data.eventId);
      if (currentEvent) {
        currentEvent.isAttending = data.isAttending;
      }
    });
  }
}
