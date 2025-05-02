import { HttpService } from '../../core/services/http.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { filter, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { DEFAULT_PER_PAGE, PaginationInterface } from '../../shared/modules/pagination/pagination.component';
import { PaginateResponseInterface } from '../../shared/interfaces/common/pagination-response.interface';
import { FilterStateInterface } from '../../shared/modules/filter/interfaces/filter-state.interface';
import { UserInterface } from '../../shared/interfaces/user/user.interface';
import { UserManagementApiEnum } from '../enums/user-management-api.enum';
import { TagInterface } from '../../core/interfaces/tag.interface';
import { CommonService } from '../../core/services/common.service';
import { TitleService } from '../../core/services/title.service';
import { UserDataService } from '../services/user.data.service';
import { TaxonomyEnum } from '../../core/enums/taxonomy.enum';
import { CoreService } from 'src/app/core/services/core.service';
import { Router } from '@angular/router';
import { UserComponentsEnum } from '../enums/user-component.enum';
import { ViewportScroller } from '@angular/common';

@UntilDestroy()
@Component({
  selector: 'neo-users-list',
  templateUrl: 'users-list.component.html',
  styleUrls: ['users-list.component.scss']
})
export class UsersListComponent implements OnInit, OnDestroy {
  apiRoutes = UserManagementApiEnum;
  usersList: UserInterface[];
  exportModal: boolean;
  lastNameAsc: boolean = true;
  companyAsc: boolean;
  statusNameAsc: boolean;
  sortingCriteria: Record<string, string> = {
    lastname: 'lastname',
    company: 'company',
    statusname: 'statusname'
  };
  searchString: string;
  taxonomyEnum = TaxonomyEnum;
  paging: PaginationInterface;
  defaultItemPerPage = DEFAULT_PER_PAGE;
  rolesFilter: string = '';
  statusesFilter: string = '';
  interestsFilter: string = '';
  showClearButton: boolean;
  requestParams: string;
  orderBy: string;
  initialFiltersState: FilterStateInterface = null;
  initialLoad: boolean = true;
  routesEnum: string[] = [`${UserComponentsEnum.EditUserComponent}`];
  tdTitleClick: string;

  constructor(
    private httpService: HttpService,
    public commonService: CommonService,
    private userDataService: UserDataService,
    private titleService: TitleService,
    private readonly coreService: CoreService,
    private router: Router,
    private viewPort: ViewportScroller
  ) {}

  ngOnInit(): void {
    this.coreService.elementNotFoundData$
      .pipe(
        untilDestroyed(this),
        filter(data => !data)
      )
      .subscribe(() => {
        this.searchString = this.userDataService.searchStr ?? null;
        this.loadFilterData();
        this.titleService.setTitle('userManagement.pageTitleLabel');
      });
  }

  ngOnDestroy(): void {
    const routesFound = this.routesEnum.some(val => this.coreService.getOngoingRoute().includes(val));
    if (!routesFound) {
      this.commonService.clearFilters(this.initialFiltersState);
      this.userDataService.clearRequestParams();
    }
  }

  getUsersBySearch(searchStr?: string): void {
    this.searchString = searchStr;
    this.commonService.clearFilters(this.initialFiltersState);
  }

  getUsersList(searchStr?: string, skip?: number): void {
    this.userDataService
      .getUserList(searchStr, skip, this.orderBy, this.createFilterByParam())
      .subscribe((users: PaginateResponseInterface<UserInterface>) => {
        this.usersList = users.dataList;
        this.paging = {
          ...this.paging,
          skip,
          total: users.count
        };        
        this.viewPort.scrollToPosition([0, 0]);
      });
  }

  sortCriteriaSelection(sortDirection: string, sortKey: string, secondSortCol: string, thirdSortCol: string): void {
    this.tdTitleClick = sortKey;
    if (this[sortDirection]) {
      this.sortUsersList(`${this.sortingCriteria[sortKey]}.desc`);
    } else {
      this.sortUsersList(`${this.sortingCriteria[sortKey]}.asc`);
    }

    this[secondSortCol] = false;
    this[thirdSortCol] = false;

    this[sortDirection] = !this[sortDirection];
  }

  sortUsersList(OrderBy: string): void {
    this.orderBy = OrderBy;

    this.userDataService
      .getUserList(
        this.searchString,
        this.paging.skip,
        OrderBy,
        this.searchString ? '' : this.createFilterByParam(),
        false
      )
      .subscribe((res: PaginateResponseInterface<UserInterface>) => {
        this.usersList = res.dataList;
      });
  }

  createFilterByParam(): string {
    let param: string = '';

    if (this.rolesFilter) {
      param = this.rolesFilter;
    }
    if (this.interestsFilter) {
      param = param + '&' + this.interestsFilter;
    }
    if (this.statusesFilter) {
      param = param + '&' + this.statusesFilter;
    }

    return param;
  }

  clearFilters(): void {
    this.commonService.clearFilters(this.initialFiltersState);
  }

  updatePaging(page: number): void {
    const skip: number = (page - 1) * this.defaultItemPerPage;
    this.getUsersList(this.searchString, skip);
  }

  parseFilterQuery(initialFiltersState: FilterStateInterface, param: string, queryParam: string): number[] | string {
    const filterBy: TagInterface[] = initialFiltersState?.parameter[param].filter(c => c['checked'] === true);
    const filterById: number[] = filterBy?.map(f => f.id);
    return filterById?.length ? `${queryParam}=${filterById}` : '';
  }

  private loadFilterData(): void {
    this.commonService
      .filterState()
      .pipe(
        untilDestroyed(this),
        filter(initialFiltersState => !!initialFiltersState),
        switchMap(initialFiltersState => {
          this.initialFiltersState = initialFiltersState;

          if (this.initialFiltersState) {
            this.initialFiltersState.parameter['roles'] = this.initialFiltersState.parameter['roles'].filter(r => {
              return r.name !== 'All';
            });
          }

          this.showClearButton = this.commonService.isAnyFilterChecked(initialFiltersState);

          this.rolesFilter = this.parseFilterQuery(initialFiltersState, 'roles', 'roleids').toString();

          this.interestsFilter = this.parseFilterQuery(initialFiltersState, 'categories', 'categoryids').toString();
          this.statusesFilter = this.parseFilterQuery(initialFiltersState, 'statuses', 'statusids').toString();

          this.setLastAppliedOrderBy();
          this.lastNameAsc = this.orderBy == `${this.sortingCriteria.lastname}.asc` ? true : false;
          this.companyAsc = this.orderBy == `${this.sortingCriteria.company}.asc` ? true : false;
          this.statusNameAsc = this.orderBy == `${this.sortingCriteria.statusname}.asc` ? true : false;
          let skip: number = this.getLastAppliedSkip();
          skip = this.initialLoad && (this.searchString == null || this.searchString.trim() == '') ? skip : 0;

          return this.userDataService.getUserList(this.searchString, skip, this.orderBy, this.createFilterByParam());
        })
      )
      .subscribe(users => {
        this.initialLoad = false;
        this.usersList = users.dataList;
        this.paging = {
          skip: this.userDataService.skip ?? 0,
          take: this.defaultItemPerPage,
          total: users.count!
        };
      });
  }

  // Set last applied order by
  setLastAppliedOrderBy(): void {
    this.orderBy =
      this.initialLoad && this.userDataService.orderBy
        ? this.userDataService.orderBy
        : `${this.sortingCriteria.lastname}.asc`;
  }

  // To return if any  last applied skip or 0
  getLastAppliedSkip(): number {
    return this.initialLoad &&
      this.userDataService.skip != null &&
      this.userDataService.skip != undefined &&
      this.userDataService.skip != 0
      ? this.userDataService.skip
      : 0;
  }
}
