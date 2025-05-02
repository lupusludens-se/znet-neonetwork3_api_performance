import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { EditEventCategoriesComponent } from './components/edit-event-categories/edit-event-categories.component';
import { GeographicInterestModule } from '../../../shared/modules/geographic-interest/geographic-interest.module';
import { InterestsTopicModule } from '../../../shared/modules/interests-topic/interests-topic.module';
import { SectionsPanelComponent } from './components/sections-panel/sections-panel.component';
import { FormFooterModule } from '../../../shared/modules/form-footer/form-footer.module';
import { CreateEventService } from '../+create-event/services/create-event.service';
import { ContentLinksModule } from '../content-links/content-links.module';
import { ModalModule } from '../../../shared/modules/modal/modal.module';
import { EventsService } from '../../services/events.service';
import { EventDatesModule } from '../event-dates/event-dates.module';
import { EventUsersModule } from '../event-users/event-users.module';
import { HighlightsModule } from '../highlights/highlights.module';
import { ModeratorsModule } from '../moderators/moderators.module';
import { EditEventService } from './services/edit-event.service';
import { TagsStepModule } from '../tags-step/tags-step.module';
import { SharedModule } from '../../../shared/shared.module';
import { EditEventComponent } from './edit-event.component';
import { TextInputModule } from '../../../shared/modules/controls/text-input/text-input.module';
import { TextareaControlModule } from '../../../shared/modules/controls/textarea-control/textarea-control.module';
import { ControlErrorModule } from '../../../shared/modules/controls/control-error/control-error.module';
import { EventRegionsModule } from '../../../shared/modules/event-regions/event-regions.module';
import { BlueCheckboxModule } from '../../../shared/modules/blue-checkbox/blue-checkbox.module';
import { EventTemplateModalComponent } from './components/event-template-modal/event-template-modal.component';
import { RadioControlModule } from 'src/app/shared/modules/radio-control/radio-control.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: EditEventComponent
  }
];

@NgModule({
  declarations: [EditEventComponent, SectionsPanelComponent, EditEventCategoriesComponent, EventTemplateModalComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    FormFooterModule,
    ReactiveFormsModule,
    EventDatesModule,
    ContentLinksModule,
    HighlightsModule,
    ModeratorsModule,
    TagsStepModule,
    GeographicInterestModule,
    InterestsTopicModule,
    EventUsersModule,
    ModalModule,
    TextInputModule,
    TextareaControlModule,
    ControlErrorModule,
    EventRegionsModule,
    BlueCheckboxModule,
    RadioControlModule
  ],
  providers: [EventsService, CreateEventService, EditEventService]
})
export class EditEventModule {}
