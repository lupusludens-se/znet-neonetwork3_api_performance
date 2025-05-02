import { AfterViewChecked, Component, ElementRef, OnDestroy, OnInit, Renderer2, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { Observable, Subject, of, switchMap, takeUntil } from 'rxjs';
import { catchError, filter, map } from 'rxjs/operators';

import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import * as dayjs from 'dayjs';
import { CustomValidator } from '../../../shared/validators/custom.validator';

import { HttpService } from '../../../core/services/http.service';
import { AuthService } from '../../../core/services/auth.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { CoreService } from '../../../core/services/core.service';

import { ImageInterface } from '../../../shared/interfaces/image.interface';
import { AttachmentTypeEnum } from '../../enums/attachment-type.enum';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import { MessageInterface } from '../../interfaces/message.interface';
import { DEFAULT_PER_PAGE, PaginationInterface } from '../../../shared/modules/pagination/pagination.component';
import { ConversationUserInterface } from '../../interfaces/conversation-user.interface';
import { AttachmentInterface } from '../../../shared/interfaces/attachment.interface';
import { ConversationInterface } from '../../interfaces/conversation.interface';
import { SearchResultInterface } from '../../../shared/interfaces/search-result.interface';
import { ProjectInterface } from '../../../shared/interfaces/projects/project.interface';

import { FormControlStatusEnum } from '../../../shared/enums/form-control-status.enum';
import { BlobTypeEnum, GeneralApiEnum } from '../../../shared/enums/api/general-api.enum';
import { MessageApiEnum } from '../../enums/message-api.enum';
import { ProjectApiRoutes } from '../../../projects/shared/constants/project-api-routes.const';
import { MAX_IMAGE_SIZE } from '../../../shared/constants/image-size.const';
import { RolesEnum } from '../../../shared/enums/roles.enum';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';

import { TextEditorComponent } from '../../../shared/modules/text-editor/text-editor.component';
import { ProjectStatusEnum } from '../../../shared/enums/projects/project-status.enum';
import { MessageRoutesEnum } from '../../enums/message-routes.enum';
import { MessagesService } from '../../services/messages.service';
import { CommonService } from 'src/app/core/services/common.service';
import { FilterStateInterface } from 'src/app/shared/modules/filter/interfaces/filter-state.interface';
import { TranslateService } from '@ngx-translate/core';
import { InitiativeAttachedContent } from 'src/app/initiatives/interfaces/initiative-attached.interface';
import { InitiativeApiEnum } from 'src/app/initiatives/enums/initiative-api.enum';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';

@UntilDestroy()
@Component({
  selector: 'neo-message-history',
  templateUrl: './message-history.component.html',
  styleUrls: ['./message-history.component.scss', '../../styles/common.scss']
})
export class MessageHistoryComponent implements OnInit, OnDestroy, AfterViewChecked {
  @ViewChild('editorComponent') editorComponent: TextEditorComponent;
  @ViewChild('messageHistory') messageHistory: ElementRef;

  linkProjectModal: boolean;
  linkModal: boolean;
  contactUsModal: boolean;
  isLoading: boolean;
  imageViewModal: boolean;
  defaultItemPerPage = DEFAULT_PER_PAGE;
  showDeleteModal: boolean = false;
  contentType: string = InitiativeModulesEnum[InitiativeModulesEnum.Messages];
  currentImageIndex: number;
  userCount: number;
  attachmentType = AttachmentTypeEnum;

  attachments: ImageInterface[] = [];
  links: Record<string, string | AttachmentTypeEnum>[] = [];
  link: Record<string, string> = {
    text: '',
    link: ''
  };

  messages: MessageInterface[];
  paging: PaginationInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null
  };

  linkFormGroup: FormGroup = new FormGroup({
    text: new FormControl(null, [CustomValidator.required, CustomValidator.nameExcludeSymbols]),
    link: new FormControl(null, [Validators.required, CustomValidator.url])
  });
  projectFormGroup: FormGroup = new FormGroup({
    projectId: new FormControl(null, Validators.required)
  });

  projects$: Observable<SearchResultInterface[]>;
  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  conversationInfo$: Observable<ConversationInterface> = this.getConversationInfo();
  projectFormSubmitted: boolean;
  linksFormSubmitted: boolean;
  userListActive: boolean;
  userListClick: boolean;
  readonly userStatuses = UserStatusEnum;

  isTextEditorFocused: boolean = false;
  disableScrollDown = false;

  private maxImageSize: number = MAX_IMAGE_SIZE;
  private pageDataLoad$: Subject<void> = new Subject<void>();
  private readAll$: Subject<void> = new Subject<void>();
  private fileSelected$: Subject<FormData> = new Subject<FormData>();
  private filterState: FilterStateInterface = null;
  private unsubscribe$: Subject<void> = new Subject<void>();
  routesToNotClearFilters: string[] = [`${MessageRoutesEnum.MessagesComponent}`];
  selectedDeleteMessage: MessageInterface;
  selectedEditMessage: MessageInterface;
  showDiscardModal: boolean;
  attachToInitiative: boolean = false;
  discussionId: number;

  constructor(
    private readonly authService: AuthService,
    private readonly httpService: HttpService,
    private readonly snackbarService: SnackbarService,
    private readonly coreService: CoreService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly renderer: Renderer2,
    private messagesService: MessagesService,
    private commonService: CommonService,
    private translateService: TranslateService,
    private activityService: ActivityService
  ) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(() => {
      this.discussionId = this.activatedRoute.snapshot.params.id;
    });

    this.listedForFileSelected();
    this.listenToLoadMessages();
    this.listenToReadAll();
    this.listenForUserListClick();
    this.listenForFilterState();
    this.pageDataLoad$.next();
  }

  private listenForFilterState(): void {
    this.commonService
      .filterState()
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(initialFiltersState => !!initialFiltersState)
      )
      .subscribe(entity => {
        this.filterState = entity;
      });
  }

  ngAfterViewChecked(): void {
    if (!this.messageHistory) return;
    this.scrollToBottom();
  }
  ngOnDestroy(): void {
    this.pageDataLoad$.next();
    this.pageDataLoad$.complete();

    this.fileSelected$.next(null);
    this.fileSelected$.complete();

    this.readAll$.next();
    this.readAll$.complete();

    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    const routesFound = this.routesToNotClearFilters.some(val => this.coreService.getOngoingRoute().includes(val));
    if (!routesFound) {
      this.messagesService.clearPreferencesData();
      this.commonService.clearFilters(this.filterState);
    }
  }

  onScroll() {
    let element = this.messageHistory.nativeElement;
    let atBottom = element.scrollHeight - element.scrollTop === element.clientHeight;
    if (this.disableScrollDown && atBottom) {
      this.disableScrollDown = false;
    } else {
      this.disableScrollDown = true;
    }
  }

  private scrollToBottom(): void {
    if (this.disableScrollDown) {
      return;
    }
    this.messageHistory.nativeElement.scrollTop = this.messageHistory.nativeElement.scrollHeight;
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
    this.disableScrollDown = false;
  }

  removeAttachment(index: number): void {
    this.attachments.splice(index, 1);
    this.disableScrollDown = false;
  }

  saveLink(): void {
    this.linksFormSubmitted = true;

    if (this.linkFormGroup.status === FormControlStatusEnum.Invalid) return;

    this.linkModal = false;
    this.links.push({
      ...this.linkFormGroup.value,
      type: AttachmentTypeEnum.Link
    });
    this.linkFormGroup.reset();
    this.disableScrollDown = false;
  }

  hasError(controlName: string, formGroup: string): boolean {
    const control = this[formGroup]?.get(controlName);

    return control?.invalid && control?.touched && control?.dirty;
  }

  getUser(user: ConversationUserInterface): UserInterface {
    const userName = user.name.split(' ');

    return {
      id: user.id,
      firstName: user.statusId === UserStatusEnum.Deleted ? 'Deleted' : userName[0],
      lastName: user.statusId === UserStatusEnum.Deleted ? 'User' : userName[1],
      image: user.statusId === UserStatusEnum.Deleted ? null : user?.image
    } as UserInterface;
  }

  get disabled(): boolean {
    const value = this.editorComponent?.editor?.nativeElement?.innerText.replace('/n', '').trim();
    return !value && !value?.length && !this.links?.length && !this.attachments.length;
  }

  sendMessage(): void {
    if (this.disabled) {
      return;
    }

    const images = this.attachments.map(attachment => ({
      type: AttachmentTypeEnum.Media,
      imageName: attachment.name,
      link: attachment.uri,
      text: attachment.name
    }));

    const links = this.links.map(link => ({
      type: AttachmentTypeEnum.Link,
      text: link.text,
      link: link.link
    }));

    const formData: MessageInterface = {
      attachments: [...images, ...links] as AttachmentInterface[],
      text: this.coreService.modifyEditorText(this.editorComponent.editor.nativeElement as HTMLElement)
    };

    this.httpService
      .post(
        `${MessageApiEnum.Conversations}/${this.activatedRoute.snapshot.params.id}/${MessageApiEnum.Messages}`,
        formData
      )
      .subscribe(() => {
        this.attachments = [];
        this.links = [];
        this.editorComponent.editor.nativeElement.innerHTML = null;
        this.pageDataLoad$.next();
      });
    this.disableScrollDown = false;
  }

  isToday(messageDate: Date): boolean {
    const currentDate = dayjs(new Date());
    const date = dayjs(messageDate + 'Z');
    const hours = currentDate.diff(date, 'hour');

    return hours <= 24;
  }

  conversationUser(currentUser: UserInterface, users: ConversationUserInterface[]): ConversationUserInterface {
    return users.filter(user => user.id !== currentUser.id)[0];
  }

  conversationUsers(
    currentUser: UserInterface,
    users: ConversationUserInterface[],
    from: number,
    to: number
  ): ConversationUserInterface[] {
    users = users.filter(user => user.id !== currentUser.id);
    this.userCount = users.length - 5;
    return users.slice(from, to);
  }

  searchProject(search?: string): void {
    this.projects$ = this.httpService
      .get<PaginateResponseInterface<ProjectInterface>>(ProjectApiRoutes.projectsList, {
        search,
        filterBy: `statusids=${ProjectStatusEnum.Active}`
      })
      .pipe(
        map(projectList =>
          projectList.dataList.map(projectResponse => ({
            id: projectResponse.id,
            name: projectResponse.title
          }))
        )
      );
  }

  setProject(project: ProjectInterface): void {
    this.projectFormGroup.get('projectId').patchValue(project.id);
  }

  linkToProject(conversationInfo: ConversationInterface): void {
    this.projectFormSubmitted = true;
    if (this.projectFormGroup.invalid) return;

    const formData = {
      message: this.messages[0],
      sourceTypeId: conversationInfo.sourceTypeId,
      users: conversationInfo.users.map(user => ({ id: user.id })),
      subject: conversationInfo.subject,
      projectId: this.projectFormGroup.value.projectId
    };

    this.httpService
      .put(`${MessageApiEnum.Conversations}/${this.activatedRoute.snapshot.params.id}`, formData)
      .subscribe(() => {
        this.linkProjectModal = false;
        this.conversationInfo$ = this.getConversationInfo();
      });
    this.disableScrollDown = false;
  }

  getAttachments(messageIndex: number): AttachmentInterface[] {
    if (messageIndex !== null) {
      return this.messages[messageIndex].attachments.filter(attachment => attachment.type === AttachmentTypeEnum.Media);
    } else {
      return this.attachments.map(
        attachment =>
        ({
          image: attachment
        } as AttachmentInterface)
      );
    }
  }

  isInConversation(currentUser: UserInterface, conversationInfo: ConversationInterface): boolean {
    if (
      conversationInfo.users.length == 2 &&
      (conversationInfo.users[0].statusId === this.userStatuses.Deleted ||
        conversationInfo.users[1].statusId === this.userStatuses.Deleted)
    ) {
      return false;
    } else {
      return conversationInfo.users.some(user => user.id === currentUser.id);
    }
  }

  hideProjectAttachButton(currentUser: UserInterface, users: ConversationUserInterface[]): boolean {
    return (
      users.filter(user => user.id !== currentUser.id).some(user => user.isSolutionProvider) &&
      currentUser.roles.some(role => role.id === RolesEnum.SolutionProvider)
    );
  }

  checkIfUserIsCorporate(currentUser: UserInterface, users: ConversationUserInterface[]): boolean {
    return (
      currentUser.roles.some(role => role.id === RolesEnum.Corporation)
    );
  }

  closeLinkProjectModal(): void {
    this.linkProjectModal = false;
    this.projectFormSubmitted = false;
    this.projectFormGroup.reset();
    this.disableScrollDown = false;
  }

  closeLinksModal(): void {
    this.linkModal = false;
    this.linkFormGroup.reset();
    this.linksFormSubmitted = false;
    this.disableScrollDown = false;
  }

  goBack() {
    this.commonService.goBack();
  }

  private listedForFileSelected() {
    this.fileSelected$
      .pipe(
        untilDestroyed(this),
        filter(file => !!file),
        switchMap(formData => {
          this.isLoading = true;

          return this.httpService.post<ImageInterface>(
            GeneralApiEnum.Media,
            formData,
            {
              BlobType: BlobTypeEnum.Conversations
            },
            false
          ).pipe(
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

  private listenToLoadMessages(): void {
    this.pageDataLoad$
      .pipe(
        untilDestroyed(this),
        switchMap(() => {
          const paging = this.coreService.deleteEmptyProps({
            ...this.paging,
            expand: 'user,user.image,user.company,attachments,attachments.image'
          });

          return this.httpService.get<PaginateResponseInterface<MessageInterface>>(
            `${MessageApiEnum.Conversations}/${this.activatedRoute.snapshot.params.id}/${MessageApiEnum.Messages}`,
            paging
          );
        })
      )
      .subscribe(response => {
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip ? this.paging?.skip : 0,
          total: response.count
        };

        this.messages = response.dataList.reverse();
        this.readAll$.next();
      });
  }

  private getConversationInfo(): Observable<ConversationInterface> {
    return this.httpService.get<ConversationInterface>(
      `${MessageApiEnum.Conversations}/${this.activatedRoute.snapshot.params.id}`,
      {
        expand:
          'discussionusers,discussionusers.users,discussionusers.users.image,discussionusers.users.roles,' +
          'discussionusers.users.company,project'
      }
    );
  }

  private listenToReadAll(): void {
    this.readAll$
      .pipe(
        untilDestroyed(this),
        switchMap(() =>
          this.httpService.put<unknown>(
            `${MessageApiEnum.Conversations}/${this.activatedRoute.snapshot.params.id}/${MessageApiEnum.Messages}`
          )
        )
      )
      .subscribe(() => this.coreService.badgeDataFetch$.next());
  }

  private listenForUserListClick(): void {
    this.renderer.listen('window', 'click', () => {
      if (!this.userListClick) {
        this.userListActive = false;
      }
      this.userListClick = false;
    });
  }

  onMouseEnter(message: MessageInterface) {
    message.showActions = true;
  }

  onMouseLeave(message: MessageInterface) {
    setTimeout(() => {
      message.showActions = false;
    }, 100);
  }


  deleteMessage(message: MessageInterface) {
    this.selectedDeleteMessage = message;
    this.showDeleteModal = true;
    this.showDiscardModal = false;
    message.showEdit = false;
    if (this.messages.findIndex(x => x.showEdit == true) > -1) {
      alert("There are records opened already for editing.")
    }
  }

  confirmDelete() {
    this.showDeleteModal = false;
    this.httpService
      .delete(MessageApiEnum.DeleteMessage + `/${this.selectedDeleteMessage.id}`)
      .pipe(
        takeUntil(this.unsubscribe$),
        catchError(error => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(error);
        })
      )
      .subscribe((result) => {
        if (result == true) {
          this.snackbarService.showSuccess(
            this.translateService.instant('messages.deleteModal.successMessage')
          );
          this.selectedDeleteMessage.text = "This message has been deleted.";
          this.selectedDeleteMessage.statusId = 0;
          this.selectedDeleteMessage = null;
        }
      });
  }

  editMessage(message: MessageInterface) {
    message.showEdit = true;
    message.showRequiredMessage = false;
    this.selectedEditMessage = message;
  }

  closeDeletePopup() {
    this.showDeleteModal = false;
    this.selectedDeleteMessage = null
  }

  trackAttachToInitiativeActivity(){
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        buttonName: this.translateService.instant('initiative.attachContent.attachMsgContentLabel'),
        moduleName: InitiativeModulesEnum[InitiativeModulesEnum.Messages]
      })
      ?.subscribe();
  }
}
