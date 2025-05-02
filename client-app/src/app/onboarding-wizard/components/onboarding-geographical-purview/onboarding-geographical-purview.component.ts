import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { ChangeDetectionStrategy, Component } from '@angular/core';

import { OnboardingWizardService } from '../../services/onboarding-wizard.service';
import { RegionsService } from '../../../shared/services/regions.service';
import { OnboardingStepsEnum } from '../../enums/onboarding-steps.enum';
import { TagInterface } from '../../../core/interfaces/tag.interface';

@Component({
  selector: 'neo-onboarding-geographical-purview',
  templateUrl: 'onboarding-geographical-purview.component.html',
  styleUrls: ['../../onboarding-wizard.component.scss', 'onboarding-geographical-purview.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class OnboardingGeographicalPurviewComponent {
  stepsList = OnboardingStepsEnum;
  selectAll: boolean;
  formSubmitted: boolean;

  constructor(
    public controlContainer: ControlContainer,
    private onboardingWizardService: OnboardingWizardService,
    public regionsService: RegionsService
  ) {}

  goForward(step: OnboardingStepsEnum): void {
    this.formSubmitted = true;

    if (this.controlContainer.control?.get('regions').invalid) return;

    this.changeStep(step);
  }

  updateForm(selectedRegions: TagInterface[]): void {
    this.controlContainer.control?.get('regions')?.patchValue(selectedRegions);
  }

  changeStep(step: OnboardingStepsEnum) {
    this.onboardingWizardService.changeStep(step);
  }
}
