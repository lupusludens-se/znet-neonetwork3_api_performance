import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { filter, first, Subject, switchMap, takeUntil } from 'rxjs';

import { CommonService } from '../core/services/common.service';
import { TitleService } from '../core/services/title.service';
import { CommunityDataService } from './services/community.data.service';
import { TranslateService } from '@ngx-translate/core';

import {
  CommunityType,
  COMPANIES_OPTIONS,
  COMPANIES_OPTIONS_ALL,
  COMPANIES_OPTIONS_CORPORATIONS,
  FOLLOWING_SORT_OPTIONS,
  PEOPLE_OPTIONS
} from './constants/parameter.const';
import { DEFAULT_PER_PAGE } from '../shared/modules/pagination/pagination.component';
import { TagInterface } from '../core/interfaces/tag.interface';
import { PaginateResponseInterface } from '../shared/interfaces/common/pagination-response.interface';
import { FilterDataInterface } from '../shared/modules/filter/interfaces/filter-data.interface';
import { FilterChildDataInterface } from '../shared/modules/filter/interfaces/filter-child-data.interface';
import { FilterStateInterface } from '../shared/modules/filter/interfaces/filter-state.interface';
import { CommunityInterface } from './interfaces/community.interface';
import { CommunitySearchInterface } from './interfaces/community.search.interface';

import { TaxonomyEnum } from '../core/enums/taxonomy.enum';
import { FormControl, FormGroup } from '@angular/forms';
import { CommunityFiltersInterface } from './interfaces/community-filters.interface';
import { CoreService } from '../core/services/core.service';
import { CommunityComponentEnum } from './enums/community-component.enum';
import { NetwrokStatsInterface } from '../shared/interfaces/netwrok-stats.interface';
import { AuthService } from '../core/services/auth.service';
import { UserInterface } from '../shared/interfaces/user/user.interface';
import { ViewportScroller } from '@angular/common';
import { RolesEnum } from '../shared/enums/roles.enum';
import { SortingOptionsKeyValuePair } from '../shared/modules/sort-dropdown/sort-dropdown.component';
import { TaxonomyFilterComponent } from '../shared/modules/filter/components/taxonomy-filter/taxonomy-filter.component';
import { TaxonomyHierarchicalFilterComponent } from '../shared/modules/filter/components/taxonomy-hierarchical-filter/taxonomy-hierarchical-filter.component';
import { FilterExpandComponent } from '../shared/modules/filter/components/filter-expand/filter-expand.component';

@Component({
  templateUrl: './community.component.html',
  styleUrls: ['./community.component.scss']
})
export class CommunityComponent implements OnInit, OnDestroy {
  communityData: CommunityInterface[];
  networkStats: NetwrokStatsInterface;
  companiesOptions: TagInterface[] = COMPANIES_OPTIONS;
  peopleOptions: TagInterface[] = PEOPLE_OPTIONS;
  routesToNotClearFilters: string[] = [
    `${CommunityComponentEnum.UserProfile}`,
    `${CommunityComponentEnum.CompanyProfile}`
  ];
  peopleTypeForm: FormGroup = new FormGroup({
    peopleControl: new FormControl(null)
  });
  companiesTypeForm: FormGroup = new FormGroup({
    control: new FormControl(null)
  });
  peopleType = RolesEnum;
  taxonomyEnum = TaxonomyEnum;
  showClearButton: boolean;
  currentUser: UserInterface;
  updatePage: boolean = false;

  defaultItemPerPage = DEFAULT_PER_PAGE;

  filterStateKey: string;
  unCheckFilterName: string;
  filters: CommunityFiltersInterface = {
    companiesSelected: null,
    peopleSelectedOptions: null,
    initialFiltersState: null,
    disableIndustries: false,
    disableRegions: false,
    disableCompanies: false,
    disablePeopleFilter: false,
    disableProjectsUserFilter: false,
    disableProjectsCompanyFilter: false,
    listActive: true,
    followingFilterTitle: '',
    companiesClassActive: false,
    peopleFilterClassActive: false,
    title: this.translateService.instant('projects.suggestedForYouLabel'),
    forYouFilterTitle: this.translateService.instant('general.forYouLabel'),
    paging: null,
    followingSelectedSortOrder: 'all'
  };

  loadForYou$: Subject<void> = new Subject<void>();
  loadFollowing$: Subject<void> = new Subject<void>();
  loadCompanies$: Subject<void> = new Subject<void>();
  loadAllPeople$: Subject<void> = new Subject<void>();
  loadCommunity$: Subject<void> = new Subject<void>();

  private unsubscribe$: Subject<void> = new Subject<void>();
  subscription: any;
  auth = AuthService;

  followingSortingOptions: SortingOptionsKeyValuePair[] = FOLLOWING_SORT_OPTIONS;
  @ViewChild('industryFilter') industryFilterComponent: TaxonomyFilterComponent;
  @ViewChild('regionFilter') regionFilterComponent: TaxonomyHierarchicalFilterComponent;
  @ViewChild('categoryFilter') categoryFilterComponent: TaxonomyFilterComponent;
  @ViewChild('projectCapabilitiesFilter') projectCapabilitiesFilterComponenet: TaxonomyFilterComponent;
  @ViewChild('companyExpandFilter') companyExpandFilterComp: FilterExpandComponent;
  @ViewChild('peopleExpandFilter') peopleExpandFilterComp: FilterExpandComponent;
  constructor(
    private readonly commonService: CommonService,
    private readonly titleService: TitleService,
    private readonly communityDataService: CommunityDataService,
    private readonly translateService: TranslateService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly router: Router,
    private readonly coreService: CoreService,
    private readonly authService: AuthService,
    private viewPort: ViewportScroller
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle('title.communityLabel');
    this.subscription = this.authService.currentUser().subscribe(val => {
      this.currentUser = val;
      if (val !== null) {
        this.coreService.elementNotFoundData$
          .pipe(
            takeUntil(this.unsubscribe$),
            filter(data => !data)
          )
          .subscribe(() => {
            this.filterStateKey = this.commonService.getFilterStateKey();
            this.initFilterData();
            this.listenForYou();
            this.listenFollowing();
            this.listenCompanies();
            this.listenAllPeople();
            this.listenToLoadCommunity();
            this.listenForFilterState();
          });
      }
    });
  }

  ngOnDestroy(): void {
    if (this.currentUser) {
      this.unsubscribe$.next();
      this.unsubscribe$.complete();

      this.filters.disableIndustries = false;
      this.filters.disableRegions = false;
      this.filters.disableProjectsUserFilter = false;
      this.unCheckFilterName = '';
      const routesFound = this.routesToNotClearFilters.some(val => this.coreService.getOngoingRoute().includes(val));
      if (!routesFound) {
        this.clearFilters(false);
        sessionStorage.removeItem(this.filterStateKey);
      }
    }
  }

  clearAll() {
    this.filters.title = this.translateService.instant('projects.suggestedForYouLabel');
    this.filters.companiesSelected = null;
    this.filters.companiesClassActive = false;
    this.companiesTypeForm.get('control').setValue(null, { emitEvent: false });
    this.filters.peopleSelectedOptions = null;
    this.peopleTypeForm.get('peopleControl').setValue(null, { emitEvent: false });
    this.filters.peopleFilterClassActive = false;
    this.clearFilters();
  }

  clearFilters(
    ignoreSearch: boolean = true,
    ignoreItemUnCheck: boolean = true,
    optionalFilter: string = this.translateService.instant('secondaryFilters.clearAllFilterLabel'),
    skipFilter: string = null
  ): void {
    this.commonService.clearFilters(
      this.filters.initialFiltersState,
      ignoreSearch,
      undefined,
      ignoreItemUnCheck,
      optionalFilter,
      false,
      skipFilter
    );
    this.changeIndustries(this.filters.disableIndustries);
    this.changeRegions(this.filters.disableRegions);
    this.changeProjects(this.filters.disableProjectsUserFilter);
    this.changeProjectCapabilities(this.filters.disableProjectsCompanyFilter);
  }

  search(value: string): void {
    this.filters.initialFiltersState.search = value;
    this.filters.paging.skip = 0;
    this.clearFilters(true, true, '');
  }

  updatePaging(page: number): void {
    const nextSkip = (page - 1) * this.defaultItemPerPage;
    if (nextSkip !== this.filters.paging.skip) {
      this.filters.paging.skip = nextSkip;
      this.updatePage = true;
      this.commonService.filterState$.next(this.filters.initialFiltersState);
    }
  }

  applyFilter(filterName: string): void {
    switch (filterName) {
      case this.translateService.instant('general.forYouLabel'): {
        this.filters.title = this.translateService.instant('projects.suggestedForYouLabel');
        this.filters.followingFilterTitle = '';
        this.unCheckFilterName = '';
        break;
      }
      case this.translateService.instant('general.followingBtnUpper'): {
        this.filters.title = this.translateService.instant('general.followingBtnUpper');
        this.filters.forYouFilterTitle = '';
        this.filters.followingFilterTitle = this.translateService.instant('general.followingBtnUpper');
        this.unCheckFilterName = '';
        break;
      }
    }
    this.filters.disableIndustries = false;
    this.filters.paging.skip = 0;
    this.filters.disableRegions = false;
    this.filters.disableProjectsUserFilter = false;
    this.filters.companiesSelected = null;
    this.filters.companiesClassActive = false;
    this.filters.peopleSelectedOptions = null;
    this.filters.peopleFilterClassActive = false;
    this.filters.listActive = true;
    this.clearFilters(true, false);
  }

  optionsChangeForPeopleFilter(selectedOption: TagInterface): void {
    this.filters.paging.skip = 0;
    if (!this.filters.initialFiltersState) {
      this.filters.initialFiltersState = {
        parameter: '',
        search: ''
      };
    }
    //disabling all other filters which not required, when any people filter selected
    this.filters.disableIndustries = true;
    this.filters.disableRegions = false;
    this.filters.disableProjectsUserFilter = false;
    this.filters.disableProjectsCompanyFilter = true;
    this.filters.disableCompanies = true;
    this.filters.companiesSelected = null;
    this.filters.peopleSelectedOptions = selectedOption;
    this.filters.companiesClassActive = false;
    this.filters.followingFilterTitle = '';
    this.filters.peopleFilterClassActive = true;
    this.filters.listActive = true;
    this.filters.title = this.translateService.instant(
      selectedOption?.id === RolesEnum.All
        ? 'community.allUsersLabel'
        : selectedOption?.id === RolesEnum.Corporation
        ? 'community.corporationsLabel'
        : 'community.solutionProvidersLabel'
    );
    this.filters.forYouFilterTitle = '';
    this.unCheckFilterName = '';
    this.clearFilters(true, false, this.translateService.instant('secondaryFilters.industriesFilterLabel'));
    this.industryFilterComponent?.collapseFilter();
    this.projectCapabilitiesFilterComponenet?.collapseFilter();
    this.companyExpandFilterComp?.collapseFilter();
  }

  companiesChange(selectedOption: TagInterface): void {
    this.filters.paging.skip = 0;
    this.filters.disableRegions = true;
    this.filters.disableIndustries = false;
    this.filters.disableProjectsUserFilter = true;
    this.filters.disableProjectsCompanyFilter = selectedOption?.id === COMPANIES_OPTIONS_CORPORATIONS;
    this.filters.companiesSelected = selectedOption;
    this.filters.companiesClassActive = true;
    this.filters.followingFilterTitle = '';
    this.filters.peopleSelectedOptions = null;
    this.filters.peopleFilterClassActive = false;
    this.filters.disablePeopleFilter = true;
    this.filters.title = this.translateService.instant(
      selectedOption?.id === COMPANIES_OPTIONS_ALL
        ? 'community.allCompaniesLabel'
        : selectedOption?.id === COMPANIES_OPTIONS_CORPORATIONS
        ? 'community.corporationCompniesLabel'
        : 'community.solutionProviderCompaniesLabel'
    );
    this.filters.forYouFilterTitle = '';
    this.clearFilters(true, false, null, this.translateService.instant('secondaryFilters.industriesFilterLabel'));
    if (selectedOption?.id === COMPANIES_OPTIONS_CORPORATIONS)
      this.projectCapabilitiesFilterComponenet.collapseFilter();
    this.regionFilterComponent?.collapseFilter();
    this.categoryFilterComponent?.collapseFilter();
    this.peopleExpandFilterComp?.collapseFilter();
  }

  itemClicked(): void {
    sessionStorage.setItem(this.filterStateKey, JSON.stringify(this.filters));
    this.commonService.setPreviousFilterStateKey(this.filterStateKey);
  }

  private initFilterData(): void {
    let sessionData = sessionStorage.getItem(this.filterStateKey);
    if (!!sessionData || (!!this.activatedRoute.snapshot.queryParams.filtered && sessionData !== null)) {
      this.filters = JSON.parse(sessionStorage.getItem(this.filterStateKey));

      if (this.filters.companiesSelected) {
        this.companiesTypeForm.get('control').setValue(this.filters.companiesSelected?.id);
      }

      if (this.filters.peopleSelectedOptions) {
        this.peopleTypeForm.get('peopleControl').setValue(this.filters.peopleSelectedOptions?.id);
      }

      this.updatePage = true;
      this.commonService.filterState$.next(this.filters.initialFiltersState);
    } else {
      this.listenToQueryParamsChange();
    }

    sessionStorage.removeItem(this.filterStateKey);
  }

  private listenForYou(): void {
    this.loadForYou$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          const parameters = this.getParams(this.filters.initialFiltersState);
          this.showClearButton = false;
          const searchObject: CommunitySearchInterface = {
            skip: this.filters.paging?.skip,
            orderBy: 'asc',
            filterBy: 'foryou',
            search: parameters.search
          };

          return this.communityDataService.getCommunityList(searchObject);
        })
      )
      .subscribe(communityList => {
        this.fillCommunityData(communityList);
      });
  }

  private changeIndustries(value: boolean) {
    if (this.filters.initialFiltersState.parameter) {
      const industries = this.filters.initialFiltersState.parameter[TaxonomyEnum.industries] as FilterDataInterface[];
      industries.forEach(i => (i.disabled = value));
    }
  }

  private changeRegions(value: boolean) {
    if (this.filters.initialFiltersState.parameter) {
      const regions = this.filters.initialFiltersState.parameter[TaxonomyEnum.regions] as FilterChildDataInterface[];
      regions.forEach(r => {
        r.disabled = value;
        r.childElements?.forEach(ch => (ch.disabled = value));
      });
    }
  }

  private changeProjects(value: boolean) {
    if (this.filters.initialFiltersState.parameter) {
      const projects = this.filters.initialFiltersState.parameter[
        TaxonomyEnum.categories
      ] as FilterChildDataInterface[];
      projects.forEach(r => {
        r.disabled = value;
        r.childElements?.forEach(ch => (ch.disabled = value));
      });
    }
  }

  private changeProjectCapabilities(value: boolean) {
    if (this.filters.initialFiltersState.parameter) {
      const projectCapabilities = this.filters.initialFiltersState.parameter[
        TaxonomyEnum.projectCapabilities
      ] as FilterChildDataInterface[];
      projectCapabilities.forEach(r => {
        r.disabled = value;
        r.childElements?.forEach(ch => (ch.disabled = value));
      });
    }
  }

  private listenFollowing(): void {
    let searchStr = '';
    this.loadFollowing$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          const parameters = this.getParams(this.filters.initialFiltersState);
          const filterParameter = parameters.parameter;
          searchStr = this.filters.initialFiltersState.search ?? '';
          this.showClearButton = this.commonService.isAnyFilterChecked(this.filters.initialFiltersState);
          const searchObject: CommunitySearchInterface = {
            skip: this.filters.paging?.skip,
            orderBy: 'asc',
            filterBy: `onlyfollowed${!filterParameter?.length ? '' : '&' + filterParameter}`,
            search: parameters.search
          };
          if (this.filters.followingSelectedSortOrder !== 'all') {
            searchObject.orderBy = this.filters.followingSelectedSortOrder;
          }

          return this.communityDataService.getCommunityList(searchObject);
        })
      )
      .subscribe(communityList => {
        if (!this.filters.initialFiltersState.search) this.filters.initialFiltersState.search = searchStr;
        this.fillCommunityData(communityList);
      });
  }

  private listenCompanies(): void {
    let searchStr = '';
    this.loadCompanies$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          searchStr = this.filters.initialFiltersState.search ?? '';
          const parameters = this.getParams(this.filters.initialFiltersState);

          this.showClearButton = this.commonService.isAnyFilterChecked(this.filters.initialFiltersState);
          const searchObject: CommunitySearchInterface = {
            skip: this.filters.paging?.skip,
            orderBy: 'asc',
            filterBy:
              parameters.parameter +
              (!parameters.parameter?.length ? '' : '&') +
              `communityItemType=${CommunityType.Company}`,
            search: parameters.search
          };
          if (this.filters.companiesSelected?.id !== COMPANIES_OPTIONS_ALL)
            searchObject.filterBy += '&companyType=' + this.filters.companiesSelected?.id;

          return this.communityDataService.getCommunityList(searchObject);
        })
      )
      .subscribe(communityList => {
        if (!this.filters.initialFiltersState.search) this.filters.initialFiltersState.search = searchStr;
        this.fillCommunityData(communityList);
      });
  }

  private listenAllPeople(): void {
    let searchStr = '';
    this.loadAllPeople$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          searchStr = this.filters.initialFiltersState.search ?? '';
          const parameters = this.getParams(this.filters.initialFiltersState);

          this.showClearButton = this.commonService.isAnyFilterChecked(this.filters.initialFiltersState);
          const searchObject: CommunitySearchInterface = {
            skip: this.filters.paging?.skip,
            orderBy: 'asc',
            filterBy:
              parameters.parameter +
              (!parameters.parameter?.length ? '' : '&') +
              `communityItemType=${CommunityType.User}`,
            search: parameters.search
          };
          if (this.filters.peopleSelectedOptions?.id !== this.peopleType.All) {
            let roleTypeIds =
              this.filters.peopleSelectedOptions?.id === this.peopleType.Corporation
                ? this.peopleType.Corporation + ',' + this.peopleType.Internal
                : this.peopleType.SPAdmin + ',' + this.peopleType.SolutionProvider;
            searchObject.filterBy += '&roletypes=' + roleTypeIds;
          }

          return this.communityDataService.getCommunityList(searchObject);
        })
      )
      .subscribe(communityList => {
        if (!this.filters.initialFiltersState.search) this.filters.initialFiltersState.search = searchStr;
        this.fillCommunityData(communityList);
      });
  }

  private listenForFilterState(): void {
    this.commonService
      .filterState()
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(initialFiltersState => !!initialFiltersState)
      )
      .subscribe(filterState => {
        let searchStr = filterState ? filterState?.search : '';
        this.filters.initialFiltersState = filterState;
        if (this.filters.initialFiltersState) this.filters.initialFiltersState.search = searchStr;

        if (!this.updatePage && this.filters.paging) {
          this.filters.paging.skip = 0;
        } else {
          this.updatePage = false;
        }

        const isAllUsChecked = this.commonService.isAllUsChecked(this.filters.initialFiltersState);
        this.updateQueryParameters();

        this.commonService.toggleUsStateOptions(this.filters.initialFiltersState, !isAllUsChecked);

        switch (this.filters.title) {
          case this.translateService.instant('projects.suggestedForYouLabel'): {
            if (
              this.commonService.isAnyFilterChecked(this.filters.initialFiltersState) ||
              this.filters.initialFiltersState.search
            ) {
              this.filters.title = this.translateService.instant('projects.filterResultsLabel');
              this.checkandEnableBasedOnFilterSelection();
              this.filters.forYouFilterTitle = '';
              this.loadCommunity$.next();
            } else {
              this.filters.forYouFilterTitle = this.translateService.instant('general.forYouLabel');
              this.industryFilterComponent?.collapseFilter();
              this.regionFilterComponent?.collapseFilter();
              this.categoryFilterComponent?.collapseFilter();
              this.projectCapabilitiesFilterComponenet?.collapseFilter();
              this.companyExpandFilterComp?.collapseFilter();
              this.peopleExpandFilterComp?.collapseFilter();
              this.loadForYou$.next();
            }
            break;
          }
          case this.translateService.instant('general.followingBtnUpper'): {
            if (
              this.commonService.isAnyFilterChecked(this.filters.initialFiltersState) ||
              this.filters.initialFiltersState.search
            ) {
              this.filters.title = this.translateService.instant('projects.filterResultsLabel');
              this.checkandEnableBasedOnFilterSelection();
              this.filters.followingFilterTitle = '';
              this.loadCommunity$.next();
            } else {
              this.filters.forYouFilterTitle = '';
              this.filters.followingFilterTitle = this.translateService.instant('general.followingBtnUpper');
              this.industryFilterComponent?.collapseFilter();
              this.regionFilterComponent?.collapseFilter();
              this.categoryFilterComponent?.collapseFilter();
              this.projectCapabilitiesFilterComponenet?.collapseFilter();
              this.companyExpandFilterComp?.collapseFilter();
              this.peopleExpandFilterComp?.collapseFilter();
              this.loadFollowing$.next();
            }
            break;
          }
          case this.translateService.instant('community.allCompaniesLabel'):
          case this.translateService.instant('community.corporationCompniesLabel'):
          case this.translateService.instant('community.solutionProviderCompaniesLabel'): {
            this.filters.forYouFilterTitle = '';
            this.filters.followingFilterTitle = '';
            this.loadCompanies$.next();
            break;
          }
          case this.translateService.instant('community.allUsersLabel'):
          case this.translateService.instant('community.corporationsLabel'):
          case this.translateService.instant('community.solutionProvidersLabel'): {
            this.filters.forYouFilterTitle = '';
            this.filters.followingFilterTitle = '';
            this.loadAllPeople$.next();
            break;
          }
          case this.translateService.instant('projects.filterResultsLabel'): {
            if (
              this.commonService.isAnyFilterChecked(this.filters.initialFiltersState) ||
              this.filters.initialFiltersState.search
            ) {
              this.filters.forYouFilterTitle = '';
              this.filters.followingFilterTitle = '';
              this.checkandEnableBasedOnFilterSelection();
              this.filters.title = this.translateService.instant('projects.filterResultsLabel');
              this.loadCommunity$.next();
            } else {
              this.filters.followingFilterTitle = '';
              this.filters.title = this.translateService.instant('projects.suggestedForYouLabel');
              this.filters.forYouFilterTitle = this.translateService.instant('general.forYouLabel');
              this.industryFilterComponent?.collapseFilter();
              this.regionFilterComponent?.collapseFilter();
              this.categoryFilterComponent?.collapseFilter();
              this.projectCapabilitiesFilterComponenet?.collapseFilter();
              this.companyExpandFilterComp?.collapseFilter();
              this.peopleExpandFilterComp?.collapseFilter();
              this.loadForYou$.next();
            }
            break;
          }
        }
      });
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
  }

  private hasFilterSelected(): boolean {
    return (
      !!this.filters.companiesSelected ||
      this.filters.title === this.translateService.instant('community.userRoleLabel') ||
      !!this.filters.initialFiltersState.search ||
      this.commonService.isAnyFilterChecked(this.filters.initialFiltersState)
    );
  }

  private listenToLoadCommunity(): void {
    let searchStr = '';
    this.loadCommunity$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          searchStr = this.filters.initialFiltersState.search ?? '';
          const parameters = this.getParams(this.filters.initialFiltersState);

          this.showClearButton = this.commonService.isAnyFilterChecked(this.filters.initialFiltersState);

          const searchObject: CommunitySearchInterface = {
            skip: this.filters.paging?.skip,
            orderBy: 'asc',
            search: parameters.search,
            filterBy: parameters.parameter
          };

          if (
            this.commonService.isAnyParticularFilterChecked(this.filters.initialFiltersState, 'categories') ||
            this.commonService.isAnyParticularFilterChecked(this.filters.initialFiltersState, 'regions')
          ) {
            searchObject.filterBy =
              parameters.parameter +
              (!parameters.parameter?.length ? '' : '&') +
              `communityItemType=${CommunityType.User}`;
          } else if (
            this.commonService.isAnyParticularFilterChecked(this.filters.initialFiltersState, 'projectCapabilities') ||
            this.commonService.isAnyParticularFilterChecked(this.filters.initialFiltersState, 'industries')
          ) {
            searchObject.filterBy =
              parameters.parameter +
              (!parameters.parameter?.length ? '' : '&') +
              `communityItemType=${CommunityType.Company}`;
          }

          return this.communityDataService.getCommunityList(searchObject);
        })
      )
      .subscribe(communityList => {
        if (!this.filters.initialFiltersState.search) this.filters.initialFiltersState.search = searchStr;
        this.fillCommunityData(communityList);
      });
  }

  private fillCommunityData(communityList: PaginateResponseInterface<CommunityInterface>): void {
    this.communityData = communityList.dataList;
    this.filters.paging = {
      ...this.filters.paging,
      skip: this.filters.paging?.skip ? this.filters.paging?.skip : 0,
      total: communityList.count
    };
    this.viewPort.scrollToPosition([0, 0]);
  }

  private getParams(params: FilterStateInterface): FilterStateInterface {
    return {
      ...params,
      parameter: this.getQueryString(params?.parameter)
    };
  }

  private getQueryString(params): string {
    // TODO: later on back-end it will be rewritten - so, need to remove this "replace" statements
    return Object.keys(params)
      .map(key => {
        const items = params[key].filter(item => !!item.checked || item?.childElements?.length);

        if (items.some(item => item?.childElements)) {
          return CommunityComponent.getFilterChildDataQuery(key, items);
        } else if (items.length) {
          return CommunityComponent.getFilterDataInterfaceQuery(key, items);
        }
      })
      .filter(item => !!item)
      .join('&')
      .replace('industries', 'industryids')
      .replace('regions', 'regionids')
      .replace('categories', 'categoryIds');
  }

  private static getFilterDataInterfaceQuery(key: string, items: FilterDataInterface[]): string {
    return `${key}=${items.map(item => {
      if (item.checked) {
        return item?.id;
      }
    })}`;
  }

  private static getFilterChildDataQuery(key: string, items: FilterChildDataInterface[]): string {
    const query = `${key}=${items
      .map(item =>
        item.checked
          ? `${item?.id},` + CommunityComponent.childSelectedFilters(item)
          : '' + `${CommunityComponent.childSelectedFilters(item)}`
      )
      .filter(n => n)}`;

    return !query.endsWith('=') ? query : '';
  }

  private static childSelectedFilters(filter: FilterChildDataInterface): string {
    return filter.childElements
      .filter(child => !!child.checked && !child.disabled)
      .map(child => child?.id)
      .join(',');
  }

  private listenToQueryParamsChange(): void {
    this.activatedRoute.queryParamMap.pipe(takeUntil(this.unsubscribe$), first()).subscribe(params => {
      const following = params.get('following');
      if (following) {
        this.filters.paging = {
          skip: 0,
          take: this.defaultItemPerPage,
          total: 0
        };
        this.filters.title = this.translateService.instant('general.followingBtnUpper');
      } else {
        this.filters.title = this.translateService.instant('projects.suggestedForYouLabel');
      }
    });
  }

  private checkandEnableBasedOnFilterSelection(): void {
    if (
      this.commonService.isAnyParticularFilterChecked(this.filters.initialFiltersState, TaxonomyEnum.regions) ||
      this.commonService.isAnyParticularFilterChecked(this.filters.initialFiltersState, TaxonomyEnum.categories)
    ) {
      this.filters.companiesClassActive = false;
      this.filters.companiesSelected = null;
      this.filters.disableIndustries = true;
      this.filters.peopleFilterClassActive = true;
      this.filters.disableProjectsUserFilter = false;
      this.filters.disableProjectsCompanyFilter = true;
      this.filters.disableRegions = false;
      this.filters.title = this.translateService.instant('community.allUsersLabel');
      this.industryFilterComponent?.collapseFilter();
      this.companyExpandFilterComp?.collapseFilter();
      this.projectCapabilitiesFilterComponenet?.collapseFilter();
      this.filters.peopleSelectedOptions = this.peopleOptions.find(
        x => x.name == this.translateService.instant('community.allUsersLabel')
      );
      this.peopleTypeForm.get('peopleControl').setValue(this.filters?.peopleSelectedOptions?.id, { emitEvent: false });
    } else if (
      this.commonService.isAnyParticularFilterChecked(this.filters.initialFiltersState, TaxonomyEnum.industries) ||
      this.commonService.isAnyParticularFilterChecked(
        this.filters.initialFiltersState,
        TaxonomyEnum.projectCapabilities
      )
    ) {
      this.filters.companiesClassActive = true;
      this.filters.disableIndustries = false;
      this.filters.peopleFilterClassActive = false;
      this.filters.peopleSelectedOptions = null;
      this.filters.disablePeopleFilter = false;
      this.filters.disableProjectsCompanyFilter = false;
      this.filters.disableRegions = true;
      this.filters.disableProjectsUserFilter = true;
      this.filters.title = this.translateService.instant('community.allCompaniesLabel');
      this.regionFilterComponent?.collapseFilter();
      this.categoryFilterComponent?.collapseFilter();
      this.peopleExpandFilterComp?.collapseFilter();
      this.filters.companiesSelected = this.companiesOptions.find(x => x.name == 'All Companies');
      this.companiesTypeForm.get('control').setValue(this.filters.companiesSelected?.id, { emitEvent: false });
    }
  }

  public onFollowingSortOrderChange(sortOrder: string): void {
    this.filters.followingSelectedSortOrder = sortOrder;
    this.filters.paging.skip = 0;
    this.filters.initialFiltersState.search = '';
    this.clearFilters(true, false);
    this.loadFollowing$.next();
  }

  public followOptionChange(): void {
    if (this.filters.followingFilterTitle == this.translateService.instant('general.followingBtnUpper')) {
      this.communityData = this.communityData.filter(x => x.isFollowed == true);
    }
  }
}
