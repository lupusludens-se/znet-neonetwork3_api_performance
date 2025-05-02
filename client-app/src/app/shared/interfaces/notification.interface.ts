import { NotificationTypeEnum } from '../enums/notification-type.enum';

export interface NotificationInterface {
  id: number;
  type: NotificationTypeEnum;
  isRead: boolean;
  details: any;
}
