import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { catchError, of, Subject, switchMap, tap, finalize, interval, startWith } from 'rxjs';
import { ViewInitiativeService } from '../../services/view-initiative.service';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { InitiativeSavedContentListRequestInterface } from '../../interfaces/initiative-saved-content-list-request.interface';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { InitiativeToolInterface } from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { LocationStrategy } from '@angular/common';
import { Router } from '@angular/router';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { TranslateService } from '@ngx-translate/core';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { InitiativeContentTabEnum } from 'src/app/initiatives/shared/enums/initiative-content-tabs.enum';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { takeUntil } from 'rxjs/operators';
@Component({
  selector: 'neo-tools-section',
  templateUrl: './tools-section.component.html',
  styleUrls: ['./tools-section.component.scss']
})
export class ToolsSectionComponent implements OnInit {
  @Input() initiativeId: number;
  @Input() initiativeTitle: string;
  @Input() isAdminOrTeamMember: boolean = false;
  @Output() toolClick = new EventEmitter<void>();
  public loadInitiativeSavedTools$ = new Subject<void>();
  defaultItemPerPage = 4;
  page = 1;
  paging: PaginationInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null
  };
  savedTools: PaginateResponseInterface<InitiativeToolInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: 0
  };
  requestData: InitiativeSavedContentListRequestInterface = {
    skip: 0,
    take: 6,
    total: 0,
    includeCount: true
  };
  isLoading = true;
  showDeleteModal = false;
  clickedToolId: number;
  toolsRecommendationsCounter: string = '';
  private unsubscribe$ = new Subject<void>();
  options: MenuOptionInterface[] = [
    {
      icon: 'trash-can-red',
      name: 'initiative.viewInitiative.deleteSavedContentLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];

  constructor(
    private readonly viewInitiativeService: ViewInitiativeService,
    private readonly activityService: ActivityService,
    private readonly locationStrategy: LocationStrategy,
    public readonly router: Router,
    private readonly initiativeSharedService: InitiativeSharedService,
    private readonly snackbarService: SnackbarService,
    private readonly translateService: TranslateService
  ) {}

  ngOnInit() {
    this.loadRecommendationCount();
    this.loadInitiativeSavedTools();
    this.loadInitiativeSavedTools$.next();
  }

  loadInitiativeSavedTools() {
    this.loadInitiativeSavedTools$
      .pipe(
        switchMap(() => {
          const requestData = { ...this.requestData };
          delete requestData['skip'];
          delete requestData['take'];
          return this.viewInitiativeService.getSavedToolsForInitiative(this.initiativeId, this.requestData);
        }),
        tap(data => {
          this.savedTools.count = data.count;
          this.savedTools.dataList.push(...data.dataList);
        }),
        finalize(() => {})
      )
      .subscribe(() => {
        if (this.isLoading) this.isLoading = false;
      });
  }

  onLoadMoreData() {
    this.page++;
    this.defaultItemPerPage = 3;
    this.paging.skip = this.savedTools.dataList.length;
    if (this.savedTools.dataList.length < this.savedTools.count) {
      this.loadMoreInitiativeSavedTools();
    }
  }

  loadMoreInitiativeSavedTools() {
    this.requestData.includeCount = true;
    this.requestData.skip = this.paging.skip;
    this.requestData.take = this.defaultItemPerPage;
    this.loadInitiativeSavedTools$.next();
  }

  routeToToolPage(path: string, id: number) {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ToolClick, { toolId: id, initiativeId: this.initiativeId })
      ?.subscribe();

    const getBaseHref = location.origin + this.locationStrategy.getBaseHref();
    const serializedUrl = getBaseHref + this.router.serializeUrl(this.router.createUrlTree([`${path}`]));
    window.open(serializedUrl, '_blank');

    this.toolClick.emit();
  }

  optionClick(id: number): void {
    this.showDeleteModal = true;
    this.clickedToolId = id;
  }

  closeDeletePopup() {
    this.showDeleteModal = false;
  }

  confirmDelete() {
    this.showDeleteModal = false;
    this.initiativeSharedService
      .deleteSavedContent(this.initiativeId, this.clickedToolId, InitiativeModulesEnum.Tools)
      .pipe(
        catchError(error => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(error);
        }),
        tap(result => {
          if (result) {
            this.snackbarService.showSuccess(
              this.translateService.instant('initiative.viewInitiative.successToolRemoveMessage')
            );
            this.savedTools.dataList = this.savedTools.dataList.filter(p => p.id !== this.clickedToolId);
            this.savedTools.count--;
            if (this.savedTools.dataList.length < 4 && this.savedTools.count >= 4) {
              this.page = 0;
              this.onLoadMoreData();
            }
          } else {
            this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          }
        }),
        finalize(() => {})
      )
      .subscribe();
  }

  viewAllToolRecommendations(): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativeModuleViewAllClick, {
        id: this.initiativeId,
        title: this.initiativeTitle,
        resourceType: InitiativeModulesEnum[InitiativeModulesEnum.Tools]
      })
      ?.subscribe();
    this.router.navigate(['decarbonization-initiatives', this.initiativeId, 'tools'], {
      queryParams: {
        tab:
          this.savedTools.dataList.length && this.toolsRecommendationsCounter === ''
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
          this.initiativeSharedService.getNewRecommendationsCount(this.initiativeId, InitiativeModulesEnum.Tools)
        ),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(data => {
        this.toolsRecommendationsCounter = data ? this.initiativeSharedService.displayCounter(data[0].toolsCount) : '';
      });
  }
}
