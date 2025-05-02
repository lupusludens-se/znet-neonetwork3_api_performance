import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { ContentLinksComponent } from './content-links.component';
import { SharedModule } from '../../../shared/shared.module';
import { TextInputModule } from '../../../shared/modules/controls/text-input/text-input.module';
import { ControlErrorModule } from '../../../shared/modules/controls/control-error/control-error.module';

@NgModule({
  declarations: [ContentLinksComponent],
  exports: [ContentLinksComponent],
  imports: [SharedModule, FormsModule, ReactiveFormsModule, TextInputModule, ControlErrorModule]
})
export class ContentLinksModule {}
