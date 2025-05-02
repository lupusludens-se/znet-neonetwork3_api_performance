import { Component, EventEmitter, Input, OnInit, Output, OnDestroy } from '@angular/core';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { InitiativeSavedContentListRequestInterface } from '../../interfaces/initiative-saved-content-list-request.interface';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { catchError, interval, of, startWith, Subject, switchMap, takeUntil } from 'rxjs';
import { ActivityService } from 'src/app/core/services/activity.service';
import { LocationStrategy } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';
import { ViewInitiativeService } from '../../services/view-initiative.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { Router } from '@angular/router';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { InitiativeProjectInterface } from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { InitiativeContentTabEnum } from 'src/app/initiatives/shared/enums/initiative-content-tabs.enum';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';

@Component({
  selector: 'neo-project-section',
  templateUrl: './project-section.component.html',
  styleUrls: ['./project-section.component.scss']
})
export class ProjectSectionComponent implements OnInit, OnDestroy {
  showDeleteModal = false;
  savedProjects: PaginateResponseInterface<InitiativeProjectInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: 0
  };
  defaultItemPerPage = 4;
  page = 1;
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

  clickedProjectId: number;
  @Input() initiativeId: number;
  @Input() initiativeTitle: string;
  @Input() isAdminOrTeamMember: boolean = false;
  @Output() postClick = new EventEmitter<void>();
  @Output() tagClick = new EventEmitter<void>();
  private loadInitiativeSavedProjects$ = new Subject<void>();
  private unsubscribe$ = new Subject<void>();
  type = TaxonomyTypeEnum;
  projectsRecommendationsCounter: string = '';
  constructor(
    private readonly activityService: ActivityService,
    private locationStrategy: LocationStrategy,
    private initiativeSharedService: InitiativeSharedService,
    public router: Router,
    private viewInitiativeService: ViewInitiativeService,
    private readonly snackbarService: SnackbarService,
    private translateService: TranslateService
  ) {}

  ngOnInit() {
    this.loadRecommendationCount();
    this.loadSavedProjects();
    this.loadInitiativeSavedProjects$.next();
  }

  loadSavedProjects() {
    this.loadInitiativeSavedProjects$
      .pipe(
        switchMap(() => {
          const requestData = { ...this.requestData };
          delete requestData.skip;
          delete requestData.take;
          return this.viewInitiativeService.getSavedProjectsForInitiative(this.initiativeId, requestData);
        }),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(data => {
        this.savedProjects.count = data.count;
        this.savedProjects.dataList.push(...data.dataList);
        if (this.isLoading) this.isLoading = false;
      });
  }

  onLoadMoreData() {
    this.page++;
    this.defaultItemPerPage = 3;
    this.paging.skip = this.savedProjects.dataList.length;
    if (this.savedProjects.dataList.length < this.savedProjects.count) {
      this.loadMoreInitiativeSavedProjects();
    }
  }

  loadMoreInitiativeSavedProjects() {
    this.requestData.includeCount = true;
    this.requestData.skip = this.paging.skip;
    this.requestData.take = this.defaultItemPerPage;
    this.loadInitiativeSavedProjects$.next();
  }

  optionClick(id: number): void {
    this.showDeleteModal = true;
    this.clickedProjectId = id;
  }

  closeDeletePopup() {
    this.showDeleteModal = false;
  }

  confirmDelete() {
    this.showDeleteModal = false;
    this.initiativeSharedService
      .deleteSavedContent(this.initiativeId, this.clickedProjectId, InitiativeModulesEnum.Projects)
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
            this.translateService.instant('initiative.viewInitiative.successProjectRemoveMessage')
          );
          const index = this.savedProjects.dataList.findIndex(p => p.id === this.clickedProjectId);
          if (index !== -1) {
            this.savedProjects.dataList.splice(index, 1);
            this.savedProjects.count--;
            if (this.savedProjects.dataList.length < 4 && this.savedProjects.count >= 4) {
              this.page = 0;
              this.onLoadMoreData();
            }
          }
        } else {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      });
  }

  viewProjects(): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativeModuleViewAllClick, {
        id: this.initiativeId,
        title: this.initiativeTitle,
        resourceType: InitiativeModulesEnum[InitiativeModulesEnum.Projects]
      })
      ?.subscribe();
    this.router.navigate(['decarbonization-initiatives', this.initiativeId, 'projects'], {
      queryParams: {
        tab:
          this.savedProjects.dataList.length && this.projectsRecommendationsCounter === ''
            ? InitiativeContentTabEnum.Saved
            : InitiativeContentTabEnum.Recommended
      }
    });
  }

  openProject(project: InitiativeProjectInterface) {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ProjectView, {
        projectId: project?.id,
        initiativeId: this.initiativeId
      })
      ?.subscribe();
    window.open(`projects/${project?.id}`, '_blank');
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this.loadInitiativeSavedProjects$.complete();
  }
  loadRecommendationCount(): void {
    interval(60000)
      .pipe(
        startWith(0),
        switchMap(() =>
          this.initiativeSharedService.getNewRecommendationsCount(this.initiativeId, InitiativeModulesEnum.Projects)
        ),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(data => {
        this.projectsRecommendationsCounter = data
          ? this.initiativeSharedService.displayCounter(data[0].projectsCount)
          : '';
      });
  }
}
