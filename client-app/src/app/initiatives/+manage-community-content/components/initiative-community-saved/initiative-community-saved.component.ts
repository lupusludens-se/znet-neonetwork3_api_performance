import { Component, Input, OnInit } from '@angular/core';
import { InitiativeCommunityContentService } from '../../services/initiative-community-content.service';
import { InitiativeSavedContentListRequestInterface } from 'src/app/initiatives/+initatives/+view-initiative/interfaces/initiative-saved-content-list-request.interface';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { InitiativeCommunityInterface } from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { catchError, of } from 'rxjs';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { TranslateService } from '@ngx-translate/core';
import { InitiativeCommunityItemParentModuleEnum } from 'src/app/initiatives/shared/contents/community-item/community-item.component';
import { TitleService } from 'src/app/core/services/title.service';

@Component({
  selector: 'neo-initiative-community-saved',
  templateUrl: './initiative-community-saved.component.html',
  styleUrls: ['./initiative-community-saved.component.scss']
})
export class InitiativeCommunitySavedComponent implements OnInit {
  showDeleteModal = false;
  clickedUserId: number;
  currentPageNumber = 1;
  hasDataLoaded : boolean = false;
  responseData: PaginateResponseInterface<InitiativeCommunityInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: -1
  };

  @Input() initiativeId: number;
  readonly defaultPageItems = 12;
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

  initiativeParentModuleEnum = InitiativeCommunityItemParentModuleEnum;

  constructor(
    private readonly initiativeCommunityContentService: InitiativeCommunityContentService,
    private readonly initiativeSharedService: InitiativeSharedService,
    private readonly snackbarService: SnackbarService,
    private readonly translateService: TranslateService,
    private titleService: TitleService
  ) {}

  ngOnInit(): void {
    this.loadSavedCommunityUsers();
    this.titleService.setTitle('initiative.viewInitiative.viewAllCommunityLabel');
  }

  changePage(page: number): void {
    this.currentPageNumber = page;
    this.requestData.skip = this.paging.skip = (page - 1) * this.paging.take;
    this.loadSavedCommunityUsers();
  }
  loadSavedCommunityUsers() {
    this.initiativeCommunityContentService
      .getSavedCommunityUsersOfAnInitiative(this.initiativeId, this.requestData)
      .subscribe(data => {
        this.responseData = data;
        this.hasDataLoaded = true;
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

  showRemoveModal(selectedUserId: number) {
    this.showDeleteModal = true;
    this.clickedUserId = selectedUserId;
  }

  confirmDelete() {
    this.showDeleteModal = false;
    this.initiativeSharedService
      .deleteSavedContent(this.initiativeId, this.clickedUserId, InitiativeModulesEnum.Community)
      .pipe(
        catchError(() => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(false);
        })
      )
      .subscribe(
        result => {
          if (result) {
            this.snackbarService.showSuccess(
              this.translateService.instant('initiative.viewInitiative.successCommunityRemoveMessage')
            );
            const removeIndex = this.responseData.dataList.findIndex(p => p.id === this.clickedUserId);
            if (removeIndex > -1) this.responseData.dataList.splice(removeIndex, 1);
            this.responseData.dataList.length > 0 || this.currentPageNumber === 1
              ? this.changePage(this.currentPageNumber)
              : this.changePage(this.currentPageNumber - 1);
          } else {
            this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          }
        },
        () => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      );
  }
}
