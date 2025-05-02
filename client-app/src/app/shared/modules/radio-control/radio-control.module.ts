import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RadioControlComponent } from './radio-control.component';
import { ControlErrorModule } from '../controls/control-error/control-error.module';
import { SvgIconsModule } from '@ngneat/svg-icon';

@NgModule({
  declarations: [RadioControlComponent],
  imports: [CommonModule, ReactiveFormsModule, ControlErrorModule, SvgIconsModule],
  exports: [RadioControlComponent]
})
export class RadioControlModule {}
