import { Component, OnInit, OnDestroy, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil, catchError } from 'rxjs/operators';
import { CommonService } from 'src/app/core/services/common.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { InitiativeLearnRecommendedComponent } from '../initiative-learn-recommended-tab/initiative-learn-recommended.component';
import { InitiativeLearnSavedComponent } from '../initiative-learn-saved-tab/initiative-learn-saved.component';
import { InitiativeLearnContentService } from '../../services/initiative-learn-content.service';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { InitiativeContentTabEnum } from 'src/app/initiatives/shared/enums/initiative-content-tabs.enum';
import { TranslateService } from '@ngx-translate/core';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';

@Component({
  selector: 'neo-initiative-learn',
  templateUrl: './initiative-learn.component.html',
  styleUrls: ['./initiative-learn.component.scss']
})
export class InitiativeLearnComponent implements OnInit, OnDestroy {
  selectedTab: InitiativeContentTabEnum = InitiativeContentTabEnum.Recommended;
  isLoading = false;
  initiativeId: number;
  articleIds: number[] = [];
  isCounterActive = false;
  counter = 0;
  articleRecommendationsCounter = '';

  @ViewChild(InitiativeLearnRecommendedComponent, { static: false })
  initiativeLearnRecommendedComponent: InitiativeLearnRecommendedComponent;

  @ViewChild(InitiativeLearnSavedComponent)
  childComponent: InitiativeLearnSavedComponent;

  private unsubscribe$ = new Subject<void>();

  constructor(
    private snackbarService: SnackbarService,
    private initiativeLearnContentService: InitiativeLearnContentService,
    private commonService: CommonService,
    private route: ActivatedRoute,
    private readonly translateService: TranslateService,
    private initiativeSharedService: InitiativeSharedService,
    private activityService: ActivityService,
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
        buttonName: this.translateService.instant('general.saveLabel') + this.translateService.instant('general.selectionsLabel'),
        moduleName: InitiativeModulesEnum[InitiativeModulesEnum.Learn]
      })
      ?.subscribe();
    this.articleIds = this.initiativeLearnContentService.getSelectedTiles().LearnContent.map(item => item.id);
    if (this.articleIds.length > 0) {
      this.initiativeSharedService
        .saveSelectedContentsForAnInitiative(this.initiativeId, false, this.articleIds)
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
            this.childComponent.loadSavedArticles();
          }
          let successMessage = this.translateService.instant('general.contentLabel');
          if (this.articleIds.length > 1) {
            successMessage = successMessage + 's';
          }
          this.snackbarService.showSuccess(
            `${successMessage} ${this.translateService.instant('initiative.viewInitiative.successfullySavedMessage')}`
          );
          this.counter = 0;
          this.initiativeLearnContentService.clearSelectedTiles();
          this.initiativeLearnRecommendedComponent.counter = 0;
          this.initiativeLearnRecommendedComponent.selectedTiles = [];
          this.initiativeLearnRecommendedComponent.loadArticles(this.articleIds.length);
        });
    }
  }

  ngOnDestroy(): void {
    this.initiativeLearnContentService.clearSelectedTiles();
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this.initiativeSharedService.updateInitiativeContentLastViewedDate(this.initiativeId, InitiativeModulesEnum.Learn)
    .pipe(takeUntil(this.unsubscribe$))
    .subscribe();
  }

  private loadRecommendationCount(): void {
    this.initiativeSharedService
      .getNewRecommendationsCount(this.initiativeId, InitiativeModulesEnum.Learn)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(result => {
        if (result.length > 0) {
          this.articleRecommendationsCounter = this.initiativeSharedService.displayCounter(result[0].articlesCount);
          this.isLoading = true;
          this.cdr.detectChanges();
        }
      });
  }
  updateArticlesRecommendationsCounter(counter: number): void {
    this.articleRecommendationsCounter = this.initiativeSharedService.displayCounter(counter);
  }
}
