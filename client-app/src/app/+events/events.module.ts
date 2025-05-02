// modules
import { MonthService, ScheduleModule } from '@syncfusion/ej2-angular-schedule';
import { EventsRoutingModule } from './events-routing.module';
import { SharedModule } from '../shared/shared.module';
import { NgModule } from '@angular/core';
import { UserAvatarModule } from '../shared/modules/user-avatar/user-avatar.module';
import { ContentTagModule } from '../shared/modules/content-tag/content-tag.module';
import { ModalModule } from '../shared/modules/modal/modal.module';
import { MembersListModule } from '../shared/modules/members-list/members-list.module';
import { PaginationModule } from '../shared/modules/pagination/pagination.module';
import { NoResultsModule } from '../shared/modules/no-results/no-results.module';
import { EmptyPageModule } from '../shared/modules/empty-page/empty-page.module';

// components
import { EventDateTimeComponent } from './component/events-list/event-date-time/event-date-time.component';
import { EventItemComponent } from './component/events-list/event-item/event-item.component';
import { EventsListComponent } from './component/events-list/events-list.component';
import { SchedulerComponent } from './component/schedule/schedule.component';
import { EventsComponent } from './events.component';
import { EventViewComponent } from './component/event-view/event-view.component';
import { EventsSliderComponent } from './component/events-slider/events-slider.component';

// services
import { EventService } from './services/event.service';
import { UserProfileService } from '../shared/services/user.service';
import { EventsPublicListComponent } from './component/events-public-list/events-public-list.component';
import { PublicEventsBannerComponent } from './component/public-events-banner/public-events-banner.component';
import { PublicModule } from '../public/public.module';

@NgModule({
  declarations: [
    EventsComponent,
    SchedulerComponent,
    EventsListComponent,
    EventItemComponent,
    EventDateTimeComponent,
    EventViewComponent,
    EventsSliderComponent,
    EventsPublicListComponent,
    PublicEventsBannerComponent
  ],
  imports: [
    SharedModule,
    EventsRoutingModule,
    ScheduleModule,
    UserAvatarModule,
    ContentTagModule,
    ModalModule,
    MembersListModule,
    PaginationModule,
    NoResultsModule,
    EmptyPageModule,
    PublicModule
  ],
  providers: [MonthService, EventService, UserProfileService]
})
export class EventsModule {}
