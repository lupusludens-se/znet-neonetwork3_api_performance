import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InitiativeGeographicScaleComponent } from './initiative-geographic-scale.component';
import { TranslateModule } from '@ngx-translate/core';
import { BlueCheckboxModule } from 'src/app/shared/modules/blue-checkbox/blue-checkbox.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { FilterHeaderModule } from 'src/app/shared/modules/filter-header/filter-header.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [InitiativeGeographicScaleComponent],
  imports: [CommonModule, ReactiveFormsModule, TranslateModule, BlueCheckboxModule, SharedModule, FilterHeaderModule],
  exports: [InitiativeGeographicScaleComponent]
})
export class InitiativeGeographicScaleModule {}
