import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ContentLocationComponent } from './content-location.component';
import { DotDecorModule } from '../dot-decor/dot-decor.module';
import { PublicModule } from 'src/app/public/public.module';

@NgModule({
  declarations: [ContentLocationComponent],
  imports: [CommonModule, DotDecorModule, PublicModule],
  exports: [ContentLocationComponent]
})
export class ContentLocationModule {}
