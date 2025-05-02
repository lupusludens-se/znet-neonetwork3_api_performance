import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { PaginationComponent } from './pagination.component';
import { SvgIconsModule } from '@ngneat/svg-icon';

@NgModule({
  declarations: [PaginationComponent],
  imports: [CommonModule, SvgIconsModule],
  exports: [PaginationComponent]
})
export class PaginationModule {}
