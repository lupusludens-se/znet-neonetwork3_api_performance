// modules
import { NgModule } from '@angular/core';
import { NotificationsRoutingModule } from './notifications-routing.module';
import { SharedModule } from '../shared/shared.module';
import { NotificationModule } from '../shared/modules/notification/notification.module';

// components
import { NotificationsComponent } from './notifications.component';

@NgModule({
  declarations: [NotificationsComponent],
  imports: [NotificationsRoutingModule, SharedModule, NotificationModule]
})
export class NotificationsModule {}
