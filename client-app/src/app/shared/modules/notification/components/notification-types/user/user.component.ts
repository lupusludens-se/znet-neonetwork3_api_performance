import { Component, Input } from '@angular/core';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';

import { NotificationInterface } from '../../../../../interfaces/notification.interface';
import { NotificationsService } from '../../../../../../core/services/notifications.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'neo-user-follows',
  templateUrl: './user.component.html'
})
export class UserComponent {
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
  readonly userStatuses = UserStatusEnum;

  constructor(
    private readonly notificationsService: NotificationsService,
    private readonly translateService: TranslateService
  ) {}

  private truncateNotificationText(value: NotificationInterface): void {
    let wholeTextLength;
    let staticTextLength;

    if (value?.details?.count) {
      staticTextLength =
        (value.details.count === 2
          ? this.translateService.instant('notificationType.twoUsersFollowsMeLabel')?.length
          : this.translateService.instant('notificationType.multipleFollowsMeLabel')?.length) || 0;
      wholeTextLength = value.details.followerName.length + +staticTextLength;
    } else {
      staticTextLength = this.translateService.instant('notificationType.followsMeLabel')?.length || 0;
      wholeTextLength = value.details.followerName.length + staticTextLength;
    }

    this.innerNotification.details.followerName = this.notificationsService.getTruncatedName(
      this.innerNotification.details.followerName,
      wholeTextLength,
      staticTextLength
    );
  }
}
