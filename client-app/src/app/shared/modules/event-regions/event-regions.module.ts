import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { BlueCheckboxModule } from '../blue-checkbox/blue-checkbox.module';
import { FilterHeaderModule } from '../filter-header/filter-header.module';
import { EventRegionsComponent } from './event-regions.component';
import { TranslateModule } from '@ngx-translate/core';
import { SvgIconsModule } from '@ngneat/svg-icon';

@NgModule({
  declarations: [EventRegionsComponent],
  exports: [EventRegionsComponent],
  imports: [CommonModule, ReactiveFormsModule, BlueCheckboxModule, FilterHeaderModule, TranslateModule, SvgIconsModule]
})
export class EventRegionsModule {}
