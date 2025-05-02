import { ChangeDetectorRef, Component, OnInit, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CoreService } from '../../services/core.service';
import { MenuOptionInterface } from '../../../shared/modules/menu/interfaces/menu-option.interface';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { BADGE_DATA_RELOAD_TIME } from '../../../shared/constants/time.const';
import { UnreadCountersInterface } from '../../interfaces/unread-counters.interface';
import { TranslateService } from '@ngx-translate/core';
import { SignTrackingSourceEnum } from '../../enums/sign-tracking-source-enum';
import { Subject } from 'rxjs';
import { CommonService } from '../../services/common.service';
import { PublicAccessRoutes } from '../../constants/public-access-routes';

@UntilDestroy()
@Component({
  selector: 'neo-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  userStatuses = UserStatusEnum;
  showScheduleDemoModal: boolean = false;
  signTrackingSourceEnum: string = '';
  modalTitle: string = this.translateService.instant('scheduleDemo.title');
  private userMenuOptions: MenuOptionInterface[] = [
    {
      icon: 'settings',
      name: 'header.settingsLabel',
      link: 'settings'
    },
    {
      icon: 'lock', // lock icon on design and text is 'Forgot password'
      name: 'header.changePasswordLabel',
      link: 'changePassword'
    },
    {
      icon: 'log-out',
      name: 'header.logOutLabel',
      link: 'logOut',
      iconSize: 'text-m',
      customClass: 'pl-16-imp',
      appSeperatorForLastElement: true
    }
  ];

  searchBtnClick: boolean;
  showSearch: boolean;
  currentUser: UserInterface;
  badgeCount: UnreadCountersInterface;
  auth = AuthService;
  isSkeletonHidden: boolean = false;

  showHeaderSkeleton: boolean = false;
  private unsubscribe$: Subject<void> = new Subject<void>();

  get hideSearch(): boolean {
    return !location.pathname.includes('search') && !this.showSearch;
  }

  constructor(
    public translateService: TranslateService,
    private readonly router: Router,
    private readonly authService: AuthService,
    public readonly coreService: CoreService,
    private readonly cdr: ChangeDetectorRef,
    private readonly renderer: Renderer2,
    private commonService: CommonService
  ) {}

  ngOnInit(): void {
    this.signTrackingSourceEnum = SignTrackingSourceEnum.ZeigoNetwork;
    this.authService.currentUser$.subscribe(val => {
      if (val != null) {
        this.currentUser = val;
        this.coreService.getBadgeData();
        setInterval(() => this.coreService.getBadgeData(), BADGE_DATA_RELOAD_TIME);
        this.listenForBadgeUpdateRequest();
      }

    });

    this.listenForSearchOutsideClick();

    this.cdr.detectChanges();

    this.coreService.globalSearchActive$
      .pipe(untilDestroyed(this))
      .subscribe(showSearch => (this.showSearch = showSearch));

    this.authService
      .currentUser()
      .pipe(untilDestroyed(this))
      .subscribe((user: UserInterface) => {
        this.currentUser = user;
        this.cdr.detectChanges();
      });

    this.coreService.ongoingRoute$.pipe(untilDestroyed(this)).subscribe(route => {
      if (route !== null) {
        if (PublicAccessRoutes.some(publicRoute => route.toLowerCase().includes(publicRoute.toLowerCase()))) {
          this.isSkeletonHidden = true;
        } else {
          this.isSkeletonHidden = false;
        }
      }
    });
    this.coreService.guardCheckEndedRoute$.pipe(untilDestroyed(this)).subscribe(val => {
      if (val != null) {
        this.isSkeletonHidden = true;
      }
    });

    this.coreService.badgeCount$.pipe(untilDestroyed(this)).subscribe(badgeCount => (this.badgeCount = badgeCount));
  }

  login() {
    this.authService.loginRedirect();
  }

  optionClick(option: MenuOptionInterface): void {
    if (option.link === 'logOut') {
      this.authService.logout();

      return;
    } else if (option.link === 'changePassword') {
      this.authService.changePassword();

      return;
    }
    this.router.navigate([`/${option.link}`]).then();
  }

  menuOptions(user: UserInterface): MenuOptionInterface[] {
    if (!this.userMenuOptions.some(option => option.link.includes('user-profile'))) {
      this.userMenuOptions = [
        {
          icon: 'account',
          name: 'header.profileLabel',
          link: `/user-profile/${user.id}`
        },
        ...this.userMenuOptions
      ];
    }

    return this.userMenuOptions;
  }

  toggleSearchBox(): void {
    this.searchBtnClick = true;
    this.coreService.globalSearchActive$.next(true);
  }

  badgeCountRefresh(): void {
    this.coreService.getBadgeData();
  }

  private listenForBadgeUpdateRequest(): void {
    this.coreService
      .badgeDataFetch()
      .pipe(untilDestroyed(this))
      .subscribe(() => this.coreService.getBadgeData());
  }

  private listenForSearchOutsideClick(): void {
    this.renderer.listen('window', 'click', () => {
      if (!this.searchBtnClick) {
        this.coreService.globalSearchActive$.next(false);
      }

      this.searchBtnClick = false;
    });
  }

  showScheduleDemo(showStatus: boolean) {
    this.showScheduleDemoModal = showStatus;
    if (showStatus) {
      this.modalTitle = this.translateService.instant('scheduleDemo.title');
    }
  }
}
