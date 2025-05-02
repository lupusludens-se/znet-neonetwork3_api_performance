import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NoResultsComponent } from './no-results.component';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [NoResultsComponent],
  imports: [CommonModule, TranslateModule],
  exports: [NoResultsComponent]
})
export class NoResultsModule {}
