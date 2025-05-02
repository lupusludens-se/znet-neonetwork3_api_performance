import { Component, Input } from '@angular/core';
import { NotificationInterface } from '../../../../../interfaces/notification.interface';
import { NotificationsService } from '../../../../../../core/services/notifications.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'neo-deleted-user',
  templateUrl: './deleted-user.component.html'
})
export class DeletedUserComponent {
  @Input() showWrapper: boolean;
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

  constructor(
    private readonly notificationsService: NotificationsService,
    private readonly translateService: TranslateService
  ) {}

  private truncateNotificationText(value: NotificationInterface): void {
    if (value?.details?.userName) {
      const staticTextLength = this.translateService.instant('notificationType.deletedAccountLabel')?.length || 0;
      const wholeTextLength = value.details.userName.length + staticTextLength;

      this.innerNotification.details.userName = this.notificationsService.getTruncatedName(
        this.innerNotification.details.userName,
        wholeTextLength,
        staticTextLength
      );
    }
  }
}
