import { NgModule } from '@angular/core';

import { SharedModule } from '../../../shared/shared.module';
import { TagsStepComponent } from './tags-step.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ControlErrorModule } from '../../../shared/modules/controls/control-error/control-error.module';

@NgModule({
  declarations: [TagsStepComponent],
  exports: [TagsStepComponent],
  imports: [SharedModule, ReactiveFormsModule, ControlErrorModule]
})
export class TagsStepModule {}
