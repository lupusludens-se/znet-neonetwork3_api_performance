import { Component, OnDestroy, OnInit } from '@angular/core';
import { DEFAULT_PER_PAGE, PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { FeedbackInterface } from './interfaces/feedback.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { HttpService } from 'src/app/core/services/http.service';
import { FeedbackService } from './services/feedback.service';
import { Subject, switchMap, takeUntil } from 'rxjs';
import { CoreService } from 'src/app/core/services/core.service';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { TitleService } from 'src/app/core/services/title.service';
import { FeedbackRoutesEnum } from './enums/feedback.enum';
import { ExportModule } from 'src/app/shared/enums/export-module.enum';
import { Router } from '@angular/router';
import { ViewportScroller } from '@angular/common';

@Component({
  selector: 'neo-feedback-management',
  templateUrl: './feedback-management.component.html',
  styleUrls: ['./feedback-management.component.scss']
})
export class FeedbackManagementComponent implements OnInit, OnDestroy {
  feedbacksList: FeedbackInterface[];
  exportModal: boolean;
  lastNameAsc: boolean = false;
  companyAsc: boolean;
  createdDateAsc: boolean;
  ratingAsc: boolean;
  sortingCriteria: Record<string, string> = {
    lastName: 'lastName',
    company: 'company',
    rating: 'rating',
    createdOn: 'createdOn'
  };
  paging: PaginationInterface = {
    skip: 0,
    take: DEFAULT_PER_PAGE,
    total: null
  };
  defaultItemPerPage = DEFAULT_PER_PAGE;
  showClearButton: boolean;
  requestParams: string;
  exportModule = ExportModule.FeedbackExport;
  orderBy: string;
  initialLoad: boolean = true;
  tdTitleClick: string;
  private unsubscribe$: Subject<void> = new Subject<void>();
  pageDataLoad$: Subject<void> = new Subject<void>();
  pageData: PaginateResponseInterface<FeedbackInterface>;
  routesToNotClearFilters: string[] = [`${FeedbackRoutesEnum.UserFeedbackDetailsComponent}`];
  constructor(
    private httpService: HttpService,
    private readonly coreService: CoreService,
    private readonly feedbackService: FeedbackService,
    private titleService: TitleService,
    private router: Router,
    private viewPort: ViewportScroller
  ) {
    this.fetchPaginationState();
    this.fetchSortingState();
  }

  ngOnInit(): void {
    this.titleService.setTitle('feedbackManagement.pageTitleLabel');
    this.fetchFeedbacks();
    this.pageDataLoad$.next();
  }

  fetchPaginationState() {
    this.feedbackService
      .getPaging()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(page => {
        this.paging = page;
      });
  }

  fetchSortingState() {
    this.feedbackService
      .getOrderBy()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(orderBy => {
        this.orderBy = orderBy;
      });
  }

  fetchFeedbacks() {
    this.pageDataLoad$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          this.setLastAppliedOrderBy();
          if (this.initialLoad) {
            this.setTitleClick();
            this.lastNameAsc = this.orderBy == `${this.sortingCriteria.lastName}.asc` ? true : false;
            this.companyAsc = this.orderBy == `${this.sortingCriteria.company}.asc` ? true : false;
            this.ratingAsc = this.orderBy == `${this.sortingCriteria.rating}.asc` ? true : false;
            this.createdDateAsc = this.orderBy == `${this.sortingCriteria.createdOn}.asc` ? true : false;
          }

          const paging = this.coreService.deleteEmptyProps({
            ...this.paging,
            expand: 'user,user.image,user.company,user.roles',
            IncludeCount: true,
            OrderBy: this.orderBy,
            FilterBy: null
          });
          return this.feedbackService.fetchFeedbacks(paging);
        })
      )
      .subscribe((val: any) => {
        this.initialLoad = false;
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip ? this.paging?.skip : 0,
          total: val?.count
        };
        this.feedbackService.setPaging(this.paging);
        this.pageData = val;
        this.pageData.dataList = this.pageData.dataList.map((item: any) => ({
          ...item,
          roles: item.feedbackUser.roles.filter(x => x.id != RolesEnum.All)
        }));

        this.viewPort.scrollToPosition([0, 0]);
      });
  }

  // Set last applied order by
  setLastAppliedOrderBy(): void {
    this.orderBy =
      this.initialLoad && this.feedbackService.orderBy$.getValue()
        ? this.feedbackService.orderBy$.getValue()
        : this.orderBy;
  }

  // set last applied title click
  setTitleClick() {
    if (this.feedbackService.orderBy$.getValue() !== null) {
      let criteriaName = this.feedbackService.orderBy$.getValue().split('.')[0];
      if (
        this.sortingCriteria.lastName === criteriaName ||
        this.sortingCriteria.company === criteriaName ||
        this.sortingCriteria.rating === criteriaName ||
        this.sortingCriteria.createdOn === criteriaName
      ) {
        this.tdTitleClick = this.sortingCriteria[criteriaName];
      }
    }
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    const routesFound = this.routesToNotClearFilters.some(val => this.coreService.getOngoingRoute().includes(val));
    if (!routesFound) {
      this.feedbackService.clearAll();
    }
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.defaultItemPerPage;
    this.feedbackService.setPaging(this.paging);
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
      this.sortFeedbacksList(`${this.sortingCriteria[sortKey]}.desc`);
    } else {
      this.sortFeedbacksList(`${this.sortingCriteria[sortKey]}.asc`);
    }

    this[secondSortCol] = false;
    this[thirdSortCol] = false;
    this[fourthSortCol] = false;

    this[sortDirection] = !this[sortDirection];
  }

  sortFeedbacksList(OrderBy: string): void {
    this.orderBy = OrderBy;
    this.feedbackService.setOrderBy(OrderBy);
    this.pageDataLoad$.next();
  }

  redirectToFeedbackDetails(feedbackId: number): void {
    if (feedbackId > 0) {
      this.router.navigateByUrl(`/admin/user-feedback/${feedbackId}`);
    }
  }
}
