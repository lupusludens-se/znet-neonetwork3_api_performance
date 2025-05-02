import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { filter, Observable } from 'rxjs';

import { AuthService } from '../../../../../core/services/auth.service';

import { ForumTopicInterface } from '../../../../../+forum/interfaces/forum-topic.interface';
import { UserInterface } from '../../../../interfaces/user/user.interface';
import { TaxonomyTypeEnum } from '../../../../enums/taxonomy-type.enum';
import { SaveContentService } from '../../../../services/save-content.service';
import { HttpService } from '../../../../../core/services/http.service';
import { ForumApiEnum } from '../../../../../+forum/enums/forum-api.enum';
import { UserRoleInterface } from '../../../../interfaces/user/user-role.interface';
import { RolesEnum } from '../../../../enums/roles.enum';
import { FollowingService } from '../../../../../core/services/following.service';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { ForumComponentClickType } from '../../../../../shared/enums/forum-component-click-type.enum';
import { CoreService } from 'src/app/core/services/core.service';

@Component({
  selector: 'neo-forum-topic',
  templateUrl: 'forum-topic.component.html',
  styleUrls: ['forum-topic.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ForumTopicComponent implements OnInit {
  @Input() cssClasses: string;
  @Input() topic: ForumTopicInterface;
  @Input() showControls: boolean = true;

  @Output() deleted: EventEmitter<void> = new EventEmitter<void>();
  @Output() elementClick: EventEmitter<ForumComponentClickType> = new EventEmitter<ForumComponentClickType>();
  @Output() liked: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() saveClick: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() pinClick: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() userFollowClick: EventEmitter<boolean> = new EventEmitter<boolean>();

  type = TaxonomyTypeEnum;
  deleteModal: boolean;
  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  readonly userStatuses = UserStatusEnum;

  constructor(
    private readonly authService: AuthService,
    private readonly saveContentService: SaveContentService,
    private readonly httpService: HttpService,
    private readonly followingService: FollowingService,
    private readonly coreService: CoreService
  ) {}

  ngOnInit(): void {
    this.listenForUserFollow();
  }

  getUser(): UserInterface {
    const userName = this.topic.firstMessage.user?.name.split(' ');

    if (!userName) return;

    return {
      image:
        this.topic.firstMessage.user?.statusId === UserStatusEnum.Deleted ? null : this.topic.firstMessage.user?.image,
      firstName: this.topic.firstMessage.user?.statusId === UserStatusEnum.Deleted ? 'Deleted' : userName[0],
      lastName: this.topic.firstMessage.user?.statusId === UserStatusEnum.Deleted ? 'User' : userName[1]
    } as UserInterface;
  }

  isMember(currentUserId: number): boolean {
    if (this.topic?.isPrivate) {
      return this.topic.users.some(user => user?.id === currentUserId);
    }

    return true;
  }

  like(): void {
    if (this.topic.firstMessage.isLiked) {
      this.httpService
        .delete<unknown>(
          `${ForumApiEnum.Forum}/${this.topic.id}/${ForumApiEnum.Messages}/${this.topic.firstMessage.id}/likes`,
          null
        )
        .subscribe(() => {
          this.topic.firstMessage.isLiked = false;
          this.topic.firstMessage.likesCount -= 1;
          this.liked.emit(this.topic.firstMessage.isLiked);
        });
    } else {
      this.httpService
        .post<unknown>(
          `${ForumApiEnum.Forum}/${this.topic.id}/${ForumApiEnum.Messages}/${this.topic.firstMessage.id}/likes`,
          null
        )
        .subscribe(() => {
          this.topic.firstMessage.isLiked = true;
          this.topic.firstMessage.likesCount += 1;
          this.liked.emit(this.topic.firstMessage.isLiked);
        });
    }
  }

  save(): void {
    const methodName = this.topic.isSaved ? 'deleteForum' : 'saveForum';

    this.saveContentService[methodName](this.topic.id).subscribe(() => {
      this.topic.isSaved = !this.topic.isSaved;
      this.saveClick.emit(this.topic.isSaved);
    });
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
        this.pinClick.emit(this.topic.isPinned);
      });
  }

  isAdmin(roles: UserRoleInterface[]): boolean {
    return roles?.some(role => role.id === RolesEnum.Admin && role.isSpecial);
  }

  delete(): void {
    this.httpService.delete<unknown>(`${ForumApiEnum.Forum}/${this.topic.id}`).subscribe(() => {
      this.deleteModal = false;
      this.deleted.emit();
    });
  }

  toggleModal(): void {
    this.deleteModal = !this.deleteModal;
    if (this.deleteModal) {
      this.elementClick.emit(ForumComponentClickType.Delete);
    }
  }

  private listenForUserFollow(): void {
    this.followingService
      .followedUser()
      .pipe(filter(val => val.userId === this.topic.firstMessage.user.id))
      .subscribe(() => {
        this.topic.firstMessage.user.isFollowed = !this.topic.firstMessage.user.isFollowed;
        this.userFollowClick.emit(this.topic.firstMessage.user.isFollowed);
      });
  }
}
