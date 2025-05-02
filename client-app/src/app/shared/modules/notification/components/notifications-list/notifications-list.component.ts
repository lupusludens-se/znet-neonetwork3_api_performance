import { Component, EventEmitter, Input, Output } from '@angular/core';

import { NotificationInterface } from '../../../../interfaces/notification.interface';
import { NotificationTypeEnum } from '../../../../enums/notification-type.enum';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'neo-notifications-list',
  templateUrl: './notifications-list.component.html',
  styleUrls: ['./notifications-list.component.scss'],
  providers: [DatePipe]
})
export class NotificationsListComponent {
  @Input() notifications: NotificationInterface[];
  @Input() showWrapper: boolean;
  @Input() shortVersion: boolean;
  @Input() spaceBetween: string = 'mt-20';

  @Output() notificationClick: EventEmitter<NotificationInterface> = new EventEmitter<NotificationInterface>();

  notificationType = NotificationTypeEnum;
}
