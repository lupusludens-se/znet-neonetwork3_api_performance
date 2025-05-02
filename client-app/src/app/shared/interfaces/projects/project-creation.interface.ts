import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { ProjectStatusEnum } from '../../enums/projects/project-status.enum';

export interface ProjectBatteryStorageDetailsInterface {
  statusId: ProjectStatusEnum;
  minimumAnnualPeakKW: number;
  contractStructures: Partial<TagInterface>[];
  contractStructureName: string;
  minimumTermLength: number;
  valuesProvided: Partial<TagInterface>[];
  timeAndUrgencyConsiderations: string;
  additionalComments: string;
}

export interface ProjectFuelCellsDetailsInterface {
  statusId: ProjectStatusEnum;
  minimumAnnualSiteKWh: number;
  contractStructures: Partial<TagInterface>[];
  minimumTermLength: number;
  valuesProvided: Partial<TagInterface>[];
  timeAndUrgencyConsiderations: string;
  additionalComments: string;
}

export interface ProjectCarbonOffsetDetailsInterface {
  statusId: number;
  timeAndUrgencyConsiderations: string;
  additionalComments: string;
  minimumPurchaseVolume: number;
  stripLengths: Partial<TagInterface>[];
  valuesProvided: Partial<TagInterface>[];
}

export interface ProjectCommunitySolarDetailsInterface {
  statusId: number;
  timeAndUrgencyConsiderations: string;
  additionalComments: string;
  minimumAnnualMWh: number;
  totalAnnualMWh: number;
  utilityTerritory: string;
  projectAvailable: true;
  projectAvailabilityApproximateDate: string;
  isInvestmentGradeCreditOfOfftakerRequired: boolean;
  contractStructures: Partial<TagInterface>[];
  minimumTermLength: number;
  valuesProvided: Partial<TagInterface>[];
}

export interface ProjectEacPurchasingDetailsInterface {
  statusId: number;
  timeAndUrgencyConsiderations: string;
  additionalComments: string;
  minimumPurchaseVolume: number;
  stripLengths: TagInterface[] | number[];
  minimumTermLength: number;
  valuesProvided: Partial<TagInterface>[];
}

export interface ProjectEfficiencyAuditsDetailsInterface {
  statusId: number;
  timeAndUrgencyConsiderations: string;
  additionalComments: string;
  contractStructures: Partial<TagInterface>[];
  minimumTermLength: number;
  isInvestmentGradeCreditOfOfftakerRequired: boolean;
  valuesProvided: Partial<TagInterface>[];
}

export interface ProjectEfficiencyMeasuresDetailsInterface {
  statusId: number;
  timeAndUrgencyConsiderations: string;
  additionalComments: string;
  contractStructures: Partial<TagInterface>[];
  minimumTermLength: number;
  isInvestmentGradeCreditOfOfftakerRequired: boolean;
  valuesProvided: Partial<TagInterface>[];
}

export interface ProjectEmergingTechnologiesDetailsInterface {
  statusId: number;
  timeAndUrgencyConsiderations: string;
  additionalComments: string;
  minimumAnnualValue: number;
  energyUnitId: number;
  contractStructures: Partial<TagInterface>[];
  minimumTermLength: number;
  valuesProvided: Partial<TagInterface>[];
}

export interface ProjectEvChargingDetailsInterface {
  statusId: number;
  timeAndUrgencyConsiderations: string;
  additionalComments: string;
  minimumChargingStationsRequired: number;
  contractStructures: Partial<TagInterface>[];
  minimumTermLength: number;
  valuesProvided: Partial<TagInterface>[];
}

export interface ProjectOnsiteSolarDetailsInterface {
  statusId: number;
  timeAndUrgencyConsiderations: string;
  additionalComments: string;
  minimumAnnualSiteKWh: number;
  contractStructures: Partial<TagInterface>[];
  minimumTermLength: number;
  valuesProvided: Partial<TagInterface>[];
}

export interface ProjectRenewableElectricityDetailsInterface {
  statusId: number;
  timeAndUrgencyConsiderations: string;
  additionalComments: string;
  minimumAnnualSiteKWh: number;
  minimumTermLength: number;
  purchaseOptions: TagInterface[] | number[]; // !! should be changed on the BE to Taginterface on POST and GET
  valuesProvided: Partial<TagInterface>[];
}

export interface ProjectGreenTariffDetailsInterface {
  statusId: number;
  timeAndUrgencyConsiderations: string;
  additionalComments: string;
  utilityName: string;
  programWebsite: string;
  minimumPurchaseVolume: number;
  termLengthId: number;
  termLengthName: string;
  valuesProvided: Partial<TagInterface>[];
}

export interface ProjectOffsitePpaInterface {
  statusId: ProjectStatusEnum;
  latitude: number;
  longitude: number;
  isoRtoId: number;
  isoRtoName: string;
  productTypeId: number;
  productTypeName: string;
  commercialOperationDate: string;
  valuesToOfftakers: TagInterface[];
  ppaTermYearsLength: number;
  totalProjectNameplateMWACCapacity: number;
  totalProjectExpectedAnnualMWhProductionP50: number;
  minimumOfftakeMWhVolumeRequired: number;
  notesForPotentialOfftakers: string;
  settlementTypeId: number;
  settlementHubOrLoadZoneId: number;
  forAllPriceEntriesCurrencyId: number;
  contractPricePerMWh: number;
  floatingMarketSwapIndexDiscount: string;
  floatingMarketSwapFloor: string;
  floatingMarketSwapCap: string;
  pricingStructureId: number;
  eacId: number;
  eacCustom: string;
  eacValue: string;
  settlementPriceIntervalId: number;
  settlementPriceIntervalCustom: number;
  settlementCalculationIntervalId: number;
  additionalNotesForSEOperationsTeam: string;
  projectMWCurrentlyAvailable: string;
  upsidePercentageToDeveloper: string;
  upsidePercentageToOfftaker: string;
  discountAmount: number;
}
