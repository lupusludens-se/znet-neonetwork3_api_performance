import { Component, Input, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { catchError, of } from "rxjs";
import { SnackbarService } from "src/app/core/services/snackbar.service";
import { InitiativeModulesEnum } from "src/app/initiatives/enums/initiative-modules.enum";
import { InitiativeToolInterface } from "src/app/initiatives/shared/models/initiative-resources.interface";
import { InitiativeSharedService } from "src/app/initiatives/shared/services/initiative-shared.service";
import { PaginateResponseInterface } from "src/app/shared/interfaces/common/pagination-response.interface";
import { PaginationInterface } from "src/app/shared/modules/pagination/pagination.component";
import { InitiativeToolContentService } from "../../services/initiative-tool-content.service";
import { InitiativeSavedContentListRequestInterface } from "src/app/initiatives/+initatives/+view-initiative/interfaces/initiative-saved-content-list-request.interface";
import { TitleService } from "src/app/core/services/title.service";

@Component({
  selector: 'neo-initiative-tools-saved',
  templateUrl: './initiative-tools-saved-tab.component.html',
  styleUrls: ['./initiative-tools-saved-tab.component.scss']
})
export class InitiativeToolsSavedComponent implements OnInit {
  
  @Input() initiativeId: number;

  currentPageNumber = 1;
  defaultPageItems = 12;
  showDeleteModal = false;
  clickedToolId: number;
  responseData: PaginateResponseInterface<InitiativeToolInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: -1
  };
  requestData: InitiativeSavedContentListRequestInterface = {
    skip: 0,
    take: this.defaultPageItems,
    total: 0,
    includeCount: true
  };

  paging: PaginationInterface = {
    take: this.defaultPageItems,
    skip: 0,
    total: null
  };

  constructor(
    private initiativeToolContentService: InitiativeToolContentService,
    private snackbarService: SnackbarService,
    private initiativeSharedService: InitiativeSharedService,
    private translateService: TranslateService,
    private titleService: TitleService
  ) {}

  ngOnInit() {
    this.loadSavedTools();
    this.titleService.setTitle('initiative.viewInitiative.viewAllToolsLabel');
  }

  changePage(page: number): void {
    this.currentPageNumber = page;
    this.requestData.skip = this.paging.skip = (page - 1) * this.paging.take;
    this.loadSavedTools();
  }

  loadSavedTools() {
    this.initiativeToolContentService.getSavedToolsOfAnInitiative(this.initiativeId, this.requestData).subscribe(data => {
      this.responseData = data;
      this.paging = {
        ...this.paging,
        skip: this.paging.skip ?? 0,
        total: data.count
      };
    });
  }

  closeDeletePopup() {
    this.showDeleteModal = false;
  }

  showRemoveModal(selectedToolId: number) {
    this.showDeleteModal = true;
    this.clickedToolId = selectedToolId;
  }

  confirmDelete() {
    this.showDeleteModal = false;
    this.initiativeSharedService
      .deleteSavedContent(this.initiativeId, this.clickedToolId, InitiativeModulesEnum.Tools)
      .pipe(
        catchError(error => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(error);
        })
      )
      .subscribe(result => {
        if (result) {
          this.snackbarService.showSuccess(
            this.translateService.instant('initiative.viewInitiative.successToolRemoveMessage')
          );
          const removeIndex = this.responseData.dataList.findIndex(p => p.id === this.clickedToolId);
          if (removeIndex > -1) this.responseData.dataList.splice(removeIndex, 1);
          this.changePage(this.responseData.dataList.length > 0 || this.currentPageNumber === 1 ? this.currentPageNumber : this.currentPageNumber - 1);
        } else {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      });
  }
}
