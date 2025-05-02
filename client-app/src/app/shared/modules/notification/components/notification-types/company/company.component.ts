import { Component, Input } from '@angular/core';

import { NotificationInterface } from '../../../../../interfaces/notification.interface';
import { NotificationsService } from '../../../../../../core/services/notifications.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'neo-company',
  templateUrl: './company.component.html'
})
export class CompanyComponent {
  @Input() truncateName: boolean;

  get notification(): NotificationInterface {
    return this.innerNotification;
  }

  @Input() set notification(value: NotificationInterface) {
    if (value) {
      this.innerNotification = value;

      if (this.truncateName && value?.details?.companyName) {
        this.truncateCompany(value);
      }
    }
  }

  private innerNotification: NotificationInterface;

  constructor(
    private readonly notificationsService: NotificationsService,
    private readonly translateService: TranslateService
  ) {}

  private truncateCompany(value: NotificationInterface): void {
    let wholeTextLength;
    let staticTextLength;

    if (value?.details?.count) {
      staticTextLength =
        (this.translateService.instant('notificationType.hasPostedLabel')?.length || 0) +
        (this.translateService.instant('notificationType.newProjectsLabel')?.length || 0);
      wholeTextLength = value.details.companyName.length + value.details.count.toString().length + staticTextLength;
    } else {
      staticTextLength = this.translateService.instant('notificationType.companyIFollowPostProjectLabel')?.length || 0;
      wholeTextLength = value.details.companyName.length + (value?.details?.projectTitle?.length || 0);
    }

    this.innerNotification.details.companyName = this.notificationsService.getTruncatedName(
      this.innerNotification.details.companyName,
      wholeTextLength,
      staticTextLength
    );
  }
}
