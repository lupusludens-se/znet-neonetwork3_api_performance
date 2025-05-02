// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { SharedModule } from '../../../shared.module';

// components
import { DropdownComponent } from './dropdown.component';
import { ControlErrorModule } from '../control-error/control-error.module';

@NgModule({
  declarations: [DropdownComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, SvgIconsModule, SharedModule, ControlErrorModule],
  exports: [DropdownComponent]
})
export class DropdownModule {}
