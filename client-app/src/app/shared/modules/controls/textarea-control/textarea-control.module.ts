import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ControlErrorModule } from '../control-error/control-error.module';
import { TextareaControlComponent } from './textarea-control.component';

@NgModule({
  declarations: [TextareaControlComponent],
  exports: [TextareaControlComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, ControlErrorModule]
})
export class TextareaControlModule {}
