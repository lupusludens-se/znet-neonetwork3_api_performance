import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { Observable, of, Subject, switchMap, take, throwError } from 'rxjs';
import { catchError, filter } from 'rxjs/operators';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { TitleService } from '../../../core/services/title.service';
import { HttpService } from '../../../core/services/http.service';
import { CoreService } from '../../../core/services/core.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { AuthService } from '../../../core/services/auth.service';

import { ForumSearchResultInterface } from '../../interfaces/forum-search-result.interface';
import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import { ForumTopicInterface } from '../../interfaces/forum-topic.interface';
import { TagParentInterface } from '../../../core/interfaces/tag-parent.interface';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { ImageInterface } from '../../../shared/interfaces/image.interface';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';

import { BlobTypeEnum, GeneralApiEnum } from '../../../shared/enums/api/general-api.enum';
import { CommonApiEnum } from '../../../core/enums/common-api.enum';
import { ForumApiEnum } from '../../enums/forum-api.enum';
import { AttachmentTypeEnum } from '../../../+messages/enums/attachment-type.enum';
import { FormControlStatusEnum } from '../../../shared/enums/form-control-status.enum';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { PermissionService } from 'src/app/core/services/permission.service';
import { CustomValidator } from '../../../shared/validators/custom.validator';
import { FooterService } from 'src/app/core/services/footer.service';
import { TranslateService } from '@ngx-translate/core';
import { DESCRIPTION_MAX_LENGTH, DESCRIPTION_TEXT_MAX_LENGTH } from 'src/app/+admin/constants/forum.constants';
import { ForumUserResponseInterface } from '../../interfaces/forum-user-response.interface';
import { CommonService } from 'src/app/core/services/common.service';

@UntilDestroy()
@Component({
  selector: 'neo-start-a-discussion',
  templateUrl: './start-a-discussion.component.html',
  styleUrls: ['./start-a-discussion.component.scss']
})
export class StartADiscussionComponent implements OnInit, OnDestroy {
  searchResults: ForumSearchResultInterface[];
  attachments: ImageInterface[] = [];
  formSubmitted: boolean;
  permissionTypes = PermissionTypeEnum;
  descriptionErrorMessage: string;
  searchStr: string;
  createDiscussion: boolean;
  locationSpecific: boolean;
  isLoading: boolean;
  selectedUsers: UserInterface[] = [];

  formGroup: FormGroup = new FormGroup({
    title: new FormControl(null, CustomValidator.required),
    description: new FormControl(null, [CustomValidator.required, Validators.maxLength(DESCRIPTION_MAX_LENGTH)]),
    topics: new FormArray([]),
    regions: new FormArray([]),
    isPinned: new FormControl(null)
  });

  regions$: Observable<TagParentInterface[]> = this.httpService.get<TagParentInterface[]>(CommonApiEnum.Regions, {
    filterBy: 'parentids=0'
  });

  categories$: Observable<TagInterface[]> = this.httpService.get<TagInterface[]>(CommonApiEnum.Categories);

  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  fileSelected$: Subject<FormData> = new Subject<FormData>();

  DESCRIPTION_MAX_LENGTH = DESCRIPTION_MAX_LENGTH;
  DESCRIPTION_TEXT_MAX_LENGTH = DESCRIPTION_TEXT_MAX_LENGTH;
  descriptionCount: number;

  constructor(
    private readonly router: Router,
    private readonly titleService: TitleService,
    private readonly httpService: HttpService,
    private readonly coreService: CoreService,
    private readonly snackbarService: SnackbarService,
    private readonly authService: AuthService,
    private readonly footerService: FooterService,
    public readonly permissionService: PermissionService,
    private commonService: CommonService,
    private readonly translateService: TranslateService
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle('forum.startDiscussionLabel');
    this.listedForFileSelected();
  }

  ngOnDestroy(): void {
    this.fileSelected$.next(null);
    this.fileSelected$.complete();
    this.footerService.setFooterDisabled(false);
  }

  searchPaste(event: any): void {
    let clipboardData = event.clipboardData;
    let pastedText = clipboardData.getData('text');
    this.searchStr = pastedText;
    this.fetchData();
  }

  search(value: string): void {
    this.searchStr = value;
    this.fetchData();
  }

  navigateToNewDiscussion(): void {
    this.createDiscussion = true;
    this.formGroup.get('title').setValue(this.searchStr);
    this.footerService.setFooterDisabled(true);
  }

  navigateToDiscussion(discussion: Record<string, string | number>): void {
    this.router.navigate([`forum/topic/${discussion?.id}`]);
  }

  saveData(): void {
    this.formSubmitted = true;
    if (this.descriptionCount > this.DESCRIPTION_TEXT_MAX_LENGTH) {
      this.snackbarService.showError(
        this.translateService.instant('forum.discussionTextMaxLengthError') +
          this.DESCRIPTION_TEXT_MAX_LENGTH +
          this.translateService.instant('general.characters')
      );
      return;
    }
    if (this.formGroup.controls['description'].value?.length > this.DESCRIPTION_MAX_LENGTH) {
      this.snackbarService.showError(this.translateService.instant('forum.discussionFormattingMaxLengthError'));
      return;
    }
    if (this.formGroup.status === FormControlStatusEnum.Invalid) {
      Object.keys(this.formGroup.value).map(key => {
        this.formGroup.controls[key].markAsTouched();
        this.formGroup.controls[key].markAsDirty();
      });

      return;
    }

    const formData = this.formGroup.value;
    const discussion = {
      subject: formData.title,
      firstMessage: {
        text: formData.description,
        textContent: this.coreService.convertToPlain(formData.description),
        attachments: this.attachments.map(attachment => ({
          text: attachment.name,
          link: attachment.uri,
          type: AttachmentTypeEnum.Media,
          imageName: attachment.name
        }))
      },
      regions: formData.regions.map(regionId => ({ id: regionId })),
      categories: formData.topics.map(topicId => ({ id: topicId })),
      isPinned: !!formData.isPinned,
      isPrivate: !!this.selectedUsers.length,
      users: this.selectedUsers.map(
        user =>
          ({
            id: user.id
          } as ForumUserResponseInterface)
      )
    };

    this.httpService
      .post<ForumTopicInterface>(ForumApiEnum.Forum, discussion)
      .pipe(take(1))
      .subscribe(
        response => {
          this.router.navigate([`/forum/topic/${response.id}`]);
        },
        error => {
          this.snackbarService.showError('general.defaultErrorLabel');
          return throwError(error);
        }
      );
  }

  cancelClick(): void {
    this.router.navigate(['/forum']);
  }

  backClick(): void {
    this.createDiscussion = false;
    this.footerService.setFooterDisabled(false);
  }

  hasDescriptionError(controlName: string): boolean {
    const control = this.formGroup?.get(controlName);
    if (this.descriptionCount > this.DESCRIPTION_TEXT_MAX_LENGTH) {
      this.descriptionErrorMessage =
        this.translateService.instant('forum.discussionTextMaxLengthError') +
        this.DESCRIPTION_TEXT_MAX_LENGTH +
        this.translateService.instant('general.characters');
      return true;
    }

    this.descriptionErrorMessage =
      this.translateService.instant('form.discussionLabel') + this.translateService.instant('general.requiredLabel');

    return control?.invalid && control?.touched && control?.dirty;
  }

  hasError(controlName: string): boolean {
    const control = this.formGroup?.get(controlName);

    return control?.invalid && control?.touched && control?.dirty;
  }

  addItem(controlName: string, value: number) {
    const formArray = this.formGroup.get(controlName) as FormArray;

    if (formArray.value.includes(value)) {
      const index = formArray.value.findIndex(item => item === value);
      formArray.removeAt(index);
    } else {
      formArray.push(new FormControl(value));
    }
  }

  listedForFileSelected() {
    this.fileSelected$
      .pipe(
        untilDestroyed(this),
        filter(formData => formData != null),
        switchMap(formData => {
          this.isLoading = true;

          return this.httpService
            .post<ImageInterface>(
              GeneralApiEnum.Media,
              formData,
              {
                BlobType: BlobTypeEnum.Forums
              },
              false
            )
            .pipe(
              catchError(() => {
                this.snackbarService.showError('general.defaultImageUploadErrorLabel');
                return of(null); // Return a null observable to continue the stream
              })
            );
        })
      )
      .subscribe(image => {
        this.isLoading = false;
        if (image) {
          this.attachments.push(image);
        }
      });
  }

  resetRegions(): void {
    const regions = this.formGroup.get('regions') as FormArray;

    this.locationSpecific = false;

    while (regions.length !== 0) {
      regions.removeAt(0);
    }
  }

  goBack() {
    this.commonService.goBack();
  }

  private fetchData(): void {
    this.httpService
      .get<PaginateResponseInterface<ForumTopicInterface>>(
        ForumApiEnum.Forum,
        this.coreService.deleteEmptyProps({ search: this.searchStr })
      )
      .pipe(take(1))
      .subscribe(
        response => {
          this.searchResults = response.dataList.map(data => ({
            id: data?.id,
            name: data?.subject,
            responsesCount: data.responsesCount
          }));
        },
        error => {
          this.snackbarService.showError('general.defaultErrorLabel');
          return throwError(error);
        }
      );
  }

  selectedUsersUpdated(users: UserInterface[]): void {
    this.selectedUsers = users;
  }
}
