import { ControlContainer, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { merge } from 'rxjs';

import { ProjectOffsitePpaInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { CountryInterface } from '../../../../shared/interfaces/user/country.interface';
import { CustomValidator } from '../../../../shared/validators/custom.validator';
import { SETTLEMENT_HUB_LIST } from '../../constants/settlement-hub-list.const';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { ProjectTypesSteps } from '../../enums/project-types-name.enum';
import { AddProjectService } from '../../services/add-project.service';
import { INT32_MAX } from 'src/app/shared/constants/math.const';

@UntilDestroy()
@Component({
  selector: 'neo-offsite-ppa-private',
  templateUrl: './offsite-ppa-private.component.html',
  styles: [
    `
      neo-dropdown,
      neo-number-input {
        width: 340px;
      }
    `
  ],
  styleUrls: ['../../add-project.component.scss', './offsite-ppa-private.component.scss']
})
export class OffsitePpaPrivateComponent implements OnInit {
  stepsList = AddProjectStepsEnum;
  projectTypes = ProjectTypesSteps;
  settlementTypes: TagInterface[] = [
    { name: 'None', id: null },
    { name: 'Hub', id: 1 },
    { name: 'Busbar', id: 2 },
    { name: 'Loadzone', id: 3 }
  ];
  settlementHubsList: TagInterface[] = SETTLEMENT_HUB_LIST?.sort((a, b) => a.sort - b.sort);
  currencyList: TagInterface[] = [
    { name: 'USD', id: 1 },
    { name: 'EUR', id: 2 },
    { name: 'GBP', id: 3 },
    { name: 'AUD', id: 4 },
    { name: 'CAD', id: 5 },
    { name: 'MXN', id: 6 }
  ];
  pricingStructureList: TagInterface[] = [
    { name: 'Plain CFD', id: 1 },
    { name: 'Upside share', id: 2 },
    { name: 'Market following', id: 3 },
    { name: 'Fixed discount to market', id: 4 }
  ];
  eacTypesList: TagInterface[] = [
    { name: 'REC', id: 1 },
    { name: 'Green-E REC', id: 2 },
    { name: 'GO', id: 3 },
    { name: 'REGO', id: 4 },
    { name: ' I-REC', id: 6 },
    { name: 'LGC', id: 6 },
    { name: 'Other', id: 7 }
  ];
  eacOtherId: number = this.eacTypesList.find(e => e.name.toLowerCase() === 'other')?.id;
  settlementIntervalsList: TagInterface[] = [
    { name: 'Day-Ahead', id: 1 },
    { name: 'Real-Time', id: 2 },
    { name: 'Intraday', id: 3 },
    { name: 'Other', id: 4 }
  ];
  settlementIntervalsOtherId: number = this.settlementIntervalsList.find(e => e.name.toLowerCase() === 'other')?.id;
  settlementCalculationList: TagInterface[] = [
    { name: 'Hourly', id: 1 },
    { name: 'Monthly', id: 2 },
    { name: 'Semi-annual', id: 3 },
    { name: 'Annual', id: 4 }
  ];
  form: FormGroup = this.formBuilder.group({
    settlementType: [''],
    settlementHubOrLoadZone: [''],
    forAllPriceEntriesCurrency: ['', Validators.required],
    contractPricePerMWh: ['', [Validators.required, CustomValidator.floatNumber, Validators.maxLength(100)]],
    floatingMarketSwapIndexDiscount: ['', Validators.maxLength(100)],
    floatingMarketSwapFloor: ['', Validators.maxLength(100)],
    floatingMarketSwapCap: ['', Validators.maxLength(100)],
    pricingStructure: ['', Validators.required],
    upsidePercentageToDeveloper: ['', [Validators.min(0), Validators.max(100)]],
    upsidePercentageToOfftaker: ['', [Validators.min(0), Validators.max(100)]],
    eac: ['', Validators.required],
    eacCustom: ['', Validators.maxLength(100)],
    eacValue: ['', Validators.maxLength(100)],
    settlementPriceInterval: ['', Validators.required],
    settlementPriceIntervalCustom: ['', Validators.maxLength(100)],
    settlementCalculationInterval: ['', Validators.required],
    additionalNotesForSEOperationsTeam: ['', Validators.maxLength(500)],
    projectMWCurrentlyAvailable: ['', [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]],
    discountAmount: ['']
  });

  settlementTypeId: number = this.settlementTypes[0].id;
  settlementHubOrLoadZoneId: number = this.settlementHubsList[0].id;
  forAllPriceEntriesCurrencyId: number = this.currencyList[0].id;
  pricingStructureId: number = this.pricingStructureList[0].id;
  eacId: number = this.eacTypesList[0].id;
  settlementPriceIntervalId: number = this.settlementIntervalsList[0].id;
  settlementCalculationIntervalId: number = this.settlementCalculationList[0].id;
  showHubLoadZoneControl: boolean;
  upsideShareSelected: boolean;
  showDraftModal: boolean;
  fixedDiscountToMarketSelected: boolean;
  formSubmitted: boolean;
  upsideShareError: ValidationErrors | null;

  constructor(
    public addProjectService: AddProjectService,
    public controlContainer: ControlContainer,
    private changeDetectorRef: ChangeDetectorRef,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    if (this.addProjectService.currentFlowData?.projectDetails) {
      const formVal: ProjectOffsitePpaInterface = this.addProjectService.currentFlowData
        ?.projectDetails as ProjectOffsitePpaInterface;

      this.form.patchValue({
        contractPricePerMWh: formVal.contractPricePerMWh,
        floatingMarketSwapIndexDiscount: formVal.floatingMarketSwapIndexDiscount,
        floatingMarketSwapFloor: formVal.floatingMarketSwapFloor,
        floatingMarketSwapCap: formVal.floatingMarketSwapCap,
        eacCustom: formVal.eacCustom,
        eacValue: formVal.eacValue,
        settlementPriceIntervalCustom: formVal.settlementPriceIntervalCustom,
        projectMWCurrentlyAvailable: formVal.projectMWCurrentlyAvailable,
        additionalNotesForSEOperationsTeam: formVal.additionalNotesForSEOperationsTeam,
        upsidePercentageToDeveloper: formVal.upsidePercentageToDeveloper,
        upsidePercentageToOfftaker: formVal.upsidePercentageToOfftaker,
        discountAmount: formVal.discountAmount,
        // * dropdown controls
        settlementType: this.settlementTypes.filter(s => s.id === formVal.settlementTypeId)[0] ?? '',
        settlementHubOrLoadZone:
          this.settlementHubsList.filter(s => s.id === formVal.settlementHubOrLoadZoneId)[0] ?? '',
        forAllPriceEntriesCurrency:
          this.currencyList.filter(s => s.id === formVal.forAllPriceEntriesCurrencyId)[0] ?? '',
        pricingStructure: this.pricingStructureList.filter(s => s.id === formVal.pricingStructureId)[0] ?? '',
        eac: this.eacTypesList.filter(s => s.id === formVal.eacId)[0] ?? '',
        settlementPriceInterval:
          this.settlementIntervalsList.filter(s => s.id === formVal.settlementPriceIntervalId)[0] ?? '',
        settlementCalculationInterval:
          this.settlementCalculationList.filter(s => s.id === formVal.settlementCalculationIntervalId)[0] ?? ''
      });

      this.settlementTypeId = formVal.settlementTypeId;
      this.settlementHubOrLoadZoneId = formVal.settlementHubOrLoadZoneId;
      this.forAllPriceEntriesCurrencyId = formVal.forAllPriceEntriesCurrencyId;
      this.pricingStructureId = formVal.pricingStructureId;
      this.eacId = formVal.eacId;

      this.settlementPriceIntervalId = formVal.settlementPriceIntervalId;
      this.settlementCalculationIntervalId = formVal.settlementCalculationIntervalId;

      this.showHubLoadZoneControl = this.settlementTypeId === 1 || this.settlementTypeId === 3;
      this.upsideShareSelected = this.pricingStructureId === 2;
      this.fixedDiscountToMarketSelected = this.pricingStructureId === 4;

      this.eacTypeCheck(formVal.eacId);
      this.settlementPriceIntervalCheck(formVal.settlementPriceIntervalId);
    }
  }

  changeStep(step: number): void {
    this.form.controls['forAllPriceEntriesCurrency'].addValidators(Validators.required);
    this.form.controls['contractPricePerMWh'].addValidators(Validators.required);
    this.form.controls['pricingStructure'].addValidators(Validators.required);
    this.form.controls['eac'].addValidators(Validators.required);
    this.form.controls['settlementPriceInterval'].addValidators(Validators.required);
    this.form.controls['settlementCalculationInterval'].addValidators(Validators.required);
    this.form.controls['projectMWCurrentlyAvailable'].addValidators(Validators.required);
    this.updateFormValidity();

    this.formSubmitted = true;

    if (
      this.form.controls['pricingStructure']?.value.id === 2 &&
      +this.form.controls['upsidePercentageToDeveloper'].value +
        +this.form.controls['upsidePercentageToOfftaker'].value !==
        100
    ) {
      this.upsideShareError = { total: 100 };

      merge(
        this.form.controls['upsidePercentageToDeveloper'].valueChanges,
        this.form.controls['upsidePercentageToOfftaker'].valueChanges
      )
        .pipe(untilDestroyed(this))
        .subscribe(() => {
          this.upsideShareError = null;
        });
    }

    if (this.form.invalid || this.upsideShareError) return;

    this.updateProjectDetailsForm();
    this.addProjectService.changeStep(step);
  }

  goBack(step: number): void {
    this.updateProjectDetailsForm();
    this.addProjectService.changeStep(step);
  }

  setControlValue(dropdownVal: TagInterface | CountryInterface, controlName: string) {
    this[controlName] = dropdownVal.id;
  }

  hubLoadZoneCheck(dropdownVal: TagInterface | CountryInterface) {
    this.showHubLoadZoneControl = dropdownVal.id === 1 || dropdownVal.id === 3;

    if (dropdownVal.id !== 1 && dropdownVal.id !== 3) {
      this.form.patchValue({ settlementHubOrLoadZone: '' });
      this.settlementHubOrLoadZoneId = null;
    }
  }

  eacTypeCheck(dropdownVal: number) {
    if (dropdownVal === this.eacOtherId) {
      this.form.controls['eacCustom']?.addValidators(Validators.required);
      this.changeDetectorRef.detectChanges();
      this.form.controls['eacCustom']?.updateValueAndValidity();
    } else {
      this.form.controls['eacCustom']?.removeValidators(Validators.required);
      this.form.controls['eacCustom']?.updateValueAndValidity();
    }
  }

  settlementPriceIntervalCheck(dropdownVal: number) {
    if (dropdownVal === this.settlementIntervalsOtherId) {
      this.form.controls['settlementPriceIntervalCustom']?.addValidators(Validators.required);
      this.changeDetectorRef.detectChanges();
      this.form.controls['settlementPriceIntervalCustom']?.updateValueAndValidity();
    } else {
      this.form.controls['settlementPriceIntervalCustom']?.removeValidators(Validators.required);
      this.form.controls['settlementPriceIntervalCustom']?.updateValueAndValidity();
    }
  }

  upsideShareCheck(dropdownValId: number) {
    this.upsideShareSelected = dropdownValId === 2;
    this.fixedDiscountToMarketSelected = dropdownValId === 4;

    if (this.fixedDiscountToMarketSelected) {
      this.form.controls['discountAmount']?.setValidators(Validators.required);
      this.changeDetectorRef.detectChanges();
      this.form.controls['discountAmount']?.updateValueAndValidity();
    }

    if (!this.fixedDiscountToMarketSelected) {
      this.form.controls['discountAmount']?.removeValidators(Validators.required);
      this.changeDetectorRef.detectChanges();
      this.form.controls['discountAmount']?.updateValueAndValidity();
    }

    if (this.upsideShareSelected) {
      this.form.controls['upsidePercentageToDeveloper'].setValidators(CustomValidator.required);

      this.form.controls['upsidePercentageToOfftaker'].setValidators(CustomValidator.required);
    } else {
      this.form.controls['upsidePercentageToDeveloper'].setValue('');
      this.form.controls['upsidePercentageToDeveloper'].removeValidators(CustomValidator.required);

      this.form.controls['upsidePercentageToOfftaker'].setValue('');
      this.form.controls['upsidePercentageToOfftaker'].removeValidators(CustomValidator.required);
    }

    this.form.get('upsidePercentageToOfftaker').updateValueAndValidity();
    this.form.get('upsidePercentageToDeveloper').updateValueAndValidity();
  }

  private updateProjectDetailsForm(): void {
    const formForPayload = { ...this.form.value };
    delete formForPayload.settlementType;
    delete formForPayload.settlementHubOrLoadZone;
    delete formForPayload.forAllPriceEntriesCurrency;
    delete formForPayload.pricingStructure;
    delete formForPayload.eac;
    delete formForPayload.settlementPriceInterval;
    delete formForPayload.settlementCalculationInterval;

    this.addProjectService.updateProjectTypeData(formForPayload);
    this.addProjectService.updateProjectTypeData({
      settlementTypeId: this.settlementTypeId,
      settlementHubOrLoadZoneId: this.settlementHubOrLoadZoneId,
      forAllPriceEntriesCurrencyId: this.forAllPriceEntriesCurrencyId,
      pricingStructureId: this.pricingStructureId,
      eacId: this.eacId,
      settlementPriceIntervalId: this.settlementPriceIntervalId,
      settlementCalculationIntervalId: this.settlementCalculationIntervalId
    });
  }

  saveDraft(): void {
    if (
      this.form.controls['pricingStructure']?.value.id === 2 &&
      +this.form.controls['upsidePercentageToDeveloper'].value +
        +this.form.controls['upsidePercentageToOfftaker'].value !==
        100
    ) {
      this.upsideShareError = { total: 100 };
      merge(
        this.form.controls['upsidePercentageToDeveloper'].valueChanges,
        this.form.controls['upsidePercentageToOfftaker'].valueChanges
      )
        .pipe(untilDestroyed(this))
        .subscribe(() => {
          this.upsideShareError = null;
        });
    } else {
      this.updateProjectDetailsForm();
    }

    this.form.controls['forAllPriceEntriesCurrency'].removeValidators(Validators.required);
    this.form.controls['contractPricePerMWh'].removeValidators(Validators.required);
    this.form.controls['pricingStructure'].removeValidators(Validators.required);
    this.form.controls['eac'].removeValidators(Validators.required);
    this.form.controls['settlementPriceInterval'].removeValidators(Validators.required);
    this.form.controls['settlementCalculationInterval'].removeValidators(Validators.required);
    this.form.controls['projectMWCurrentlyAvailable'].removeValidators(Validators.required);
    this.form.controls['eacCustom']?.removeValidators(Validators.required);
    this.form.controls['eacCustom']?.updateValueAndValidity();
    this.form.controls['settlementPriceIntervalCustom']?.removeValidators(Validators.required);
    this.form.controls['settlementPriceIntervalCustom']?.updateValueAndValidity();
    this.formSubmitted = true;
    this.updateFormValidity();

    if (this.form.invalid || this.upsideShareError) return;

    this.addProjectService.updateProjectTypeData(this.form.value);
    this.showDraftModal = true;
  }

  updateFormValidity(): void {
    this.form.controls['forAllPriceEntriesCurrency'].updateValueAndValidity();
    this.form.controls['contractPricePerMWh'].updateValueAndValidity();
    this.form.controls['pricingStructure'].updateValueAndValidity();
    this.form.controls['eac'].updateValueAndValidity();
    this.form.controls['settlementPriceInterval'].updateValueAndValidity();
    this.form.controls['settlementCalculationInterval'].updateValueAndValidity();
    this.form.controls['projectMWCurrentlyAvailable'].updateValueAndValidity();
  }
}
