import { Component, Input } from '@angular/core';
import { NotificationInterface } from '../../../../../interfaces/notification.interface';
import { NotificationsService } from '../../../../../../core/services/notifications.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'neo-project-notification',
  templateUrl: './project.component.html'
})
export class ProjectComponent {
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
    let wholeTextLength;
    let staticTextLength;

    if (value?.details?.fieldName) {
      staticTextLength = this.translateService.instant('notificationType.changesToProjectISavedLabel')?.length || 0;
      wholeTextLength = value.details.fieldName.length + staticTextLength + (value.details.projectTitle?.length || 0);

      this.innerNotification.details.fieldName = this.notificationsService.getTruncatedName(
        this.innerNotification.details.fieldName,
        wholeTextLength,
        staticTextLength
      );
    } else if (value?.details?.projectTitle) {
      staticTextLength =
        this.translateService.instant('notificationType.multipleChangesToProjectISavedLabel')?.length || 0;
      wholeTextLength = value.details.projectTitle.length + staticTextLength;

      this.innerNotification.details.projectTitle = this.notificationsService.getTruncatedName(
        this.innerNotification.details.projectTitle,
        wholeTextLength,
        staticTextLength
      );
    }
  }
}
