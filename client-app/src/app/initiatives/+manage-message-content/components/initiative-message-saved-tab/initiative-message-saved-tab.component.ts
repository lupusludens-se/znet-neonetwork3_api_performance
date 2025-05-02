import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { catchError, tap } from "rxjs/operators";
import { of } from "rxjs";
import { SnackbarService } from "src/app/core/services/snackbar.service";
import { InitiativeModulesEnum } from "src/app/initiatives/enums/initiative-modules.enum";
import { PaginateResponseInterface } from "src/app/shared/interfaces/common/pagination-response.interface";
import { PaginationInterface } from "src/app/shared/modules/pagination/pagination.component";
import { InitiativeMessageContentService } from "../../services/initiative-message-content.service";
import { InitiativeMessageInterface } from "src/app/initiatives/shared/models/initiative-resources.interface";
import { MenuOptionInterface } from "src/app/shared/modules/menu/interfaces/menu-option.interface";
import { TableCrudEnum } from "src/app/shared/modules/table/enums/table-crud.enum";
import { InitiativeSharedService } from "src/app/initiatives/shared/services/initiative-shared.service";
import { InitiativeSavedContentListRequestInterface } from "src/app/initiatives/+initatives/+view-initiative/interfaces/initiative-saved-content-list-request.interface";
import { AuthService } from "src/app/core/services/auth.service";
import { untilDestroyed } from "@ngneat/until-destroy";
import { UserInterface } from "src/app/shared/interfaces/user/user.interface";
import { TitleService } from "src/app/core/services/title.service";
import { LocationStrategy } from "@angular/common";
import { Router } from "@angular/router";

@Component({
  selector: 'neo-initiative-message-saved',
  templateUrl: './initiative-message-saved-tab.component.html',
  styleUrls: ['./initiative-message-saved-tab.component.scss']
})
export class InitiativeMessageSavedComponent implements OnInit {
  @Input() initiativeId: number;
  @Output() deleteStatusEmit = new EventEmitter<boolean>();
  showDeleteModal = false;
  selectedDiscussionId: number;
  currentPageNumber = 1;
  defaultPageItems = 12;
  responseData: PaginateResponseInterface<InitiativeMessageInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: -1
  };
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
  options: MenuOptionInterface[] = [
    {
      icon: 'trash-can-red',
      name: 'initiative.viewInitiative.deleteSavedContentLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];
  currentUser: UserInterface;

  constructor(
    private initiativeMessageContentService: InitiativeMessageContentService,
    private initiativeSharedService: InitiativeSharedService,
    private snackbarService: SnackbarService,
    private translateService: TranslateService,
    private authService: AuthService,
    private titleService: TitleService,
    private locationStrategy: LocationStrategy,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.authService.currentUser().subscribe((user: UserInterface) => {
      if (user) {
        this.currentUser = user;
        this.loadSavedDiscussions();
      }
    });
    this.titleService.setTitle('initiative.viewInitiative.viewAllMessagesLabel');
  }

  openMessage(selectedTile: InitiativeMessageInterface): void {
    const getBaseHref = location.origin + this.locationStrategy.getBaseHref();
    const serializedUrl = getBaseHref + this.router.serializeUrl(this.router.createUrlTree(['messages/'])) + "/" + selectedTile.id;
    window.open(serializedUrl, '_blank');

  }

  changePage = (page: number): void => {
    this.currentPageNumber = page;
    this.requestData.skip = this.paging.skip = (page - 1) * this.paging.take;
    this.loadSavedDiscussions();
  }

  optionClick = (id: number): void => {
    this.showDeleteModal = true;
    this.selectedDiscussionId = id;
  }

  loadSavedDiscussions = (): void => {
    this.initiativeMessageContentService.getSavedMessages(this.initiativeId, this.requestData)
      .pipe(
        tap(data => {
          this.responseData = data;
          this.paging = {
            ...this.paging,
            skip: this.paging?.skip || 0,
            total: data.count
          };
        }),
        catchError(error => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(error);
        })
      )
      .subscribe();
  }

  closeDeletePopup = (): void => {
    this.showDeleteModal = false;
  }

  showRemoveModal = (selectedDiscussionId: number): void => {
    this.showDeleteModal = true;
    this.selectedDiscussionId = selectedDiscussionId;
  }

  confirmDelete = (): void => {
    this.showDeleteModal = false;
    this.initiativeSharedService
      .deleteSavedContent(this.initiativeId, this.selectedDiscussionId, InitiativeModulesEnum.Messages)
      .pipe(
        tap(result => {
          if (result) {
            this.snackbarService.showSuccess(
              this.translateService.instant('initiative.viewInitiative.successConverationRemoveMessage')
            );
            const removeIndex = this.responseData.dataList.findIndex(p => p.id === this.selectedDiscussionId);
            if (removeIndex > -1) {
              this.responseData.dataList.splice(removeIndex, 1);
              this.changePage(this.responseData.dataList.length > 0 || this.currentPageNumber === 1 ? this.currentPageNumber : this.currentPageNumber - 1);

              this.deleteStatusEmit.emit(true);
            }
          } else {
            this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          }
        }),
        catchError(error => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(error);
        })
      )
      .subscribe();
  }
}
