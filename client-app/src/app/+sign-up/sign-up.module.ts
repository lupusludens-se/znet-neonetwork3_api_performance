// modules
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SignUpRoutingModule } from './sign-up-routing.module';
import { SharedModule } from '../shared/shared.module';
import { RadioControlModule } from '../shared/modules/radio-control/radio-control.module';
import { DropdownModule } from '../shared/modules/controls/dropdown/dropdown.module';
import { BlueCheckboxModule } from '../shared/modules/blue-checkbox/blue-checkbox.module';
import { VerticalLineDecorModule } from '../shared/modules/vertical-line-decor/vertical-line-decor.module';
import { PrivacyPolicyModule } from '../shared/modules/privacy-policy/privacy-policy.module';
import { TermOfUseModule } from '../shared/modules/term-of-use/term-of-use.module';
import { FooterModule } from '../shared/modules/footer/footer.module';

// components
import { SignUpComponent } from './sign-up.component';
import { CompleteComponent } from './components/complete/complete.component';

// providers
import { RECAPTCHA_SETTINGS, RecaptchaModule, RecaptchaSettings } from 'ng-recaptcha';
import { environment } from '../../environments/environment';
import { TextInputModule } from '../shared/modules/controls/text-input/text-input.module';
import { ControlErrorModule } from '../shared/modules/controls/control-error/control-error.module';
import { TextareaControlModule } from '../shared/modules/controls/textarea-control/textarea-control.module';

@NgModule({
  declarations: [SignUpComponent, CompleteComponent],
  imports: [
    SignUpRoutingModule,
    SharedModule,
    RadioControlModule,
    ReactiveFormsModule,
    DropdownModule,
    BlueCheckboxModule,
    VerticalLineDecorModule,
    PrivacyPolicyModule,
    TermOfUseModule,
    FormsModule,
    RecaptchaModule,
    FooterModule,
    TextInputModule,
    ControlErrorModule,
    TextareaControlModule
  ],
  providers: [
    {
      provide: RECAPTCHA_SETTINGS,
      useValue: {
        siteKey: environment.recaptcha.siteKey
      } as RecaptchaSettings
    }
  ]
})
export class SignUpModule {}
