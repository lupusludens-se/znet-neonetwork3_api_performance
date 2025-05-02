import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { InitiativeContentTabEnum } from 'src/app/initiatives/shared/enums/initiative-content-tabs.enum';
import { InitiativeProjectRecommendedComponent } from '../initiative-project-recommended-tab/initiative-project-recommended.component';
import { InitiativeProjectSavedComponent } from '../initiative-project-saved-tab/initiative-project-saved.component';
import { InitiativeProjectService } from '../../services/initiative-project.service';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { CommonService } from 'src/app/core/services/common.service';
import { ActivatedRoute } from '@angular/router';
import { catchError, Subject } from 'rxjs';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { takeUntil } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
@Component({
  selector: 'neo-initiative-project',
  templateUrl: './initiative-project.component.html',
  styleUrls: ['./initiative-project.component.scss']
})
export class InitiativeProjectComponent implements OnInit, OnDestroy {
  selectedTab: InitiativeContentTabEnum = 2;
  isLoading = false;
  @ViewChild(InitiativeProjectRecommendedComponent, { static: false })
  initiativeProjectRecommendedComponent: InitiativeProjectRecommendedComponent;
  initiativeId: number;
  projectIds: number[];
  isCounterActive: boolean = false;
  counter: number = 0;
  @ViewChild(InitiativeProjectSavedComponent) childComponent: InitiativeProjectSavedComponent;
  private unsubscribe$ = new Subject<void>();
  projectsRecommendationsCounter = '';
  constructor(
    private initiativeProjectService: InitiativeProjectService,
    private initiativeSharedService: InitiativeSharedService,
    private snackbarService: SnackbarService,
    private commonService: CommonService,
    private route: ActivatedRoute,
    private activityService: ActivityService,
    private readonly translateService: TranslateService,
    private readonly cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.initiativeId = params['id'];
    });
    this.route.queryParams.subscribe(params => {
      this.selectedTab = parseInt(params['tab']) ? parseInt(params['tab']) : 1;
      if (this.selectedTab === InitiativeContentTabEnum.Saved)
        this.loadRecommendationCount();
    });
  }

  goBack() {
    this.commonService.goBack();
  }

  changeTab(tabName: InitiativeContentTabEnum): void {
    this.selectedTab = tabName;
  }

  //Handles counter values for all tabs
  handleCounterChange(counter: number) {
    this.isCounterActive = counter > 0 ? true : false;
    this.counter = counter;
  }

  selectedItemsCounter(counter: number): void {
    this.isCounterActive = counter > 0;
    this.counter = counter;
  }
  saveSelectedContents() {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        id: this.initiativeId,
        buttonName:
          this.translateService.instant('general.saveLabel') + this.translateService.instant('general.selectionsLabel'),
        moduleName: InitiativeModulesEnum[InitiativeModulesEnum.Projects]
      })
      ?.subscribe();
    this.projectIds = this.initiativeProjectService.getSelectedTiles().ProjectContent.map(item => item.id);
    if (this.projectIds.length > 0) {
      this.initiativeSharedService
        .saveSelectedContentsForAnInitiative(this.initiativeId, false, [], [], this.projectIds, [], [])
        .pipe(
          catchError(error => {
            this.snackbarService.showError('general.defaultErrorLabel');
            return error;
          })
        )
        .subscribe(() => {
          if (this.selectedTab === InitiativeContentTabEnum.Saved) {
            this.childComponent.changePage(1);
            this.childComponent.loadSavedArticles();
          }
          let successMessage = this.translateService.instant('general.projectLabel');
          if (this.projectIds.length > 1) {
            successMessage = successMessage + 's';
          }
          this.snackbarService.showSuccess(
            `${successMessage} ${this.translateService.instant('initiative.viewInitiative.successfullySavedMessage')}`
          );
          this.resetCounter();
        });
    }
  }

  private resetCounter(): void {
    this.counter = 0;
    this.initiativeProjectService.clearSelectedTiles();
    this.initiativeProjectRecommendedComponent.counter = this.counter;
    this.initiativeProjectRecommendedComponent.selectedTiles = [];
    this.initiativeProjectRecommendedComponent.loadProjects(this.projectIds.length);
  }
  private loadRecommendationCount(): void {
    this.initiativeSharedService
      .getNewRecommendationsCount(this.initiativeId, InitiativeModulesEnum.Projects)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(result => {
        if (result.length > 0) {
          this.projectsRecommendationsCounter = this.initiativeSharedService.displayCounter(result[0].projectsCount);
          this.isLoading = true;
          this.cdr.detectChanges();
        }
      });
  }

  ngOnDestroy(): void {
    this.initiativeProjectService.clearSelectedTiles();
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this.initiativeSharedService.updateInitiativeContentLastViewedDate(this.initiativeId, InitiativeModulesEnum.Projects)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe();
  }
  updateProjectsRecommendationsCounter(counter: number): void {
    this.projectsRecommendationsCounter = this.initiativeSharedService.displayCounter(counter);
  }
}
