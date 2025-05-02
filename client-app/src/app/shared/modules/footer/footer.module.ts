// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VerticalLineDecorModule } from '../vertical-line-decor/vertical-line-decor.module';
import { TranslateModule } from '@ngx-translate/core';
import { PrivacyPolicyModule } from '../privacy-policy/privacy-policy.module';
import { TermOfUseModule } from '../term-of-use/term-of-use.module';

// components
import { FooterComponent } from './footer.component';

@NgModule({
  declarations: [FooterComponent],
  exports: [FooterComponent],
  imports: [CommonModule, VerticalLineDecorModule, TranslateModule, PrivacyPolicyModule, TermOfUseModule]
})
export class FooterModule {}
