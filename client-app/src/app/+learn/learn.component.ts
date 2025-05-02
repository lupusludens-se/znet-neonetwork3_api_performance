import { Component, OnDestroy, OnInit } from '@angular/core';

import { filter, Subject, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { TranslateService } from '@ngx-translate/core';
import { HttpService } from '../core/services/http.service';
import { CommonService } from '../core/services/common.service';
import { CoreService } from '../core/services/core.service';
import { TitleService } from '../core/services/title.service';
import { AuthService } from '../core/services/auth.service';
import { ActivityService } from '../core/services/activity.service';

import { PostInterface } from './interfaces/post.interface';
import { FilterStateInterface } from '../shared/modules/filter/interfaces/filter-state.interface';
import { PaginateResponseInterface } from '../shared/interfaces/common/pagination-response.interface';
import { FilterDataInterface } from '../shared/modules/filter/interfaces/filter-data.interface';
import { FilterChildDataInterface } from '../shared/modules/filter/interfaces/filter-child-data.interface';
import { UserInterface } from '../shared/interfaces/user/user.interface';

import { CommonApiEnum } from '../core/enums/common-api.enum';
import { TaxonomyEnum } from '../core/enums/taxonomy.enum';
import { RolesEnum } from '../shared/enums/roles.enum';
import { ActivityTypeEnum } from '../core/enums/activity/activity-type.enum';

import { ALL_ARTICLES, FOR_YOU_PARAMETER, SAVED_PARAMETER } from './constants/parameter.const';
import { ActivatedRoute, Router } from '@angular/router';
import { LearnFiltersInterface } from './interfaces/learn-filter.interface';
import { LearnComponentsEnum } from './enums/learn-component.enum';
import { NetwrokStatsInterface } from '../shared/interfaces/netwrok-stats.interface';
import { CommunityDataService } from '../+community/services/community.data.service';
import { PaginationInterface } from '../shared/modules/pagination/pagination.component';
import { SignTrackingSourceEnum } from '../core/enums/sign-tracking-source-enum';
@UntilDestroy()
@Component({
  templateUrl: './learn.component.html',
  styleUrls: ['./learn.component.scss']
})
export class LearnComponent implements OnInit, OnDestroy {
  postData: PaginateResponseInterface<PostInterface>;
  defaultPerPage: number = 15;
  contactUsModal: boolean;
  initLoad: boolean = true;
  taxonomyEnum = TaxonomyEnum;
  showClearButton: boolean;
  notSolutionUser: boolean = true;
  currentUser: UserInterface;
  searchRegExp: RegExp = new RegExp("^[a-zA-Z\\d_'.\\s]{1,100}$");
  routesEnum: string[] = [`${LearnComponentsEnum.ArticleDetailsComponent}`];
  filters: LearnFiltersInterface = {
    title: this.mainFilter('general.forYouLabel'),
    searchStr: null,
    initialFiltersState: null
  };
  networkStats: NetwrokStatsInterface = null;
  articleCount: number = 0;
  articleCountStop: any;

  private fetchData$: Subject<void> = new Subject<void>();
  private filterStateKey: string;
  resultPending: boolean = true;
  isPublicUser: boolean;
  subscription: any;
  auth = AuthService;
  showBorder: boolean;
  searchValue: string;
  pagination: PaginationInterface = {
    skip: 0,
    take: this.defaultPerPage,
    total: null
  };
  signTrackingSourceEnum = SignTrackingSourceEnum.ZeigoNetwork;
  constructor(
    public readonly commonService: CommonService,
    private readonly httpService: HttpService,
    private readonly coreService: CoreService,
    private readonly translateService: TranslateService,
    private readonly titleService: TitleService,
    private readonly authService: AuthService,
    private readonly activityService: ActivityService,
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute,
    private readonly communityDataService: CommunityDataService
  ) {}

  ngOnInit(): void {
    this.initLoad = true;
    this.titleService.setTitle('learn.learnLabel');
    this.isPublicUser = !(this.auth.isLoggedIn() || this.auth.needSilentLogIn());
    this.subscription = this.authService.currentUser().subscribe(val => {
      this.currentUser = val;
      if (val !== null) {
        this.notSolutionUser = !this.currentUser.roles.some(
          role => role.id === RolesEnum.SolutionProvider || role.id === RolesEnum.SPAdmin
        );
        this.coreService.elementNotFoundData$
          .pipe(
            untilDestroyed(this),
            filter(data => !data)
          )
          .subscribe(() => {
            this.filterStateKey = this.commonService.getFilterStateKey();
            this.commonService.getLearnFilter$().subscribe(val => {
              if (this.initLoad) {
                this.initFilterData(val);
              }
            });
            this.initFilterData();
            this.listenForLoadData();
            this.listenForFilterState();
          });
      } else if (this.isPublicUser) {
        this.communityDataService.getNetorkStats().subscribe(val => {
          this.networkStats = val;
          this.initializeCount();
        });
      }
    });
    this.initLoad = false;
  }

  ngOnDestroy(): void {
    if (this.currentUser !== null) {
      this.fetchData$.next();
      this.fetchData$.complete();
      const routesFound = this.routesEnum.some(val => this.coreService.getOngoingRoute().includes(val));
      if (!routesFound) {
        this.clearFilters(false);
      }
    }
  }
  //#region public methods
  setMainFilterCategory(filterName: string): void {
    this.filters.title = filterName;
    this.clearFilters(false);
  }

  search(value: string): void {
    if (this.currentUser) {
      this.filters.initialFiltersState.search = value;
      this.filters.initialFiltersState.skip = 0;
      this.filters.searchStr = value;

      if (
        (this.filters.title === this.mainFilter('general.savedLabel') ||
          this.filters.title === this.mainFilter('general.forYouLabel')) &&
        value
      ) {
        this.filters.title = this.mainFilter('learn.allArticlesLabel');
      }

      this.commonService.clearFilters(this.filters.initialFiltersState, true, undefined, true, '', true);
    } else {
      this.searchValue = value;
      if (value) {
        this.allArticlesSelectedPage(0);
      } else {
        this.initialPubliUserPage();
      }
    }
  }

  clearFilters(ignoreSearch: boolean = true): void {
    if (!ignoreSearch) {
      this.filters.searchStr = '';
    }

    this.commonService.clearFilters(this.filters.initialFiltersState, ignoreSearch);
  }

  onContactNEONetworkClick(): void {
    this.contactUsModal = true;
    this.activityService.trackElementInteractionActivity(ActivityTypeEnum.ConnectWithNEOClick)?.subscribe();
  }

  onTechnologiesSolutionsClick(name: string): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.TechnologiesSolutionsClick, { buttonName: name })
      ?.subscribe();
  }

  changePage(page: number): void {
    if (this.currentUser) {
      const nextSkip = (page - 1) * this.defaultPerPage;
      if (nextSkip !== this.filters.initialFiltersState.skip) {
        this.filters.initialFiltersState.skip = (page - 1) * this.defaultPerPage;
        this.commonService.filterState$.next(this.filters.initialFiltersState);
      }
    } else this.allArticlesSelectedPage(page - 1);
  }

  private allArticlesSelectedPage(index: number) {
    this.pagination.skip = index * this.defaultPerPage;
    this.resultPending = true;
    this.httpService
      .get<PaginateResponseInterface<PostInterface>>(CommonApiEnum.Articles, {
        filterby: ALL_ARTICLES,
        expand: 'categories,regions,solutions,technologies,contenttags',
        includeCount: true,
        search: this.searchValue,
        ...this.pagination
      })
      .subscribe(post => {
        this.filters.title = this.mainFilter('learn.allArticlesLabel');
        this.resultPending = false;
        this.postData = post;
      });
  }

  private initialPubliUserPage() {
    this.filters.title = this.mainFilter('general.forYouLabel');
    this.resultPending = true;
    this.httpService
      .get<PaginateResponseInterface<PostInterface>>(CommonApiEnum.Articles, {
        filterby: FOR_YOU_PARAMETER,
        expand: 'categories,regions,solutions,technologies,contenttags',
        includeCount: true
      })
      .subscribe(post => {
        this.resultPending = false;
        this.postData = post;
      });
  }

  postClicked(): void {
    sessionStorage.setItem(this.filterStateKey, JSON.stringify(this.filters));
  }
  //#endregion

  //region private methods
  private initFilterData(isFilteredorSearched?: boolean): void {
    if (
      this.commonService.hasFilterSessionData(
        this.activatedRoute,
        this.filterStateKey,
        this.translateService.instant('general.forYouLabel'),
        isFilteredorSearched
      )
    ) {
      this.filters = JSON.parse(sessionStorage.getItem(this.filterStateKey));
      this.filters.initialFiltersState.search = this.filters.searchStr;
      this.commonService.filterState$.next(this.filters.initialFiltersState);
    }
    this.initLoad = false;
    sessionStorage.removeItem(this.filterStateKey);
  }

  private getParams(filterState: FilterStateInterface): FilterStateInterface {
    const params = { ...filterState };

    return {
      ...params,
      parameter: typeof params.parameter === 'object' ? null : params.parameter,
      filterby: this.getQueryString(params?.parameter)
    };
  }

  private getQueryString(params): string {
    return Object.keys(params)
      .map(key => {
        const items = params[key]?.filter(item => !!item.checked || item?.childElements?.length);
        if (items.some(item => item?.childElements)) {
          return LearnComponent.getFilterChildDataQuery(key, items);
        } else if (items.length) {
          return LearnComponent.getFilterDataInterfaceQuery(key, items);
        }
      })
      .filter(item => !!item)
      .join('&');
  }

  private mainFilter(key: string): string {
    return this.translateService.instant(key);
  }

  private static getFilterDataInterfaceQuery(key: string, items: FilterDataInterface[]): string {
    const query = `${key}=${items.map(item => {
      if (item.checked) {
        return item.id;
      }
    })}`;

    return query.match(/\d/g) ? query : '';
  }

  private static getFilterChildDataQuery(key: string, items: FilterChildDataInterface[]): string {
    const selections = items
      .map(item => {
        if (item.checked && item?.childElements.some(item => item.checked && !item.disabled)) {
          const selectedChildrenIds = item.childElements
            .filter(child => !!child.checked && !child.disabled)
            .map(child => child.id);
          return [item.id, ...selectedChildrenIds];
        } else if (item.checked && !item?.childElements.some(item => item.checked && !item.disabled)) {
          return item.id;
        } else if (item?.childElements.some(item => item.checked && !item.disabled)) {
          return item.childElements.filter(child => !!child.checked).map(child => child.id);
        }
      })
      .filter(selection => !!selection);

    const query = `${key}=${selections.toString()}`;
    return query.match(/\d/g) ? query : '';
  }

  private updateQueryParameters(): void {
    let queryParams = { ...this.activatedRoute.snapshot.queryParams };
    if (!this.hasFilterSelected()) {
      delete queryParams.filtered;
    } else {
      queryParams.filtered = 'true';
    }

    if (this.activatedRoute.snapshot.queryParams.filtered !== queryParams.filtered) {
      this.router.navigate([], { queryParams });
    }
    this.commonService.setLearnFilter(queryParams?.filtered);
  }

  private hasFilterSelected(): boolean {
    return (
      this.filters.title === this.translateService.instant('general.savedLabel') ||
      this.filters.title === this.translateService.instant('learn.allArticlesLabel') ||
      !!this.filters.initialFiltersState.search ||
      this.commonService.isAnyFilterChecked(this.filters.initialFiltersState)
    );
  }
  //#endregion

  //#region observers
  private listenForLoadData(): void {
    this.resultPending = true;
    this.fetchData$
      .pipe(
        untilDestroyed(this),
        filter(
          () =>
            this.filters.title === this.mainFilter('learn.allArticlesLabel') ||
            this.filters.title === this.mainFilter('general.savedLabel')
        ),
        switchMap(() => {
          const parameters = this.getParams(this.filters.initialFiltersState);

          if (this.filters.title === this.mainFilter('general.savedLabel')) {
            parameters.filterby = SAVED_PARAMETER;
          }

          this.showClearButton = this.commonService.isAnyFilterChecked(this.filters.initialFiltersState);

          if (this.filters.title === this.mainFilter('learn.allArticlesLabel') && !this.showClearButton) {
            parameters.filterby = ALL_ARTICLES;
            parameters.orderby = 'modified.desc';
            parameters.search = this.filters.searchStr;
          }

          this.resultPending = true;

          return this.httpService.get<PaginateResponseInterface<PostInterface>>(
            `${CommonApiEnum.Articles}`,
            this.coreService.deleteEmptyProps({
              ...parameters,
              includeCount: true,
              expand: 'categories,regions,solutions,technologies,contenttags',
              skip: this.filters.initialFiltersState?.skip ? this.filters.initialFiltersState?.skip : 0,
              take: this.filters.initialFiltersState?.take
                ? this.filters.initialFiltersState?.take
                : this.defaultPerPage
            })
          );
        })
      )
      .subscribe(post => {
        this.resultPending = false;
        this.postData = post;
      });
  }

  private listenForFilterState(): void {
    this.commonService
      .filterState()
      .pipe(
        untilDestroyed(this),
        filter(initialFiltersState => !!initialFiltersState)
      )
      .subscribe(initialFiltersState => {
        this.filters.initialFiltersState = initialFiltersState;
        this.filters.initialFiltersState.search = this.filters.searchStr;
        const isAnyFilterChecked = this.commonService.isAnyFilterChecked(this.filters.initialFiltersState);
        this.updateQueryParameters();
        if (isAnyFilterChecked) {
          this.filters.title = this.mainFilter('learn.allArticlesLabel');
        }

        if (
          this.filters.title === this.mainFilter('general.savedLabel') ||
          this.filters.title === this.mainFilter('learn.allArticlesLabel')
        ) {
          this.fetchData$.next();
        }
      });
  }

  initializeCount() {
    this.articleCount = Math.ceil(this.networkStats?.totalArticleCount - this.networkStats?.totalArticleCount * 0.35);
    this.articleCountStop = setInterval(() => {
      this.articleCount = this.articleCount + 20;
      if (this.articleCount >= this.networkStats?.totalArticleCount) {
        clearInterval(this.articleCountStop);
        this.articleCount = this.networkStats?.totalArticleCount;
      }
    }, 100);
  }
}
