// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// components
import { TopPanelComponent } from './components/top-panel/top-panel.component';

@NgModule({
  declarations: [TopPanelComponent],
  exports: [TopPanelComponent],
  imports: [CommonModule]
})
export class TopPanelModule {}
