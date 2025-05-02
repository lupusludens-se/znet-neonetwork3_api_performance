import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, of, Subject, Subscription, switchMap, take, throwError } from 'rxjs';
import { catchError, filter } from 'rxjs/operators';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { TitleService } from '../../../core/services/title.service';
import { HttpService } from '../../../core/services/http.service';
import { CoreService } from '../../../core/services/core.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { AuthService } from '../../../core/services/auth.service';
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
import { HttpErrorResponse } from '@angular/common/http';
import { FirstMessageInterface } from '../../interfaces/first-message.interface';
import { ForumDataService } from '../../services/forum-data.service';
import { DESCRIPTION_MAX_LENGTH, DESCRIPTION_TEXT_MAX_LENGTH } from 'src/app/+admin/constants/forum.constants';
import { TranslateService } from '@ngx-translate/core';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';

@UntilDestroy()
@Component({
  selector: 'neo-edit-discussion',
  templateUrl: './edit-discussion.component.html',
  styleUrls: ['./edit-discussion.component.scss']
})
export class EditDiscussionComponent implements OnInit, OnDestroy {
  attachments: ImageInterface[] = [];
  formSubmitted: boolean;
  permissionTypes = PermissionTypeEnum;
  createDiscussion: boolean;
  locationSpecific: boolean;
  isLoading: boolean;
  descriptionErrorMessage: string;
  userRoles = RolesEnum;

  formGroup: FormGroup;

  regions$: Observable<TagParentInterface[]> = this.httpService.get<TagParentInterface[]>(CommonApiEnum.Regions, {
    filterBy: 'parentids=0'
  });

  categories$: Observable<TagInterface[]> = this.httpService.get<TagInterface[]>(CommonApiEnum.Categories);

  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  fileSelected$: Subject<FormData> = new Subject<FormData>();

  DESCRIPTION_MAX_LENGTH = DESCRIPTION_MAX_LENGTH;
  DESCRIPTION_TEXT_MAX_LENGTH = DESCRIPTION_TEXT_MAX_LENGTH;
  descriptionCount: number;

  topic: ForumTopicInterface;
  responses: FirstMessageInterface[];
  subscription: Subscription;
  selectedUsers: UserInterface[] = [];
  forumUserIds: number[];
  isUserAdmin: boolean;

  constructor(
    private readonly router: Router,
    private readonly titleService: TitleService,
    private readonly httpService: HttpService,
    private readonly coreService: CoreService,
    private readonly snackbarService: SnackbarService,
    private readonly authService: AuthService,
    private readonly footerService: FooterService,
    public readonly permissionService: PermissionService,
    private readonly activatedRoute: ActivatedRoute,
    private forumDataService: ForumDataService,
    private readonly translateService: TranslateService
  ) {}

  ngOnInit(): void {
    this.formGroup = new FormGroup({
      title: new FormControl('', CustomValidator.required),
      description: new FormControl('', [CustomValidator.required, Validators.maxLength(DESCRIPTION_MAX_LENGTH)]),
      topics: this.getValueasFormArray([]),
      regions: this.getValueasFormArray([]),
      isPinned: new FormControl([])
    });
    this.titleService.setTitle('forum.editDiscussionLabel');
    this.listedForFileSelected();
    this.subscription = this.authService.currentUser().subscribe(val => {
      if (val !== null) {
        this.isUserAdmin = val.roles.some(r => r.id === this.userRoles.Admin);
        this.fetchDiscussionDetails();
      }
    });
  }

  ngOnDestroy(): void {
    this.fileSelected$.next(null);
    this.fileSelected$.complete();
    this.footerService.setFooterDisabled(false);
    this.subscription.unsubscribe();
  }

  navigateToDiscussion(discussion: Record<string, string | number>): void {
    this.router.navigate([`forum/topic/${discussion?.id}`]);
  }

  fetchDiscussionDetails(): void {
    let expand = 'discussionusers.users,discussionusers.users.image,categories,regions,saved';
    this.forumDataService
      .getDiscussionById(this.activatedRoute.snapshot.params.id, expand)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            this.router.navigate(['/forum']);
            this.coreService.elementNotFoundData$.next({
              iconKey: 'forum',
              mainTextTranslate: 'forum.notFoundText',
              buttonTextTranslate: 'forum.notFoundButton',
              buttonLink: '/forum'
            });
          }

          return throwError(error);
        })
      )
      .subscribe((topic: ForumTopicInterface) => {
        this.descriptionCount = this.coreService.convertToPlain(topic.firstMessage.text ?? '')?.length ?? 0;
        this.topic = topic;
        if (this.topic.firstMessage.attachments !== null) {
          this.topic.firstMessage.attachments.forEach(attachment => {
            let image: ImageInterface = {
              blobType: attachment.type,
              uri: attachment.link,
              name: attachment.imageName
            };
            this.attachments.push(image);
          });
        }
        const allPermissions = JSON.parse(localStorage.getItem(this.authService.currentUserKey));
        if (allPermissions) {
          if (
            this.topic.firstMessage.user.id !== this.authService.currentUser$.getValue().id &&
            !this.permissionService.hasPermission(allPermissions, PermissionTypeEnum.ForumManagement)
          ) {
            this.router.navigate([`/forum/topic/${this.topic?.id}`]);
          }
        }
        this.formGroup = new FormGroup({
          title: new FormControl(this.topic.subject, CustomValidator.required),
          description: new FormControl(this.topic.firstMessage.text, [
            CustomValidator.required,
            Validators.maxLength(DESCRIPTION_MAX_LENGTH)
          ]),
          topics: this.getValueasFormArray(this.topic.categories),
          regions: this.getValueasFormArray(this.topic.regions),
          isPinned: new FormControl(this.topic.isPinned)
        });

        if (this.topic.responsesCount > 0) {
          this.loadResponsesData(this.topic.firstMessage.id);
        }

        this.locationSpecific = this.topic?.regions?.length > 0;
        this.forumUserIds = this.topic.users.map(u => u.id);
        this.selectedUsers = this.topic.users.map(
          user =>
            ({
              id: user.id
            } as UserInterface)
        );
        this.titleService.setTitle(topic.subject);
      });
  }

  private getValueasFormArray(list): FormArray {
    let formArray = new FormArray([]);
    if (list.length > 0) {
      for (let item of list) {
        formArray.push(new FormControl(item.id));
      }
    }
    return formArray;
  }

  private loadResponsesData(parentMessageId: number): void {
    if (parentMessageId) {
      this.httpService
        .get<PaginateResponseInterface<FirstMessageInterface>>(
          `${ForumApiEnum.Forum}/${this.activatedRoute.snapshot.params.id}/${ForumApiEnum.Messages}`,
          {
            expand: 'user,user.company,user.image,user.follower,attachments,attachments.image,messagelikes,replies',
            parentMessageId
          }
        )
        .pipe(take(1))
        .subscribe(responses => {
          this.responses = responses.dataList;
        });
    }
  }

  updateDiscussion(): void {
    this.formSubmitted = true;
    if (this.descriptionCount > this.DESCRIPTION_TEXT_MAX_LENGTH) {
      this.snackbarService.showError(
        this.translateService.instant('forum.discussionTextMaxLengthError') +
          this.DESCRIPTION_TEXT_MAX_LENGTH +
          this.translateService.instant('general.characters')
      );
      return;
    }
    if (this.formGroup.controls['description'].value.length > this.DESCRIPTION_MAX_LENGTH) {
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
      ...this.topic,
      subject: formData.title,
      firstMessage: {
        ...this.topic.firstMessage,
        text: formData.description,
        textContent: this.coreService.convertToPlain(formData.description),
        attachments: this.attachments.map(attachment => ({
          text: attachment.name,
          link: attachment.uri,
          type: AttachmentTypeEnum.Media,
          imageName: attachment.name,
          messageId: this.topic.firstMessage.id
        }))
      },
      regions: formData.regions.map(regionId => ({ id: regionId })),
      categories: formData.topics.map(topicId => ({ id: topicId })),
      users: this.selectedUsers.map(user => ({ id: user.id }))
    };

    this.forumDataService.updateDiscussion(discussion).subscribe(
      response => {
        if (response !== null && response.id == undefined) {
          this.snackbarService.showError('forum.updateDiscussionFailureLabel');
          return;
        }

        this.snackbarService.showSuccess('forum.updateDiscussionSuccessLabel');
        this.router.navigate([`/forum/topic/${response.id}`]);
      },
      error => {
        this.snackbarService.showError('forum.updateDiscussionFailureLabel');
        return throwError(error);
      }
    );
  }

  cancelClick(): void {
    this.router.navigate([`/forum/topic/${this.topic?.id}`]).then();
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

  selectedUsersUpdated(users: UserInterface[]): void {
    this.selectedUsers = users;
  }
}
