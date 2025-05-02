import { TagInterface } from '../../../../core/interfaces/tag.interface';

export interface TaxonomiesPayloadInterface {
  categories: TagInterface[];
  solutions: TagInterface[];
  technologies: TagInterface[];
  regions: TagInterface[];
}
