import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ContentTagComponent } from './content-tag.component';
import { PublicModule } from 'src/app/public/public.module';

@NgModule({
  declarations: [ContentTagComponent],
  imports: [CommonModule, PublicModule],
  exports: [ContentTagComponent]
})
export class ContentTagModule {}
