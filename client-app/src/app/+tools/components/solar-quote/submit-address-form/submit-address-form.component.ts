import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Component, Input, OnDestroy } from '@angular/core';

import { Router } from '@angular/router';

import { HttpService } from '../../../../core/services/http.service';
import { SnackbarService } from '../../../../core/services/snackbar.service';

import { RadioControlInterface } from '../../../../shared/modules/radio-control/radio-control.component';
import { ContractStructureTypeEnum } from '../../../enums/contract-structure-type.enum';
import { QuoteInterestsTypeEnum } from '../../../enums/quote-interests-type.enum';
import { AreaTypeEnum } from '../../../enums/area-type.enum';
import { FormControlStatusEnum } from '../../../../shared/enums/form-control-status.enum';
import { ToolsApiEnum } from '../../../../shared/enums/api/tools-api.enum';
import { Subject } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { INT32_MAX } from 'src/app/shared/constants/math.const';
import { CustomValidator } from '../../../../shared/validators/custom.validator';

@Component({
  selector: 'neo-submit-address-form',
  templateUrl: './submit-address-form.component.html',
  styleUrls: ['./../indicative-quote-request-form/indicative-quote-request-form.component.scss']
})
export class SubmitAddressFormComponent implements OnDestroy {
  @Input() toolId: number;

  formGroup: FormGroup = this.formBuilder.group({
    siteAddress: [null, [Validators.required, Validators.maxLength(250)]],
    annualPower: [null, [Validators.required, Validators.maxLength(10)]],
    roofAvailable: [null, Validators.required],
    roofArea: [null, [Validators.maxLength(10)]],
    roofAreaType: [null],
    landAvailable: [null, Validators.required],
    landArea: [null, [Validators.maxLength(15)]],
    landAreaType: [null],
    carportAvailable: [null, Validators.required],
    carportArea: [null, [Validators.maxLength(15)]],
    carportAreaType: [null],
    buildingOwned: [null, Validators.required],
    contractStructures: new FormArray([], Validators.required),
    interests: new FormArray([], Validators.required),
    additionalComments: [null, [Validators.maxLength(4000)]]
  });

  areaTypeOptions: RadioControlInterface[] = [
    {
      id: AreaTypeEnum.Feet,
      name: 'FT'
    },
    {
      id: AreaTypeEnum.Meters,
      name: 'M'
    }
  ];
  questionOptions: RadioControlInterface[] = [
    {
      id: 0,
      name: this.translateService.instant('general.noLabel')
    },
    {
      id: 1,
      name: this.translateService.instant('general.yesLabel')
    }
  ];
  ownQuestionOptions: RadioControlInterface[] = [
    {
      id: 0,
      name: this.translateService.instant('general.noLabel')
    },
    {
      id: 1,
      name: this.translateService.instant('general.yesOwnLabel')
    }
  ];
  contactStructureOptions = ContractStructureTypeEnum;
  quoteInterestsOptions = QuoteInterestsTypeEnum;
  formControlStatus = FormControlStatusEnum;

  private unsubscribe$: Subject<void> = new Subject<void>();
  formSubmitted: boolean;

  constructor(
    private readonly router: Router,
    private readonly httpService: HttpService,
    private readonly snackbarService: SnackbarService,
    private readonly translateService: TranslateService,
    private formBuilder: FormBuilder
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  onCancel(): void {
    this.router.navigate(['/tools']);
  }

  save(): void {
    this.formSubmitted = true;

    this.validateAreaControls('roof');
    this.validateAreaControls('land');
    this.validateAreaControls('carport');

    if (
      this.formGroup.invalid ||
      this.formGroup.controls['roofAreaType'].invalid ||
      this.formGroup.controls['landAreaType'].invalid ||
      this.formGroup.controls['carportAreaType'].invalid ||
      this.formGroup.controls['roofArea'].invalid ||
      this.formGroup.controls['carportArea'].invalid ||
      this.formGroup.controls['landArea'].invalid
    ) {
      Object.keys(this.formGroup.value).map(key => {
        this.formGroup.controls[key].markAsDirty();
        this.formGroup.controls[key].markAsTouched();
      });

      return;
    }

    this.requestQuote();
  }

  getKeys(enumData: unknown): string[] {
    return Object.keys(enumData).filter(x => !(parseInt(x) >= 0));
  }

  setCheckboxStatus(controlName, value: number): void {
    const companyFormArray = this.getFormArray(controlName);

    if (companyFormArray?.length) {
      const companyIndex = companyFormArray?.controls.findIndex(item => item.value === value);

      if (companyIndex >= 0) {
        companyFormArray.removeAt(companyIndex);
      } else {
        companyFormArray.push(new FormControl(value));
      }
    } else {
      companyFormArray.push(new FormControl(value));
    }
  }

  clearInputs(value: RadioControlInterface, fieldNamesToClear: string[]) {
    if (value.name === this.translateService.instant('general.noLabel')) {
      fieldNamesToClear.forEach(fieldName => {
        this.getControl(fieldName).patchValue(null);
      });
    }
  }

  private getFormArray(property: string): FormArray {
    return this.formGroup.get(property) as FormArray;
  }

  private requestQuote(): void {
    this.httpService.post<unknown>(ToolsApiEnum.SubmitAnAddress, this.formGroup.value).subscribe(
      () => this.router.navigate([`/tools/thank-you/${this.toolId}`]),
      () => this.snackbarService.showError('general.defaultErrorLabel')
    );
  }

  private validateAreaControls(controlKey: string) {
    if (this.formGroup.controls[`${controlKey}Available`].value === 1) {
      this.formGroup.controls[`${controlKey}Area`].addValidators(CustomValidator.required);
      this.formGroup.controls[`${controlKey}AreaType`].addValidators(Validators.required);
    } else {
      this.formGroup.controls[`${controlKey}Area`].removeValidators(CustomValidator.required);
      this.formGroup.controls[`${controlKey}AreaType`].removeValidators(Validators.required);
    }
    this.formGroup.controls[`${controlKey}Area`].updateValueAndValidity();
    this.formGroup.controls[`${controlKey}AreaType`].updateValueAndValidity();
  }

  private getControl(propertyName: string): AbstractControl {
    return this.formGroup.get(propertyName);
  }
}
