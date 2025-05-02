// modules
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { LandingRoutingModule } from './landing-routing.module';
import { VerticalLineDecorModule } from '../shared/modules/vertical-line-decor/vertical-line-decor.module';
import { ModalModule } from '../shared/modules/modal/modal.module';
import { PrivacyPolicyModule } from '../shared/modules/privacy-policy/privacy-policy.module';
import { TermOfUseModule } from '../shared/modules/term-of-use/term-of-use.module';

// components
import { LandingComponent } from './landing.component';
import { OpportunitiesComponent } from './components/opportunities/opportunities.component';
import { SolutionsComponent } from './components/solutions/solutions.component';
import { ClientsCarouselComponent } from './components/clients-carousel/clients-carousel.component';
import { ClientsFeedbackComponent } from './components/clients-feedback/clients-feedback.component';
import { ContactComponent } from './components/contact/contact.component';
import { CookiesComponent } from './components/cookies/cookies.component';
import { FooterModule } from '../shared/modules/footer/footer.module';

// providers
import { RECAPTCHA_SETTINGS, RecaptchaModule, RecaptchaSettings } from 'ng-recaptcha';
import { environment } from '../../environments/environment';

@NgModule({
  declarations: [
    LandingComponent,
    OpportunitiesComponent,
    SolutionsComponent,
    ClientsCarouselComponent,
    ClientsFeedbackComponent,
    ContactComponent,
    CookiesComponent
  ],
  imports: [
    SharedModule,
    LandingRoutingModule,
    VerticalLineDecorModule,
    PrivacyPolicyModule,
    ModalModule,
    ReactiveFormsModule,
    TermOfUseModule,
    FooterModule,
    RecaptchaModule
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
export class LandingModule {}
