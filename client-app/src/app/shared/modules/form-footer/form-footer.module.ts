// modules
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { SharedModule } from '../../shared.module';

// components
import { FormFooterComponent } from './form-footer.component';

@NgModule({
  declarations: [FormFooterComponent],
  exports: [FormFooterComponent],
  imports: [TranslateModule, SharedModule]
})
export class FormFooterModule {}
