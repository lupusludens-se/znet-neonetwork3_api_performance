import { Component, OnDestroy, OnInit } from '@angular/core';
import { catchError, Subject, switchMap, throwError } from 'rxjs';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { PermissionService } from 'src/app/core/services/permission.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { TitleService } from 'src/app/core/services/title.service';
import { CompanyInterface } from 'src/app/shared/interfaces/user/company.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { DEFAULT_COMPANIES_PER_PAGE } from './constants/parameter.const';
import { CompanyDataService } from './services/company.data.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ViewportScroller } from '@angular/common';

@UntilDestroy()
@Component({
  selector: 'neo-company-management',
  templateUrl: './company-management.component.html',
  styleUrls: ['./company-management.component.scss']
})
export class CompanyManagementComponent implements OnInit, OnDestroy {
  companiesList: CompanyInterface[];
  searchString: string;
  defaultItemPerPage = DEFAULT_COMPANIES_PER_PAGE;
  paging: PaginationInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null
  };
  orderBy: string;
  nameAsc: boolean = true;
  typeAsc: boolean;
  statusAsc: boolean;
  sortingCriteria: Record<string, string> = {
    name: 'name',
    type: 'type',
    status: 'status'
  };

  showAdd: boolean;
  currentUser: UserInterface;
  loadCompanies$: Subject<void> = new Subject<void>();
  tdTitleClick: string;

  constructor(
    private titleService: TitleService,
    private companyDataService: CompanyDataService,
    private readonly snackbarService: SnackbarService,
    private readonly authService: AuthService,
    private readonly permissionService: PermissionService,
    private viewPort: ViewportScroller
  ) { }

  ngOnInit(): void {
    this.titleService.setTitle('companyManagement.pageTitleLabel');
    this.orderBy = `${this.sortingCriteria.name}.asc`;
    this.listenToLoadCompanies();
    this.listenForCurrentUser();
    this.loadCompanies$.next();
  }

  ngOnDestroy(): void {
    this.loadCompanies$.next();
    this.loadCompanies$.complete();
  }

  sortCriteriaSelection(sortDirection: string, sortKey: string, secondSortCol: string, thirdSortCol: string): void {
    this.tdTitleClick = sortKey;
    if (this[sortDirection]) {
      this.sortCompaniesList(`${this.sortingCriteria[sortKey]}.desc`);
    } else {
      this.sortCompaniesList(`${this.sortingCriteria[sortKey]}.asc`);
    }

    this[secondSortCol] = false;
    this[thirdSortCol] = false;

    this[sortDirection] = !this[sortDirection];
  }

  sortCompaniesList(OrderBy: string): void {
    this.orderBy = OrderBy;
    this.loadCompanies$.next();
  }

  updatePaging(page: number): void {
    this.paging.skip = (page - 1) * this.defaultItemPerPage;
    this.loadCompanies$.next();
  }

  private listenToLoadCompanies(): void {
    this.loadCompanies$
      .pipe(
        untilDestroyed(this),
        switchMap(() => {
          return this.companyDataService.getCompanyList(this.searchString, this.paging.skip, this.orderBy);
        }),
        catchError(error => {
          this.snackbarService.showError('general.defaultErrorLabel');
          return throwError(error);
        })
      )
      .subscribe(companies => {
        this.companiesList = companies.dataList;
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip ? this.paging?.skip : 0,
          total: companies.count
        };
        this.viewPort.scrollToPosition([0, 0]);

      });
  }

  private listenForCurrentUser(): void {
    this.authService.currentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      if (currentUser) {
        this.showAdd = this.permissionService.userHasPermission(this.currentUser, PermissionTypeEnum.CompanyManagement);
      }
    });
  }

  updateCompaniesList() {
    if ((this.paging.total - 1) % this.defaultItemPerPage === 0) {
      this.paging.skip = this.paging.skip - this.defaultItemPerPage;
    }

    this.loadCompanies$.next();
  }
}
