import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { catchError, interval, of, startWith, Subject, switchMap, takeUntil } from 'rxjs';
import { InitiativeCommunityInterface } from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { InitiativeSavedContentListRequestInterface } from '../../interfaces/initiative-saved-content-list-request.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { ViewInitiativeService } from '../../services/view-initiative.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { Router } from '@angular/router';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { TranslateService } from '@ngx-translate/core';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { InitiativeContentTabEnum } from 'src/app/initiatives/shared/enums/initiative-content-tabs.enum';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { InitiativeCommunityItemParentModuleEnum } from 'src/app/initiatives/shared/contents/community-item/community-item.component';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';

@Component({
  selector: 'neo-community-section',
  templateUrl: './community-section.component.html',
  styleUrls: ['./community-section.component.scss']
})
export class CommunitySectionComponent implements OnInit, OnDestroy {
  showDeleteModal = false;
  initiativeParentModuleEnum = InitiativeCommunityItemParentModuleEnum;
  savedCommunityUsers: PaginateResponseInterface<InitiativeCommunityInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: 0
  };
  tagNumber: number;
  counterLength: number;
  defaultItemPerPage = 4;
  page = 1;
  clickedUserId: number;
  @Input() initiativeId: number;
  @Input() initiativeTitle: string;
  @Input() isAdminOrTeamMember: boolean = false;
  public loadInitiativeSavedCommunityUsers$ = new Subject<void>();
  private unsubscribe$ = new Subject<void>();
  usersRecommendationsCounter = '';
  paging: PaginationInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null
  };
  requestData: InitiativeSavedContentListRequestInterface = {
    skip: 0,
    take: 6,
    total: 0,
    includeCount: true
  };
  isLoading: boolean = true;
  options: MenuOptionInterface[] = [
    {
      icon: 'trash-can-red',
      name: 'initiative.viewInitiative.deleteSavedContentLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];
  communityUsersRecommendationsCounter: string = '';
  constructor(
    private readonly authService: AuthService,
    private viewInitiativeService: ViewInitiativeService,
    private readonly snackbarService: SnackbarService,
    private readonly router: Router,
    private initiativeSharedService: InitiativeSharedService,
    private readonly translateService: TranslateService,
    private readonly activityService: ActivityService
  ) {}

  ngOnInit() {
    this.loadRecommendationCount();
    this.loadSavedCommunityUsers();
    this.loadInitiativeSavedCommunityUsers$.next();
  }

  loadSavedCommunityUsers() {
    this.loadInitiativeSavedCommunityUsers$
      .pipe(
        switchMap(() => {
          const requestData = { ...this.requestData };
          return this.viewInitiativeService.getSavedCommunityUsersForAnInitiative(this.initiativeId, requestData);
        }),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(data => {
        this.savedCommunityUsers.count = data.count;
        this.savedCommunityUsers.dataList.push(...data.dataList);
        if (this.isLoading) this.isLoading = false;
      });
  }

  onLoadMoreData() {
    this.page++;
    this.defaultItemPerPage = 3;
    this.paging.skip = this.savedCommunityUsers.dataList.length;
    const isLastPage = this.savedCommunityUsers.dataList.length === this.savedCommunityUsers.count;
    if (!isLastPage) this.loadMoreInitiativeSavedUsers();
  }

  loadMoreInitiativeSavedUsers() {
    this.requestData.includeCount = true;
    this.requestData.skip = this.paging.skip;
    this.requestData.take = this.defaultItemPerPage;
    this.loadInitiativeSavedCommunityUsers$.next();
  }

  optionClick(id: number): void {
    this.showDeleteModal = true;
    this.clickedUserId = id;
  }

  closeDeletePopup() {
    this.showDeleteModal = false;
  }

  confirmDelete() {
    this.showDeleteModal = false;
    this.initiativeSharedService
      .deleteSavedContent(this.initiativeId, this.clickedUserId, InitiativeModulesEnum.Community)
      .pipe(
        catchError(error => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(error);
        }),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(result => {
        if (result === true) {
          this.snackbarService.showSuccess(
            this.translateService.instant('initiative.viewInitiative.successCommunityRemoveMessage')
          );
          const index = this.savedCommunityUsers.dataList.findIndex(p => p.id === this.clickedUserId);
          this.savedCommunityUsers.dataList.splice(index, 1);
          this.savedCommunityUsers.count--;
          if (this.savedCommunityUsers.dataList.length < 4 && this.savedCommunityUsers.count >= 4) {
            this.page = 0;
            this.onLoadMoreData();
          }
        } else {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      });
  }

  viewAllCommunityRecommendations(): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativeModuleViewAllClick, {
        id: this.initiativeId,
        title: this.initiativeTitle,
        resourceType: InitiativeModulesEnum[InitiativeModulesEnum.Community]
      })
      ?.subscribe();
    this.router.navigate(['decarbonization-initiatives', this.initiativeId, 'community'], {
      queryParams: {
        tab:
          this.savedCommunityUsers.dataList.length && this.communityUsersRecommendationsCounter === ''
            ? InitiativeContentTabEnum.Saved
            : InitiativeContentTabEnum.Recommended
      }
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this.loadInitiativeSavedCommunityUsers$.complete();
    this.counterLength = 0;
  }
  loadRecommendationCount(): void {
    interval(60000)
      .pipe(
        startWith(0),
        switchMap(() =>
          this.initiativeSharedService.getNewRecommendationsCount(this.initiativeId, InitiativeModulesEnum.Community)
        ),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(data => {
        this.communityUsersRecommendationsCounter = data
          ? this.initiativeSharedService.displayCounter(data[0].communityUsersCount)
          : '';
      });
  }
}
