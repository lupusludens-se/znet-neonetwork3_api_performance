import { Component, Input } from '@angular/core';

import { NotificationInterface } from '../../../../../interfaces/notification.interface';
import { NotificationTypeEnum } from '../../../../../enums/notification-type.enum';
import { CHANGE_EVENT_NOTIFICATION_DETAILS, EVENT_NOTIFICATION_DETAILS } from '../../consts/notifications.const';

@Component({
  selector: 'neo-event',
  templateUrl: './event.component.html'
})
export class EventComponent {
  @Input() notification: NotificationInterface;

  notificationType = NotificationTypeEnum;
  changeEventNotificationDetails = CHANGE_EVENT_NOTIFICATION_DETAILS;
  eventNotificationDetails = EVENT_NOTIFICATION_DETAILS;
}
