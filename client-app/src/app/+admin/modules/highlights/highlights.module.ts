import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { HighlightsComponent } from './highlights.component';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [HighlightsComponent],
  exports: [HighlightsComponent],
  imports: [CommonModule, ReactiveFormsModule, TranslateModule]
})
export class HighlightsModule {}
