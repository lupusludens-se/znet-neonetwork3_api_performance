import { CalendarModule } from '@syncfusion/ej2-angular-calendars';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { SquareRadioControlModule } from '../../../shared/modules/square-radio-control/square-radio-control.module';
import { GeographicInterestModule } from '../../../shared/modules/geographic-interest/geographic-interest.module';
import { InterestsTopicModule } from '../../../shared/modules/interests-topic/interests-topic.module';
import { CreateEventFormComponent } from './components/create-event-form/create-event-form.component';
import { EventSidePanelComponent } from './components/event-side-panel/event-side-panel.component';
import { DropdownModule } from '../../../shared/modules/controls/dropdown/dropdown.module';
import { ContentTagModule } from '../../../shared/modules/content-tag/content-tag.module';
import { FormFooterModule } from '../../../shared/modules/form-footer/form-footer.module';
import { EventInviteComponent } from './components/event-invite/event-invite.component';
import { ContentLinksModule } from '../content-links/content-links.module';
import { RegionsService } from '../../../shared/services/regions.service';
import { EventUsersModule } from '../event-users/event-users.module';
import { EventDatesModule } from '../event-dates/event-dates.module';
import { CreateEventService } from './services/create-event.service';
import { HighlightsModule } from '../highlights/highlights.module';
import { ModeratorsModule } from '../moderators/moderators.module';
import { CreateEventComponent } from './create-event.component';
import { TagsStepModule } from '../tags-step/tags-step.module';
import { SharedModule } from '../../../shared/shared.module';
import { TextInputModule } from '../../../shared/modules/controls/text-input/text-input.module';
import { TextareaControlModule } from '../../../shared/modules/controls/textarea-control/textarea-control.module';
import { ControlErrorModule } from '../../../shared/modules/controls/control-error/control-error.module';
import { EventRegionsModule } from '../../../shared/modules/event-regions/event-regions.module';
import { BlueCheckboxModule } from '../../../shared/modules/blue-checkbox/blue-checkbox.module';
import { EventsService } from '../../services/events.service';
import { RadioControlModule } from 'src/app/shared/modules/radio-control/radio-control.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: CreateEventComponent
  }
];

@NgModule({
  declarations: [CreateEventComponent, CreateEventFormComponent, EventInviteComponent, EventSidePanelComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    FormFooterModule,
    ContentTagModule,
    DropdownModule,
    SquareRadioControlModule,
    FormsModule,
    GeographicInterestModule,
    InterestsTopicModule,
    CalendarModule,
    HighlightsModule,
    TagsStepModule,
    ModeratorsModule,
    ContentLinksModule,
    EventDatesModule,
    EventUsersModule,
    TextInputModule,
    TextareaControlModule,
    ControlErrorModule,
    EventRegionsModule,
    BlueCheckboxModule,
    RadioControlModule,
    ModalModule
  ],
  providers: [CreateEventService, EventsService, RegionsService]
})
export class CreateEventModule {}
