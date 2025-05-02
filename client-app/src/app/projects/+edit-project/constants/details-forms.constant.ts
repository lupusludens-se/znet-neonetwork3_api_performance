import { CustomValidator } from '../../../shared/validators/custom.validator';
import { Validators } from '@angular/forms';
import { INT32_MAX } from 'src/app/shared/constants/math.const';

export const BatteryStorageDetailsFromControls = {
  minimumAnnualPeakKW: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  contractStructures: [''],
  minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  valuesProvided: [''],
  timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
  additionalComments: ['', Validators.maxLength(200)]
};

export const CarbonOffsetDetailsFromControls = {
  minimumPurchaseVolume: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  stripLengths: [[]],
  minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  valuesProvided: [''],
  timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
  additionalComments: ['', Validators.maxLength(200)]
};

export const CommunitySolarDetailsFromControls = {
  minimumAnnualMWh: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  totalAnnualMWh: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  utilityTerritory: ['', [Validators.maxLength(100)]],
  projectAvailable: [''],
  projectAvailabilityApproximateDate: [''],
  isInvestmentGradeCreditOfOfftakerRequired: [''],
  contractStructures: [''],
  minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  valuesProvided: [''],
  timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
  additionalComments: ['', Validators.maxLength(200)]
};

export const EacPurchasingDetailsFromControls = {
  minimumPurchaseVolume: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  stripLengths: [[]],
  minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  valuesProvided: [''],
  timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
  additionalComments: ['', Validators.maxLength(200)]
};

export const EfficiencyAuditDetailsFromControls = {
  contractStructures: [''],
  minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  isInvestmentGradeCreditOfOfftakerRequired: [null],
  valuesProvided: [''],
  timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
  additionalComments: ['', Validators.maxLength(200)]
};

export const EfficiencyMeasuresDetailsFromControls = {
  contractStructures: [''],
  isInvestmentGradeCreditOfOfftakerRequired: [null],
  minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  valuesProvided: [''],
  timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
  additionalComments: ['', Validators.maxLength(200)]
};

export const EmergingTechnologiesDetailsFromControls = {
  contractStructures: [''],
  minimumAnnualValue: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  energyUnit: [''],
  energyUnitId: [''],
  minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  valuesProvided: [''],
  timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
  additionalComments: ['', Validators.maxLength(200)]
};

export const EvChargingDetailsFromControls = {
  contractStructures: [''],
  minimumChargingStationsRequired: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  valuesProvided: [''],
  timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
  additionalComments: ['', Validators.maxLength(200)]
};

export const FuelCellsDetailsFromControls = {
  minimumAnnualSiteKWh: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  contractStructures: [''],
  minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  valuesProvided: [''],
  timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
  additionalComments: ['', Validators.maxLength(200)]
};

export const GreenTariffDetailsFromControls = {
  utilityName: ['', [Validators.maxLength(100)]],
  programWebsite: ['', [CustomValidator.url, Validators.maxLength(2048)]],
  minimumPurchaseVolume: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  termLengthId: [''],
  valuesProvided: [''],
  timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
  additionalComments: ['', Validators.maxLength(200)]
};

export const OffsitePpaDetailsFromControls = {
  isoRtoId: [''],
  customIsoRto: [''],
  productTypeId: [''],
  latitude: [''],
  longitude: [''],
  isoRto: [''],
  productType: [''],
  commercialOperationDate: [''],
  valuesToOfftakers: [[]],
  ppaTermYearsLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  totalProjectNameplateMWACCapacity: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  totalProjectExpectedAnnualMWhProductionP50: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  minimumOfftakeMWhVolumeRequired: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  notesForPotentialOfftakers: ['', Validators.maxLength(500)],
  /* PRIVATE*/
  customSettlementHubOrLoadZone: [''],
  settlementTypeId: [''],
  settlementHubOrLoadZoneId: [''],
  forAllPriceEntriesCurrencyId: [''],
  pricingStructureId: [''],
  eacId: [''],
  settlementPriceIntervalId: [''],
  settlementCalculationIntervalId: [''],
  customEAC: [''],
  settlementType: [''],
  settlementHubOrLoadZone: [''],
  forAllPriceEntriesCurrency: [''],
  contractPricePerMWh: ['', [Validators.maxLength(100), CustomValidator.floatNumber]],
  floatingMarketSwapIndexDiscount: ['', Validators.maxLength(100)],
  floatingMarketSwapFloor: ['', Validators.maxLength(100)],
  floatingMarketSwapCap: ['', Validators.maxLength(100)],
  pricingStructure: [''],
  upsidePercentageToDeveloper: ['', [Validators.min(0), Validators.max(100)]],
  upsidePercentageToOfftaker: ['', [Validators.min(0), Validators.max(100)]],
  eac: [''],
  eacCustom: ['', Validators.maxLength(100)],
  eacValue: ['', Validators.maxLength(100)],
  settlementPriceInterval: [''],
  settlementPriceIntervalCustom: ['', Validators.maxLength(100)],
  settlementCalculationInterval: [''],
  additionalNotesForSEOperationsTeam: ['', Validators.maxLength(500)],
  projectMWCurrentlyAvailable: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  discountAmount: ['']
};

export const OnsiteSolarDetailsFromControls = {
  minimumAnnualSiteKWh: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  contractStructures: [''],
  minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  valuesProvided: [''],
  timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
  additionalComments: ['', Validators.maxLength(200)]
};

export const RenewableElectricityDetailsFromControls = {
  minimumAnnualSiteKWh: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  purchaseOptions: [''],
  minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
  valuesProvided: [''],
  timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
  additionalComments: ['', Validators.maxLength(200)]
};
