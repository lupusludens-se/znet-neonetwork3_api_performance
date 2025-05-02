import { Component, OnInit } from '@angular/core';
import { AdminNavigationInterface } from './interfaces/admin-navigation.interface';
import { TitleService } from '../core/services/title.service';
import { HttpService } from '../core/services/http.service';
import { catchError, Observable, throwError } from 'rxjs';
import { AdmitUserEnum } from '../admit/emuns/admit-user.enum';
import { AuthService } from '../core/services/auth.service';
import { PermissionService } from '../core/services/permission.service';
import { UserInterface } from '../shared/interfaces/user/user.interface';
import { PermissionTypeEnum } from '../core/enums/permission-type.enum';
import { environment } from '../../environments/environment';
import { CommonApiEnum } from '../core/enums/common-api.enum';
import { SnackbarService } from '../core/services/snackbar.service';

@Component({
  selector: 'neo-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  adminNavigation: AdminNavigationInterface[] = [];

  admitUsersBadge$: Observable<Record<string, number>> = this.httpService.get(AdmitUserEnum.PendingCounter);

  currentUser: UserInterface;

  showModal: boolean;
  constructor(
    private readonly titleService: TitleService,
    private readonly httpService: HttpService,
    private readonly authService: AuthService,
    private readonly permissionService: PermissionService,
    private readonly snackbarService: SnackbarService
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle('admin.adminManagementLabel');
    this.listenForCurrentUser();
  }

  hasPermission(permission: number): boolean {
    return this.permissionService.userHasPermission(this.currentUser, permission);
  }

  private listenForCurrentUser(): void {
    this.authService.currentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      if (currentUser) {
        this.initAdminNavigation();
      }
    });
  }

  private initAdminNavigation(): void {
    if (this.hasPermission(PermissionTypeEnum.UserAccessManagement)) {
      this.adminNavigation.push({
        icon: 'user-account',
        title: 'userManagement.pageTitleLabel',
        addButtonName: 'userManagement.addUserLabel',
        addButtonLink: './user-management/add',
        viewButtonName: 'admin.viewUsersLabel',
        viewButtonLink: './user-management',
        secondButtonName: 'admitUsers.pageTitle',
        secondButtonLink: './admit-users',
        isSecondBadgeEnabled: true
      });
    }

    this.adminNavigation.push(
      {
        icon: 'building',
        title: 'companyManagement.pageTitleLabel',
        addButtonName: 'companyManagement.addCompanyLabel',
        addButtonLink: './company-management/add',
        addButtonDisable: !this.hasPermission(PermissionTypeEnum.CompanyManagement),
        viewButtonName: 'companyManagement.pageTitleLabel',
        viewButtonLink: './company-management'
      },
      {
        icon: 'tools',
        title: 'toolsManagement.toolManagementLabel',
        addButtonName: 'toolsManagement.addToolLabel',
        addButtonLink: './tool-management/add',
        addButtonDisable: !this.hasPermission(PermissionTypeEnum.ToolManagement),
        viewButtonName: 'admin.viewToolsLabel',
        viewButtonLink: './tool-management'
      },
      {
        icon: 'admin-initiatives',
        title: 'admin.viewInitiativesTitle',
        addButtonName: 'admin.viewInitiativesBtnLabel',
        addButtonLink: './initiatives',
        enableArrowIcon: true
      }
    );

    if (this.hasPermission(PermissionTypeEnum.AnnouncementManagement)) {
      this.adminNavigation.push({
        icon: 'admin-announcement',
        title: 'announcement.announcementsLabel',
        addButtonName: 'announcement.addAnnouncementLabel',
        addButtonLink: './announcements/add',
        viewButtonName: 'announcement.viewAnnouncementsLabel',
        viewButtonLink: './announcements'
      });
    }

    if (this.hasPermission(PermissionTypeEnum.ForumManagement)) {
      this.adminNavigation.push({
        icon: 'forum',
        title: 'forum.forumLabel',
        addButtonName: 'forum.addPostLabel',
        addButtonLink: '../forum/start-a-discussion',
        viewButtonName: 'forum.viewForumLabel',
        viewButtonLink: '../forum'
      });
    }

    this.adminNavigation.push(
      {
        icon: 'learn',
        title: 'learn.learnLabel',
        addButtonName: 'admin.wordPressLoginLabel',
        addButtonLink: environment.wordpressUrl,
        viewButtonName: 'admin.viewLearnLabel',
        viewButtonLink: '../learn',
        secondButtonName: 'admin.wordPressDataSyncLabel',
        isSecondButtonRequest: true,
        isExternalLink: true
      },
      {
        icon: 'events',
        title: 'settings.eventsLabel',
        addButtonName: 'events.addEventLabel',
        addButtonLink: './events/create-an-event',
        addButtonDisable: !this.hasPermission(PermissionTypeEnum.EventManagement),
        viewButtonName: 'events.viewEventsLabel',
        viewButtonLink: '../events'
      },
      {
        icon: 'feedback',
        title: 'feedbackManagement.feedbackLabel',
        addButtonName: 'feedbackManagement.viewFeedbackLabel',
        addButtonLink: './user-feedback',
        enableArrowIcon: true
      },
      {
        icon: 'mail',
        title: 'admin.emailAlertsSettingLabel',
        addButtonName: 'admin.emailAlertsSettingLabel',
        addButtonLink: './email-alerts'
      }
    );
  }

  dataSync(): void {
    this.httpService
      .post(CommonApiEnum.ArticlesSync)
      .pipe(
        catchError(err => {
          this.showModal = false;
          this.snackbarService.showError('general.defaultErrorLabel');
          return throwError(err);
        })
      )
      .subscribe(() => {
        this.showModal = false;
        this.snackbarService.showSuccess('admin.wordPressDataSyncModal.SuccessLabel');
      });
  }
}
