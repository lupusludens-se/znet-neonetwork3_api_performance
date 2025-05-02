import { CommonModule } from '@angular/common';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { NgModule } from '@angular/core';
import { SortDropdownComponent } from './sort-dropdown.component';

@NgModule({
  declarations: [SortDropdownComponent],
  imports: [CommonModule, SvgIconsModule],
  exports: [SortDropdownComponent]
})
export class SortDropdownModule {}
