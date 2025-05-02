import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { UserProfileApiEnum } from '../shared/enums/api/user-profile-api.enum';
import { CountryCheckboxInterface } from '../shared/interfaces/user/country.interface';
import { UserProfileInterface } from './interfaces/user-profile.interface';
import { RegionsService } from '../shared/services/regions.service';
import { TitleService } from '../core/services/title.service';
import { HttpService } from '../core/services/http.service';
import { AuthService } from '../core/services/auth.service';
import { TaxonomyTypeEnum } from '../shared/enums/taxonomy-type.enum';
import { RolesEnum } from '../shared/enums/roles.enum';
import { ActivityTypeEnum } from '../core/enums/activity/activity-type.enum';
import { ActivityService } from '../core/services/activity.service';
import { UserStatusEnum } from '../user-management/enums/user-status.enum';
import { catchError, Subject, throwError } from 'rxjs';
import { CoreService } from '../core/services/core.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonService } from '../core/services/common.service';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { takeUntil } from 'rxjs/operators';
import { UserInterface } from '../shared/interfaces/user/user.interface';
import { TranslateService } from '@ngx-translate/core';
import { FilterStateInterface } from '../shared/modules/filter/interfaces/filter-state.interface';
import { CommunityComponentEnum } from '../+community/enums/community-component.enum';
import { MemberInterface } from '../shared/modules/members-list/interfaces/member.interface';
import { FollowingService } from '../core/services/following.service';
import { TagInterface } from '../core/interfaces/tag.interface';
import { name } from '@azure/msal-angular/packageMetadata';

@UntilDestroy()
@Component({
  selector: 'neo-user-profile',
  templateUrl: 'user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit, OnDestroy {
  showRegions: boolean;
  showInterests: boolean;
  userProfile: UserProfileInterface;
  userProfileApiRoutes = UserProfileApiEnum;
  type = TaxonomyTypeEnum;
  userRoles = RolesEnum;
  corporationRole: boolean;
  readonly userStatuses = UserStatusEnum;
  IsPrivateUser?: boolean;
  attachToInitiative: boolean = false;
  contentType: string = InitiativeModulesEnum[InitiativeModulesEnum.Community];
  userId: number;
  isCorporateUser: boolean = false;
  isNotInternalOrAdminUserRole: boolean = true;
  private unsubscribe$ = new Subject<void>();
  routesToNotClearFilters: string[] = [CommunityComponentEnum.CommunityComponent];
  intialFilterState: FilterStateInterface;
  followersModal: boolean;
  followers: MemberInterface[];
  search: string;
  @Output() followClick: EventEmitter<MemberInterface> = new EventEmitter<MemberInterface>();
  currentUser: UserInterface;
  isSPUser: boolean = false;
  skillsByCategory: TagInterface[] = [];
  skillsCount: number = 0;
  showSkills: boolean;
  constructor(
    private httpService: HttpService,
    private activatedRoute: ActivatedRoute,
    public authService: AuthService,
    private titleService: TitleService,
    private activityService: ActivityService,
    public regionsService: RegionsService,
    private readonly router: Router,
    private commonService: CommonService,
    private readonly coreService: CoreService,
    private translateService: TranslateService,
    private readonly followingService: FollowingService
  ) { }

  ngOnInit(): void {
    this.regionsService.getContinentsList();
    this.activatedRoute.params.subscribe(params => {
      this.userId = params['id'];
      this.listenForCurrentUser();
      this.listenForFilterState();
     
    });
    
    this.activatedRoute.params.pipe(untilDestroyed(this)).subscribe(() => this.getUser());
  }

  private listenForCurrentUser(): void {
    this.authService.currentUser().subscribe(currentUser => {
      if (currentUser) {
        this.currentUser = currentUser;
        this.isCorporateUser = (currentUser as UserInterface).roles.some(role => role.id === RolesEnum.Corporation);
        
      }
    });
  }

  getUser(): void {
    this.httpService
      .get<UserProfileInterface>(
        `${this.userProfileApiRoutes.UserProfiles}/${this.activatedRoute.snapshot.params.id}`,
        {
          expand:
            'user.company,user.country,state,categories,urlLinks,user.image,user.role,followers,userprofile.regions,skills'
        }
      )
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            this.router.navigate(['/community']);
            this.coreService.elementNotFoundData$.next({
              iconKey: 'user-unavailable',
              mainTextTranslate: 'userProfile.notFoundText',
              buttonTextTranslate: 'userProfile.notFoundButton',
              buttonLink: '/community'
            });
          }

          return throwError(error);
        })
      )
      .subscribe(res => {
        this.userProfile = res;
        this.followers = [...res.followers];
        this.isNotInternalOrAdminUserRole = !this.userProfile.user.roles.some(
          r => r.id === this.userRoles.Internal || r.id === this.userRoles.Admin
        );
        this.corporationRole = this.userProfile.user.roles.some(r => r.id === this.userRoles.Corporation);
        this.skillsCount = res.skillsByCategory.length;
        this.isSPUser = this.userProfile.user.roles.some(r => r.id === this.userRoles.SPAdmin || r.id === this.userRoles.SolutionProvider);
        if(this.corporationRole||this.isSPUser)
        this.skillsByCategory = res.skillsByCategory.map(skill => {
          return { id: skill.categorySkillId, name: `${skill.categoryName} - ${skill.skillName}` }
        }).sort((a, b) => a.name.localeCompare(b.name));
        
        this.IsPrivateUser = this.userProfile.user.isPrivateUser;
        this.titleService.setTitle(`${this.userProfile.user.firstName} ${this.userProfile.user.lastName}`);
      });
  }

  changeFollowing(): void {
    if (this.userProfile.isFollowed) {
      this.httpService
        .delete(
          `${this.userProfileApiRoutes.Followers}/${this.userProfile.userId}`,
          {},
          { id: this.userProfile?.userId }
        )
        .subscribe(() => {
          this.getUser();
        });
    } else {
      this.httpService
        .post(`${this.userProfileApiRoutes.Followers}/${this.userProfile.userId}`, {
          id: this.userProfile?.userId
        })
        .subscribe(() => {
          this.getUser();
        });
    }
  }

  closeInterests(saved: boolean): void {
    this.showInterests = false;

    if (saved) {
      this.updateCurrentUser();
      this.getUser();
    }
  }

  closeSkills(saved: boolean): void {
    this.showSkills = false;

    if (saved) {
      this.updateCurrentUser();
      this.getUser();
    }
  }

  updateRegions(regions: CountryCheckboxInterface[]) {
    this.httpService.post(this.userProfileApiRoutes.UserInterests, { regions }).subscribe(() => {
      this.updateCurrentUser();
      this.getUser();
      this.showRegions = false;
    });
  }

  updateCurrentUser(): void {
    this.authService.getCurrentUser$.next(true);
  }

  onExternalLinkClick(url, text): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.LinkClick, { name: text, url: url })
      ?.subscribe();
  }

  back(): void {
    this.commonService.goBack();
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    const routesFound = this.routesToNotClearFilters.some(val => this.coreService.getOngoingRoute().includes(val));
    if (!routesFound) {
      this.commonService.clearFilters(this.intialFilterState);
      let previousfilterStateKey = this.commonService.getPreviousFilterStateKey();
      if (previousfilterStateKey && sessionStorage.getItem(previousfilterStateKey) != null)
        sessionStorage.removeItem(previousfilterStateKey);
    }
  }

  trackAttachToInitiativeActivity() {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        buttonName: this.translateService.instant('initiative.attachContent.attachCommunityLabel'),
        moduleName: InitiativeModulesEnum[InitiativeModulesEnum.Community]
      })
      ?.subscribe();
  }

  private listenForFilterState(): void {
    this.commonService
      .filterState()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(filterState => {
        if (filterState) {
          this.intialFilterState = filterState;
        }
      });
  }

  clear(): void {
    this.followers = [...this.userProfile.followers];
  }

  searchMembers(search: string): void {
    const searchToLoverCase = search.toLowerCase();
    this.search = search;
    this.followers = this.userProfile.followers.filter(
      user =>
        `${user.firstName} ${user.lastName}`.toLowerCase().includes(searchToLoverCase) ||
        user?.company?.toLowerCase()?.includes(searchToLoverCase)
    );
  }

  showfollowersModal() {
    if (this.userProfile?.followersCount > 0) {
      this.followersModal = true;
    }
  }

  followUser(user: MemberInterface): void {
    this.followingService.followUser(user?.id, user?.isFollowed)
    user.isFollowed = !user.isFollowed;
  }
}
