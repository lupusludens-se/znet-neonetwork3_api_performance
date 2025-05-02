import { Component, Input } from '@angular/core';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';

import { NotificationInterface } from '../../../../../interfaces/notification.interface';
import { NotificationsService } from '../../../../../../core/services/notifications.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'neo-company-employee',
  templateUrl: './company-employee.component.html'
})
export class CompanyEmployeeComponent {
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
    if (this.innerNotification.details?.userName) {
      let staticTextLength =
        this.translateService.instant('notificationType.companyIFollowAddEmployeeLabel')?.length || 0;
      let wholeTextLength = (this.innerNotification?.details?.userName?.length || 0) + staticTextLength;

      if (this.innerNotification?.details?.userStatusId !== this.userStatuses.Deleted) {
        wholeTextLength += value.details?.companyName?.length || 0;
      }

      this.innerNotification.details.userName = this.notificationsService.getTruncatedName(
        this.innerNotification.details.userName,
        wholeTextLength,
        staticTextLength
      );
    }
  }
}
