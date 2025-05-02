import { Component, OnInit } from '@angular/core';
import { RolesEnum } from '../shared/enums/roles.enum';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, tap, throwError } from 'rxjs';
import { catchError, filter } from 'rxjs/operators';

import { TranslateService } from '@ngx-translate/core';
import { HttpService } from '../core/services/http.service';

import { RadioControlInterface } from '../shared/modules/radio-control/radio-control.component';
import { CountryInterface } from '../shared/interfaces/user/country.interface';
import { TagInterface } from '../core/interfaces/tag.interface';

import { CommonApiEnum } from '../core/enums/common-api.enum';
import { AuthService } from '../core/services/auth.service';
import { FormControlStatusEnum } from '../shared/enums/form-control-status.enum';

import { CustomValidator } from '../shared/validators/custom.validator';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { JoiningInterestDetailsSchema } from '../admit/constants/parameter.const';
import { SnackbarService } from '../core/services/snackbar.service';
import { SignTrackingSourceEnum } from '../core/enums/sign-tracking-source-enum';
import { ActivityLocation } from '../core/enums/activity/activity-location.enum';
import { ActivityTypeEnum } from '../core/enums/activity/activity-type.enum';
import { ThankYouPopupServiceService } from '../public/services/thank-you-popup-service.service';
import { TitleService } from '../core/services/title.service';

@UntilDestroy()
@Component({
  selector: 'neo-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {
  termOfUseModal: boolean;
  termsAndConditions: boolean;
  disableInput: boolean = false;
  rolesEnum = RolesEnum;
  emailServerError: string;
  source: string = '';
  showSuccess: boolean;
  joiningInteresDetailsLength: number = 0;
  joiningInterestDetailsTextMaxLength = JoiningInterestDetailsSchema.joiningInterestDetailsTextMaxLength;
  roles: RadioControlInterface[] = [
    {
      id: RolesEnum.Corporation,
      name: this.translateService.instant('signUp.roles.corporationLabel')
    },
    {
      id: RolesEnum.SolutionProvider,
      name: this.translateService.instant('signUp.roles.solutionProviderLabel')
    },
    {
      id: RolesEnum.Internal,
      name: this.translateService.instant('signUp.roles.employeeLabel')
    }
  ];
  formSubmitted: boolean;

  formGroup: FormGroup = new FormGroup({
    roleId: new FormControl(null, Validators.required),
    firstName: new FormControl(null, [
      Validators.required,
      CustomValidator.userName,
      Validators.minLength(2),
      Validators.maxLength(64)
    ]),
    lastName: new FormControl(null, [
      Validators.required,
      CustomValidator.userName,
      Validators.minLength(2),
      Validators.maxLength(64)
    ]),
    companyName: new FormControl(null, [
      Validators.required,
      CustomValidator.companyNameExcludeSymbols,
      Validators.maxLength(250)
    ]),
    email: new FormControl(null, [Validators.required, CustomValidator.email, Validators.maxLength(70)]),
    countryId: new FormControl(null, Validators.required),
    heardViaId: new FormControl('', Validators.required),
    timeZoneClientId: new FormControl(Intl.DateTimeFormat().resolvedOptions().timeZone),
    recaptchaToken: new FormControl(null, Validators.required),
    joiningInterestDetails: new FormControl(null)
  });

  countries$: Observable<CountryInterface[]>;
  heardVia$: Observable<TagInterface[]> = this.httpService
    .get<TagInterface[]>(CommonApiEnum.HeardVia)
    .pipe(tap(response => (this.heardVia = response)));
  private heardVia: TagInterface[];

  constructor(
    private route: ActivatedRoute,
    private titleService: TitleService,
    private readonly translateService: TranslateService,
    private readonly httpService: HttpService,
    private readonly authService: AuthService,
    private readonly router: Router,
    private snackbarService: SnackbarService,
    private readonly thankYouPopupServiceService: ThankYouPopupServiceService
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle('signUp.registerationBrowserTitleLabel');
    this.authService
      .currentUser()
      .pipe(untilDestroyed(this))
      .subscribe(user => {
        if (user) {
          this.router.navigate(['/dashboard']);
        }
      });

    if (location.search && parseInt(location.search?.slice(location.search?.length - 1)) > 0) {
      this.formGroup.get('roleId').setValue(parseInt(location.search?.slice(location.search?.length - 1)));
    }

    this.formGroup.controls['email'].valueChanges
      .pipe(
        filter(() => !!this.emailServerError),
        untilDestroyed(this)
      )
      .subscribe(() => {
        this.emailServerError = null;
      });

    this.route.queryParamMap.subscribe((queryParam: any) => {
      if (
        queryParam.params['source'] &&
        (queryParam.params['source'] == SignTrackingSourceEnum.Zeigo ||
          queryParam.params['source'] == SignTrackingSourceEnum.ZeigoNetwork)
      ) {
        this.source = queryParam.params['source'];
      }
    });

    this.thankYouPopupServiceService.showStatus$.next(false);
  }

  searchCountries(search: string): void {
    this.countries$ = this.httpService.get<CountryInterface[]>(CommonApiEnum.Countries, {
      search
    });

    if (!search) {
      this.formGroup.get('countryId').patchValue(null);
    }
  }

  setCountry(country: CountryInterface): void {
    this.formGroup.get('countryId').patchValue(country.id);
  }

  logIn(): void {
    this.authService.loginRedirect();
  }

  hasError(controlName: string): boolean {
    const control = this.formGroup?.get(controlName);

    return control?.invalid && control?.touched && control?.dirty;
  }

  createAccount(): void {
    this.formSubmitted = true;

    if (this.formGroup.invalid || !this.termsAndConditions) return;
    
    if (this.formGroup.status === FormControlStatusEnum.Invalid) {
      Object.keys(this.formGroup.value).map(key => {
        this.formGroup.get(key).markAsDirty();
        this.formGroup.get(key).markAsTouched();
      });

      return;
    }
    
    if (this.joiningInteresDetailsLength > this.joiningInterestDetailsTextMaxLength) {
      return;
    }
    if (this.formGroup.controls['joiningInterestDetails'].value?.length > this.joiningInterestDetailsTextMaxLength) {
      this.snackbarService.showError(
        this.translateService.instant('companyManagement.form.aboutFormattingMaxLengthError')
      );
      return;
    }

    const signUpData = {
      ...this.formGroup.value,
      companyName: this.formGroup.get('companyName')?.value?.trim(),
      heardViaId: this.formGroup?.value?.heardViaId?.id
    };

    const activityData = {
      typeId: ActivityTypeEnum.SignupClick,
      locationId: ActivityLocation.RegistrationPage,
      details: JSON.stringify({ source: this.source == '' ? SignTrackingSourceEnum.Default : this.source })
    };

    const formData = {
      SignUpData: signUpData,
      ActivityData: activityData
    };

    this.httpService
      .post(CommonApiEnum.UserPending, formData)
      .pipe(
        untilDestroyed(this),
        catchError(error => {
          this.emailServerError = error?.error?.errors['signUpData.Email']?.[0];

          if (this.emailServerError) {
            this.formGroup.get('email').markAsDirty();
            this.formGroup.get('email').markAsTouched();
            this.formGroup.get('email').setErrors(Validators.email);
            this.emailServerError = 'general.emailExistLabel';
          }

          return throwError(error);
        })
      )
      .subscribe(() => {
        this.showSuccess = true;
      });
  }

  changeRole(control: RadioControlInterface): void {
    if (control.id === RolesEnum.Internal) {
      this.formGroup.get('companyName').setValue(control.name.replace(' Employee', ''));
      this.formGroup.get('heardViaId').setValue(this.heardVia.find(item => item.name.includes('Employee')));
      this.disableInput = true;
    } else {
      this.formGroup.get('companyName').setValue(null);
      this.formGroup.get('heardViaId').setValue('');
      this.disableInput = false;
    }
  }

  clearCountry(): void {
    this.formGroup.get('countryId').patchValue(null);
  }
}
