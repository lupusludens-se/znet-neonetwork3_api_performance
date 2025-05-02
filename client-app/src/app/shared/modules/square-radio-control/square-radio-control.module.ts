// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// components
import { SquareRadioControlComponent } from './component/square-radio-control/square-radio-control.component';

@NgModule({
  declarations: [SquareRadioControlComponent],
  exports: [SquareRadioControlComponent],
  imports: [CommonModule]
})
export class SquareRadioControlModule {}
