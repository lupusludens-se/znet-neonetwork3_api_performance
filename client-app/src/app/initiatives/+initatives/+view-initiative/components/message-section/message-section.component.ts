import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject, switchMap, catchError, of, interval, startWith, takeUntil } from 'rxjs';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { InitiativeContentTabEnum } from 'src/app/initiatives/shared/enums/initiative-content-tabs.enum';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { InitiativeSavedContentListRequestInterface } from '../../interfaces/initiative-saved-content-list-request.interface';
import { ViewInitiativeService } from '../../services/view-initiative.service';
import { InitiativeMessageInterface } from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { NewRecommendationCounterInterface } from '../../interfaces/new-recommendation-counter';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { InitiativeProgress } from 'src/app/initiatives/interfaces/initiative-progress.interface';
@Component({
  selector: 'neo-message-section',
  templateUrl: './message-section.component.html',
  styleUrls: ['./message-section.component.scss']
})
export class MessageSectionComponent implements OnInit, OnDestroy {
  showDeleteModal = false;
  savedMessages: PaginateResponseInterface<InitiativeMessageInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: 0
  };
  tagNumber: number;
  counterLength: number;
  defaultItemPerPage = 4;
  page = 1;
  clickedMessageId: number;

  @Input() initiativeProgress: InitiativeProgress;
  @Input() isAdminOrTeamMember: boolean = false;

  public loadInitiativeSavedMessages$ = new Subject<void>();
  private unsubscribe$ = new Subject<void>();
  messageRecommendationsCounter = '';
  currentUser: UserInterface;
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
  isLoading = true;
  options: MenuOptionInterface[] = [
    {
      icon: 'trash-can-red',
      name: 'initiative.viewInitiative.deleteSavedContentLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];
  hasAccessToInitiative(currentUser: UserInterface): boolean {
    return (
      !!currentUser && this.initiativeProgress.collaborators.some(collaborator => collaborator.id === currentUser.id)
    );
  }

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
    this.loadSavedMessages();
    this.listenForCurrentUser();
    this.loadInitiativeSavedMessages$.next();
  }

  private listenForCurrentUser(): void {
    this.authService
      .currentUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(currentUser => {
        this.currentUser = currentUser;
      });
  }

  loadSavedMessages() {
    this.loadInitiativeSavedMessages$
      .pipe(
        switchMap(() => {
          const requestData = { ...this.requestData };
          return this.viewInitiativeService.getSavedMessagesOfAnInitiative(
            this.initiativeProgress.initiativeId,
            requestData
          );
        }),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(data => {
        this.savedMessages.count = data.count;
        this.savedMessages.dataList.push(...data.dataList);
        if (this.isLoading) this.isLoading = false;
      });
  }

  onLoadMoreData() {
    this.page++;
    this.defaultItemPerPage = 3;
    this.paging.skip = this.savedMessages.dataList.length;
    const isLastPage = this.savedMessages.dataList.length === this.savedMessages.count;
    if (!isLastPage) this.loadMoreInitiativeSavedMessages();
  }

  loadMoreInitiativeSavedMessages() {
    this.requestData.includeCount = true;
    this.requestData.skip = this.paging.skip;
    this.requestData.take = this.defaultItemPerPage;
    this.loadInitiativeSavedMessages$.next();
  }

  optionClick(id: number): void {
    this.showDeleteModal = true;
    this.clickedMessageId = id;
  }

  closeDeletePopup() {
    this.showDeleteModal = false;
  }

  confirmDelete() {
    this.showDeleteModal = false;
    this.initiativeSharedService
      .deleteSavedContent(this.initiativeProgress.initiativeId, this.clickedMessageId, InitiativeModulesEnum.Messages)
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
            this.translateService.instant('initiative.viewInitiative.successConverationRemoveMessage')
          );
          const index = this.savedMessages.dataList.findIndex(p => p.id === this.clickedMessageId);
          this.savedMessages.dataList.splice(index, 1);
          this.savedMessages.count--;
          if (this.savedMessages.dataList.length < 4 && this.savedMessages.count >= 4) {
            this.page = 0;
            this.onLoadMoreData();
          }
        } else {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      });
  }

  viewMessages(): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativeModuleViewAllClick, {
        id: this.initiativeProgress.initiativeId,
        title: this.initiativeProgress.title,
        resourceType: InitiativeModulesEnum[InitiativeModulesEnum.Messages]
      })
      ?.subscribe();
    this.router.navigate(['decarbonization-initiatives', this.initiativeProgress.initiativeId, 'messages'], {
      queryParams: {
        tab:
          this.savedMessages.dataList.length && this.messageRecommendationsCounter === ''
            ? InitiativeContentTabEnum.Saved
            : InitiativeContentTabEnum.Recommended
      }
    });
  }

  loadRecommendationCount(): void {
    interval(60000)
      .pipe(
        startWith(0),
        switchMap(() =>
          this.initiativeSharedService.getNewRecommendationsCount(
            this.initiativeProgress.initiativeId,
            InitiativeModulesEnum.Messages
          )
        ),
        takeUntil(this.unsubscribe$)
      )
      .subscribe((data: NewRecommendationCounterInterface[]) => {
        if (data?.length > 0) {
          this.messageRecommendationsCounter = this.initiativeSharedService.displayCounter(data[0].messagesUnreadCount);
        }
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this.loadInitiativeSavedMessages$.complete();
    this.counterLength = 0;
  }
}
