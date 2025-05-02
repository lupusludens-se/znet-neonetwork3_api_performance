import { Component, Input } from '@angular/core';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { ConversationUserInterface } from '../../interfaces/conversation-user.interface';
import { ConversationInterface } from '../../interfaces/conversation.interface';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { Router } from '@angular/router';

@Component({
  selector: 'neo-message-user-detail',
  templateUrl: './message-user-detail.component.html',
  styleUrls: ['./message-user-detail.component.scss']
})
export class MessageUserDetailComponent {
  @Input() conversation: ConversationInterface;
  @Input() currentuser: UserInterface;
  @Input() direction: string;
  userStatuses = UserStatusEnum;
  @Input() selectedTab: 'inbox' | 'network' = 'network';

  constructor(private readonly router: Router) { }

  conversationUser(
    currentUser: UserInterface,
    users: ConversationUserInterface[],
    selectedTab: string,
    direction: string
  ): ConversationUserInterface {
    if (selectedTab === 'network' && direction === 'from') {
      let length = users.length;
      return users[length - 1];
    } else if (selectedTab === 'network' && direction === 'to') {
      return users[0];
    } else {
      return users.filter(user => user.id !== currentUser.id)[0];
    }
  }

  conversationUsers(
    currentUser: UserInterface,
    users: ConversationUserInterface[],
    from: number,
    to: number
  ): ConversationUserInterface[] {
    if (this.selectedTab === 'network') {
      users = users.slice(0, users.length - 1);
    }

    return users.filter(user => user.id !== currentUser.id).slice(from, to);
  }

  getUser(user: ConversationUserInterface): UserInterface {
    const userName = user.name.split(' ');

    return {
      image: user?.statusId === UserStatusEnum.Deleted ? null : user?.image,
      imageName: user?.statusId === UserStatusEnum.Deleted ? '' : user?.image?.name,
      firstName: user?.statusId === UserStatusEnum.Deleted ? 'Deleted' : userName[0],
      lastName: user?.statusId === UserStatusEnum.Deleted ? 'User' : userName[1]
    } as UserInterface;
  }


	openUserProfile(user?: ConversationUserInterface): void {
    if (!user || user.statusId === UserStatusEnum.Deleted) {
      return;
    }

    this.router.navigateByUrl('/user-profile/' + user.id);
  }
}
