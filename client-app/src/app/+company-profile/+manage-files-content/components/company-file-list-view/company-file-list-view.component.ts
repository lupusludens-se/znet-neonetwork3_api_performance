import { ChangeDetectorRef, Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { catchError, filter, of, Subject, switchMap, takeUntil, throwError } from 'rxjs';
import { CommonService } from 'src/app/core/services/common.service';
import { FileService } from 'src/app/shared/services/file.service';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { FileBlobInterface } from 'src/app/shared/interfaces/file-blob.interface';
import { BlobTypeEnum } from 'src/app/shared/enums/api/general-api.enum';
import { CompanyFileInterface, FileInterface } from 'src/app/shared/interfaces/file.interface';
import { FileTypeEnum, FileTabEnum } from 'src/app/shared/enums/file-type.enum';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { CoreService } from 'src/app/core/services/core.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { HttpService } from 'src/app/core/services/http.service';
import { CompanyApiEnum } from 'src/app/shared/enums/api/company-api-enum';
import { CompanyProfileService } from 'src/app/+company-profile/services/company-profile.service';
import { FileExtensionEnum } from 'src/app/shared/enums/file-extension.enum';
import { TitleService } from 'src/app/core/services/title.service';

@Component({
  selector: 'neo-company-file-list-view',
  templateUrl: './company-file-list-view.component.html',
  styleUrls: ['./company-file-list-view.component.scss']
})
export class CompanyFileListViewComponent implements OnInit, OnDestroy {
  @Input() type: string = FileTabEnum[FileTabEnum['Public']];
  @Output() fileUploadEmitter = new EventEmitter<boolean>();
  @Output() pageChangeDetected = new EventEmitter<boolean>();

  roles = RolesEnum;
  fileDescription: string;
  formData = new FormData();
  keepBothFiles = false;
  file: File;
  private fileSelected$ = new Subject<FormData>();
  private unsubscribe$ = new Subject<void>();
  fileUploadModal = false;
  selectedFile: FileInterface;
  showFileDuplicateModal = false;
  showDeleteModal = false;
  companyId: number;
  currentUser: UserInterface;
  fileType: FileTypeEnum;
  fileBlobName: string;
  oldExistingFileName: string;
  fileName: string;
  fileVersion: number;
  fileExtension: string;
  files: FileInterface[];
  initialLoad = true;
  clickedFileId: number;
  defaultItemPerPage = 10;
  currentPageNumber = 1;
  companyFiles: PaginateResponseInterface<CompanyFileInterface> = {
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
    size: 'size',
    modifiedBy: 'modifiedBy'
  };
  tdTitleClick: string;
  isLoading: boolean;
  showFileMaxModal = false;
  isAdmin = false;
  pageDataLoad$ = new Subject<void>();
  sectionTitle: string;
  infoTitle: any;
  sectionDescription: string;
  selectedFileTab: FileTabEnum;
  constructor(
    private commonService: CommonService,
    private readonly httpService: HttpService,
    private fileService: FileService,
    private snackbarService: SnackbarService,
    private translateService: TranslateService,
    private coreService: CoreService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private titleService: TitleService,
    private cdr: ChangeDetectorRef,
    private companyProfileService: CompanyProfileService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.companyId = params['id'];
      this.titleService.setTitle('community.fileManagement.viewAllFilesLabel');
    });

    this.companyProfileService.getTab().subscribe(tab => {
      this.selectedFileTab = tab;
      this.type =
        tab === FileTabEnum['Public'] ? FileTabEnum[FileTabEnum['Public']] : FileTabEnum[FileTabEnum['Private']];
      if (this.currentUser) {
        this.setSectionTitle();
      }
    });

    this.listenForFileSelected();
    this.loadCompanyFiles();
    this.listenForCurrentUser();
    this.pageDataLoad$.next();
    this.fileDescription = this.translateService.instant('community.permittedFileDescription');
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this.fileService.clearAll();
    this.companyProfileService.setSource(FileTabEnum['Public']);
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

  onFileSelect(value: any): void {
    if (!value.isEditMode) {
      const file: File = value.event?.target?.files[0];
      if (!file) {
        this.snackbarService.showError('general.wrongLargeFileSizeLabel');
        return;
      }

      this.file = file;
      this.fileName = value.title;
      this.fileExtension = file.name.split('.').pop();
      this.fileType = this.fileService.isFileImage(this.fileExtension) ? FileTypeEnum.Image : FileTypeEnum.Document;
      this.formData = new FormData();
      this.formData.append('file', file);
      this.fileService
        .validateFileCountAndIsFileExistByCompanyId(
          this.companyId,
          `${this.fileName + '.' + this.fileExtension}`,
          `${this.selectedFileTab === FileTabEnum['Public'] ? false : true}`
        )
        .pipe(
          catchError(() => {
            this.snackbarService.showError('general.defaultErrorLabel');
            return throwError(() => new Error('File validation error'));
          })
        )
        .subscribe(file => {
          if (!file.isExist) {
            this.fileSelected$.next(this.formData);
          } else {
            this.oldExistingFileName = file.actualFileName;
            this.fileBlobName = file.blobName;
            this.fileUploadModal = false;
            this.showFileDuplicateModal = true;
            this.fileVersion = file.fileVersion;
          }
        });
    } else {
      if (value.title == this.selectedFile.actualFileTitle) {
        this.fileUploadModal = false;
        this.snackbarService.showInfo(this.translateService.instant('general.noChangesAvailable'));
      } else {
        this.fileService
          .updateFileTitleOfSelectedFileByCompany(this.selectedFile.id, value.title)
          .pipe(
            catchError(() => {
              this.snackbarService.showError(this.translateService.instant('community.defaultErrorLabel'));
              return of(false);
            })
          )
          .subscribe(updated => {
            if (updated) {
              this.showFileDuplicateModal = false;
              this.snackbarService.showSuccess(this.translateService.instant('community.fileTitleUpdateSuccess'));
              this.pageDataLoad$.next();
              this.fileUploadModal = false;
            } else {
              this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
            }
          });
      }
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

  loadCompanyFiles() {
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
          const apiEndpoint =
            this.selectedFileTab === FileTabEnum['Public']
              ? CompanyApiEnum.GetCompaniesPublicFiles
              : CompanyApiEnum.GetCompaniesPrivateFiles;
          return this.httpService.get<PaginateResponseInterface<CompanyFileInterface>>(
            `${apiEndpoint}/${this.companyId}`,
            paging
          );
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
        this.companyFiles = {
          ...this.companyFiles,
          count: data.count,
          dataList: [...data.dataList]
        };
        this.isLoading = true;
      });
  }

  private listenForFileSelected(): void {
    this.fileSelected$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(formData => {
          const fileBlob: FileBlobInterface = {
            overwrite: false,
            newFileName: this.fileName,
            blobType: BlobTypeEnum.Companies,
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
          const file: CompanyFileInterface = {
            extension: this.fileExtension,
            type: this.fileType,
            link: data.uri,
            blobName: data.name,
            actualFileName: this.file.name,
            actualFileTitle: this.fileName,
            size: this.file.size,
            version: this.keepBothFiles ? ++this.fileVersion : 0,
            isPrivate: this.type === FileTabEnum[FileTabEnum['Private']]
          };
          return this.fileService.saveCompanyFile(file, this.companyId).pipe(
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
          this.snackbarService.showSuccess(this.translateService.instant('community.fileUploadSuccess'));
          this.keepBothFiles = false;
          this.fileUploadModal = false;
          this.selectedFile = null;
          this.pageDataLoad$.next();
        }
      });
  }

  isUserIsAdminOrSpAdminUserFromSameCompany(): boolean {
    return this.isInRole(this.currentUser, RolesEnum.Admin) || (this.currentUser?.companyId.toString() === this.companyId.toString() && this.isInRole(this.currentUser, RolesEnum.SPAdmin));
  }

  confirmDelete() {
    this.showDeleteModal = false;
    this.fileService
      .deleteCompanyFile(this.clickedFileId, this.type === FileTabEnum[FileTabEnum['Private']])
      .pipe(
        catchError(() => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(false);
        }),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(result => {
        if (result) {
          this.pageDataLoad$.next();
          if (
            this.companyFiles.count > this.defaultItemPerPage &&
            this.companyFiles.count % this.defaultItemPerPage === 1
          ) {
            this.currentPageNumber -= 1;
            this.changePage(this.currentPageNumber);
          } else {
            this.pageDataLoad$.next();
          }
          this.snackbarService.showSuccess(this.translateService.instant('general.deleteFileSuccessMessage'));
        } else {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      });
  }

  optionClick(id: number): void {
    this.showDeleteModal = true;
    this.clickedFileId = id;
  }

  deleteFile(id: number): void {
    this.optionClick(id);
  }

  editFile(selectedFileId: number) {
    this.fileUploadModal = true;
    this.selectedFile = this.companyFiles.dataList.find(file => file.id === selectedFileId);
  }

  downloadFile(fileBlobName: string, fileName: string, fileExtension: string): void {
    this.fileService
      .downloadFileByCompanyId(fileBlobName)
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
      blobType: BlobTypeEnum.Companies,
      blobName: this.fileBlobName
    };
    this.fileService.uploadFile(formData, fileBlob).subscribe(
      uploadedFile => {
        if (uploadedFile.name) {
          this.fileService
            .updateFileModifiedDateAndSizeOfAFileByCompany(this.fileBlobName, this.file.size)
            .subscribe(isFileDateUpdated => {
              if (isFileDateUpdated) {
                this.showFileDuplicateModal = false;
                this.snackbarService.showSuccess(this.translateService.instant('community.fileUploadSuccess'));
                this.pageDataLoad$.next();
                this.fileUploadModal = false;
              }
            });
        }
      },
      () => {
        this.snackbarService.showError(this.translateService.instant('community.fileUploadError'));
      }
    );
  }

  keepBothTheFiles() {
    this.keepBothFiles = true;
    this.fileSelected$.next(this.formData);
  }

  private listenForCurrentUser(): void {
    this.authService
      .currentUser()
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(user => !!user)
      )
      .subscribe(currentUser => {
        this.currentUser = currentUser;
        this.setSectionTitle();
      });
  }

  private setSectionTitle() {
    if (this.type === FileTabEnum[FileTabEnum['Public']]) {
      this.infoTitle = this.translateService.instant('community.publicInfoTitle');
      this.sectionDescription = this.translateService.instant('community.publicInfoDescription');
      if (
        this.isInRole(this.currentUser, this.roles.Admin) ||
        ((this.isInRole(this.currentUser, this.roles.SPAdmin) ||
          this.isInRole(this.currentUser, this.roles.SolutionProvider)) &&
          this.currentUser.companyId.toString() === this.companyId.toString())
      ) {
        this.sectionTitle = this.type + ' ' + this.translateService.instant('general.filesLabel');
      } else {
        this.sectionTitle = this.translateService.instant('general.documentsLabel');
      }
    } else {
      this.sectionDescription = this.translateService.instant('community.privateInfoDescription');
      this.infoTitle = this.translateService.instant('community.privateInfoTitle');
      if (
        this.isInRole(this.currentUser, this.roles.Admin) ||
        ((this.isInRole(this.currentUser, this.roles.SPAdmin) ||
          this.isInRole(this.currentUser, this.roles.SolutionProvider)) &&
          this.currentUser.companyId.toString() === this.companyId.toString())
      ) {
        this.sectionTitle = this.type + ' ' + this.translateService.instant('general.filesLabel');
      }
    }
  }

  isInRole(currentUser: UserInterface, roleId: RolesEnum): boolean {
    return currentUser.roles.some(role => role.id === roleId && role.isSpecial) ?? false;
  }

  changeTab(tabName: FileTabEnum): void {
    this.companyProfileService.setSource(tabName);
    this.pageDataLoad$.next();
  }

  checkIfUserCanAccess(sectionType: string): boolean {
    return (
      sectionType === FileTabEnum[FileTabEnum['Public']]?.toLowerCase() ||
      (sectionType === FileTabEnum[FileTabEnum['Private']]?.toLowerCase() &&
        (this.isInRole(this.currentUser, this.roles.Admin) ||
          (this.currentUser.companyId.toString() === this.companyId.toString() &&
            (this.isInRole(this.currentUser, this.roles.SolutionProvider) ||
              this.isInRole(this.currentUser, this.roles.SPAdmin)))))
    );
  }

  isCurrentUserAdminOrSPAdmin(): boolean {
    return (
      this.isInRole(this.currentUser, this.roles.Admin) ||
      (this.currentUser?.companyId.toString() === this.companyId.toString() &&
        this.isInRole(this.currentUser, this.roles.SPAdmin))
    );
  }

  getNoFilesMessage(): string {
    return this.translateService.instant('community.noFilesUploadedMessage').replace('_fileType', this.type);
  }
}
