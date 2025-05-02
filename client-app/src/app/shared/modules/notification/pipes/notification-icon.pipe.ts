import { Pipe, PipeTransform } from '@angular/core';
import { NotificationTypeEnum } from '../../../enums/notification-type.enum';

@Pipe({
  name: 'icon'
})
export class NotificationIconPipe implements PipeTransform {
  transform(value: NotificationTypeEnum): string {
    switch (value) {
      case NotificationTypeEnum.CommentsMyTopic:
        return 'forum';
      case NotificationTypeEnum.LikesMyTopic:
        return 'forum';
      case NotificationTypeEnum.RepliesToMyComment:
        return 'forum';
      case NotificationTypeEnum.RepliesToTopicIFollow:
        return 'forum';
      case NotificationTypeEnum.MentionsMeInComment:
        return 'forum';
      case NotificationTypeEnum.InvitesMeToEvent:
        return 'calender';
      case NotificationTypeEnum.ChangesToEventInvited:
        return 'calender';
      case NotificationTypeEnum.FollowsMe:
        return 'community';
      case NotificationTypeEnum.MessagesMe:
        return 'lines-in-message';
      case NotificationTypeEnum.AdminAlert:
        return 'alert';
      case NotificationTypeEnum.ChangesToProjectISaved:
        return 'projects';
      case NotificationTypeEnum.CompanyIFollowPostProject:
        return 'projects';
      case NotificationTypeEnum.UserRegistered:
        return 'account';
      case NotificationTypeEnum.CompanyIFollowAddEmployee:
        return 'community';
      case NotificationTypeEnum.UserDeleted:
        return 'alert';
      case NotificationTypeEnum.NewForumCreated:
        return 'forum';
      case NotificationTypeEnum.NewPrivateForumCreated:
        return 'forum';
      case NotificationTypeEnum.UserAutoApproved:
        return 'account';
      case NotificationTypeEnum.ContactZeigoNetwork:
        return 'communication-bubble';
      case NotificationTypeEnum.NewFeedback:
        return 'feedback-menu';
      case NotificationTypeEnum.NewInitiativeCreated:
        return 'initiative-notification';
    }
  }
}
