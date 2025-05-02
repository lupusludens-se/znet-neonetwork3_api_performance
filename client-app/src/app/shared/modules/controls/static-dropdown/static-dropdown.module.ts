import { CommonModule } from '@angular/common';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { NgModule } from '@angular/core';
import { StaticDropdownComponent } from './static-dropdown.component';
import { BlueCheckboxModule } from '../../blue-checkbox/blue-checkbox.module';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [StaticDropdownComponent],
  imports: [CommonModule, SvgIconsModule,BlueCheckboxModule, SharedModule],
  exports: [StaticDropdownComponent]
})
export class StaticDropdownModule {}
