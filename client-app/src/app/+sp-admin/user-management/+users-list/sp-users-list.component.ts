import { Component, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { filter } from 'rxjs';
import { CommonService } from 'src/app/core/services/common.service';
import { CoreService } from 'src/app/core/services/core.service';
import { TitleService } from 'src/app/core/services/title.service';
import { ExportModule } from 'src/app/shared/enums/export-module.enum';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { FilterStateInterface } from 'src/app/shared/modules/filter/interfaces/filter-state.interface';
import { DEFAULT_PER_PAGE, PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { UserComponentsEnum } from 'src/app/user-management/enums/user-component.enum';
import { UserManagementApiEnum } from 'src/app/user-management/enums/user-management-api.enum';
import { UserDataService } from 'src/app/user-management/services/user.data.service';

@UntilDestroy()
@Component({
  selector: 'neo-sp-users-list',
  templateUrl: './sp-users-list.component.html',
  styleUrls: ['./sp-users-list.component.scss']
})
export class SPUsersListComponent implements OnInit {
  apiRoutes = UserManagementApiEnum;
  usersList: UserInterface[];
  exportModal: boolean;
  exportModule = ExportModule.CompanyUsersExport;
  lastNameAsc: boolean = true;
  statusNameAsc: boolean;
  emailAsc: boolean;
  titleAsc: boolean;
  sortingCriteria: Record<string, string> = {
    lastname: 'lastname',
    email: 'email',
    statusname: 'statusname',
    title: 'title'
  };
  paging: PaginationInterface;
  defaultItemPerPage = DEFAULT_PER_PAGE;
  showClearButton: boolean;
  requestParams: string;
  orderBy: string;
  initialFiltersState: FilterStateInterface = null;
  initialLoad: boolean = true;
  routesEnum: string[] = [`${UserComponentsEnum.EditUserComponent}`];
  tdTitleClick: string;

  constructor(
    public commonService: CommonService,
    private userDataService: UserDataService,
    private titleService: TitleService,
    private readonly coreService: CoreService
  ) {}

  ngOnInit(): void {
    this.coreService.elementNotFoundData$
      .pipe(
        untilDestroyed(this),
        filter(data => !data)
      )
      .subscribe(() => {
        this.getUsersList();
        this.titleService.setTitle('userManagement.pageTitleLabel');
      });
  }

  getUsersList(skip?: number): void {
    this.userDataService
      .getCompanyUserList(skip, this.orderBy)
      .subscribe((users: PaginateResponseInterface<UserInterface>) => {
        this.usersList = users.dataList;
        this.paging = {
          ...this.paging,
          skip,
          total: users.count
        };
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

    this.lastNameAsc = this.orderBy == `${this.sortingCriteria.lastname}.asc` ? true : false;
    this.emailAsc = this.orderBy == `${this.sortingCriteria.email}.asc` ? true : false;
    this.statusNameAsc = this.orderBy == `${this.sortingCriteria.statusname}.asc` ? true : false;
    this.titleAsc = this.orderBy == `${this.sortingCriteria.title}.asc` ? true : false;
  }

  sortUsersList(OrderBy: string): void {
    this.orderBy = OrderBy;

    this.userDataService
      .getCompanyUserList(this.paging.skip, OrderBy)
      .subscribe((res: PaginateResponseInterface<UserInterface>) => {
        this.usersList = res.dataList;
      });
  }

  updatePaging(page: number): void {
    const skip: number = (page - 1) * this.defaultItemPerPage;
    this.getUsersList(skip);
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
