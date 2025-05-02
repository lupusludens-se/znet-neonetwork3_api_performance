import { ControlContainer, FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';

import { OnboardingWizardService } from '../../services/onboarding-wizard.service';
import { OnboardingStepsEnum, YESNODATA } from '../../enums/onboarding-steps.enum';
import { CommonService } from '../../../core/services/common.service';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'neo-onboarding-decarbonization-solutions',
  templateUrl: './onboarding-decarbonization-solutions.component.html',
  styleUrls: ['../../onboarding-wizard.component.scss', './onboarding-decarbonization-solutions.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OnboardingDecarbonizationSolutionsComponent {
  stepsList = OnboardingStepsEnum;
  formSubmitted: boolean;
  privacy: boolean;
  showErr: boolean = false;
  yesNoData: TagInterface[] = YESNODATA;
  subscribeToWelcomeSeriesVal: number = 1;

  constructor(public controlContainer: ControlContainer, public onboardingWizardService: OnboardingWizardService) { }

  goForward(step: OnboardingStepsEnum): void {
    this.formSubmitted = true;
    if (this.controlContainer.control.get('linkedInUrl').invalid) return;

    if (this.subscribeToWelcomeSeriesVal == 1 && !this.privacy) {
      this.showErr = true;
      return;
    }
    this.showErr = false;
    this.changeStep(step);
  }

  changeStep(step: OnboardingStepsEnum): void {
    this.onboardingWizardService.changeStep(step);
  }

  validatePrivacy() {
    this.privacy = !this.privacy;
    this.showErr = !this.privacy;
  }

  change(tag: TagInterface) {
    this.controlContainer.control.get('subscribeToWelcomeSeries').setValue(tag.id);
    this.subscribeToWelcomeSeriesVal = tag.id;

    if (this.subscribeToWelcomeSeriesVal == 2) {
      this.showErr = false;
      this.privacy = false;
    }
  }
}
