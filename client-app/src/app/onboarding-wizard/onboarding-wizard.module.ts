// modules
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { DropdownModule } from '../shared/modules/controls/dropdown/dropdown.module';
import { SharedModule } from '../shared/shared.module';
import { GeographicInterestModule } from '../shared/modules/geographic-interest/geographic-interest.module';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

// components
import { RouterModule, Routes } from '@angular/router';
import { OnboardingWizardComponent } from './onboarding-wizard.component';
import { OnboardingSidebarComponent } from './components/onboarding-sidebar/onboarding-sidebar.component';
import { OnboardingWizardService } from './services/onboarding-wizard.service';
import { WizardNavControlsComponent } from './components/wizard-nav-controls/wizard-nav-controls.component';
import { OnboardingResponsibilitiesComponent } from './components/onboarding-responsibilities/onboarding-responsibilities.component';
import { OnboardingLocationComponent } from './components/onboarding-location/onboarding-location.component';
import { OnboardingRoleComponent } from './components/onboarding-role/onboarding-role.component';
import { OnboardingGeographicalPurviewComponent } from './components/onboarding-geographical-purview/onboarding-geographical-purview.component';
import { OnboardingStartComponent } from './components/onboarding-start/onboarding-start.component';
import { OnboardingPersonalInfoComponent } from './components/onboarding-personal-info/onboarding-personal-info.component';
import { OnboardingInterestsComponent } from './components/onboarding-interests/onboarding-interests.component';
import { OnboardingFinishComponent } from './components/onboarding-finish/onboarding-finish.component';
import { RegionsService } from '../shared/services/regions.service';
import { TextInputModule } from '../shared/modules/controls/text-input/text-input.module';
import { ControlErrorModule } from '../shared/modules/controls/control-error/control-error.module';
import { OnboardingDecarbonizationSolutionsComponent } from './components/onboarding-decarbonization-solutions/onboarding-decarbonization-solutions.component';
import { RadioControlModule } from '../shared/modules/radio-control/radio-control.module';
import { BlueCheckboxModule } from '../shared/modules/blue-checkbox/blue-checkbox.module';

const routes: Routes = [{ path: '', component: OnboardingWizardComponent }];

@NgModule({
  declarations: [
    OnboardingWizardComponent,
    OnboardingRoleComponent,
    OnboardingSidebarComponent,
    WizardNavControlsComponent,
    OnboardingResponsibilitiesComponent,
    OnboardingLocationComponent,
    OnboardingGeographicalPurviewComponent,
    OnboardingStartComponent,
    OnboardingPersonalInfoComponent,
    OnboardingInterestsComponent,
    OnboardingFinishComponent,
    OnboardingDecarbonizationSolutionsComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    SvgIconsModule,
    TranslateModule,
    DropdownModule,
    SharedModule,
    GeographicInterestModule,
    TextInputModule,
    ControlErrorModule,
    RadioControlModule,
    BlueCheckboxModule
  ],
  exports: [OnboardingWizardComponent],
  providers: [OnboardingWizardService, RegionsService]
})
export class OnboardingWizardModule {}
