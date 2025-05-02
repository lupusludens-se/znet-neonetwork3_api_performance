import { Component, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

import { UserStatusEnum } from './user-management/enums/user-status.enum';
import { AuthService } from './core/services/auth.service';
import { BreadcrumbService } from './core/services/breadcrumb.service';
import { FooterService } from './core/services/footer.service';
import { environment } from 'src/environments/environment';
import { CoreService } from './core/services/core.service';

import { NotFoundDataInterface } from './core/interfaces/not-found-data.interface';
import { Subject, takeUntil } from 'rxjs';
import { ThankYouPopupServiceService } from './public/services/thank-you-popup-service.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'neo-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  userStatuses = UserStatusEnum;
  notFoundData: NotFoundDataInterface;
  isModalVisible: boolean = false;

  private unsubscribe$: Subject<void> = new Subject<void>();
  enableRefreshPopup: boolean = false;
  isNewSession: boolean = false;
  isFeedbackModalVisible: boolean;

  constructor(
    private readonly translateService: TranslateService,
    private readonly breadcrumbService: BreadcrumbService,
    public readonly neoAuthService: AuthService,
    public readonly footerService: FooterService,
    private readonly coreService: CoreService,
    private readonly thankYouPopupServiceService: ThankYouPopupServiceService,
    private httpClient: HttpClient
  ) {
    // Validate the Initial Load Performance based on path
    let currentDate = new Date();
  }

  get isDashboard(): boolean {
    return location.pathname.includes('dashboard');
  }

  get isAuthRedirect(): boolean {
    return location.pathname.includes('auth-redirect');
  }

  get disableFooter(): boolean {
    return (
      location.pathname.includes('/add') ||
      location.pathname.includes('/edit') ||
      location.pathname.includes('/create') ||
      location.pathname.includes('/review') ||
      location.pathname.includes('/email-alerts') ||
      location.pathname.includes('/settings')
    );
  }

  get reducePadding(): boolean {
    return (
      location.pathname.includes('/projects/') ||
      location.pathname.includes('/learn/') ||
      location.pathname.includes('/events/') ||
      location.pathname.includes('/tools/') ||
      location.pathname.includes('/messages/') ||
      location.pathname.includes('/user-profile/') ||
      location.pathname.includes('/company-profile/') ||
      location.pathname.includes('/forum/topic/') ||
      location.pathname.includes('forum/start-a-discussion')
    );
  }

  get isLanding(): boolean {
    return (
      location.pathname === environment.postLogoutRedirect ||
      location.pathname.includes('/sign-up') ||
      location.pathname.includes('/unsubscribe') ||
      location.pathname.includes('/auth-redirect') ||
      location.pathname.includes('/resetcomplete') ||
      location.pathname === `${environment.postLogoutRedirect}403` ||
      location.pathname === `${environment.postLogoutRedirect}404` ||
      location.pathname === `${environment.postLogoutRedirect}423`
    );
  }

  public ngOnInit(): void {
    this.isNewSession = true;
    this.translateService.use('en');
    this.neoAuthService.listenToAuthSubjects();
    this.coreService.elementNotFoundData$
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((data: NotFoundDataInterface) => (this.notFoundData = data));

    this.thankYouPopupServiceService.showStatus$.pipe(takeUntil(this.unsubscribe$)).subscribe(item => {
      this.isModalVisible = item;
    });

    this.coreService.showFeedbackPopup$.pipe(takeUntil(this.unsubscribe$)).subscribe(item => {
      this.isFeedbackModalVisible = item;
    });

    this.checkAndUpdateUserIfAnyNewVersion();
    setInterval(() => {
      this.isNewSession = false;
      this.checkAndUpdateUserIfAnyNewVersion();
    }, 14400000);
  }
  checkAndUpdateUserIfAnyNewVersion() {
    let currentDate = new Date();
    let lastUpdatedDateString: string = null;
    let lastUpdatedDate = null;
    lastUpdatedDateString =
      sessionStorage.getItem('lastUpdatedDate') !== undefined || sessionStorage.getItem('lastUpdatedDate') !== null
        ? sessionStorage.getItem('lastUpdatedDate')
        : null;
    lastUpdatedDate = lastUpdatedDateString != null ? new Date(lastUpdatedDateString) : null;
    // later needs to be added new Date(currentDate.toDateString()) > new Date(lastUpdatedDate.getTime())
    if (lastUpdatedDate == null || currentDate.getTime() > lastUpdatedDate.getTime()) {
      this.httpClient.get('assets/version.json' + '?v=' + currentDate.getTime()).subscribe(
        response => {
          let newVersion = response.toString();
          let existingVersion =
            sessionStorage.getItem('version') != undefined || sessionStorage.getItem('version') != null
              ? parseInt(sessionStorage.getItem('version')?.split('.')[1])
              : 0;
          if (existingVersion < parseInt(newVersion.split('.')[1])) {
            sessionStorage.setItem('version', newVersion);
            if (!this.isNewSession) {
              this.enableRefreshPopup = true;
            }
          }
        },
        err => {
          console.error('Error fetching JSON:', err);
        }
      );
    }
  }

  refreshBrowser() {
    localStorage.setItem('lastUpdatedDate', new Date().toString());
    window.location.reload();
    this.enableRefreshPopup = false;
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  closeFeedback() {
    const feedbackFormSubmitStatus = this.coreService.feedbackPopupSubmitted$.getValue();
    if (feedbackFormSubmitStatus) {
      this.coreService.showFeedbackPopup$.next(false);
    }
  }
}
