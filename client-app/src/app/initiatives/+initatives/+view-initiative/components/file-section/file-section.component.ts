import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { catchError, of, Subject, switchMap, takeUntil, throwError } from 'rxjs';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { FileInterface } from 'src/app/shared/interfaces/file.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { InitiativeSavedContentListRequestInterface } from '../../interfaces/initiative-saved-content-list-request.interface';
import { FileTypeEnum } from 'src/app/shared/enums/file-type.enum';
import { TranslateService } from '@ngx-translate/core';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { FileExtensionEnum } from 'src/app/shared/enums/file-extension.enum';
import { FileService } from 'src/app/shared/services/file.service';
import { FileBlobInterface } from 'src/app/shared/interfaces/file-blob.interface';
import { BlobTypeEnum } from 'src/app/shared/enums/api/general-api.enum';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { Router } from '@angular/router';
import { CoreService } from 'src/app/core/services/core.service';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { InitiativeProgress } from 'src/app/initiatives/interfaces/initiative-progress.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'neo-file-section',
  templateUrl: './file-section.component.html',
  styleUrls: ['./file-section.component.scss']
})
export class FileSectionComponent implements OnInit, OnDestroy {
  readonly defaultItemPerPage = 6;

  showFileDuplicateModal = false;
  showDeleteModal = false;
  showFileMaxModal = false;
  keepBothFiles = false;
  disableReplace = false;
  contactUsModal = false;
  formData = new FormData();
  file: File;
  initiativeFiles: PaginateResponseInterface<FileInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: 0
  };
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

  @Input() initiativeProgress: InitiativeProgress;
  @Input() isAdminOrTeamMember: boolean = false;

  private fileSelected$ = new Subject<FormData>();
  private unsubscribe$ = new Subject<void>();
  isLoading = true;
  oldExistingFileName: string;
  fileName: string;
  fileVersion: number;
  fileExtension: string;
  clickedFileId: number;
  loadInitiativeFiles$ = new Subject<void>();
  options: MenuOptionInterface[] = [
    {
      icon: 'trash-can-red',
      name: 'actions.deleteLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];
  fileType: FileTypeEnum;
  fileBlobName: string;
  fileDescription: string;
  fileUploadModal: boolean = false;
  currentUser: UserInterface;

  constructor(
    private snackbarService: SnackbarService,
    private fileService: FileService,
    private translateService: TranslateService,
    public router: Router,
    private coreService: CoreService,
    private initiativeSharedService: InitiativeSharedService,
    private activityService: ActivityService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.authService
      .currentUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(user => {
        if (user) {
          this.currentUser = user;
        }
      });
    this.listenForFileSelected();
    this.loadInitiativeFiles();
    this.loadInitiativeFiles$.next();
    this.fileDescription = this.translateService.instant('initiative.viewInitiative.permittedFileDescription');
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  getFileType(type: number): string {
    return FileTypeEnum[type];
  }

  getFileKey(type: number): string {
    return type === FileTypeEnum.Image ? 'image' : 'file';
  }

  getFileExtension(extension: number): string {
    return FileExtensionEnum[extension];
  }

  hasAccessToDeleteForInitiativeFiles(currentUser: UserInterface, file: FileInterface): boolean {
    return this.fileService.hasAccessToDeleteForInitiativeFiles(
      currentUser,
      file,
      this.initiativeProgress.user.id === currentUser.id,
      this.initiativeProgress.collaborators.some(collaborator => collaborator.id === currentUser.id)
    );
  }

  onFileSelect(value): void {
    let event = value.event;
    if (this.initiativeFiles.count >= 5) {
      this.showFileMaxModal = true;
      return;
    }
    const file: File = event?.target?.files[0];
    if (!file) {
      this.snackbarService.showError('general.wrongLargeFileSizeLabel');
      return;
    }

    this.file = file;
    this.fileName = value.title;
    this.fileExtension = file.name.substring(file.name.lastIndexOf('.') + 1);
    this.fileType = this.fileService.isFileImage(this.fileExtension) ? FileTypeEnum.Image : FileTypeEnum.Document;
    this.formData = new FormData();
    this.formData.append('file', file);
    this.fileService
      .validateFileCountAndIsFileExist(this.initiativeProgress.initiativeId, this.fileName + '.' + this.fileExtension)
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

  loadInitiativeFiles() {
    this.loadInitiativeFiles$
      .pipe(
        switchMap(() => {
          const paging = this.coreService.deleteEmptyProps({
            ...this.paging,
            IncludeCount: true,
            FilterBy: null
          });
          return this.fileService.getSavedFilesForInitiative(this.initiativeProgress.initiativeId, paging);
        })
      )
      .subscribe(data => {
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip || 0,
          total: data?.count
        };
        this.initiativeFiles.count = data.count;
        if (this.requestData.take === this.defaultItemPerPage) {
          this.initiativeFiles.dataList = [];
        }
        this.initiativeFiles.dataList.push(...data.dataList);
        if (this.isLoading) this.isLoading = false;
      });
  }

  onLoadMoreData() {
    this.paging = {
      ...this.paging,
      skip: 0,
      take: this.initiativeFiles.dataList.length + 3
    };
    if (this.initiativeFiles.dataList.length !== this.initiativeFiles.count) {
      this.loadMoreInitiativeFiles();
    }
  }

  closePopup(): void {
    this.showFileDuplicateModal = false;
  }

  closeDeletePopup(): void {
    this.showDeleteModal = false;
  }

  closeFileMaxPopup(): void {
    this.showFileMaxModal = false;
  }

  optionClick(id: number): void {
    this.showDeleteModal = true;
    this.clickedFileId = id;
  }

  loadMoreInitiativeFiles() {
    this.requestData.includeCount = true;
    this.requestData.skip = this.paging.skip;
    this.requestData.take = this.defaultItemPerPage;
    this.loadInitiativeFiles$.next();
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
          return this.fileService.saveInitiativeFile(file, this.initiativeProgress.initiativeId).pipe(
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
          this.loadInitiativeFiles$.next();
          this.fileUploadModal = false;
        }
      });
  }

  viewAllFiles() {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativeModuleViewAllClick, {
        id: this.initiativeProgress.initiativeId,
        title: this.initiativeProgress.title,
        resourceType: InitiativeModulesEnum[InitiativeModulesEnum.Files]
      })
      ?.subscribe();

    this.router.navigate([`/decarbonization-initiatives/${this.initiativeProgress.initiativeId}/files`]);
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
            .updateFileModifiedDateAndSize(this.fileBlobName, this.file.size, this.initiativeProgress.initiativeId)
            .subscribe(isFileDateUpdated => {
              if (isFileDateUpdated) {
                this.showFileDuplicateModal = false;
                this.snackbarService.showSuccess(
                  this.translateService.instant('initiative.viewInitiative.fileUploadSuccess')
                );
                this.loadInitiativeFiles$.next();
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

  confirmDelete() {
    this.showDeleteModal = false;
    this.initiativeSharedService
      .deleteSavedContent(this.initiativeProgress.initiativeId, this.clickedFileId, InitiativeModulesEnum.Files)
      .subscribe(
        result => {
          if (result) {
            this.snackbarService.showSuccess(
              this.translateService.instant('initiative.viewInitiative.successContentRemoveMessage')
            );
            this.loadInitiativeFiles$.next();
          } else {
            this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          }
          this.initiativeFiles.dataList.splice(
            this.initiativeFiles.dataList.findIndex(p => p.id === this.clickedFileId),
            1
          );
        },
        () => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      );
  }

  trackFileActivity() {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        id: this.initiativeProgress.initiativeId,
        buttonName: this.translateService.instant('general.uploadLabel'),
        moduleName: InitiativeModulesEnum[InitiativeModulesEnum.Files]
      })
      ?.subscribe();
  }

  checkFileCount() {
    if (this.initiativeFiles.count >= 5) {
      this.showFileMaxModal = true;
      return;
    }
    this.fileUploadModal = true;
  }
}
