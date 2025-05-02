import { Component, OnInit, OnDestroy, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError } from 'rxjs';
import { CommonService } from 'src/app/core/services/common.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { InitiativeContentTabEnum } from 'src/app/initiatives/shared/enums/initiative-content-tabs.enum';
import { InitiativeMessageRecommendedComponent } from '../initiative-message-recommended-tab/initiative-message-recommended.component';
import { InitiativeMessageSavedComponent } from '../initiative-message-saved-tab/initiative-message-saved-tab.component';
import { InitiativeMessageContentService } from '../../services/initiative-message-content.service';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { TranslateService } from '@ngx-translate/core';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';

@Component({
  selector: 'neo-initiative-message',
  templateUrl: './initiative-message.component.html',
  styleUrls: ['./initiative-message.component.scss']
})
export class InitiativeMessageComponent implements OnInit, OnDestroy {
  private static readonly MAX_SINGLE_DIGIT = 9;
  private static readonly MAX_DOUBLE_DIGIT = 99;

  selectedTab: InitiativeContentTabEnum = InitiativeContentTabEnum.Recommended;
  isLoading = false;
  @ViewChild(InitiativeMessageRecommendedComponent, { static: false })
  initiativeMessageRecommendedComponent: InitiativeMessageRecommendedComponent;
  initiativeId: number;
  discussionIds: number[] = [];
  isCounterActive = false;
  counter = 0;
  discussionUnreadCount = '';
  @ViewChild(InitiativeMessageSavedComponent) childComponent: InitiativeMessageSavedComponent;

  constructor(
    private initiativeMessageContentService: InitiativeMessageContentService,
    private snackbarService: SnackbarService,
    private commonService: CommonService,
    private route: ActivatedRoute,
    private readonly cdr: ChangeDetectorRef,
    private readonly translateService: TranslateService,
    private initiativeSharedService: InitiativeSharedService,
    private activityService: ActivityService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.initiativeId = +params['id'];
    });
    this.route.queryParams.subscribe(params => {
      this.selectedTab = +params['tab'] || InitiativeContentTabEnum.Recommended;
    });
    this.loadRecommendationCount();
  }

  goBack(): void {
    this.commonService.goBack();
  }

  changeTab(tabName: InitiativeContentTabEnum): void {
    this.selectedTab = tabName;
    this.loadRecommendationCount();
  }

  selectedItemsCounter(counter: number): void {
    this.isCounterActive = counter > 0;
    this.counter = counter;
  }

  saveSelectedContents(): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        id: this.initiativeId,
        buttonName:
          this.translateService.instant('general.saveLabel') + this.translateService.instant('general.selectionsLabel'),
        moduleName: InitiativeModulesEnum[InitiativeModulesEnum.Messages]
      })
      ?.subscribe();
    this.discussionIds = this.initiativeMessageContentService.getSelectedTiles().MessageContent.map(item => item.id);
    if (this.discussionIds.length > 0) {
      this.initiativeSharedService
        .saveSelectedContentsForAnInitiative(this.initiativeId, false, [], [], [], [], this.discussionIds)
        .pipe(
          catchError(error => {
            this.snackbarService.showError('general.defaultErrorLabel');
            throw error;
          })
        )
        .subscribe(() => {
          if (this.selectedTab === InitiativeContentTabEnum.Saved) {
            this.childComponent.changePage(1);
            this.childComponent.loadSavedDiscussions();
          }
          let successMessage = this.translateService.instant('general.messageLabel');
          if (this.discussionIds.length > 1) {
            successMessage = successMessage + 's';
          }
          this.snackbarService.showSuccess(
            `${successMessage} ${this.translateService.instant('initiative.viewInitiative.successfullySavedMessage')}`
          );
          this.resetCounter();
          this.loadRecommendationCount();
        });
    }
  }

  ngOnDestroy(): void {
    this.initiativeMessageContentService.clearSelectedTiles();
  }

  private loadRecommendationCount(): void {
    this.initiativeSharedService
      .getNewRecommendationsCount(this.initiativeId, InitiativeModulesEnum.Messages)
      .subscribe(result => {
        if (result?.length > 0) {
          this.discussionUnreadCount = this.initiativeSharedService.displayCounter(result[0].messagesUnreadCount);
          this.isLoading = true;
          this.cdr.detectChanges();
        }
      });
  }

  private resetCounter(): void {
    this.counter = 0;
    this.initiativeMessageContentService.clearSelectedTiles();
    this.initiativeMessageRecommendedComponent.selectedTiles = [];
    this.initiativeMessageRecommendedComponent.loadDiscussions(this.discussionIds.length);
  }

  reloadCounter(status: boolean): void {
    if (status) {
      this.loadRecommendationCount();
    }
  }
}
