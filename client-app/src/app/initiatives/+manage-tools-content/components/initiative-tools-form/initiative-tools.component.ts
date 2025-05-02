import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { catchError, takeUntil } from "rxjs/operators";
import { CommonService } from "src/app/core/services/common.service";
import { SnackbarService } from "src/app/core/services/snackbar.service";
import { InitiativeSharedService } from "src/app/initiatives/shared/services/initiative-shared.service";
import { InitiativeToolsRecommendedComponent } from "../initiative-tools-recommended-tab/initiative-tools-recommended.component";
import { InitiativeToolsSavedComponent } from "../initiative-tools-saved-tab/initiative-tools-saved-tab.component";
import { InitiativeToolContentService } from "../../services/initiative-tool-content.service";
import { InitiativeContentTabEnum } from "src/app/initiatives/shared/enums/initiative-content-tabs.enum";
import { InitiativeModulesEnum } from "src/app/initiatives/enums/initiative-modules.enum";
import { Subject } from "rxjs";
import { TranslateService } from "@ngx-translate/core";
import { ActivityTypeEnum } from "src/app/core/enums/activity/activity-type.enum";
import { ActivityService } from "src/app/core/services/activity.service";

@Component({
  selector: 'neo-initiative-tool',
  templateUrl: './initiative-tools.component.html',
  styleUrls: ['./initiative-tools.component.scss']
})
export class InitiativeToolsComponent implements OnInit {
  isLoading = false;
  toolIds: number[] = [];
  initiativeId!: number;
  selectedTab: InitiativeContentTabEnum = InitiativeContentTabEnum.Saved;
  toolRecommendationsCounter = '';
  counter = 0;
  isCounterActive = false;
  counterLength!: number;
  private unsubscribe$ = new Subject<void>();
  @ViewChild(InitiativeToolsSavedComponent) childComponent!: InitiativeToolsSavedComponent;
  @ViewChild(InitiativeToolsRecommendedComponent, { static: false })
  initiativeToolRecommendedComponent!: InitiativeToolsRecommendedComponent;
  constructor(
    private initiativeToolContentService: InitiativeToolContentService,
    private commonService: CommonService,
    private initiativeSharedService: InitiativeSharedService,
    private snackbarService: SnackbarService,
    private route: ActivatedRoute,
    private readonly translateService: TranslateService,
    private readonly cdr: ChangeDetectorRef,
    private activityService: ActivityService
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.initiativeId = +params['id'];
    });
    this.route.queryParams.subscribe(params => {
      this.selectedTab = +params['tab'] || InitiativeContentTabEnum.Saved;
      if (this.selectedTab === InitiativeContentTabEnum.Saved)
        this.loadRecommendationCount();
    });
  }

  goBack() {
    this.commonService.goBack();
  }

  handleCounterChange(counter: number) {
    this.isCounterActive = counter > 0;
    this.counter = counter;
  }

  changeTab(tabName: InitiativeContentTabEnum): void {
    this.selectedTab = tabName;
  }

  saveSelectedContents() {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        id: this.initiativeId,
        buttonName:
          this.translateService.instant('general.saveLabel') + this.translateService.instant('general.selectionsLabel'),
        moduleName: InitiativeModulesEnum[InitiativeModulesEnum.Tools]
      })
      ?.subscribe();
    this.toolIds = this.initiativeToolContentService.getSelectedTiles().ToolContent.map(item => item.id);
    if (this.toolIds.length > 0) {
      this.initiativeSharedService
        .saveSelectedContentsForAnInitiative(this.initiativeId, false, null, null, null, this.toolIds, null)
        .pipe(
          catchError(error => {
            this.snackbarService.showError('general.defaultErrorLabel');
            throw error;
          })
        )
        .subscribe(() => {
          if (this.selectedTab === InitiativeContentTabEnum.Saved) {
            this.childComponent.changePage(1);
            this.childComponent.loadSavedTools();
          }
          let successMessage = this.translateService.instant('general.toolLabel');
          if (this.toolIds.length > 1) {
            successMessage = successMessage + 's';
          }
          this.snackbarService.showSuccess(
            `${successMessage} ${this.translateService.instant('initiative.viewInitiative.successfullySavedMessage')}`
          );
          this.counter = 0;
          this.initiativeToolContentService.clearSelectedTiles();
          this.initiativeToolRecommendedComponent.selectedTiles = [];
          this.initiativeToolRecommendedComponent.loadTools(this.toolIds.length);
        });
    }
  }
  private loadRecommendationCount(): void {
    this.initiativeSharedService.getNewRecommendationsCount(this.initiativeId, InitiativeModulesEnum.Tools)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(result => {
        if (result.length > 0) {
          this.toolRecommendationsCounter = this.initiativeSharedService.displayCounter(result[0].toolsCount);
          this.isLoading = true;
          this.cdr.detectChanges();
        }
      });
  }
  updateToolsRecommendationsCounter(counter: number): void {
    this.toolRecommendationsCounter = this.initiativeSharedService.displayCounter(counter);
  }
  ngOnDestroy(): void {
    this.initiativeSharedService.updateInitiativeContentLastViewedDate(this.initiativeId, InitiativeModulesEnum.Tools)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe();
  }
}
