// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { RouterModule } from '@angular/router';

// components
import { ProjectComponent } from './components/notification-types/project/project.component';
import { NotificationsListComponent } from './components/notifications-list/notifications-list.component';
import { CompanyEmployeeComponent } from './components/notification-types/company-employee/company-employee.component';
import { UserComponent } from './components/notification-types/user/user.component';
import { UserTopicComponent } from './components/notification-types/user-topic/user-topic.component';
import { MessageComponent } from './components/notification-types/message/message.component';
import { CompanyComponent } from './components/notification-types/company/company.component';
import { EventComponent } from './components/notification-types/event/event.component';
import { NotificationLoaderComponent } from './components/notification-loader/notification-loader.component';

// pipes
import { NotificationIconPipe } from './pipes/notification-icon.pipe';
import { DeletedUserComponent } from './components/notification-types/deleted-user/deleted-user.component';
import { FeedbackComponent } from './components/notification-types/feedback/feedback.component';
import { InitiativeComponent } from './components/notification-types/initiative/initiative.component';

@NgModule({
  declarations: [
    NotificationsListComponent,
    UserComponent,
    UserTopicComponent,
    MessageComponent,
    EventComponent,
    NotificationIconPipe,
    ProjectComponent,
    CompanyComponent,
    CompanyEmployeeComponent,
    NotificationLoaderComponent,
    DeletedUserComponent,
    FeedbackComponent,
    InitiativeComponent
  ],
  exports: [NotificationsListComponent, NotificationLoaderComponent, CompanyComponent],
  imports: [SharedModule, RouterModule]
})
export class NotificationModule {}
