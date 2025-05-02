import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { OnboardingStepsEnum } from '../../enums/onboarding-steps.enum';
import { OnboardingWizardService } from '../../services/onboarding-wizard.service';

@Component({
  selector: 'neo-onboarding-role',
  templateUrl: 'onboarding-role.component.html',
  styleUrls: ['../../onboarding-wizard.component.scss', 'onboarding-role.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OnboardingRoleComponent {
  @Input() corporationFlow: boolean;
  stepsList = OnboardingStepsEnum;
  formSubmitted: boolean;

  constructor(public controlContainer: ControlContainer, public onboardingWizardService: OnboardingWizardService) {}

  changeStep(): void {
    this.formSubmitted = true;

    if (this.controlContainer.control!.get('jobTitle')!.invalid) return;

    if (this.corporationFlow) {
      this.onboardingWizardService.changeStep(this.stepsList.Responsibilities);
    } else {
      this.onboardingWizardService.changeStep(this.stepsList.Location);
    }
  }
}
