import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { interval, Observable, Subject, switchMap, takeUntil } from 'rxjs';
import { DashboardInitiativeService } from 'src/app/+dashboard/components/corporation-dashboard/initiatives/services/initiative-information.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { DashboardClickElementActionTypeEnum } from 'src/app/core/enums/activity/dashboard-click-element-action-type.enum';
import { FirstClickInfoActivityDetailsInterface } from 'src/app/core/interfaces/activity-details/first-click-info-activity-details.interface';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { ActivityService } from 'src/app/core/services/activity.service';
import { InitiativeViewSource } from 'src/app/initiatives/+decarbonization-initiatives/enums/initiative-view-source';
import { NewRecommendationCounterInterface } from 'src/app/initiatives/+initatives/+view-initiative/interfaces/new-recommendation-counter';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { InitiativeProgress, InitiativeStep } from 'src/app/initiatives/interfaces/initiative-progress.interface';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { UserInterface } from '../../interfaces/user/user.interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { PaginationIncludeCountInterface } from '../pagination/pagination.component';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';

@Component({
  selector: 'neo-initiative-information',
  templateUrl: './initiative-information.component.html',
  styleUrls: ['./initiative-information.component.scss']
})
export class InitiativeInformationComponent implements OnInit {
  type = TaxonomyTypeEnum;
  regionsToShow: TagInterface[];
  defaultItemPerPage = 6;
  @Input() paging: PaginationIncludeCountInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null,
    includeCount: true
  };
  initiativesData: PaginateResponseInterface<InitiativeProgress>;
  unsubscribe$: Subject<void> = new Subject<void>();
  @Input() initiativeSource: InitiativeViewSource;
  @Output() elementClick: EventEmitter<FirstClickInfoActivityDetailsInterface> =
    new EventEmitter<FirstClickInfoActivityDetailsInterface>();
  @Output() updateSourceAndPagingDetailsEvent: EventEmitter<PaginationIncludeCountInterface> =
    new EventEmitter<PaginationIncludeCountInterface>();
  allInitiativeIds: number[] = [];
  counterLength: number;
  hasLoaded: boolean = false;
  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  currentUserId: number;
  newRecommendation: NewRecommendationCounterInterface[];
  yourInitiativesCount: number = 0;
  constructor(
    private readonly activatedRoute: ActivatedRoute,
    @Inject(DashboardInitiativeService) private readonly dashboardInitiativesService: DashboardInitiativeService,
    private readonly activityService: ActivityService,
    private readonly router: Router,
    private initiativeSharedService: InitiativeSharedService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.getInitiativeData();
  }

  getInitiativeData() {
    this.activatedRoute.params
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          return this.dashboardInitiativesService.getInitiativesForUser(this.initiativeSource, this.paging);
        })
      )
      .subscribe(response => {
        if (response) {
          this.hasLoaded = true;
          this.initiativesData = response;
          this.paging = {
            ...this.paging,
            skip: this.paging?.skip ? this.paging?.skip : 0,
            total: response.count
          };
          this.updateSourceAndPagingDetailsEvent.emit(this.paging);

          if (response?.dataList?.length > 0) {
            this.allInitiativeIds = response.dataList.map(item => item.initiativeId);
            this.initiativesData.dataList.forEach(i => {
              this.calculateInitiativeModuleContentCounts(i);
              i.steps.forEach(st => {
                st.isActive = st.stepId == i.currentStepId ? true : false;
                st.completed = (st.subSteps.filter(x => x.isChecked == true).length * 100) / st.subSteps.length;
              });
            });
          }
        }
      });
    if (this.initiativeSource === InitiativeViewSource.Dashboard) {
      this.authService.currentUser().subscribe(user => {
        this.currentUserId = user?.id;
        if (this.currentUserId > 0) {
          this.getYourInitiativesCount();
        }
      });
    }
    interval(60000)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(() => {
        if (this.allInitiativeIds.length > 0) {
          this.dashboardInitiativesService
            .getNewRecommendationsCount(this.allInitiativeIds, InitiativeModulesEnum.All)
            .subscribe((response: NewRecommendationCounterInterface[]) => {
              this.newRecommendation = response;
              this.initiativesData.dataList.forEach(i => {
                var item = this.newRecommendation.find(x => x.initiativeId === i.initiativeId);
                i.recommendationsCount.articlesCount = item?.articlesCount;
                i.recommendationsCount.projectsCount = item?.projectsCount;
                i.recommendationsCount.toolsCount = item?.toolsCount;
                i.recommendationsCount.communityUsersCount = item?.communityUsersCount;
                i.recommendationsCount.messagesUnreadCount = item?.messagesUnreadCount;
                this.calculateInitiativeModuleContentCounts(i);
              });
            });
        }
      });
  }
  private calculateInitiativeModuleContentCounts(i: InitiativeProgress) {
    const allCountsExceptMessage =
      (i.recommendationsCount?.articlesCount ?? 0) +
      (i.recommendationsCount?.projectsCount ?? 0) +
      (i.recommendationsCount?.toolsCount ?? 0) +
      (i.recommendationsCount?.communityUsersCount ?? 0);
    i.allNewMessageUnreadCount =
      i.recommendationsCount !== null && i.recommendationsCount.messagesUnreadCount
        ? this.initiativeSharedService.displayCounter(i.recommendationsCount.messagesUnreadCount)
        : '';
    i.allNewExceptMessageRecommendationsCount =
      allCountsExceptMessage > 0 ? this.initiativeSharedService.displayCounter(allCountsExceptMessage) : '';
  }

  getCurrentStep(steps: InitiativeStep[], currentStepId: number): string {
    return steps.find(x => x.stepId === currentStepId)?.description;
  }

  trackCreateInitiativeActivity(): void {
    this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.InitiativeCreateClick });
    this.activityService.trackElementInteractionActivity(ActivityTypeEnum.InitiativeCreate)?.subscribe();
    this.router.navigate(['create-initiative']);
  }
  redirectToInitiative(initiativeId: number, initiativeTitle: string) {
    this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.InitiativeDetailsView });
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativeDetailsView, {
        id: initiativeId,
        title: initiativeTitle
      })
      ?.subscribe();
    const path = 'decarbonization-initiatives/' + initiativeId;
    this.router.navigate([path]);
  }

  getRegions(initiative: InitiativeProgress) {
    this.regionsToShow = JSON.parse(JSON.stringify(initiative.regions));
    return this.regionsToShow.splice(0, 2);
  }

  getYourInitiativesCount() {
    let yoursInitiativesData: PaginateResponseInterface<InitiativeProgress>;
    this.dashboardInitiativesService
      .getInitiativesForUser(InitiativeViewSource.YourInitiatives, this.paging)
      .subscribe(response => {
        yoursInitiativesData = response;
        this.yourInitiativesCount = yoursInitiativesData.dataList.filter(x => x.user?.id == this.currentUserId)?.length;
      });
  }

  openDecarbonizationInitiatives() {
    const path = 'decarbonization-initiatives';
    this.router.navigate([path]);
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.defaultItemPerPage;
    this.getInitiativeData();
    this.updateSourceAndPagingDetailsEvent.emit(this.paging);
  }

  openUserProfile(user?: UserInterface): void {
    if (!user || user.statusId === UserStatusEnum.Deleted) {
      return;
    }
    this.router.navigateByUrl('/user-profile/' + user?.id);
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
