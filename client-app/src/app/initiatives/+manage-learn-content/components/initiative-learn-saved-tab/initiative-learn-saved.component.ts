import { Component, OnInit, Input } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { catchError, of } from "rxjs";
import { SnackbarService } from "src/app/core/services/snackbar.service";
import { InitiativeModulesEnum } from "src/app/initiatives/enums/initiative-modules.enum";
import { InitiativeSharedService } from "src/app/initiatives/shared/services/initiative-shared.service";
import { PaginateResponseInterface } from "src/app/shared/interfaces/common/pagination-response.interface";
import { PaginationInterface } from "src/app/shared/modules/pagination/pagination.component";
import { InitiativeLearnContentService } from "../../services/initiative-learn-content.service";
import { InitiativeArticleInterface } from "src/app/initiatives/shared/models/initiative-resources.interface";
import { InitiativeSavedContentListRequestInterface } from "src/app/initiatives/+initatives/+view-initiative/interfaces/initiative-saved-content-list-request.interface";
import { TitleService } from "src/app/core/services/title.service";

@Component({
  selector: 'neo-initiative-learn-saved',
  templateUrl: './initiative-learn-saved.component.html',
  styleUrls: ['./initiative-learn-saved.component.scss']
})
export class InitiativeLearnSavedComponent implements OnInit {
  showDeleteModal = false;
  hasDataLoaded : boolean = false;
  clickedArticleId: number;
  currentPageNumber = 1;
  responseData: PaginateResponseInterface<InitiativeArticleInterface> = {
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

  constructor(
    private readonly initiativeLearnContentService: InitiativeLearnContentService,
    private readonly initiativeSharedService: InitiativeSharedService,
    private readonly snackbarService: SnackbarService,
    private readonly translateService: TranslateService,
    private titleService: TitleService
  ) { }

  ngOnInit(): void {
    this.loadSavedArticles();
    this.titleService.setTitle('initiative.viewInitiative.viewAllLearnLabel');
  }

  changePage(page: number): void {
    this.currentPageNumber = page;
    this.requestData.skip = this.paging.skip = (page - 1) * this.paging.take;
    this.loadSavedArticles();
  }
	loadSavedArticles() {
		this.initiativeLearnContentService.getSavedArticlesOfAnInitiative(this.initiativeId, this.requestData).subscribe(data => {
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

  showRemoveModal(selectedArticleId: number) {
    this.showDeleteModal = true;
    this.clickedArticleId = selectedArticleId;
  }

  confirmDelete() {
    this.showDeleteModal = false;
    this.initiativeSharedService
      .deleteSavedContent(this.initiativeId, this.clickedArticleId, InitiativeModulesEnum.Learn)
      .pipe(
        catchError(error => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(false);
        })
      )
      .subscribe(result => {
        if (result) {
          this.snackbarService.showSuccess(
            this.translateService.instant('initiative.viewInitiative.successContentRemoveMessage')
          );
          const removeIndex = this.responseData.dataList.findIndex(p => p.id === this.clickedArticleId);
          if (removeIndex > -1) this.responseData.dataList.splice(removeIndex, 1);
          this.responseData.dataList.length > 0 || this.currentPageNumber === 1
            ? this.changePage(this.currentPageNumber)
            : this.changePage(this.currentPageNumber - 1);
        } else {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      }, error => {
        this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
      });
  }
}
