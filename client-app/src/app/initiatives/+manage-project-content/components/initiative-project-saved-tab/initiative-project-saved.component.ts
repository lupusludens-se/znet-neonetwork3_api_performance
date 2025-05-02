import { Component, Input, OnInit } from '@angular/core';
import { InitiativeProjectInterface } from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { InitiativeProjectService } from '../../services/initiative-project.service';
import { catchError, of } from 'rxjs';
import { InitiativeSavedContentListRequestInterface } from 'src/app/initiatives/+initatives/+view-initiative/interfaces/initiative-saved-content-list-request.interface';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { TranslateService } from '@ngx-translate/core';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { TitleService } from 'src/app/core/services/title.service';

@Component({
  selector: 'neo-initiative-project-saved',
  templateUrl: './initiative-project-saved.component.html',
  styleUrls: ['./initiative-project-saved.component.scss']
})
export class InitiativeProjectSavedComponent implements OnInit {
  showDeleteModal: boolean = false;
  clickedProjectId: number;
  currentPageNumber: number = 1;
  type = TaxonomyTypeEnum;
  responseData: PaginateResponseInterface<InitiativeProjectInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: -1
  };

  constructor(
    private initiativeSharedService: InitiativeSharedService,
    private initiativeProjectService: InitiativeProjectService,
    private snackbarService: SnackbarService,
    private translateService: TranslateService,
    private activityService: ActivityService,
    private titleService: TitleService
  ) {}
  @Input() initiativeId: number;
  defaultPageItems: number = 12;
  paging: PaginationInterface = {
    take: this.defaultPageItems,
    skip: 0,
    total: null
  };
  requestData: InitiativeSavedContentListRequestInterface = {
    skip: 0,
    take: this.defaultPageItems,
    total: 0,
    includeCount: true
  };

  ngOnInit(): void {
    this.loadSavedArticles();
    this.titleService.setTitle('initiative.viewInitiative.viewAllProjectsLabel');
  }
  //Method updates the value of currentPageNumber$
  changePage(page: number): void {
    this.currentPageNumber = page;
    this.requestData.skip = this.paging.skip = (page - 1) * this.paging.take;
    this.loadSavedArticles();
  }

  //Method to get contents for each page
  loadSavedArticles() {
    this.initiativeProjectService.getSavedPojectsOfAnInitiative(this.initiativeId, this.requestData).subscribe(data => {
      this.responseData = data;
      this.paging = {
        ...this.paging,
        skip: this.paging?.skip ? this.paging?.skip : 0,
        total: data.count
      };
    });
  }

  closeDeletePopup() {
    this.showDeleteModal = false;
  }

  showRemoveModal(selectedProjectId: number) {
    this.showDeleteModal = true;
    this.clickedProjectId = selectedProjectId;
  }

  confirmDelete() {
    this.showDeleteModal = false;
    this.initiativeSharedService
      .deleteSavedContent(this.initiativeId, this.clickedProjectId, InitiativeModulesEnum.Projects)
      .pipe(
        catchError(error => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(error);
        })
      )
      .subscribe(result => {
        if (result == true) {
          this.snackbarService.showSuccess(
            this.translateService.instant('initiative.viewInitiative.successProjectRemoveMessage')
          );
          var removeIndex = this.responseData.dataList.findIndex(p => p.id == this.clickedProjectId);
          if (removeIndex > -1) this.responseData.dataList.splice(removeIndex, 1);
          this.responseData.dataList.length > 0 || this.currentPageNumber == 1
            ? this.changePage(this.currentPageNumber)
            : this.changePage(this.currentPageNumber - 1);
        } else {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      });
  }

  openProject(project: InitiativeProjectInterface): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ProjectView, {
        projectId: project.id,
        initiativeId: this.initiativeId
      })
      ?.subscribe();
    window.open(`projects/${project.id}`, '_blank');
  }
}
