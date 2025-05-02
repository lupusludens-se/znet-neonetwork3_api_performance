import { Component, Input } from '@angular/core';
import { NotificationInterface } from '../../../../../interfaces/notification.interface';
import { NotificationTypeEnum } from '../../../../../enums/notification-type.enum';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { NotificationsService } from '../../../../../../core/services/notifications.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'neo-user-comment-topic',
  templateUrl: './user-topic.component.html'
})
export class UserTopicComponent {
  @Input() truncateName: boolean;

  get notification(): NotificationInterface {
    return this.innerNotification;
  }

  @Input() set notification(value: NotificationInterface) {
    if (value) {
      this.innerNotification = value;

      if (this.truncateName) {
        this.truncateNotificationText(value);
      }
    }
  }

  private innerNotification: NotificationInterface;

  notificationType = NotificationTypeEnum;
  readonly userStatuses = UserStatusEnum;

  constructor(
    private readonly notificationsService: NotificationsService,
    private readonly translateService: TranslateService
  ) {}

  private truncateNotificationText(value: NotificationInterface): void {
    if (value?.details?.userName) {
      let staticTextLength;

      switch (this.innerNotification.type as any) {
        case this.notificationType.CommentsMyTopic:
          staticTextLength = this.translateService.instant('notificationType.commentsMyTopicLabel')?.length || 0;
          break;
        case this.notificationType.LikesMyTopic:
          staticTextLength = this.translateService.instant('notificationType.likesMyTopicLabel')?.length || 0;
          break;
        case this.notificationType.RepliesToMyComment:
          staticTextLength = this.translateService.instant('notificationType.repliedToMyCommentLabel')?.length || 0;
          break;
        case this.notificationType.RepliesToTopicIFollow:
          staticTextLength = this.translateService.instant('notificationType.repliedToTopicIFollowLabel')?.length || 0;
          break;
        case this.notificationType.MentionsMeInComment:
          staticTextLength = this.translateService.instant('notificationType.mentionsMeInCommentLabel')?.length || 0;
          break;
        case this.notificationType.UserRegistered:
          staticTextLength = this.translateService.instant('notificationType.userRegisteredLabel')?.length || 0;
          break;
      }

      let wholeTextLength =
        value.details.userName.length + (value?.details?.topicTitle?.length || 0) + staticTextLength;

      this.innerNotification.details.userName = this.notificationsService.getTruncatedName(
        this.innerNotification.details.userName,
        wholeTextLength,
        staticTextLength
      );
    }
  }
}
