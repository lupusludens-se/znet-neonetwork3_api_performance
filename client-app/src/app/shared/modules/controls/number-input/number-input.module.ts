import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { NumberInputComponent } from './number-input.component';
import { ControlErrorModule } from '../control-error/control-error.module';
import { NgxMaskModule } from 'ngx-mask';

@NgModule({
  imports: [CommonModule, FormsModule, ControlErrorModule, NgxMaskModule.forChild()],
  declarations: [NumberInputComponent],
  exports: [NumberInputComponent]
})
export class NumberInputModule {}
