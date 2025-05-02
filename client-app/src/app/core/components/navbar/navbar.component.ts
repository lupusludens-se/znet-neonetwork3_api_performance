import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { navbarItems } from '../../constants/navbar-items';
import { ActivityTypeEnum } from '../../enums/activity/activity-type.enum';
import { PermissionTypeEnum } from '../../enums/permission-type.enum';
import { ActivityService } from '../../services/activity.service';
import { AuthService } from '../../services/auth.service';
import { PermissionService } from '../../services/permission.service';
import { CompanyRolesEnum } from '../../../shared/enums/company-roles.enum';
import { RolesEnum } from '../../../shared/enums/roles.enum';
import { UnreadCountersInterface } from '../../interfaces/unread-counters.interface';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CoreService } from '../../services/core.service';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { PublicAccessRoutes } from '../../constants/public-access-routes';
import { ActivityApiEnum } from '../../enums/activity/activity-api.enum';
import { ThankYouPopupServiceService } from 'src/app/public/services/thank-you-popup-service.service';
import { BehaviorSubject } from 'rxjs';

@UntilDestroy()
@Component({
  selector: 'neo-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  public navbarItems: { name: string; path: string; icon: string; cssClass?: string }[];
  activityTypeEnum: ActivityTypeEnum;
  badgeCount: UnreadCountersInterface;
  userType: string = '';
  auth = AuthService;
  isPopupVisible: boolean = false;

  constructor(
    private readonly authService: AuthService,
    private readonly permissionService: PermissionService,
    private readonly activityService: ActivityService,
    private readonly router: Router,
    public readonly coreService: CoreService,
    private msalBroadcastService: MsalBroadcastService,
    private thankYouPopupServiceService: ThankYouPopupServiceService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.activityTypeEnum = ActivityTypeEnum.NavMenuItemClick;
    this.listenForCurrentUser();
  }

  onNavItemClick(name: string): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.NavMenuItemClick, { navMenuItemName: name?.replace(/\s/g, '') })
      ?.subscribe();
  }

  private listenForCurrentUser(): void {
    this.authService.currentUser().subscribe(currentUser => {
      this.checkIfUserIsPrivateOrPublic();
      if (!currentUser) {
        return;
      }

      this.coreService.badgeCount$.pipe(untilDestroyed(this)).subscribe(badgeCount => (this.badgeCount = badgeCount));
      this.navbarItems = [];
      navbarItems.forEach(ni => this.navbarItems.push(ni));

      if (!this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.AdminAll)) {
        this.removeNavbarItem('admin');
      }
      if (!this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.ManageOwnCompany)) {
        this.removeNavbarItem('manage');
      }

      if (
        !this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.ProjectCatalogView) &&
        !this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.ProjectsManageAll)
      ) {
        this.removeNavbarItem('projects');
      }

      if (
        (!this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.ProjectsManageOwn) &&
          !this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.ProjectsManageAll)) ||
        (currentUser.company.typeId === CompanyRolesEnum.Corporation &&
          currentUser.roles.some(r => r.id === RolesEnum.Internal))
      ) {
        this.removeNavbarItem('project library');
      }
      this.cdr.detectChanges();
    });
  }

  private removeNavbarItem(navbarLabel: string): void {
    const index = this.navbarItems.findIndex(i => i.name.toLowerCase() === navbarLabel);
    if (index !== -1) this.navbarItems.splice(index, 1);
  }

  checkIfUserIsPrivateOrPublic(): void {
    if (AuthService.isLoggedIn() || AuthService.needSilentLogIn()) {
      this.userType = 'Private';
    } else {
      this.coreService.ongoingRoute$.subscribe(route => {
        if (route !== null) {

          if (!route.includes(ActivityApiEnum.PublicActivity)) {
            this.thankYouPopupServiceService.showStatus$.next(false);
          }

          if (PublicAccessRoutes.includes(route)) {
            this.navbarItems = [];
            navbarItems.filter(item => item.hideInPublic != true).forEach(ni => this.navbarItems.push(ni));
            this.userType = 'Public';
          }
        }
      });
      this.coreService.guardCheckEndedRoute$.subscribe(val => {
        if (val !== null) {
          if (AuthService.isLoggedIn() || AuthService.needSilentLogIn()) {
            this.userType = 'Private';
            this.navbarItems = [];
            navbarItems.forEach(ni => this.navbarItems.push(ni));
            return;
          }
          this.navbarItems = [];
          navbarItems.filter(item => item.hideInPublic != true).forEach(ni => this.navbarItems.push(ni));
          this.userType = 'Public';
        }
      });
    }
  }

  toggleFeedbackPopup(event): void {
    if (event == 'nav') {
      const feedbackFormStatus = this.coreService.feedbackPopupSubmitted$.getValue();
      if (feedbackFormStatus) {
        this.coreService.showFeedbackPopup$.next(false);
      }
    }
    else {
      const showStatus = this.coreService.showFeedbackPopup$.getValue();
      this.coreService.showFeedbackPopup$.next(!showStatus);
    }
  }
}
