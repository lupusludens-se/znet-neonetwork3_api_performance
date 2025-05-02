import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { CreateInitiativeService } from '../../services/create-initiative.service';
import { catchError } from 'rxjs';
import { Router } from '@angular/router';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { ViewportScroller } from '@angular/common';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'neo-initiative-content',
  templateUrl: './initiative-content.component.html',
  styleUrls: ['./initiative-content.component.scss']
})
export class InitiativeContentComponent implements OnInit, OnDestroy {
  articleIds: number[];
  communityUserIds: number[];
  toolIds: number[];
  messageIds: number[];
  projectIds: number[];
  showContents = true;
  @Input() basicInitiativeDetails: { id: number; title: string } = { id: 1630, title: 'Basic Initiative Details' };
  @Input() showLeaveConfirmation: boolean;
  @Output() showLeaveConfirmationChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() nextUrl: string;
  @Output() submitAction = new EventEmitter<boolean>();
  showPopUp = false;
  showSkipAllPopUp = false;
  tabData = [
    { name: 'Learn', counter: '0', visited: false },
    { name: 'Community', counter: '0', visited: false },
    { name: 'Projects', counter: '0', visited: false },
    { name: 'Tools', counter: '0', visited: false },
    { name: 'Messages', counter: '0', visited: false }
  ];
  isCounterActive: boolean = false;
  activeTab: number = 0;
  counterLength = 0;
  initialCounter = 0;
  InitiativeModulesEnum = InitiativeModulesEnum;
  showButtons = false;
  

  constructor(
    private createInitiativeService: CreateInitiativeService,
    private router: Router,
    private snackbarService: SnackbarService,
    private initiativeSharedService: InitiativeSharedService,
    private viewPort: ViewportScroller,
    private activityService: ActivityService,
    private translateService: TranslateService
  ) { }

  ngOnInit(): void {
    this.initializeAutoAttachedContent();
    window.scrollTo(0, 0);
    this.createInitiativeService.hasContentLoaded$.subscribe(result => {
      this.showButtons = result;
    });
  }

  ngOnDestroy(): void {
    this.resetSelections();
  }

  initializeAutoAttachedContent(): void {
    if (+this.createInitiativeService.autoAttached?.contentId > 0) {
      const tab = this.tabData.find(x => x.name === this.createInitiativeService.autoAttached.contentType.toString());
      if (tab) {
        tab.counter = '1';
      }
    }
  }

  onBackButtonClick(): void {
    if (this.activeTab > 0) {
      this.activeTab--;
      this.viewPort.scrollToPosition([0, 0]);
    }
  }

  nextTab(): void {
    if (this.activeTab <= this.tabData.length - 1) {
      this.updateSelectedIds();
      this.activityService
        .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
          id: this.basicInitiativeDetails.id,
          buttonName:
            this.activeTab !== 4
              ? this.translateService.instant('initiative.createInitiative.addContent.continueSelectionButtonText')
              : this.translateService.instant('initiative.createInitiative.addContent.submitSelectionButtonText'),
          moduleName: this.activeTab !== 4 ? this.tabData[this.activeTab]?.name : null
        })
        ?.subscribe();
      if (this.activeTab === this.tabData.length - 1) {
        this.saveSelectedRecommendations();
      }
      this.viewPort.scrollToPosition([0, 0]);
      if (this.activeTab === this.tabData.length - 1) return;
      this.activeTab++;
    }
  }

  updateSelectedIds(): void {
    const selectedTiles = this.createInitiativeService.getSelectedTiles();
    switch (this.activeTab) {
      case InitiativeModulesEnum.Learn:
        this.articleIds = selectedTiles.LearnContent.map(item => item.id);
        break;
      case InitiativeModulesEnum.Community:
        this.communityUserIds = selectedTiles.CommunityContent.map(item => item.id);
        break;
      case InitiativeModulesEnum.Tools:
        this.toolIds = selectedTiles.ToolContent.map(item => item.id);
        break;
      case InitiativeModulesEnum.Messages:
        this.messageIds = selectedTiles.MessageContent.map(item => item.id);
        break;
      case InitiativeModulesEnum.Projects:
        this.projectIds = selectedTiles.ProjectContent.map(item => item.id);
        break;
    }
  }

  handleCounterChange(counter: number): void {
    this.isCounterActive = counter > 0;
    this.tabData[this.activeTab].counter = this.displayCounter(counter);
  }

  onSkip(): void {
    if (+this.tabData[this.activeTab].counter > 0 || (this.activeTab == 4 && this.whetherAnyItemSelected())) {
      this.showPopUp = true;
    } else {
      this.onConfirmSkip();
    }
  }

  onSkipAll(): void {
    this.showSkipAllPopUp = true;
  }

  onConfirmSkip(): void {
    this.tabData[this.activeTab].counter = this.displayCounter(this.initialCounter);
    this.viewPort.scrollToPosition([0, 0]);
    this.createInitiativeService.popAllSelectedContents(this.activeTab);
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        id: this.basicInitiativeDetails.id,
        buttonName: this.translateService.instant('initiative.createInitiative.addContent.skipButtonText'),
        moduleName: this.tabData[this.activeTab]?.name
      })
      ?.subscribe();
    if (this.activeTab === InitiativeModulesEnum.Messages) {
      this.saveSelectedRecommendations();
    }
    if (this.activeTab !== this.tabData.length - 1) {
      this.activeTab++;
    } else {
      this.router.navigate([`/decarbonization-initiatives/${this.basicInitiativeDetails.id}`]);
    }
    this.showPopUp = false;
  }

  onConfirmSkipAll(): void {
    this.submitAction.emit(true);
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        id: this.basicInitiativeDetails.id,
        buttonName: this.translateService.instant('initiative.createInitiative.addContent.skipAllButtonText')
      })
      ?.subscribe();
    this.router.navigate([`/decarbonization-initiatives/${this.basicInitiativeDetails.id}`]);
    this.showSkipAllPopUp = false;
  }

  saveSelectedRecommendations(): void {
    this.createInitiativeService.hasContentLoaded$.next(false);
    this.submitAction.emit(true);
    this.initiativeSharedService
      .saveSelectedContentsForAnInitiative(
        this.basicInitiativeDetails.id,
        true,
        this.articleIds,
        this.communityUserIds,
        this.projectIds,
        this.toolIds,
        this.messageIds
      )
      .pipe(
        catchError(error => {
          this.snackbarService.showError('general.defaultErrorLabel');
          this.createInitiativeService.hasContentLoaded$.next(true);
          return error;
        })
      )
      .subscribe(response => {
        if (response) {
          this.showContents = false;
          this.snackbarService.showSuccess('initiative.createInitiative.addContent.successMessageLabel');
          this.router.navigate(['/decarbonization-initiatives', this.basicInitiativeDetails.id]);
        } else {
          this.createInitiativeService.hasContentLoaded$.next(true);
          this.snackbarService.showError('general.defaultErrorLabel');
        }
      });
  }

  displayCounter(counter: number): string {
    this.counterLength = counter.toString().length;
    return this.counterLength >= 3 ? '99+' : counter.toString();
  }

  resetSelections(): void {
    this.createInitiativeService.selectedContents.LearnContent = [];
    this.createInitiativeService.selectedContents.ToolContent = [];
    this.createInitiativeService.selectedContents.CommunityContent = [];
    this.createInitiativeService.selectedContents.MessageContent = [];
    this.createInitiativeService.selectedContents.ProjectContent = [];
    this.createInitiativeService.autoAttached = null;
  }

  leavePage(): void {
    const urlSegments: string[] = this.nextUrl.includes('?') ? this.nextUrl.split('?') : [];
    if (urlSegments.length > 1) {
      this.router.navigateByUrl(this.nextUrl);
    } else {
      this.router.navigate([this.nextUrl]);
    }
  }

  cancelLeavePage(): void {
    this.showLeaveConfirmation = false;
    this.showLeaveConfirmationChange.emit(this.showLeaveConfirmation);
  }

  whetherAnyItemSelected(): boolean {
    const selectedTiles = this.createInitiativeService.getSelectedTiles();
    return (
      selectedTiles.LearnContent?.length > 0 ||
      selectedTiles.CommunityContent?.length > 0 ||
      selectedTiles.ProjectContent.length > 0 ||
      selectedTiles.ToolContent.length > 0
    );
  }
  goToTab(index: number) {
    this.updateSelectedIds();
    this.activeTab = index;
  }
  isModuleVisited(value: boolean) {
    this.tabData[this.activeTab].visited = value;
  }
}
