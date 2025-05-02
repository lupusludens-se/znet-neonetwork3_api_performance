// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

// components
import { VerticalLineDecorComponent } from './vertical-line-decor.component';

@NgModule({
  declarations: [VerticalLineDecorComponent],
  imports: [SharedModule],
  exports: [VerticalLineDecorComponent]
})
export class VerticalLineDecorModule {}
