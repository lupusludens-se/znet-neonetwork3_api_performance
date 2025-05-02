// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared.module';

// components
import { ImageViewComponent } from './image-view.component';

@NgModule({
  declarations: [ImageViewComponent],
  exports: [ImageViewComponent],
  imports: [CommonModule, SharedModule]
})
export class ImageViewModule {}
