import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ConversationUserInterface } from 'src/app/+messages/interfaces/conversation-user.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { DiscussionSourceTypeEnum } from 'src/app/shared/enums/discussion-source-type.enum';
import { InitiativeMessageInterface } from '../../models/initiative-resources.interface';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { LocationStrategy } from '@angular/common';

@Component({
  selector: 'neo-message-item',
  templateUrl: './message-item.component.html',
  styleUrls: ['./message-item.component.scss']
})
export class MessageItemComponent {
  @Input() currentUser: UserInterface;
  @Input() isSavedMessage: boolean = false;
  @Input() message: InitiativeMessageInterface;
  @Input() initiativeId: number;
  @Output() selectedMessage = new EventEmitter<InitiativeMessageInterface>();

  userStatuses = UserStatusEnum;
  showDeleteModal: boolean = false;
  messageLength: number = 71;
  discussionSourceType = DiscussionSourceTypeEnum;
  options: MenuOptionInterface[] = [
    {
      icon: 'trash-can-red',
      name: 'initiative.viewInitiative.deleteSavedContentLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];
  isUserClickEvent: boolean;

  constructor(private readonly activityService: ActivityService, private router: Router,
    private locationStrategy: LocationStrategy) { }

  openMessage = (event: Event): void => {
    event.stopPropagation();
    this.trackMessageActivity();
    window.open(`messages/${this.message.id}`, '_blank');
  };

  optionClick = (): void => {
    if (!this.isUserClickEvent) {
      this.selectedMessage.emit(this.message);
    }
    else {
      this.isUserClickEvent = false;
    }
  };

  trackMessageActivity = (): void => {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.MessageDetailsView, {
        messageId: this.message.id,
        initiativeId: this.initiativeId
      })
      ?.subscribe();
  };

  getLastMessageUser = (currentUser: UserInterface, conversation: InitiativeMessageInterface): string => {
    const lastConversationUserId = conversation?.lastMessage?.user?.id;
    if (conversation?.lastMessage?.user?.statusId === this.userStatuses.Deleted) {
      return 'Deleted User';
    } else if (currentUser?.id === lastConversationUserId) {
      return 'You';
    } else {
      return conversation?.lastMessage?.user?.name || '';
    }
  };

  conversationUser = (currentUser: UserInterface, users: ConversationUserInterface[]): ConversationUserInterface => {
    return users.find(user => user.id !== currentUser?.id) || ({} as ConversationUserInterface);
  };

  conversationUsers = (
    currentUser: UserInterface,
    users: ConversationUserInterface[],
    from: number,
    to: number
  ): ConversationUserInterface[] => {
    return users.filter(user => user.id !== currentUser?.id).slice(from, to);
  };

  getUser = (user: ConversationUserInterface): UserInterface => {
    const [firstName, lastName] = user.name.split(' ');
    return {
      image: user?.statusId === UserStatusEnum.Deleted ? null : user?.image,
      imageName: user?.statusId === UserStatusEnum.Deleted ? '' : user?.image?.name,
      firstName: user?.statusId === UserStatusEnum.Deleted ? 'Deleted' : firstName,
      lastName: user?.statusId === UserStatusEnum.Deleted ? 'User' : lastName
    } as UserInterface;
  };

  openUserProfile(user?: ConversationUserInterface): void {
    this.isUserClickEvent = true;
    if (!user || user.statusId === UserStatusEnum.Deleted) {
      return;
    }
    const getBaseHref = location.origin + this.locationStrategy.getBaseHref();
    const serializedUrl = getBaseHref + this.router.serializeUrl(this.router.createUrlTree(['user-profile/'])) + "/" + user.id;
    window.open(serializedUrl, '_blank');
  }
}
