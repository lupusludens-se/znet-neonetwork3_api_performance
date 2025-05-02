import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, take, throwError } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { TitleService } from '../../../core/services/title.service';
import { HttpService } from '../../../core/services/http.service';
import { CoreService } from '../../../core/services/core.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { AuthService } from '../../../core/services/auth.service';

import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { ForumTopicInterface } from '../../interfaces/forum-topic.interface';
import { FirstMessageInterface } from '../../interfaces/first-message.interface';
import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import { ForumUserResponseInterface } from '../../interfaces/forum-user-response.interface';
import { RespondInterface } from '../../interfaces/respond.interface';
import { ForumApiEnum } from '../../enums/forum-api.enum';
import { FollowingService } from '../../../core/services/following.service';
import { TaxonomyTypeEnum } from '../../../shared/enums/taxonomy-type.enum';
import { SaveContentService } from '../../../shared/services/save-content.service';
import { UserRoleInterface } from '../../../shared/interfaces/user/user-role.interface';
import { RolesEnum } from '../../../shared/enums/roles.enum';
import { catchError } from 'rxjs/operators';
import { PermissionService } from 'src/app/core/services/permission.service';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonService } from 'src/app/core/services/common.service';

@UntilDestroy()
@Component({
  selector: 'neo-forum-thread',
  templateUrl: 'forum-thread.component.html',
  styleUrls: ['forum-thread.component.scss']
})
export class ForumThreadComponent implements OnInit {
  topic: ForumTopicInterface;
  responses: FirstMessageInterface[];

  mouseover: boolean;
  linkCopied: boolean;
  type = TaxonomyTypeEnum;

  currentImageIndex: number;
  imageViewModal: boolean;
  deleteModal: boolean;

  permissionTypes = PermissionTypeEnum;
  readonly userStatuses = UserStatusEnum;

  //#region Subjects
  currentUser$: Observable<UserInterface> = this.authService.currentUser();

  //#endregion

  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly titleService: TitleService,
    private readonly httpService: HttpService,
    private readonly coreService: CoreService,
    private readonly snackbarService: SnackbarService,
    private readonly authService: AuthService,
    private readonly followingService: FollowingService,
    private readonly saveContentService: SaveContentService,
    private readonly router: Router,
    private readonly activityService: ActivityService,
    private commonService: CommonService,
    public permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(() => {
      this.loadData();
      this.listenToUserFollow();
      this.listenToDiscussionFollow();
    });
  }

  getUser(user: ForumUserResponseInterface): UserInterface {
    const userName = user?.name.split(' ');

    if (!userName) {
      return;
    }

    return {
      image: user.statusId === UserStatusEnum.Deleted ? null : user?.image,
      firstName: user.statusId === UserStatusEnum.Deleted ? 'Deleted' : userName[0],
      lastName: user.statusId === UserStatusEnum.Deleted ? 'User' : userName[1]
    } as UserInterface;
  }

  onLinkButtonClick(): void {
    this.coreService.copyTextToClipboard(window.location.href);
    this.linkCopied = true;
    this.snackbarService.showSuccess('general.linkCopiedLabel');

    this.activityService.trackElementInteractionActivity(ActivityTypeEnum.LinkButtonClick)?.subscribe();
  }

  like(message: FirstMessageInterface): void {
    if (message.isLiked) {
      this.httpService
        .delete<unknown>(`${ForumApiEnum.Forum}/${this.topic.id}/${ForumApiEnum.Messages}/${message.id}/likes`, null)
        .subscribe(() => {
          message.isLiked = false;
          message.likesCount -= 1;
        });
    } else {
      this.httpService
        .post<unknown>(`${ForumApiEnum.Forum}/${this.topic.id}/${ForumApiEnum.Messages}/${message.id}/likes`, null)
        .subscribe(() => {
          message.isLiked = true;
          message.likesCount += 1;
        });
    }
  }

  isMember(currentUser: UserInterface): boolean {
    if (this.topic?.isPrivate) {
      const isMember = this.topic.users.some(user => user?.id === currentUser.id) || this.isAdmin(currentUser.roles);

      if (isMember) {
        return isMember;
      } else {
        this.router.navigate(['403']);
      }
    }

    return true;
  }

  saveTopic(topic: ForumTopicInterface): void {
    if (this.topic.isSaved) {
      this.saveContentService.deleteForum(topic.id).subscribe(() => {
        topic.isSaved = false;
      });
    } else {
      this.saveContentService.saveForum(topic.id).subscribe(() => {
        topic.isSaved = !topic.isSaved;
      });
    }
  }

  followUser(userId: number, unfollow?: boolean): void {
    this.followingService.followUser(userId, unfollow);
  }

  followDiscussion(userId: number, unfollow?: boolean) {
    this.followingService.followForumDiscussion(userId, unfollow);
  }

  pin(): void {
    const isPinned = !this.topic.isPinned;
    if (this.topic?.firstMessage) {
      this.topic.firstMessage.textContent = this.coreService.convertToPlain(this.topic.firstMessage.text ?? '');
    }
    this.httpService
      .put(`${ForumApiEnum.Forum}/${this.topic.id}`, {
        ...this.topic,
        isPinned: isPinned
      })
      .subscribe(() => {
        this.topic.isPinned = isPinned;
      });
  }

  isAdmin(roles: UserRoleInterface[]): boolean {
    return roles?.some(role => role.id === RolesEnum.Admin && role.isSpecial);
  }

  delete(): void {
    this.httpService.delete<unknown>(`${ForumApiEnum.Forum}/${this.topic.id}`).subscribe(() => {
      this.deleteModal = false;
      this.router.navigate(['/forum']);
    });
  }

  respond(message: RespondInterface): void {
    message.parentMessageId = this.topic.firstMessage.id;
    if (this.topic?.firstMessage) {
      this.topic.firstMessage.textContent = this.coreService.convertToPlain(this.topic.firstMessage.text ?? '');
    }
    this.httpService
      .post<unknown>(`${ForumApiEnum.Forum}/${this.topic.id}/${ForumApiEnum.Messages}/`, message)
      .subscribe(() => {
        this.loadResponsesData(this.topic.firstMessage.id);
        this.updateCommentsCount(false);
      });
  }

  toggleModal(): void {
    this.deleteModal = !this.deleteModal;
  }

  //#region Observers
  loadData(): void {
    this.httpService
      .get<ForumTopicInterface>(`${ForumApiEnum.Forum}/${this.activatedRoute.snapshot.params.id}`, {
        expand: 'discussionusers.users,discussionusers.users.image,categories,regions,saved'
      })
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
        this.topic = topic;

        if (this.topic.responsesCount > 0) {
          this.loadResponsesData(this.topic.firstMessage.id);
        } else {
          this.responses = [];
        }

        this.titleService.setTitle(topic.subject);
      });
  }

  deleteComment(index: number): void {
    this.topic.responsesCount = this.topic.responsesCount - (this.responses[index].repliesCount + 1);
    this.responses.splice(index, 1);
  }

  updateCommentsCount(isDeleteAction: boolean): void {
    this.topic.responsesCount += isDeleteAction ? -1 : 1;
  }

  goBack() {
    this.commonService.goBack();
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

  private listenToUserFollow(): void {
    this.followingService
      .followedUser()
      .pipe(untilDestroyed(this))
      .subscribe(() => {
        if (this.topic.firstMessage?.user?.id) {
          this.topic.firstMessage.user.isFollowed = !this.topic.firstMessage.user.isFollowed;
        }

        this.loadResponsesData(this.topic.firstMessage.id);
      });
  }

  private listenToDiscussionFollow(): void {
    this.followingService
      .followedForumDiscussion()
      .pipe(untilDestroyed(this))
      .subscribe(() => {
        this.topic.isFollowed = !this.topic.isFollowed;
      });
  }

  editDiscussion(topic: ForumTopicInterface) {
    this.router.navigate(['forum/edit-discussion/' + topic.id]);
  }

  getPrivateUsers(
    currentUser: UserInterface,
    users: ForumUserResponseInterface[],
    from: number,
    to: number
  ): ForumUserResponseInterface[] {
    return users?.filter(user => user.statusId === UserStatusEnum.Active).slice(from, to);
  }

  getPrivateUser(currentUser: UserInterface, users: ForumUserResponseInterface[]): ForumUserResponseInterface {
    return users?.filter(user => user.id !== currentUser.id)[0];
  }
  //#endregion
}
