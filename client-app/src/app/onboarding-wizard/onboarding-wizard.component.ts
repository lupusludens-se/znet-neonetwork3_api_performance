import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { filter } from 'rxjs/operators';

import { OnboardingWizardService } from './services/onboarding-wizard.service';
import { CustomValidator } from '../shared/validators/custom.validator';
import { OnboardingStepsEnum } from './enums/onboarding-steps.enum';
import { RegionsService } from '../shared/services/regions.service';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'neo-onboarding-wizard',
  templateUrl: './onboarding-wizard.component.html',
  styleUrls: ['./onboarding-wizard.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OnboardingWizardComponent implements OnInit, OnDestroy {
  form: FormGroup;
  stepsList = OnboardingStepsEnum;
  corporationFlow: boolean;
  corporationRole: number = 2;
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    public onboardingWizardService: OnboardingWizardService,
    private formBuilder: FormBuilder,
    private regionsService: RegionsService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.createForm();
    this.regionsService.getContinentsList();

    this.authService
      .currentUser()
      .pipe(
        filter(v => !!v),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(user => {
        this.corporationFlow = user.roles.some(r => r.id === this.corporationRole);
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  get skillsByCategory(): FormArray {
    return this.form.get('skillsByCategory') as FormArray;
  }

  createForm(): void {
    this.form = this.formBuilder.group({
      jobTitle: ['', [CustomValidator.required, Validators.minLength(2), Validators.maxLength(250)]],
      countryId: ['', Validators.required],
      country: [null],
      stateId: [''],
      state: [null],
      about: [''],
      linkedInUrl: ['', CustomValidator.linkedInUrl],
      regions: ['', Validators.required],
      categories: ['', Validators.required],
      responsibilityId: [''],
      subscribeToWelcomeSeries: [1, Validators.required],
      skillsByCategory: this.formBuilder.array([])
    });
  }
}
