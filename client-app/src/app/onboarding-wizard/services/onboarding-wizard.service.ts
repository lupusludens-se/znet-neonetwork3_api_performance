import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { OnboardingStepsEnum } from '../enums/onboarding-steps.enum';

@Injectable()
export class OnboardingWizardService {
  private currentStep: BehaviorSubject<string> = new BehaviorSubject<string>(OnboardingStepsEnum.Start);
  currentStep$: Observable<string> = this.currentStep.asObservable();

  changeStep(section: string): void {
    this.currentStep.next(section);
  }
}
