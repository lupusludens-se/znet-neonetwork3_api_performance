import { ReactiveFormsModule } from '@angular/forms';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { NgModule } from '@angular/core';

import { BlueCheckboxModule } from '../../../shared/modules/blue-checkbox/blue-checkbox.module';
import { FilterHeaderModule } from '../../../shared/modules/filter-header/filter-header.module';
import { EventUsersComponent } from './event-users.component';
import { SharedModule } from '../../../shared/shared.module';

@NgModule({
  declarations: [EventUsersComponent],
  exports: [EventUsersComponent],
  imports: [SharedModule, ReactiveFormsModule, SvgIconsModule, BlueCheckboxModule, FilterHeaderModule]
})
export class EventUsersModule {}
