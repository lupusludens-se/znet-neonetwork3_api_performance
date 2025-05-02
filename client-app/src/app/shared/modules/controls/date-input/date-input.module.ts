import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ControlErrorModule } from '../control-error/control-error.module';
import { DateInputComponent } from './date-input.component';

@NgModule({
  declarations: [DateInputComponent],
  exports: [DateInputComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, ControlErrorModule]
})
export class DateInputModule {}
