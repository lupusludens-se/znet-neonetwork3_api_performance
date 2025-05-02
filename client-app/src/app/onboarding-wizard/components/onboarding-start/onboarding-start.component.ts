import { Component, Input } from '@angular/core';

import { OnboardingWizardService } from '../../services/onboarding-wizard.service';
import { OnboardingStepsEnum } from '../../enums/onboarding-steps.enum';

@Component({
  selector: 'neo-onboarding-start',
  templateUrl: './onboarding-start.component.html',
  styleUrls: ['./onboarding-start.component.scss']
})
export class OnboardingStartComponent {
  @Input() corporationFlow: boolean;
  stepsList = OnboardingStepsEnum;

  constructor(public onboardingWizardService: OnboardingWizardService) {}

  startOnboarding(): void {
    this.onboardingWizardService.changeStep(this.stepsList.Role);
  }
}
