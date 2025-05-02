import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ControlErrorModule } from '../control-error/control-error.module';
import { TextInputComponent } from './text-input.component';

@NgModule({
  declarations: [TextInputComponent],
  exports: [TextInputComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, ControlErrorModule]
})
export class TextInputModule {}
