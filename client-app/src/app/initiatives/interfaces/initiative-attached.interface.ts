import { InitiativeModulesEnum } from '../enums/initiative-modules.enum';

export interface InitiativeAttachedContent {
  initiativeId: number;
  isAttached: boolean; 
  initiativeName: string;
  isChecked: boolean;
}

export interface InitiativeAutoAttachedDetails {
	contentType: InitiativeModulesEnum;
  contentId: number;
  isAdded: boolean;
}
