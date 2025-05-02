import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProgressStepperComponent } from './progress-stepper.component';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { SharedModule } from '../../shared.module';

@NgModule({
  declarations: [ProgressStepperComponent],
  imports: [CommonModule, SharedModule, SvgIconsModule],
  exports: [ProgressStepperComponent]
})
export class ProgressStepperModule {}
