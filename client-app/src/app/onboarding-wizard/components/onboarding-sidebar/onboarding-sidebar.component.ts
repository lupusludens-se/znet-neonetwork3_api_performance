import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { OnboardingWizardService } from '../../services/onboarding-wizard.service';
import { OnboardingStepsEnum } from '../../enums/onboarding-steps.enum';

@Component({
  selector: 'neo-onboarding-sidebar',
  templateUrl: './onboarding-sidebar.component.html',
  styleUrls: ['./onboarding-sidebar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OnboardingSidebarComponent {
  @Input() corporationFlow: boolean;
  stepsList = OnboardingStepsEnum;

  constructor(public onboardingWizardService: OnboardingWizardService) {}
}
