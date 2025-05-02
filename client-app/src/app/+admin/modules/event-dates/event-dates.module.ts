import { CalendarModule } from '@syncfusion/ej2-angular-calendars';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { TimezonesDropdownComponent } from './components/timezones-dropdown/timezones-dropdown.component';
import { TimeRangeComponent } from './components/time-range/time-range.component';
import { EventDatesComponent } from './event-dates.component';
import { TranslateModule } from '@ngx-translate/core';
import { SharedModule } from '../../../shared/shared.module';
import { ControlErrorModule } from '../../../shared/modules/controls/control-error/control-error.module';

@NgModule({
  declarations: [EventDatesComponent, TimezonesDropdownComponent, TimeRangeComponent],
  imports: [FormsModule, ReactiveFormsModule, CalendarModule, TranslateModule, SharedModule, ControlErrorModule],
  exports: [EventDatesComponent]
})
export class EventDatesModule {}
