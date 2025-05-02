import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { catchError, filter, of, Subject, switchMap, takeUntil, throwError } from 'rxjs';
import { CommonService } from 'src/app/core/services/common.service';
import { FileService } from 'src/app/shared/services/file.service';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { FileBlobInterface } from 'src/app/shared/interfaces/file-blob.interface';
import { BlobTypeEnum } from 'src/app/shared/enums/api/general-api.enum';
import { FileInterface } from 'src/app/shared/interfaces/file.interface';
import { FileTypeEnum } from 'src/app/shared/enums/file-type.enum';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { CoreService } from 'src/app/core/services/core.service';
import { ActivatedRoute } from '@angular/router';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { TitleService } from 'src/app/core/services/title.service';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { BaseInitiativeInterface } from 'src/app/initiatives/+initatives/+view-initiative/interfaces/base-initiative.interface';
import { FileExtensionEnum } from 'src/app/shared/enums/file-extension.enum';

@Component({
  selector: 'neo-file-list-view',
  templateUrl: './file-list-view.component.html',
  styleUrls: ['./file-list-view.component.scss']
})
export class FileListViewComponent implements OnInit, OnDestroy {
  file: File;
  initialLoad = true;
  showDeleteModal = false;
  clickedFileId: number;
  showFileDuplicateModal = false;
  defaultItemPerPage = 10;
  currentPageNumber = 1;
  initiativeDetails: BaseInitiativeInterface;
  initiativeFiles: PaginateResponseInterface<FileInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: -1
  };

  paging: PaginationInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null
  };
  nameAsc = true;
  fileData: string;
  dateAsc: boolean;
  typeAsc: boolean;
  sizeAsc: boolean;
  orderBy: string;
  sortingCriteria = {
    name: 'name',
    date: 'date',
    type: 'type',
    size: 'size'
  };
  initiativeId: number;
  tdTitleClick: string;
  isLoading: boolean;
  oldExistingFileName: string;
  private fileSelected$ = new Subject<FormData>();
  private unsubscribe$ = new Subject<void>();
  showFileMaxModal = false;
  keepBothFiles = false;
  contactUsModal = false;
  fileName: string;
  fileVersion: number;
  fileType: FileTypeEnum;
  fileExtension: string;
  formData = new FormData();
  fileBlobName: string;
  currentUser: UserInterface;
  isAdmin: boolean = false;
  @Output() pageChangeDetected = new EventEmitter<boolean>();
  pageDataLoad$ = new Subject<void>();
  fileUploadModal: boolean = false;
  fileDescription: string;
  disableReplace = false;

  constructor(
    private commonService: CommonService,
    private fileService: FileService,
    private snackbarService: SnackbarService,
    private translateService: TranslateService,
    private coreService: CoreService,
    private route: ActivatedRoute,
    private initiativeSharedService: InitiativeSharedService,
    private titleService: TitleService,
    private activityService: ActivityService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.initiativeId = params['id'];
    });
    this.titleService.setTitle('initiative.viewAllFilesInitiative.viewAllFilesLabel');
    this.listenForFileSelected();
    this.loadInitiativeFiles();
    this.listenForCurrentUser();
    this.loadInitiativeDetails();
    this.pageDataLoad$.next();
    this.fileDescription = this.translateService.instant('initiative.viewInitiative.permittedFileDescription');
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this.fileService.clearAll();
  }

  goBack() {
    this.commonService.goBack();
  }

  changePage(page: number): void {
    this.currentPageNumber = page;
    this.paging.skip = (page - 1) * this.defaultItemPerPage;
    this.fileService.setPaging(this.paging);
    this.pageDataLoad$.next();
  }

  onFileSelect(value): void {
    let event = value.event;
    if (this.initiativeFiles.count >= 5) {
      this.showFileMaxModal = true;
      return;
    }
    const file: File = event?.target?.files[0];
    this.file = file;
    if (file) {
      this.fileName = value.title;
      this.fileExtension = file.name.substring(file.name.lastIndexOf('.') + 1);

      this.fileType = this.fileService.isFileImage(this.fileExtension) ? FileTypeEnum.Image : FileTypeEnum.Document;

      if (event.target.files.length > 0) {
        this.formData = new FormData();
        this.formData.append('file', file);

        this.fileService
          .validateFileCountAndIsFileExist(this.initiativeId, this.fileName + '.' + this.fileExtension)
          .pipe(
            catchError(error => {
              this.snackbarService.showError('general.defaultErrorLabel');
              return;
              return throwError(error);
            })
          )
          .subscribe(file => {
            // item1 - given file existence details and item2 - initiative file count
            if (file['item2'] >= 5) {
              this.showFileMaxModal = true;
              return;
            }
            if (!file['item1'].isExist) {
              this.fileSelected$.next(this.formData);
            } else {
              this.disableReplace = file['item1']?.isOwner === false;
              this.oldExistingFileName = file['item1'].actualFileName;
              this.fileBlobName = file['item1'].blobName;
              this.showFileDuplicateModal = true;
              this.fileVersion = file['item1'].fileVersion;
            }
          });
      }
    } else {
      this.snackbarService.showError('general.wrongLargeFileSizeLabel');
    }
  }

  setTitleClick() {
    const orderByValue = this.fileService.orderBy$.getValue();
    if (orderByValue) {
      const criteriaName = orderByValue.split('.')[0];
      if (this.sortingCriteria[criteriaName]) {
        this.tdTitleClick = this.sortingCriteria[criteriaName];
      }
    }
  }

  sortCriteriaSelection(sortDirection: string, sortKey: string): void {
    this.tdTitleClick = sortKey;
    if (this[sortDirection]) {
      this.sortFilesList(`${this.sortingCriteria[sortKey]}.desc`);
    } else {
      this.sortFilesList(`${this.sortingCriteria[sortKey]}.asc`);
    }
    this[sortDirection] = !this[sortDirection];
  }

  sortFilesList(orderBy: string): void {
    this.orderBy = orderBy;
    this.fileService.setOrderBy(orderBy);
    this.pageDataLoad$.next();
  }

  setLastAppliedOrderBy(): void {
    this.orderBy =
      this.initialLoad && this.fileService.orderBy$.getValue() ? this.fileService.orderBy$.getValue() : this.orderBy;
  }

  loadInitiativeFiles() {
    this.pageDataLoad$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          this.setLastAppliedOrderBy();
          if (this.initialLoad) {
            this.setTitleClick();
            this.nameAsc = this.orderBy === `${this.sortingCriteria.name}.asc`;
            this.sizeAsc = this.orderBy === `${this.sortingCriteria.size}.asc`;
            this.typeAsc = this.orderBy === `${this.sortingCriteria.type}.asc`;
            this.dateAsc = this.orderBy === `${this.sortingCriteria.date}.asc`;
          }
          const paging = this.coreService.deleteEmptyProps({
            ...this.paging,
            IncludeCount: true,
            OrderBy: this.orderBy,
            FilterBy: null
          });
          return this.fileService.getSavedFilesForInitiative(this.initiativeId, paging);
        })
      )
      .subscribe(data => {
        this.initialLoad = false;
        this.paging = {
          ...this.paging,
          skip: this.paging.skip ?? 0,
          total: data?.count
        };
        this.fileService.setPaging(this.paging);
        this.initiativeFiles = {
          ...this.initiativeFiles,
          count: data.count,
          dataList: [...data.dataList]
        };
        this.isLoading = false;
      });
  }

  checkFileCount() {
    if (this.initiativeFiles.count >= 5) {
      this.showFileMaxModal = true;
      return;
    }
    this.fileUploadModal = true;
  }

  private listenForFileSelected() {
    this.fileSelected$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(formData => {
          const fileBlob: FileBlobInterface = {
            overwrite: false,
            newFileName: this.fileName,
            blobType: BlobTypeEnum.Initiative,
            blobName: ''
          };
          return this.fileService.uploadFile(formData, fileBlob).pipe(
            catchError(() => {
              this.snackbarService.showError('general.defaultErrorLabel');
              return of();
            })
          );
        }),
        switchMap(data => {
          const file: FileInterface = {
            extension: this.fileExtension,
            type: this.fileType,
            link: data.uri,
            blobName: data.name,
            actualFileTitle: this.fileName,
            actualFileName: this.file.name,
            size: this.file.size,
            version: this.keepBothFiles ? ++this.fileVersion : 0
          };
          return this.fileService.saveInitiativeFile(file, this.initiativeId).pipe(
            catchError(() => {
              this.snackbarService.showError('general.defaultErrorLabel');
              return of();
            })
          );
        })
      )
      .subscribe(isFileUploaded => {
        if (isFileUploaded) {
          this.showFileDuplicateModal = false;
          this.snackbarService.showSuccess(
            this.translateService.instant('initiative.viewInitiative.fileUploadSuccess')
          );
          this.keepBothFiles = false;
          this.pageDataLoad$.next();
          this.fileUploadModal = false;
        }
      });
  }

  confirmDelete() {
    this.showDeleteModal = false;
    this.initiativeSharedService
      .deleteSavedContent(this.initiativeId, this.clickedFileId, InitiativeModulesEnum.Files)
      .subscribe(
        result => {
          if (result) {
            this.snackbarService.showSuccess(
              this.translateService.instant('initiative.viewInitiative.successContentRemoveMessage')
            );
            if (
              this.initiativeFiles.count > this.defaultItemPerPage &&
              this.initiativeFiles.count % this.defaultItemPerPage === 1
            ) {
              this.currentPageNumber -= 1;
              this.changePage(this.currentPageNumber);
            } else {
              this.pageDataLoad$.next();
            }
          } else {
            this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          }
          this.isLoading = false;
        },
        () => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      );
  }

  optionClick(id: number): void {
    this.showDeleteModal = true;
    this.clickedFileId = id;
  }

  deleteFile(id: number): void {
    this.optionClick(id);
  }

  downloadFile(fileBlobName: string, fileName: string, fileExtension: string): void {
    this.fileService
      .exportFile(fileBlobName)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(data => {
        const url = window.URL.createObjectURL(data);
        const link = document.createElement('a');
        link.href = url;
        link.download = `${fileName + '.' + FileExtensionEnum[fileExtension]}`;
        link.click();
      });
  }

  closeDeletePopup(): void {
    this.showDeleteModal = false;
  }

  closePopup(): void {
    this.showFileDuplicateModal = false;
  }

  closeFileMaxPopup(): void {
    this.showFileMaxModal = false;
  }

  replaceFile() {
    const formData = new FormData();
    formData.append('file', this.file);
    const fileBlob: FileBlobInterface = {
      overwrite: true,
      newFileName: '',
      blobType: BlobTypeEnum.Initiative,
      blobName: this.fileBlobName
    };
    this.fileService.uploadFile(formData, fileBlob).subscribe(
      uploadedFile => {
        if (uploadedFile.name) {
          this.fileService
            .updateFileModifiedDateAndSize(this.fileBlobName, this.file.size, this.initiativeDetails.initiativeId)
            .subscribe(isFileDateUpdated => {
              if (isFileDateUpdated) {
                this.showFileDuplicateModal = false;
                this.snackbarService.showSuccess(
                  this.translateService.instant('initiative.viewInitiative.fileUploadSuccess')
                );
                this.pageDataLoad$.next();
                this.fileUploadModal = false;
              }
            });
        }
      },
      () => {
        this.snackbarService.showError(this.translateService.instant('initiative.viewInitiative.fileUploadError'));
      }
    );
  }

  keepBothTheFiles() {
    this.keepBothFiles = true;
    this.fileSelected$.next(this.formData);
  }
  trackFileActivity() {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        id: this.initiativeId,
        buttonName: this.translateService.instant('general.uploadLabel'),
        moduleName: InitiativeModulesEnum[InitiativeModulesEnum.Files]
      })
      ?.subscribe();
  }

  private listenForCurrentUser(): void {
    this.authService
      .currentUser()
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(user => !!user)
      )
      .subscribe(currentUser => {
        if (currentUser) {
          this.currentUser = currentUser;
          this.isAdmin = this.currentUser.roles.some(role => role.id === RolesEnum.Admin);
        }
      });
  }

  private loadInitiativeDetails(): void {
    this.initiativeSharedService
      .getInitiativeDetailsByInitiativeId(this.initiativeId)
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(user => !!user)
      )
      .subscribe(initiative => {
        this.initiativeDetails = initiative;
      });
  }

  hasAccessToDeleteForInitiativeFiles(file: FileInterface): boolean {
    return this.fileService.hasAccessToDeleteForInitiativeFiles(
      this.currentUser,
      file,
      this.initiativeDetails.user.id === this.currentUser.id,
      this.initiativeDetails.collaborators.some(collaborator => collaborator.id === this.currentUser.id)
    );
  }
}
