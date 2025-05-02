import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserRoleInterface } from '../../../../shared/interfaces/user/user-role.interface';
import { catchError, map, switchMap } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../../../core/services/http.service';
import { UserManagementApiEnum } from '../../../../user-management/enums/user-management-api.enum';
import { AnnouncementInterface } from '../interfaces/announcement.interface';
import { AnnouncementsService } from '../services/announcements.service';
import { CustomValidator } from '../../../../shared/validators/custom.validator';
import { TitleService } from '../../../../core/services/title.service';
import { SnackbarService } from '../../../../core/services/snackbar.service';
import { BlobTypeEnum, GeneralApiEnum } from '../../../../shared/enums/api/general-api.enum';
import { ImageUploadService } from '../../../../shared/services/image-upload.service';
import { Subject, throwError } from 'rxjs';
import { MAX_IMAGE_SIZE } from 'src/app/shared/constants/image-size.const';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';

@Component({
  selector: 'neo-edit-announcement',
  templateUrl: './edit-announcement.component.html',
  styleUrls: ['./edit-announcement.component.scss']
})
export class EditAnnouncementComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  apiRoutes = { ...UserManagementApiEnum, ...GeneralApiEnum };
  rolesData: UserRoleInterface[];
  announcementPayload: AnnouncementInterface;
  defaultImageSrc: string = 'assets/images/announcement-default.png';
  imageSrc: string = 'assets/images/announcement-default.png';
  blobType = BlobTypeEnum;
  formData: FormData;
  openedAnnouncement: AnnouncementInterface;
  announcementId: string;
  showConfirmAnnouncement: boolean;
  form: FormGroup = this.formBuilder.group({
    name: ['', Validators.required],
    buttonText: ['', Validators.required],
    buttonUrl: ['', [Validators.required, CustomValidator.url, Validators.maxLength(2048)]],
    audienceId: ['', Validators.required]
  });
  maxImageSize: number = MAX_IMAGE_SIZE;
  formSubmitted: boolean;

  private loadAnnouncementData$: Subject<void> = new Subject<void>();

  constructor(
    private activatedRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    private httpService: HttpService,
    private announcementService: AnnouncementsService,
    private router: Router,
    private titleService: TitleService,
    private snackbarService: SnackbarService,
    private imageUploadService: ImageUploadService
  ) {}

  ngOnInit(): void {
    this.announcementId = this.activatedRoute.snapshot.params.id;
    this.titleService.setTitle(
      this.announcementId ? 'announcement.editAnnouncementLabel' : 'announcement.addAnnouncementLabel'
    );

    if (this.announcementId) {
      // * edit announcement part
      this.listenToLoadAnnouncementData();
      this.loadAnnouncementData$.next();
    }

    this.httpService
      .get<UserRoleInterface[]>(this.apiRoutes.UserRoles)
      .pipe(
        map(roles => {
          return roles.filter(r => r.name !== 'Admin' && r.id !== RolesEnum.SPAdmin);
        })
      )
      .subscribe(r => (this.rolesData = r));
  }

  createAnnouncementWithImage(): void {
    this.formSubmitted = true;
    if (this.form.invalid) return;

    this.createAnnouncementPayload();

    if (this.imageSrc !== this.defaultImageSrc && this.imageSrc !== this.openedAnnouncement?.backgroundImage?.uri) {
      this.imageUploadService.uploadImage(this.blobType.Announcement, this.formData).subscribe(r => {
        this.announcementPayload.backgroundImageName = r.name;
        this.createAnnouncement();
      });
    } else {
      this.createAnnouncement();
    }
  }

  createAnnouncementPayload(): void {
    this.announcementPayload = {
      ...this.form.getRawValue(),
      isActive: this.openedAnnouncement?.isActive ?? true,
      backgroundImageName: null,
      oldBackgroundImageName: null
    };
  }

  createAnnouncement(): void {
    if (this.announcementId) {
      if (this.imageSrc === this.openedAnnouncement.backgroundImage?.uri) {
        this.announcementPayload.backgroundImageName = this.openedAnnouncement.backgroundImageName;
        this.announcementPayload.oldBackgroundImageName = this.openedAnnouncement.oldBackgroundImageName;
      }
      this.announcementService
        .editAnnouncement(this.announcementPayload, this.announcementId)
        .pipe(
          catchError(err => {
            if (err.status === 409) {
              this.showConfirmAnnouncement = true;
            }
            return throwError(null);
          })
        )
        .subscribe(() => {
          this.goBack();
          this.snackbarService.showSuccess('announcement.announcementUpdatedLabel');
        });
    } else {
      this.announcementService
        .createAnnouncement(this.announcementPayload)
        .pipe(
          catchError(err => {
            if (err.status === 409) {
              this.showConfirmAnnouncement = true;
            }
            return throwError(null);
          })
        )
        .subscribe(() => {
          this.goBack();
          this.snackbarService.showSuccess('announcement.announcementCreatedLabel');
        });
    }
  }

  onFileSelect(event): void {
    const isLarge = event.target.files[0].size > this.maxImageSize;
    if (isLarge) {
      this.snackbarService.showError('general.largeFileLabel');
      return;
    }

    const notSupported: boolean = event.target.files[0].type.includes('image');
    if (!notSupported) {
      this.snackbarService.showError('general.wrongFileLabel');
      return;
    }

    this.imageUploadService.renderPreview(event).then(r => {
      this.imageSrc = r;
    });

    if (event.target.files.length > 0) {
      const file: File = event.target.files[0];
      this.formData = new FormData();
      this.formData.append('file', file);
    }
  }

  updateAnnouncementStatus($event, id: number, param?: boolean) {
    this.announcementService
      .updateAnnouncement(id, $event, param)
      .pipe(
        catchError(err => {
          if (err.status === 409) {
            this.showConfirmAnnouncement = true;
          }
          return throwError(null);
        })
      )
      .subscribe(() => {
        this.showConfirmAnnouncement = false;
        this.loadAnnouncementData$.next();
      });
  }

  goBack(): void {
    this.router.navigate(['admin/announcements']).then();
  }

  removeImage() {
    this.imageSrc = this.defaultImageSrc;

    this.createAnnouncementPayload();

    this.announcementPayload.backgroundImageName = null;
    this.fileInput.nativeElement.value = null;
  }

  forceAdd(): void {
    this.announcementService.createAnnouncement(this.announcementPayload, true).subscribe(() => {
      this.goBack();
      this.snackbarService.showSuccess('announcement.announcementCreatedLabel');
    });
  }

  forceEdit(): void {
    this.announcementService
      .editAnnouncement({ ...this.announcementPayload, isActive: true }, this.announcementId, true)
      .subscribe(() => {
        this.goBack();
        this.snackbarService.showSuccess('announcement.announcementCreatedLabel');
      });
  }

  confirmChanges() {
    if (!this.openedAnnouncement?.id) {
      this.forceAdd();
    } else {
      this.forceEdit();
    }
  }

  private listenToLoadAnnouncementData(): void {
    this.loadAnnouncementData$
      .pipe(switchMap(() => this.announcementService.getAnnouncement(this.announcementId)))
      .subscribe(an => {
        this.openedAnnouncement = an;

        this.form.patchValue({
          name: an.name,
          buttonText: an.buttonText,
          buttonUrl: an.buttonUrl,
          audienceId: an.audienceId
        });

        this.announcementPayload = { ...this.form.getRawValue() };

        this.imageSrc = an.backgroundImage?.uri ?? this.defaultImageSrc;
      });
  }
}
