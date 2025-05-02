import { SvgIconsModule } from '@ngneat/svg-icon';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { GeographicInterestComponent } from './geographic-interest.component';
import { FilterHeaderModule } from '../filter-header/filter-header.module';
import { TranslateModule } from '@ngx-translate/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BlueCheckboxModule } from '../blue-checkbox/blue-checkbox.module';

@NgModule({
  declarations: [GeographicInterestComponent],
  imports: [CommonModule, ReactiveFormsModule, SvgIconsModule, FilterHeaderModule, TranslateModule, BlueCheckboxModule],
  exports: [GeographicInterestComponent]
})
export class GeographicInterestModule {}
