import { ChangeDetectorRef, Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { catchError, forkJoin, Observable, of, Subject, switchMap, takeUntil, throwError } from 'rxjs';

import { TitleService } from '../core/services/title.service';
import { HttpService } from '../core/services/http.service';
import { FollowingService } from '../core/services/following.service';

import { CompanyProfileInterface, CompanyType } from './interfaces/company-profile.interface';
import { PaginateResponseInterface } from '../shared/interfaces/common/pagination-response.interface';
import { MemberInterface } from '../shared/modules/members-list/interfaces/member.interface';
import { UserInterface } from '../shared/interfaces/user/user.interface';

import { UserProfileApiEnum } from '../shared/enums/api/user-profile-api.enum';
import { TaxonomyTypeEnum } from '../shared/enums/taxonomy-type.enum';
import { RolesEnum } from '../shared/enums/roles.enum';
import { AuthService } from '../core/services/auth.service';
import { CommonService } from '../core/services/common.service';
import { ActivityTypeEnum } from '../core/enums/activity/activity-type.enum';
import { ActivityService } from '../core/services/activity.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CoreService } from '../core/services/core.service';
import { CompanyApiEnum } from '../shared/enums/api/company-api-enum';
import {
  PaginationIncludeCountInterface,
  PaginationInterface
} from '../shared/modules/pagination/pagination.component';
import { FileInterface } from '../shared/interfaces/file.interface';
import { SPCompanyProjectInterface } from '../shared/interfaces/projects/project.interface';
import { UserRoleInterface } from '../shared/interfaces/user/user-role.interface';
import { PermissionService } from '../core/services/permission.service';
import { SaveContentService } from '../shared/services/save-content.service';
import { environment } from 'src/environments/environment';
import { FilterStateInterface } from '../shared/modules/filter/interfaces/filter-state.interface';
import { CommunityComponentEnum } from '../+community/enums/community-component.enum';

@Component({
  selector: 'neo-company-profile',
  templateUrl: 'company-profile.component.html',
  styleUrls: ['./company-profile.component.scss']
})
export class CompanyProfileComponent implements OnInit, OnDestroy {
  type = TaxonomyTypeEnum;
  companyTypes = CompanyType;
  userRoles = RolesEnum;
  company: CompanyProfileInterface;
  employees: MemberInterface[];
  currentUser: UserInterface;
  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  publicFiles: FileInterface[] = [];
  privateFiles: FileInterface[] = [];
  routesToNotClearFilters: string[] = [CommunityComponentEnum.CommunityComponent];
  paging: PaginationIncludeCountInterface = {
    take: 3,
    skip: 0,
    total: null,
    includeCount: true
  };

  projectsList: SPCompanyProjectInterface[] = [];

  private dataLoading$: Subject<void> = new Subject<void>();
  private unsubscribe$: Subject<void> = new Subject<void>();

  private filesDataLoading$: Subject<void> = new Subject<void>();
  private projectsLoading$: Subject<void> = new Subject<void>();
  enableLiveProjectsSection: boolean = false;
  intialFilterState: FilterStateInterface;
  isViewAllProjectsClicked: boolean = false;
  followersModal: boolean = false;
  companyFollowers: MemberInterface[];
  search: string;
  @Output() followClick: EventEmitter<MemberInterface> = new EventEmitter<MemberInterface>();
  constructor(
    private readonly titleService: TitleService,
    private readonly httpService: HttpService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly followingService: FollowingService,
    private readonly authService: AuthService,
    private readonly commonService: CommonService,
    private readonly router: Router,
    private readonly activityService: ActivityService,
    private readonly coreService: CoreService,
    private readonly cdr: ChangeDetectorRef,
    private readonly permissionService: PermissionService,
    private readonly saveContentService: SaveContentService
  ) {}

  ngOnInit(): void {
    this.listedForPageDataLoading();
    this.listenForUserFollowing();
    this.listenForCompanyFollowing();
    this.listenForCompanyProjects();
    this.listedForFilesDataLoading();
    this.listenForFilterState();

    this.activatedRoute.params.pipe(takeUntil(this.unsubscribe$)).subscribe(() => this.dataLoading$.next());
    this.authService.currentUser().subscribe(user => {
      if (user) {
        this.currentUser = user;
        this.filesDataLoading$.next();
      }
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    this.dataLoading$.next();
    this.dataLoading$.complete();
    const routesFound = this.routesToNotClearFilters.some(val => this.coreService.getOngoingRoute().includes(val));
    if (!routesFound) {
      const ignoreSearch = this.isViewAllProjectsClicked === true;
      this.commonService.clearFilters(this.intialFilterState, ignoreSearch);
      let previousfilterStateKey = this.commonService.getPreviousFilterStateKey();
      if (previousfilterStateKey && sessionStorage.getItem(previousfilterStateKey) != null)
        sessionStorage.removeItem(previousfilterStateKey);
    }
  }

  followUser(user: MemberInterface): void {
    this.followingService.followUser(user?.id, user?.isFollowed);
    user.isFollowed = !user.isFollowed;
  }

  followCompany(unFollow?: boolean) {
    this.followingService.followCompany(this.company.id, unFollow);
  }

  isInRole(currentUser: UserInterface, roleId: RolesEnum): boolean {
    return currentUser?.roles.some(role => role.id === roleId && role.isSpecial);
  }

  viewProjects(): void {
    this.commonService.filterState$.value.search = this.company.name;
    this.isViewAllProjectsClicked = true;
    this.router.navigate(['projects']);
  }

  onExternalLinkClick(url, text): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.LinkClick, { name: text, url: url })
      ?.subscribe();
  }

  goBack() {
    this.commonService.goBack();
  }

  private listenForUserFollowing(): void {
    this.followingService
      .followedUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(val => {
        this.employees.forEach(e => {
          if (e.id === val.userId) {
            e.isFollowed = !val.unFollow;
            if (val.unFollow) {
              e.followersCount--;
            } else {
              e.followersCount++;
            }
          }
        });
      });
  }

  private listenForCompanyFollowing(): void {
    this.followingService
      .followedCompany()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(unfollow => {
        if (unfollow) {
          this.company.followersCount--;
        } else {
          this.company.followersCount++;
        }
        this.httpService
          .get<CompanyProfileInterface>(`${CompanyApiEnum.Companies}/${this.activatedRoute.snapshot.params.id}`, {
            expand: 'image,categories,urllinks,followers.user.roles.images.company'
          })
          .subscribe(companyInfo => {
            this.company.isFollowed = !unfollow;
            this.company = companyInfo;
            this.companyFollowers = this.company.followers.map(y => y.follower);
          });
      });
  }

  private listedForPageDataLoading(): void {
    this.dataLoading$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() =>
          forkJoin([
            this.httpService.get<CompanyProfileInterface>(
              `${CompanyApiEnum.Companies}/${this.activatedRoute.snapshot.params.id}`,
              {
                expand: 'image,categories,urllinks,followers.user.roles.images.company'
              }
            ),
            this.httpService.get<PaginateResponseInterface<UserInterface>>(UserProfileApiEnum.Users, {
              expand: 'image,userprofile,userprofile.followed,userprofile.state,country',
              filterBy: `companyIds=${this.activatedRoute.snapshot.params.id}&statusIds=2`
            })
          ])
        ),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            this.router.navigate(['/community']);
            this.coreService.elementNotFoundData$.next({
              iconKey: 'building',
              mainTextTranslate: 'companyProfile.notFoundText',
              buttonTextTranslate: 'companyProfile.notFoundButton',
              buttonLink: '/community'
            });
          }

          return throwError(error);
        })
      )
      .subscribe(([companyInfo, users]) => {
        this.company = companyInfo;
        this.companyFollowers = this.company.followers.map(y => y.follower);

        this.titleService.setTitle(this.company.name);

        this.employees = users.dataList
          .map(user => ({
            id: user.id,
            firstName: user.firstName,
            lastName: user.lastName,
            image: users.dataList.find(userData => userData.id === user.id)?.image,
            isFollowed: users.dataList.find(userData => userData.id === user.id)?.userProfile?.isFollowed,
            followersCount: users.dataList.find(userData => userData.id === user.id)?.userProfile?.followersCount,
            company: users.dataList.find(userData => userData.id === user.id)?.userProfile?.jobTitle,
            statusId: user.statusId,
            isPrivateUser: user.isPrivateUser,
            userProfile: users.dataList.find(userData => userData.id === user.id)?.userProfile,
            country: user.country
          }))
          .filter(x => !x.isPrivateUser)
          .sort((a, b) => (a.firstName > b.firstName ? 1 : -1));
        if (
          this.company?.typeId === this.companyTypes.SolutionProvider &&
          (!(
            this.isInRole(this.currentUser, this.userRoles.SolutionProvider) ||
            this.isInRole(this.currentUser, this.userRoles.SPAdmin)
          ) ||
            ((this.isInRole(this.currentUser, this.userRoles.SolutionProvider) ||
              this.isInRole(this.currentUser, this.userRoles.SPAdmin)) &&
              this.currentUser.companyId.toString() === this.company.id.toString()))
        ) {
          this.projectsLoading$.next();
        }
      });
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

  private listedForFilesDataLoading(): void {
    this.filesDataLoading$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          const companyId = this.activatedRoute.snapshot.params.id;
          const pagingParam: PaginationInterface = { take: 5, skip: 0, total: null };

          const createPagingParams = (isPublic: boolean) =>
            this.coreService.deleteEmptyProps({
              ...pagingParam,
              FilterBy: null,
              includeCount: true
            });

          const privateFilesObservable =
            this.isInRole(this.currentUser, RolesEnum.SPAdmin) ||
            this.isInRole(this.currentUser, RolesEnum.Admin) ||
            this.isInRole(this.currentUser, RolesEnum.SolutionProvider)
              ? this.httpService.get<PaginateResponseInterface<FileInterface>>(
                  CompanyApiEnum.GetCompaniesPrivateFiles + `/${companyId}/`,
                  createPagingParams(false)
                )
              : of(null);

          return forkJoin([
            this.httpService.get<PaginateResponseInterface<FileInterface>>(
              CompanyApiEnum.GetCompaniesPublicFiles + `/${companyId}`,
              createPagingParams(true)
            ),
            privateFilesObservable
          ]);
        })
      )
      .subscribe(([publicFiles, privateFiles]) => {
        this.publicFiles = publicFiles?.dataList || [];
        this.privateFiles = privateFiles?.dataList || [];
      });
  }

  loadFiles(status: boolean): void {
    if (status) {
      this.filesDataLoading$.next();
    }
  }

  private listenForCompanyProjects(): void {
    this.projectsLoading$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          const companyId = this.activatedRoute.snapshot.params.id;
          return this.httpService.get<PaginateResponseInterface<SPCompanyProjectInterface>>(
            CompanyApiEnum.GetSPCompanyActiveProjects + `/${companyId}`,
            {
              expand: 'image, category, owner, regions, saved',
              take: this.paging?.take,
              skip: this.paging?.skip,
              includeCount: true
            }
          );
        })
      )
      .subscribe(projects => {
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip || 0,
          total: projects.count
        };
        this.projectsList = projects?.dataList;
        this.setImageValueForProjects(this.projectsList);
        this.enableLiveProjectsSection = this.projectsList?.length > 0;
      });
  }

  updatePaging(page: number): void {
    const skip: number = (page - 1) * 3;
    this.paging.skip = skip;
    this.projectsLoading$.next();
  }

  isAdmin(roles: UserRoleInterface[]): boolean {
    return roles?.some(role => role.id === RolesEnum.Admin && role.isSpecial);
  }

  saveProject(project: SPCompanyProjectInterface): void {
    this.saveContentService.saveProject(project.id).subscribe(() => (project.isSaved = true));
  }

  deleteSavedProject(project: SPCompanyProjectInterface): void {
    this.saveContentService.deleteProject(project.id).subscribe(() => {
      project.isSaved = false;
    });
  }

  setImageValueForProjects(projects: SPCompanyProjectInterface[]) {
    projects.forEach(project => {
      if (project.technologies.length > 0) {
        project.imageUrl = `${environment.baseAppUrl}assets/images/project-technologies-images/${project.technologies[0].slug}/${project.projectCategoryImage}.jpg`;
      } else {
        project.imageUrl = `${environment.baseAppUrl}assets/images/project-categories-images/${project.projectCategorySlug}/${project.projectCategoryImage}.jpg`;
      }
    });
  }

  searchMembers(search: string): void {
    const searchToLoverCase = search.toLowerCase();
    this.search = search;
    this.companyFollowers = this.company.followers
      .filter(
        user =>
          `${user.follower.firstName} ${user.follower.lastName}`.toLowerCase().includes(searchToLoverCase) ||
          user.follower?.company?.toLowerCase()?.includes(searchToLoverCase)
      )
      .map(follower => follower.follower);
  }

  clear(): void {
    this.companyFollowers = this.company.followers.map(follower => follower.follower);
  }

  showfollowersModal() {
    if (this.company?.followersCount > 0) {
      this.followersModal = true;
    }
  }
}
