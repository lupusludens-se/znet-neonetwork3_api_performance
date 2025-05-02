import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { ControlContainer, FormGroupDirective, Validators } from '@angular/forms';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { CountryInterface } from '../../../shared/interfaces/user/country.interface';
import { OnboardingWizardService } from '../../services/onboarding-wizard.service';
import { OnboardingStepsEnum } from '../../enums/onboarding-steps.enum';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { HttpService } from '../../../core/services/http.service';
import { AuthService } from '../../../core/services/auth.service';
import { CountriesService } from '../../../shared/services/countries.service';
import { SearchBarComponent } from '../../../shared/components/search-bar/search-bar.component';
import { US_COUNTRY_ID } from '../../../shared/constants/countries.const';

@UntilDestroy()
@Component({
  selector: 'neo-onboarding-location',
  templateUrl: 'onboarding-location.component.html',
  styleUrls: ['../../onboarding-wizard.component.scss', 'onboarding-location.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class OnboardingLocationComponent implements OnInit {
  @Input() corporationFlow: boolean;

  stepsList = OnboardingStepsEnum;
  countriesList: CountryInterface[];
  statesList: TagInterface[] = [];
  showStatesControl: boolean;
  formSubmitted: boolean;

  @ViewChild('searchStateRef') searchStateRef: SearchBarComponent;

  constructor(
    public controlContainer: ControlContainer,
    private onboardingWizardService: OnboardingWizardService,
    private httpService: HttpService,
    private changeDetRef: ChangeDetectorRef,
    private authService: AuthService,
    public countriesService: CountriesService
  ) {}

  ngOnInit(): void {
    this.formSubmitted = false;
    this.countriesService.getInitialCountriesList();

    this.authService
      .currentUser()
      .pipe(untilDestroyed(this))
      .subscribe(user => {
        if (
          this.controlContainer.control?.get('countryId')?.value === US_COUNTRY_ID ||
          (!this.controlContainer.control?.get('countryId')?.value && user.country?.id === US_COUNTRY_ID)
        ) {
          this.showStatesControl = true;
        }

        if (!this.controlContainer.control?.get('countryId')?.value) {
          this.controlContainer.control?.patchValue({
            countryId: user.countryId,
            country: user.country
          });
        }
      });
  }

  goBack(): void {
    if (this.corporationFlow) {
      this.onboardingWizardService.changeStep(this.stepsList.Responsibilities);
    } else {
      this.onboardingWizardService.changeStep(this.stepsList.Role);
    }
  }

  goForward(): void {
    this.formSubmitted = true;

    if (
      this.controlContainer.control!.get('countryId')!.invalid ||
      (this.showStatesControl && !this.controlContainer.control!.get('stateId')!.value)
    ) {
      return;
    }

    this.onboardingWizardService.changeStep(this.stepsList.PersonalInfo);
  }

  searchCountries(search: string): void {
    this.countriesList = this.countriesService.countriesList.value.filter(c => {
      return (
        c.name.toLowerCase().includes(search.toLowerCase()) ||
        c.code.toLowerCase().includes(search.toLowerCase()) ||
        c.code3.toLowerCase().includes(search.toLowerCase())
      );
    });
  }

  chooseCountry(country: CountryInterface): void {
    this.controlContainer.control?.get('countryId')?.patchValue(country.id);
    this.validateStateControl(country);
    this.controlContainer.control?.get('country')?.patchValue(country);
  }

  validateStateControl(country: CountryInterface): void {
    if (country?.code?.toLowerCase() === 'us') {
      this.showStatesControl = true;
      this.controlContainer.control?.get('stateId')?.setValidators(Validators.required);
      this.controlContainer.control?.get('stateId')?.updateValueAndValidity();
      this.changeDetRef.markForCheck();

      setTimeout(() => this.searchStateRef.searchEl.nativeElement.focus());
    } else {
      this.controlContainer.control?.get('stateId')?.clearValidators();
      this.controlContainer.control?.get('stateId')?.updateValueAndValidity();
      this.showStatesControl = false;
      this.changeDetRef.markForCheck();
    }
  }

  clearCountry(): void {
    this.controlContainer.control?.get('countryId')?.patchValue(null);
    this.controlContainer.control?.get('country')?.patchValue(null);
    this.clearState();
  }

  clearState(): void {
    this.showStatesControl = false;
    this.controlContainer.control?.get('state')?.patchValue(null);
    this.controlContainer.control?.get('stateId')?.patchValue(null);
    this.validateStateControl(this.controlContainer.control?.get('country')?.value);
  }

  searchStates(searchStr: string): void {
    this.statesList = this.countriesService.statesList.value.filter(state => {
      return (
        state.name.toLowerCase().includes(searchStr.toLowerCase()) ||
        state.abbr.toLowerCase().includes(searchStr.toLowerCase())
      );
    });
  }

  chooseState(state: TagInterface): void {
    this.controlContainer.control?.get('stateId')?.patchValue(state.id);
    this.controlContainer.control?.get('state')?.patchValue(state);
  }
}
