import { ResourceInterface } from './resource.interface';
import { TaxonomyTypeEnum } from '../../shared/enums/taxonomy-type.enum';

export interface TagInterface {
  id: number;
  name?: string;
  company?: string;
  selected?: boolean;
  isInvited?: boolean;
  technologies?: TagInterface[];
  categories?: TagInterface[];
  resources?: ResourceInterface[];
  slug?: string;
  disabled?: boolean;
  type?: TaxonomyTypeEnum;
  abbr?: string;
  sort?: number;
}
