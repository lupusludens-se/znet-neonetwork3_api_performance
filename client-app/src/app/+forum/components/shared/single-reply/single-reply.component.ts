import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { Subject, switchMap } from 'rxjs';

import { HttpService } from '../../../../core/services/http.service';
import { TranslateService } from '@ngx-translate/core';

import { UserInterface } from '../../../../shared/interfaces/user/user.interface';
import { FirstMessageInterface } from '../../../interfaces/first-message.interface';
import { ForumUserResponseInterface } from '../../../interfaces/forum-user-response.interface';
import { PaginateResponseInterface } from '../../../../shared/interfaces/common/pagination-response.interface';
import { RespondInterface } from '../../../interfaces/respond.interface';

import { ForumApiEnum } from '../../../enums/forum-api.enum';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CoreService } from 'src/app/core/services/core.service';

@UntilDestroy()
@Component({
  selector: 'neo-single-reply',
  templateUrl: 'single-reply.component.html',
  styleUrls: ['single-reply.component.scss']
})
export class SingleReplyComponent implements OnInit {
  @Input() currentUser: UserInterface;
  @Input() comment: FirstMessageInterface;
  @Input() forumId: number;
  @Input() parentPinId: number;
  @Input() parentMessageId: number;

  @Input() isChild: boolean;
  @Input() isAdmin: boolean;
  @Input() showReply: boolean;
  @Input() hideChildReplies: boolean;

  @Output() likeClick: EventEmitter<FirstMessageInterface> = new EventEmitter<FirstMessageInterface>();
  @Output() followUserClick: EventEmitter<Record<string, number | boolean>> = new EventEmitter<
    Record<string, number | boolean>
  >();
  @Output() deleteClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() loadParentReplies: EventEmitter<void> = new EventEmitter<void>();
  @Output() replyClick: EventEmitter<ForumUserResponseInterface> = new EventEmitter<ForumUserResponseInterface>();
  @Output() updateParentData: EventEmitter<boolean> = new EventEmitter<boolean>();

  commentReplies: FirstMessageInterface[];
  mouseover: boolean;
  showReplies: boolean;
  deleteModal: boolean;
  buttonText: string;
  replyMessage: string;
  readonly userStatuses = UserStatusEnum;
  inEditMode: boolean = false;

  loadReplies$: Subject<void> = new Subject<void>();

  constructor(private readonly translateService: TranslateService, private readonly httpService: HttpService,
    private readonly coreService: CoreService) { }

  ngOnInit(): void {
    this.listenToRepliesLoad();
  }

  showHideReplies(repliesAmount: number): void {
    this.showReplies = !this.showReplies;
    this.buttonText = this.showReplies
      ? this.translateService.instant('forum.showLessLabel')
      : `${this.translateService.instant('forum.viewLabel')} ${repliesAmount} ${this.translateService.instant(
        'forum.repliesLabel'
      )}...`;

    if (this.showReplies) {
      this.loadReplies$.next();
    }
  }

  getUser(user: ForumUserResponseInterface): UserInterface {
    const userName = user?.name.split(' ');

    if (!userName) return;

    return {
      image: user.statusId === UserStatusEnum.Deleted ? null : user?.image,
      firstName: user.statusId === UserStatusEnum.Deleted ? 'Deleted' : userName[0],
      lastName: user.statusId === UserStatusEnum.Deleted ? 'User' : userName[1]
    } as UserInterface;
  }

  replyToUser(user: ForumUserResponseInterface): void {
    if (this.isChild) {
      this.replyClick.emit(user);
    } else {
      this.showReply = true;
    }
  }

  pin(): void {
    
    this.httpService
      .put(`${ForumApiEnum.Forum}/${this.forumId}/${ForumApiEnum.Messages}/${this.comment.id}`, {
        isPinned: !this.comment.isPinned,
        text: this.comment.text,
        textContent : this.coreService.convertToPlain(this.comment.text ?? ''), 
        attachments: this.comment.attachments,
        parentMessageId: this.parentPinId
      })
      .subscribe(response => {
        this.comment.isPinned = response.isPinned;
      });
  }

  edit(): void {
    this.inEditMode = true;
  }

  update(record : any): void {
    this.httpService
      .put(`${ForumApiEnum.Forum}/${this.forumId}/${ForumApiEnum.Messages}/${this.comment.id}`, {
        text: record.text,
        textContent : this.coreService.convertToPlain(record.text ?? ''), 
        parentMessageId: this.parentPinId,
        userId : this.currentUser?.id
      })
      .subscribe((response: any) => {
        this.comment.text = response.text;
        this.comment.modifiedOn = response.modifiedOn;
        this.inEditMode = false;
      });
  }

  delete(): void {
    this.httpService
      .delete(`${ForumApiEnum.Forum}/${this.forumId}/${ForumApiEnum.Messages}/${this.comment.id}`)
      .subscribe(() => {
        this.deleteModal = false;
        this.deleteClick.next();
      });
  }

  replyDeleted(index: number) {
    this.commentReplies.splice(index, 1);
    this.comment.repliesCount = this.comment.repliesCount - 1;
    this.updateParentData.emit(true);
  }

  reply(message: RespondInterface): void {
    message.parentMessageId = this.parentMessageId;

    this.httpService
      .post<unknown>(`${ForumApiEnum.Forum}/${this.forumId}/${ForumApiEnum.Messages}/`, message)
      .subscribe(() => {
        this.isChild ? this.loadParentReplies.emit() : this.loadReplies$.next();
        this.showReply = false;
        this.updateParentData.emit(false);
      });
  }

  toggleModal(): void {
    this.deleteModal = !this.deleteModal;
  }

  private listenToRepliesLoad() {
    this.loadReplies$
      .pipe(
        untilDestroyed(this),
        switchMap(() =>
          this.httpService.get<PaginateResponseInterface<FirstMessageInterface>>(
            `${ForumApiEnum.Forum}/${this.forumId}/${ForumApiEnum.Messages}`,
            {
              parentMessageId: this.parentMessageId,
              expand: 'user,user.company,user.image,user.follower,messagelikes,replies'
            }
          )
        )
      )
      .subscribe(commentReplies => {
        this.commentReplies = commentReplies.dataList;
        this.comment.repliesCount = commentReplies.dataList.length;
      });
  }
}
