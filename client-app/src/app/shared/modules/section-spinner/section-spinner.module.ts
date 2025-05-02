import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SectionSpinnerComponent } from './section-spinner.component';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [SectionSpinnerComponent],
  imports: [CommonModule, TranslateModule],
  exports: [SectionSpinnerComponent]
})
export class SectionSpinnerModule {}
