import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { filter, Observable, of, Subject, switchMap, takeUntil } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { AuthService } from '../../../core/services/auth.service';
import { HttpService } from '../../../core/services/http.service';
import { TitleService } from '../../../core/services/title.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { PermissionService } from 'src/app/core/services/permission.service';

import { MultiselectOptionInterface } from '../../../shared/modules/controls/multiselect/interfaces/multiselect-option.interface';
import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { ProjectInterface } from '../../../shared/interfaces/projects/project.interface';
import { SearchResultInterface } from '../../../shared/interfaces/search-result.interface';
import { ImageInterface } from '../../../shared/interfaces/image.interface';
import { UserRoleInterface } from '../../../shared/interfaces/user/user-role.interface';
import { AttachmentInterface } from '../../../shared/interfaces/attachment.interface';

import { UserManagementApiEnum } from '../../../user-management/enums/user-management-api.enum';
import { ProjectApiRoutes } from '../../../projects/shared/constants/project-api-routes.const';
import { ConversationTypeEnum } from '../../enums/conversation-type.enum';
import { BlobTypeEnum, GeneralApiEnum } from '../../../shared/enums/api/general-api.enum';
import { RolesEnum } from '../../../shared/enums/roles.enum';
import { FormControlStatusEnum } from '../../../shared/enums/form-control-status.enum';
import { MessageApiEnum } from '../../enums/message-api.enum';
import { AttachmentTypeEnum } from '../../enums/attachment-type.enum';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';

import { UserStatusEnum } from '../../../user-management/enums/user-status.enum';
import { DiscussionSourceTypeEnum } from '../../../shared/enums/discussion-source-type.enum';

import { CustomValidator } from '../../../shared/validators/custom.validator';
import { MAX_IMAGE_SIZE } from '../../../shared/constants/image-size.const';
import { CanComponentDeactivate } from 'src/app/shared/guards/can-deactivate.guard';
import { ProjectStatusEnum } from '../../../shared/enums/projects/project-status.enum';
import { CommonService } from 'src/app/core/services/common.service';

@Component({
  selector: 'neo-new-message',
  templateUrl: './new-message.component.html',
  styleUrls: ['./new-message.component.scss', '../../styles/common.scss']
})
export class NewMessageComponent implements OnInit, OnDestroy, CanComponentDeactivate {
  linkModal: boolean;
  isLoading: boolean;
  isCurrentUserSolutionProvider: boolean;
  isSolutionProvider: boolean;
  projectSearch: string;
  conversationTypes = ConversationTypeEnum;
  sendOptionCount: number = 1;
  selectedConversationTypes: ConversationTypeEnum = ConversationTypeEnum.SendIndividually;
  attachments: ImageInterface[] = [];
  links: Record<string, string | AttachmentTypeEnum>[] = [];
  currentUser: UserInterface;
  results: MultiselectOptionInterface[];
  selectedUsers: MultiselectOptionInterface[];
  showLeaveConfirmation: boolean;
  nextUrl: string;
  formSaved: boolean;
  imageViewModal: boolean;
  currentImageIndex: number;

  formGroup: FormGroup = new FormGroup({
    subject: new FormControl(null, [CustomValidator.required]),
    projectId: new FormControl(null),
    message: new FormGroup({
      text: new FormControl(null, CustomValidator.required),
      attachments: new FormArray([])
    }),
    users: new FormArray([], Validators.required),
    url: new FormControl(null)
  });

  linkFormGroup: FormGroup = new FormGroup({
    text: new FormControl(null, [Validators.required, CustomValidator.nameExcludeSymbols]),
    link: new FormControl(null, [Validators.required, CustomValidator.url])
  });

  projects$: Observable<SearchResultInterface[]>;
  userSearch$: Subject<string> = new Subject<string>();

  private maxImageSize: number = MAX_IMAGE_SIZE;

  private unsubscribe$: Subject<void> = new Subject<void>();
  private fileSelected$: Subject<FormData> = new Subject<FormData>();

  constructor(
    private readonly titleService: TitleService,
    private readonly httpService: HttpService,
    private readonly snackbarService: SnackbarService,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute,
    private commonService: CommonService,
    private readonly permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle('messages.newMessageLabel');
    this.listenForUserSearch();
    this.listedForFileSelected();
    this.listenForQueryParams();
    this.listenForCurrentUser();
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    this.userSearch$.next(null);
    this.userSearch$.complete();
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: BeforeUnloadEvent): boolean {
    if (this.hasChanges()) {
      $event.returnValue = false;
      $event.preventDefault();
      return false;
    }

    return true;
  }

  canDeactivate(nextUrl?: string): boolean {
    if (!this.showLeaveConfirmation && this.hasChanges()) {
      this.showLeaveConfirmation = true;
      this.nextUrl = nextUrl;
      return false;
    }

    return true;
  }

  goBack() {
    this.commonService.goBack();
  }

  hasChanges(): boolean {
    return (
      !this.formSaved && (this.links.length > 0 || this.attachments.length > 0 || this.formHasValue(this.formGroup))
    );
  }

  formHasValue(formGroup: FormGroup): boolean {
    let hasValue = false;
    const keys = Object.keys(formGroup.controls);

    for (let key of keys) {
      const control = formGroup.get(key);
      if (control instanceof FormGroup) {
        hasValue = this.formHasValue(control);
      } else if (control instanceof FormArray) {
        hasValue = !!control.value.length;
      } else {
        hasValue = !!control.value;
      }

      if (hasValue) {
        break;
      }
    }

    return hasValue;
  }

  leavePage(): void {
    const urlSegments: string[] = this.nextUrl.includes('?') ? this.nextUrl.split('?') : [];
    if (urlSegments?.length > 1) {
      this.router.navigateByUrl(this.nextUrl);
    } else {
      this.router.navigate([this.nextUrl]);
    }
  }

  selectionChange(selectedUsers: MultiselectOptionInterface[]): void {
    const usersArray = this.formGroup.get('users') as FormArray;
    const users = selectedUsers.map(user => ({ id: user.id }));

    this.isSolutionProvider = selectedUsers.some(user =>
      user.roles.some(role => role.id === RolesEnum.SolutionProvider || role.id === RolesEnum.SPAdmin)
    );

    while (usersArray.length !== 0) {
      usersArray.removeAt(0);
    }

    users.forEach(user => usersArray.push(new FormControl(user)));
  }

  searchProject(search?: string): void {
    if (!search) {
      this.projectSearch = null;
      this.projects$ = null;
      this.formGroup.get('projectId').setValue(null);
    }

    this.projectSearch = search;
    this.projects$ = this.httpService
      .get<PaginateResponseInterface<ProjectInterface>>(ProjectApiRoutes.projectsList, {
        search,
        filterBy: `statusids=${ProjectStatusEnum.Active}`
      })
      .pipe(
        map(projectList =>
          projectList.dataList.map(project => ({
            id: project.id,
            name: project.title
          }))
        )
      );
  }

  setProject(project: ProjectInterface): void {
    this.formGroup.get('projectId').patchValue(project.id);
  }

  onFileSelect(event): void {
    if (event.target.files.length > 0) {
      const file: File = event.target.files[0];
      const isLarge = file.size > this.maxImageSize;

      if (isLarge) {
        this.snackbarService.showError('general.largeFileLabel');
        return;
      }

      const formData: FormData = new FormData();
      formData.append('file', file);

      this.fileSelected$.next(formData);
    }
  }

  removeAttachment(index: number): void {
    this.attachments.splice(index, 1);
  }

  isAdmin(roles: UserRoleInterface[]): boolean {
    return roles?.some(role => role.id === RolesEnum.Admin);
  }

  selectType(item: Record<string, string>): void {
    if (this.selectedConversationTypes !== +item.key) {
      const usersArray = this.formGroup.get('users') as FormArray;

      while (usersArray.length !== 0) {
        usersArray.removeAt(0);
      }

      this.selectedConversationTypes = +item.key;

      this.selectedUsers = null;
    }
  }

  saveLink(): void {
    if (this.linkFormGroup.invalid) {
      Object.keys(this.linkFormGroup.value).map(key => {
        this.linkFormGroup.get(key).markAsDirty();
        this.linkFormGroup.get(key).markAsTouched();
      });

      return;
    }

    this.linkModal = false;
    this.links.push({
      ...this.linkFormGroup.value,
      type: AttachmentTypeEnum.Link
    });

    this.linkFormGroup.reset();
  }

  cancelLinkDialog(): void {
    this.linkModal = false;
    this.linkFormGroup.reset();
  }

  hasError(controlName: string, formGroup: string): boolean {
    const control = this[formGroup]?.get(controlName);

    return control?.invalid && control?.touched && control?.dirty;
  }

  createConversation(): void {
    if (this.projectSearch) {
      this.formGroup.get('projectId').setValidators(Validators.required);
      this.formGroup.get('projectId').updateValueAndValidity();
    }
    else{
      this.formGroup.get('projectId').removeValidators(Validators.required);
      this.formGroup.get('projectId').updateValueAndValidity();
    }

    if (this.formGroup.status === FormControlStatusEnum.Invalid) {
      Object.keys(this.formGroup.value).map(key => {
        this.formGroup.get(key).markAsDirty();
        this.formGroup.get(key).markAsTouched();
      });

      const message = this.formGroup.get('message.text');
      if (message.invalid) {
        message.markAsTouched();
        message.markAsDirty();
      }

      return;
    }

    const formData = this.formGroup.value;
    const images = this.attachments.map(attachment => ({
      text: attachment.name,
      link: attachment.uri,
      type: AttachmentTypeEnum.Media,
      imageName: attachment.name
    }));

    formData.message.attachments = [...this.links, ...images];
    formData['sourceTypeId'] = DiscussionSourceTypeEnum.General;
    if (this.isSolutionProvider && this.isCurrentUserSolutionProvider) {
      formData['projectId'] = null;
    }
    this.httpService
      .post(MessageApiEnum.Conversations, formData)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(() => {
        this.formSaved = true;
        this.router.navigate(['/messages']);
      });
  }

  getAttachments(): AttachmentInterface[] {
    return this.attachments.map(
      attachment =>
        ({
          image: attachment
        } as AttachmentInterface)
    );
  }

  private listenForUserSearch(): void {
    this.userSearch$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(search =>
          this.httpService.get<PaginateResponseInterface<UserInterface>>(UserManagementApiEnum.Users, {
            expand: 'images,company,roles',
            filterBy: `statusids=${UserStatusEnum.Active}`,
            search
          })
        ),
        map(users =>
          users.dataList.map(user => ({
            id: user.id,
            image: user.image,
            firstName: user.firstName,
            lastName: user.lastName,
            name: `${user.firstName} ${user.lastName}, ${user?.company?.name}`,
            roles: user.roles
          }))
        )
      )
      .subscribe(response => {
        const selectedUsers = this.formGroup.get('users').value?.map(user => user.id);
        this.results = response.filter(user => user.id !== this.currentUser.id && !selectedUsers?.includes(user.id));
      });
  }

  private listedForFileSelected() {
    this.fileSelected$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(formData => {
          this.isLoading = true;

          return this.httpService
            .post<ImageInterface>(
              GeneralApiEnum.Media,
              formData,
              {
                BlobType: BlobTypeEnum.Conversations
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

  private listenForQueryParams(): void {
    this.activatedRoute.queryParams
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(params => !!params?.userId),
        switchMap(params =>
          this.httpService.get<UserInterface>(`${UserManagementApiEnum.Users}/${params.userId}`, {
            expand: 'images,company,roles'
          })
        )
      )
      .subscribe(response => {
        this.selectedUsers = [
          {
            id: response.id,
            image: response.image,
            firstName: response.firstName,
            lastName: response.lastName,
            name: `${response.firstName} ${response.lastName}, ${response?.company?.name}`,
            roles: response.roles
          }
        ];

        this.selectionChange(this.selectedUsers);
      });
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

        this.isCurrentUserSolutionProvider = this.currentUser.roles.some(
          role => role.id === RolesEnum.SolutionProvider || role.id === RolesEnum.SPAdmin
        );

        if (this.permissionService.userHasPermission(this.currentUser, PermissionTypeEnum.MessagesAll)) {
          this.sendOptionCount++;
        }
      });
  }
}
