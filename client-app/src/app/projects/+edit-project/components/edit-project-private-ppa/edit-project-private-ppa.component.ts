import { ControlContainer, FormGroupDirective, ValidationErrors, Validators } from '@angular/forms';
import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { ProjectOffsitePpaInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { SETTLEMENT_HUB_LIST } from '../../../+add-project/constants/settlement-hub-list.const';
import { ProjectInterface } from '../../../../shared/interfaces/projects/project.interface';
import { ProjectTypesSteps } from '../../../+add-project/enums/project-types-name.enum';
import { TagInterface } from '../../../../core/interfaces/tag.interface';

@Component({
  selector: 'neo-edit-project-private-ppa',
  templateUrl: 'edit-project-private-ppa.component.html',
  styleUrls: ['../../edit-project.component.scss', '../edit-project-ppa/edit-project-ppa.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class EditProjectPrivatePpaComponent implements OnInit {
  @Output() closePrivatePart: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() project: ProjectInterface;
  @Input() formSubmitted: boolean;
  @Input() upsideShareError: ValidationErrors | null;
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
  projectTypes = ProjectTypesSteps;

  constructor(public controlContainer: ControlContainer, private changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit(): void {
    const projectDetails: ProjectOffsitePpaInterface = this.project.projectDetails as ProjectOffsitePpaInterface;

    this.controlContainer.control.patchValue({
      contractPricePerMWh: projectDetails.contractPricePerMWh,
      floatingMarketSwapIndexDiscount: projectDetails.floatingMarketSwapIndexDiscount,
      floatingMarketSwapFloor: projectDetails.floatingMarketSwapFloor,
      floatingMarketSwapCap: projectDetails.floatingMarketSwapCap,
      eacCustom: projectDetails.eacCustom,
      eacValue: projectDetails.eacValue,
      settlementPriceIntervalCustom: projectDetails.settlementPriceIntervalCustom,
      projectMWCurrentlyAvailable: projectDetails.projectMWCurrentlyAvailable,
      additionalNotesForSEOperationsTeam: projectDetails.additionalNotesForSEOperationsTeam,
      upsidePercentageToDeveloper: projectDetails.upsidePercentageToDeveloper,
      upsidePercentageToOfftaker: projectDetails.upsidePercentageToOfftaker,
      discountAmount: projectDetails.discountAmount,
      // * dropdown controls
      settlementType: this.settlementTypes.filter(s => s.id === projectDetails.settlementTypeId)[0] ?? '',
      settlementHubOrLoadZone:
        this.settlementHubsList.filter(s => s.id === projectDetails.settlementHubOrLoadZoneId)[0] ?? '',
      forAllPriceEntriesCurrency:
        this.currencyList.filter(s => s.id === projectDetails.forAllPriceEntriesCurrencyId)[0] ?? '',
      pricingStructure: this.pricingStructureList.filter(s => s.id === projectDetails.pricingStructureId)[0] ?? '',
      eac: this.eacTypesList.filter(s => s.id === projectDetails.eacId)[0] ?? '',
      settlementPriceInterval:
        this.settlementIntervalsList.filter(s => s.id === projectDetails.settlementPriceIntervalId)[0] ?? '',
      settlementCalculationInterval:
        this.settlementCalculationList.filter(s => s.id === projectDetails.settlementCalculationIntervalId)[0] ?? '',
      /* DROPDOWNS*/
      settlementTypeId: projectDetails.settlementTypeId,
      settlementHubOrLoadZoneId: projectDetails.settlementHubOrLoadZoneId ?? this.settlementHubsList[0].id,
      forAllPriceEntriesCurrencyId: projectDetails.forAllPriceEntriesCurrencyId ?? this.currencyList[0].id,
      pricingStructureId: projectDetails.pricingStructureId ?? this.pricingStructureList[0].id,
      eacId: projectDetails.eacId ?? this.eacTypesList[0].id,
      settlementPriceIntervalId: projectDetails.settlementPriceIntervalId ?? this.settlementIntervalsList[0].id,
      settlementCalculationIntervalId:
        projectDetails.settlementCalculationIntervalId ?? this.settlementCalculationList[0].id
    });

    this.settlementTypeId = projectDetails.settlementTypeId;
    this.settlementHubOrLoadZoneId = projectDetails.settlementHubOrLoadZoneId;
    this.forAllPriceEntriesCurrencyId = projectDetails.forAllPriceEntriesCurrencyId;
    this.pricingStructureId = projectDetails.pricingStructureId;
    this.eacId = projectDetails.eacId;
    this.settlementPriceIntervalId = projectDetails.settlementPriceIntervalId;
    this.settlementCalculationIntervalId = projectDetails.settlementCalculationIntervalId;

    this.eacTypeCheck(projectDetails.eacId);
    this.settlementPriceIntervalCheck(projectDetails.settlementPriceIntervalId);
    this.hubLoadZoneCheck(projectDetails.settlementTypeId);
    this.upsideShareCheck(projectDetails.pricingStructureId);

    this.setControlValue(projectDetails.settlementTypeId, 'settlementTypeId');
    this.setControlValue(projectDetails.settlementHubOrLoadZoneId, 'settlementHubOrLoadZoneId');
    this.setControlValue(projectDetails.forAllPriceEntriesCurrencyId, 'forAllPriceEntriesCurrencyId');
    this.setControlValue(projectDetails.pricingStructureId, 'pricingStructureId');
    this.setControlValue(projectDetails.eacId, 'eacId');
    this.setControlValue(projectDetails.settlementPriceIntervalId, 'settlementPriceIntervalId');
    this.setControlValue(projectDetails.settlementCalculationIntervalId, 'settlementCalculationIntervalId');
  }

  setControlValue(dropdownValId: number, controlName: string) {
    this[controlName] = dropdownValId;
    this.controlContainer.control.patchValue({ [controlName]: dropdownValId });
  }

  hubLoadZoneCheck(dropdownValId: number) {
    this.showHubLoadZoneControl = dropdownValId === 1 || dropdownValId === 3;

    if (dropdownValId !== 1 && dropdownValId !== 3) {
      this.controlContainer.control.patchValue({ settlementHubOrLoadZone: '' });
      this.controlContainer.control.patchValue({ settlementHubOrLoadZoneId: null });
      this.settlementHubOrLoadZoneId = null;
    }
  }

  eacTypeCheck(dropdownVal: number) {
    if (dropdownVal === this.eacOtherId) {
      this.controlContainer.control.get('eacCustom')?.addValidators(Validators.required);
      this.changeDetectorRef.detectChanges();
      this.controlContainer.control.get('eacCustom')?.updateValueAndValidity();
    } else {
      this.controlContainer.control.get('eacCustom')?.removeValidators(Validators.required);
      this.controlContainer.control.get('eacCustom')?.updateValueAndValidity();
    }
  }

  settlementPriceIntervalCheck(dropdownVal: number) {
    if (dropdownVal === this.settlementIntervalsOtherId) {
      this.controlContainer.control.get('settlementPriceIntervalCustom')?.addValidators(Validators.required);
      this.changeDetectorRef.detectChanges();
      this.controlContainer.control.get('settlementPriceIntervalCustom')?.updateValueAndValidity();
    } else {
      this.controlContainer.control.get('settlementPriceIntervalCustom')?.removeValidators(Validators.required);
      this.controlContainer.control.get('settlementPriceIntervalCustom')?.updateValueAndValidity();
    }
  }

  upsideShareCheck(dropdownValId: number) {
    this.upsideShareSelected = dropdownValId === 2;
    this.fixedDiscountToMarketSelected = dropdownValId === 4;

    if (this.fixedDiscountToMarketSelected) {
      this.controlContainer.control.get('discountAmount').setValidators(Validators.required);
    }

    if (!this.fixedDiscountToMarketSelected) {
      this.controlContainer.control.get('discountAmount').removeValidators(Validators.required);
    }

    if (this.upsideShareSelected) {
      this.controlContainer.control.get('upsidePercentageToDeveloper').setValidators(Validators.required);

      this.controlContainer.control.get('upsidePercentageToOfftaker').setValidators(Validators.required);
    }
    if (!this.upsideShareSelected) {
      this.controlContainer.control.get('upsidePercentageToDeveloper').setValue('');
      this.controlContainer.control.get('upsidePercentageToDeveloper').removeValidators(Validators.required);

      this.controlContainer.control.get('upsidePercentageToOfftaker').setValue('');
      this.controlContainer.control.get('upsidePercentageToOfftaker').removeValidators(Validators.required);
    }

    this.changeDetectorRef.detectChanges();
    this.controlContainer.control.get('upsidePercentageToOfftaker')?.updateValueAndValidity();
    this.controlContainer.control.get('upsidePercentageToDeveloper')?.updateValueAndValidity();
    this.controlContainer.control.get('discountAmount')?.updateValueAndValidity();
  }
}
