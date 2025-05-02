import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { catchError, of, Subject, switchMap, takeUntil, throwError } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { BlobTypeEnum } from 'src/app/shared/enums/api/general-api.enum';
import { FileExtensionEnum } from 'src/app/shared/enums/file-extension.enum';
import { FileTabEnum, FileTypeEnum } from 'src/app/shared/enums/file-type.enum';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { FileBlobInterface } from 'src/app/shared/interfaces/file-blob.interface';
import { CompanyFileInterface, FileInterface } from 'src/app/shared/interfaces/file.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { FileService } from 'src/app/shared/services/file.service';
import { CompanyProfileService } from '../services/company-profile.service';

@Component({
  selector: 'neo-company-files-section',
  templateUrl: './company-files-section.component.html',
  styleUrls: ['./company-files-section.component.scss']
})
export class CompanyFilesSectionComponent implements OnInit {
  @Input() type: string;
  @Input() allFiles: FileInterface[];
  selectedFileTab: FileTabEnum = FileTabEnum['Public'];
  sectionTitle: string;
  roles = RolesEnum;
  fileDescription: string;
  formData = new FormData();
  keepBothFiles = false;
  file: File;
  infoTitle: string;
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
  showAnnouncementModal: boolean = false;
  options: MenuOptionInterface[] = [
    {
      icon: 'pencil',
      name: 'actions.editLabel',
      operation: TableCrudEnum.Edit
    },
    {
      icon: 'trash-can-red',
      name: 'actions.deleteLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];
  files: FileInterface[];

  @Output() fileUploadEmitter: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(
    private authService: AuthService,
    private readonly activatedRoute: ActivatedRoute,
    private fileService: FileService,
    private readonly snackbarService: SnackbarService,
    private readonly translateService: TranslateService,
    private router: Router,
    private companyProfileService: CompanyProfileService
  ) {}

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(() => {
      this.companyId = this.activatedRoute.snapshot.params.id;
      this.authService.currentUser().subscribe(user => {
        if (user) {
          this.currentUser = user;
          this.files = this.allFiles;
          this.listenForFileSelected();
          this.setSectionTitle();
          this.fileDescription = this.translateService.instant('community.permittedFileDescription');
        }
      });
    });
  }

  private setSectionTitle() {
    if (this.type === FileTabEnum[FileTabEnum['Public']].toLowerCase()) {
      this.infoTitle = this.translateService.instant('community.publicInfoTitle');
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
    return currentUser?.roles.some(role => role.id === roleId && role.isSpecial) ?? false;
  }

  optionClick(event: any, file: any): void {
    this.selectedFile = file;
    if (event.operation === TableCrudEnum.Delete) {
      this.showDeleteModal = true;
    } else {
      this.fileUploadModal = true;
    }
  }

  confirmDelete(): void {
    this.showDeleteModal = false;
    this.fileService
      .deleteCompanyFile(this.selectedFile.id, this.type === FileTabEnum[FileTabEnum['Private']].toLowerCase())
      .pipe(
        catchError(() => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(false);
        }),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(result => {
        if (result) {
          this.fileUploadEmitter.emit(true);
          this.snackbarService.showSuccess(this.translateService.instant('general.deleteFileSuccessMessage'));
        } else {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      });
  }

  getFileExtension(extension: number): string {
    return FileExtensionEnum[extension];
  }

  getFileType(type: number): string {
    return FileTypeEnum[type];
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
          `${this.type === FileTabEnum[FileTabEnum['Private']].toLowerCase()}`
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
              this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
              return of(false);
            })
          )
          .subscribe(updated => {
            if (updated) {
              this.showFileDuplicateModal = false;
              this.snackbarService.showSuccess(this.translateService.instant('community.fileTitleUpdateSuccess'));
              this.fileUploadEmitter.emit(true);
              this.fileUploadModal = false;
            } else {
              this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
            }
          });
      }
    }
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
            isPrivate: this.type === FileTabEnum[FileTabEnum['Private']].toLowerCase()
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
          this.fileUploadEmitter.emit(true);
        }
      });
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

  getNoFilesMessage(): string {
    return this.translateService.instant('community.noFilesUploadedMessage').replace('_fileType', this.type);
  }

  checkIfUserCanAccess(): boolean {
    return (
      this.type === FileTabEnum[FileTabEnum['Public']].toLowerCase() ||
      (this.type === FileTabEnum[FileTabEnum['Private']].toLowerCase() &&
        (this.isInRole(this.currentUser, this.roles.Admin) ||
          (this.currentUser.companyId.toString() === this.companyId.toString() &&
            (this.isInRole(this.currentUser, this.roles.SolutionProvider) ||
              this.isInRole(this.currentUser, this.roles.SPAdmin)))))
    );
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
                this.fileUploadEmitter.emit(true);
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

  viewAllClick() {
    this.selectedFileTab =
      this.type === FileTabEnum[FileTabEnum['Public']]?.toLowerCase() ? FileTabEnum['Public'] : FileTabEnum['Private'];
    this.companyProfileService.setSource(this.selectedFileTab);
    this.router.navigate([`/company-profile/${this.companyId}/files`]);
  }
}
