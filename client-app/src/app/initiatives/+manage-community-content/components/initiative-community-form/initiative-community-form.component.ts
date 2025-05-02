import { Component, OnInit, OnDestroy, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil, catchError } from 'rxjs/operators';
import { CommonService } from 'src/app/core/services/common.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { InitiativeContentTabEnum } from 'src/app/initiatives/shared/enums/initiative-content-tabs.enum';
import { InitiativeCommunityRecommendedComponent } from '../initiative-community-recommended/initiative-community-recommended.component';
import { InitiativeCommunitySavedComponent } from '../initiative-community-saved/initiative-community-saved.component';
import { InitiativeCommunityContentService } from '../../services/initiative-community-content.service';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { TranslateService } from '@ngx-translate/core';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
@Component({
  selector: 'neo-initiative-community-form',
  templateUrl: './initiative-community-form.component.html',
  styleUrls: ['./initiative-community-form.component.scss']
})
export class InitiativeCommunityFormComponent implements OnInit, OnDestroy {
  selectedTab: InitiativeContentTabEnum = InitiativeContentTabEnum.Recommended;
  initiativeId: number;
  userIds: number[] = [];
  isCounterActive = false;
  counter = 0;
  communityUserRecommendationsCounter = '';

  @ViewChild(InitiativeCommunityRecommendedComponent, { static: false })
  initiativeCommunityRecommendedComponent: InitiativeCommunityRecommendedComponent;

  @ViewChild(InitiativeCommunitySavedComponent)
  childComponent: InitiativeCommunitySavedComponent;

  private unsubscribe$ = new Subject<void>();
  constructor(
    private snackbarService: SnackbarService,
    private initiativeCommunityContentService: InitiativeCommunityContentService,
    private commonService: CommonService,
    private route: ActivatedRoute,
    private initiativeSharedService: InitiativeSharedService,
    private activityService: ActivityService,
    private readonly translateService: TranslateService,
    private readonly cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.route.params.pipe(takeUntil(this.unsubscribe$)).subscribe(params => {
      this.initiativeId = params['id'];
    });

    this.route.queryParams.pipe(takeUntil(this.unsubscribe$)).subscribe(params => {
      this.selectedTab = parseInt(params['tab']) || InitiativeContentTabEnum.Recommended;
      if (this.selectedTab === InitiativeContentTabEnum.Saved)
        this.loadRecommendationCount();
    });

  }

  goBack(): void {
    this.commonService.goBack();
  }

  changeTab(tabName: InitiativeContentTabEnum): void {
    this.selectedTab = tabName;
  }

  handleCounterChange(counter: number): void {
    this.isCounterActive = counter > 0;
    this.counter = counter;
  }

  saveSelectedContents(): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        id: this.initiativeId,
        buttonName:
          this.translateService.instant('general.saveLabel') + this.translateService.instant('general.selectionsLabel'),
        moduleName: InitiativeModulesEnum[InitiativeModulesEnum.Community]
      })
      ?.subscribe();
    this.userIds = this.initiativeCommunityContentService.getSelectedTiles().CommunityContent.map(item => item.id);
    if (this.userIds.length > 0) {
      this.initiativeSharedService
        .saveSelectedContentsForAnInitiative(this.initiativeId, false, [], this.userIds)
        .pipe(
          takeUntil(this.unsubscribe$),
          catchError(error => {
            this.snackbarService.showError('general.defaultErrorLabel');
            throw error;
          })
        )
        .subscribe(() => {
          if (this.selectedTab === InitiativeContentTabEnum.Saved) {
            this.childComponent.changePage(1);
            this.childComponent.loadSavedCommunityUsers();
          }
          let successMessage = this.translateService.instant('general.communityLabel');
          if (this.userIds.length > 1) {
            successMessage = successMessage + 's';
          }
          this.snackbarService.showSuccess(
            `${successMessage} ${this.translateService.instant('initiative.viewInitiative.successfullySavedMessage')}`
          );
          this.counter = 0;
          this.initiativeCommunityContentService.clearSelectedTiles();
          this.initiativeCommunityRecommendedComponent.counter = this.counter;
          this.initiativeCommunityRecommendedComponent.selectedTiles = [];
          this.initiativeCommunityRecommendedComponent.loadCommunityUsers(this.userIds.length);
        });
    }
  }

  ngOnDestroy(): void {
    this.initiativeCommunityContentService.clearSelectedTiles();
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this.initiativeSharedService.updateInitiativeContentLastViewedDate(this.initiativeId, InitiativeModulesEnum.Community)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe();
   
  }
  private loadRecommendationCount(): void {
    this.initiativeSharedService
      .getNewRecommendationsCount(this.initiativeId, InitiativeModulesEnum.Community)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(result => {
        if (result.length > 0) {
          this.communityUserRecommendationsCounter = this.initiativeSharedService.displayCounter(
            result[0].communityUsersCount
          );
          this.cdr.detectChanges();
        }
      });
  }
  updateCommunityRecommendationsCounter(counter: number): void {
    this.communityUserRecommendationsCounter = this.initiativeSharedService.displayCounter(counter);
  }
}
