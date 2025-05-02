import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { TranslateService } from '@ngx-translate/core';
import { Observable, filter, catchError, throwError } from 'rxjs';
import { JoiningInterestDetailsSchema } from 'src/app/admit/constants/parameter.const';
import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { HttpService } from 'src/app/core/services/http.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { FormControlStatusEnum } from 'src/app/shared/enums/form-control-status.enum';
import { CountryInterface } from 'src/app/shared/interfaces/user/country.interface';
import { CustomValidator } from 'src/app/shared/validators/custom.validator';

@UntilDestroy()
@Component({
  selector: 'neo-schedule-demo',
  templateUrl: './schedule-demo.component.html',
  styleUrls: ['./schedule-demo.component.scss']
})
export class ScheduleDemoComponent implements OnInit {

  showSuccess: boolean;
  joiningInteresDetailsLength: number = 0;
  termsAndConditions: boolean;
  joiningInterestDetailsTextMaxLength = JoiningInterestDetailsSchema.joiningInterestDetailsTextMaxLength;
  formSubmitted: boolean;
  emailServerError: string;
  selectedCountryName: string;
  @Output() output: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() titleEmitter: EventEmitter<boolean> = new EventEmitter<boolean>();

  businessTypeOptions: TagInterface[] = [
    { id: 0, name: "Corporate Enterprise" },
    { id: 1, name: "Small/Medium Business" },
    { id: 2, name: "Energy Solution Provider" },
    { id: 3, name: "Renewable Energy Developer" }
  ];


  iAmLookingForOptions: TagInterface[] = [
    { id: 0, name: "Learn content" },
    { id: 1, name: "Project opportunities" },
    { id: 2, name: "Events & Webinars" },
    { id: 3, name: "Knowledge sharing" },
    { id: 4, name: "Connecting with organizations" },
    { id: 5, name: "Market Analytics" },
    { id: 6, name: "Others" }
  ];


  formGroup: FormGroup = new FormGroup({
    name: new FormControl(null, [
      Validators.required,
      CustomValidator.userName,
      Validators.minLength(2),
      Validators.maxLength(64)
    ]),
    company: new FormControl(null, [
      Validators.required,
      Validators.minLength(2),
      CustomValidator.companyNameExcludeSymbols,
      Validators.maxLength(250)
    ]),
		email: new FormControl(null, [Validators.required, CustomValidator.email, Validators.maxLength(70)]),
		countryId: new FormControl(null, Validators.required),
		businessType: new FormControl('', Validators.required),
		iAmLookingFor: new FormControl('', Validators.required),
    joiningInterestDetails: new FormControl(null, [(control: AbstractControl) => CustomValidator.checkIfCharacterCountIsGreaterThanLimit(control, 200, this.translateService.instant('scheduleDemo.joiningInterestErrorLabel'))])
  });

  countries$: Observable<CountryInterface[]>;

  constructor(
    private readonly translateService: TranslateService,
    private readonly httpService: HttpService,
    private readonly authService: AuthService,
    private readonly router: Router,
    private snackbarService: SnackbarService
  ) { }

  ngOnInit(): void {

    this.formGroup.controls['email'].valueChanges
      .pipe(
        filter(() => !!this.emailServerError),
        untilDestroyed(this)
      )
      .subscribe(() => {
        this.emailServerError = null;
      });
  }

  searchCountries(search: string): void {
    this.countries$ = this.httpService.get<CountryInterface[]>(CommonApiEnum.Countries, {
      search
    });
    if (!search) {
      this.formGroup.get('countryId').patchValue(null);
      this.selectedCountryName = null;
    }
  }

  setCountry(country: CountryInterface): void {
    this.formGroup.get('countryId').patchValue(country.id);
    this.selectedCountryName = country.name;
  }

  logIn(): void {
    this.authService.loginRedirect();
  }

  hasError(controlName: string): boolean {
    const control = this.formGroup?.get(controlName);

    return control?.invalid && control?.touched && control?.dirty;
  }

  submitRequest(): void {
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
        this.translateService.instant('general.formattingMaxLengthError')
      );
      return;
    }

    const formData = {
      ...this.formGroup.value,
      company: this.formGroup.get('company')?.value?.trim(),
      businessType: this.formGroup.get('businessType')?.value.name,
      iAmLookingFor: this.formGroup.get('iAmLookingFor')?.value.name,
      country: this.selectedCountryName
    };

    this.httpService
      .post(CommonApiEnum.ScheduleDemo, formData)
      .pipe(
        untilDestroyed(this),
        catchError(error => {
          this.snackbarService.showError(
            this.translateService.instant('general.defaultErrorLabel')
          );
          return;
          return throwError(error);
        })
      )
      .subscribe(() => {
        this.titleEmitter.emit(false);
        this.showSuccess = true
      });
  }


  clearCountry(): void {
    this.formGroup.get('countryId').patchValue(null);
  }

  close(): void {
    this.output.emit(false);
  }
}
