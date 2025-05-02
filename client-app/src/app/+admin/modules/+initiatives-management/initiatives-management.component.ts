import { Component, OnDestroy, OnInit } from '@angular/core';
import { InitiativeAdminResponse } from './interfaces/initiative-admin';
import { DEFAULT_PER_PAGE, PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { Subject, switchMap, takeUntil } from 'rxjs';
import { HttpService } from 'src/app/core/services/http.service';
import { CoreService } from 'src/app/core/services/core.service';
import { TitleService } from 'src/app/core/services/title.service';
import { Router } from '@angular/router';
import { ViewportScroller } from '@angular/common';
import { InitiativeService } from './services/initiative.service';
import { InitiativeStatusEnum } from './enums/initiative-status.enum';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { ExportModule } from 'src/app/shared/enums/export-module.enum';
@Component({
  selector: 'neo-initiatives-management',
  templateUrl: './initiatives-management.component.html',
  styleUrls: ['./initiatives-management.component.scss']
})
export class InitiativesManagementComponent implements OnInit, OnDestroy {
  initiativesList: InitiativeAdminResponse[];
  initiativeStatus = InitiativeStatusEnum;
  categoryAsc: boolean = false;
  companyAsc: boolean;
  changedOnAsc: boolean;
  phaseAsc: boolean;
  statusNameAsc: boolean;
  sortingCriteria: Record<string, string> = {
    category: 'category',
    company: 'company',
    phase: 'phase',
    changedon: 'changedon',
    statusname: 'statusname' 
  };
  paging: PaginationInterface = {
    skip: 0,
    take: DEFAULT_PER_PAGE,
    total: null
  };
  searchVal: string;
  defaultItemPerPage = DEFAULT_PER_PAGE;
  orderBy: string;
  initialLoad: boolean = true;
  tdTitleClick: string;
  private unsubscribe$: Subject<void> = new Subject<void>();
  pageDataLoad$: Subject<void> = new Subject<void>();
  exportModal: boolean;
  exportModule = ExportModule.InitiativeExport;
  constructor(
    private httpService: HttpService,
    private readonly coreService: CoreService,
    private titleService: TitleService,
    private router: Router,
    private viewPort: ViewportScroller,
    private initiativeService: InitiativeService
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle('initiativeManagement.pageTitleLabel');
    this.fetchAllInitiatives();
    this.pageDataLoad$.next();
    this.initiativeService.getSearchStr().pipe(
      takeUntil(this.unsubscribe$)
    ).subscribe(searchStr => {
      this.searchVal = searchStr;
    });
  }

  fetchAllInitiatives() {
    this.pageDataLoad$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          this.setLastAppliedOrderBy();
          if (this.initialLoad) {
            this.setTitleClick();
            this.categoryAsc = this.orderBy == `${this.sortingCriteria.category}.asc` ? true : false;
            this.companyAsc = this.orderBy == `${this.sortingCriteria.company}.asc` ? true : false;
            this.phaseAsc = this.orderBy == `${this.sortingCriteria.phase}.asc` ? true : false;
            this.changedOnAsc = this.orderBy == `${this.sortingCriteria.changedon}.asc` ? true : false;
            this.statusNameAsc = this.orderBy == `${this.sortingCriteria.statusname}.asc` ? true : false;
          }

          const paging = this.coreService.deleteEmptyProps({
            ...this.paging,
            IncludeCount: true,
            OrderBy: this.orderBy,
            FilterBy: null,
            Search: this.searchVal
          });
          return this.initiativeService.fetchAllInitiatives(paging);
        })
      )
      .subscribe((val: PaginateResponseInterface<InitiativeAdminResponse>) => {
        this.initialLoad = false;
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip ? this.paging?.skip : 0,
          total: val?.count
        };
        this.initiativeService.setPaging(this.paging);

        this.initiativesList = val?.dataList?.map(val => {
          val.regionNames = val.regions.map(x => x.name).join(',');
          return val;
        });

        this.viewPort.scrollToPosition([0, 0]);
      });
  }

  // Set last applied order by
  setLastAppliedOrderBy(): void {
    this.orderBy =
      this.initialLoad && this.initiativeService.orderBy$.getValue()
        ? this.initiativeService.orderBy$.getValue()
        : this.orderBy;
  }

  getInitiativesBySearch(searchStr?: string): void {
    this.searchVal = searchStr;
    this.initiativeService.setSearchStr(searchStr);
    this.paging.skip = 0;
    this.paging.total = null;
    this.pageDataLoad$.next();
    this.orderBy = null;
  }

  // set last applied title click
  setTitleClick() {
    if (this.initiativeService.orderBy$.getValue() !== null) {
      let criteriaName = this.initiativeService.orderBy$.getValue().split('.')[0];
      if (
        this.sortingCriteria.category === criteriaName ||
        this.sortingCriteria.company === criteriaName ||
        this.sortingCriteria.phase === criteriaName ||
        this.sortingCriteria.changedon === criteriaName||
        this.sortingCriteria.statusname === criteriaName 
      ) {
        this.tdTitleClick = this.sortingCriteria[criteriaName];
      }
    }
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this.initiativeService.clearAll();
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.defaultItemPerPage;
    this.initiativeService.setPaging(this.paging);
    this.pageDataLoad$.next();
  }

  sortCriteriaSelection(
    sortDirection: string,
    sortKey: string,
    secondSortCol: string,
    thirdSortCol: string,
    fourthSortCol: string
  ): void {
    this.tdTitleClick = sortKey;
    if (this[sortDirection]) {
      this.sortInitiativesList(`${this.sortingCriteria[sortKey]}.desc`);
    } else {
      this.sortInitiativesList(`${this.sortingCriteria[sortKey]}.asc`);
    }

    this[secondSortCol] = false;
    this[thirdSortCol] = false;
    this[fourthSortCol] = false;

    this[sortDirection] = !this[sortDirection];
  }

  sortInitiativesList(OrderBy: string): void {
    this.orderBy = OrderBy;
    this.initiativeService.setOrderBy(OrderBy);
    this.pageDataLoad$.next();
  }
  openInitiative(initiative: InitiativeAdminResponse) {
    window.open(`decarbonization-initiatives/${initiative.id}`, '_blank');
  }
}
