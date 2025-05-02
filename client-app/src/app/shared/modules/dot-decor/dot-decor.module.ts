import { DotDecorComponent } from './dot-decor.component';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

@NgModule({
  declarations: [DotDecorComponent],
  imports: [CommonModule],
  exports: [DotDecorComponent]
})
export class DotDecorModule {}
