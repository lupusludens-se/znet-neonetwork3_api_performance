import { TagInterface } from '../../../core/interfaces/tag.interface';

export interface CountryInterface {
  id: number;
  name?: string;
  code?: string; // * 2 characters
  code3?: string; // * 3 characters
  states?: TagInterface[];
  isInvited?: boolean; // * for events
  selected?: boolean;
  parentId?: number;
  slug?: string;
}

export interface CountryCheckboxInterface extends CountryInterface {
  disabled?: boolean;
}
