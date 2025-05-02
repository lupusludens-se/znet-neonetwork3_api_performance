import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { Component } from '@angular/core';

import { OnboardingWizardService } from '../../services/onboarding-wizard.service';
import { OnboardingStepsEnum } from '../../enums/onboarding-steps.enum';
import { USER_RESPONSIBILITIES } from '../../../user-management/constants/responsibility.const';

@Component({
  selector: 'neo-onboarding-responsibilities',
  templateUrl: 'onboarding-responsibilities.component.html',
  styleUrls: ['../../onboarding-wizard.component.scss', 'onboarding-responsibilities.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class OnboardingResponsibilitiesComponent {
  stepsList = OnboardingStepsEnum;
  responsibilitiesList = USER_RESPONSIBILITIES;
  formSubmitted: boolean;

  constructor(public controlContainer: ControlContainer, private onboardingWizardService: OnboardingWizardService) {}

  goForward(step: OnboardingStepsEnum): void {
    this.formSubmitted = true;
    if (!this.controlContainer.control!.get('responsibilityId')!.value) return;

    this.changeStep(step);
  }

  patchForm(value: number) {
    this.controlContainer.control!.get('responsibilityId')!.patchValue(value);
  }

  changeStep(step: OnboardingStepsEnum) {
    this.onboardingWizardService.changeStep(step);
  }
}
