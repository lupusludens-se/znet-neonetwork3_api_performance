import { TagInterface } from '../../../../core/interfaces/tag.interface';

export interface EventCategoryInterface extends TagInterface {
  preSelected?: boolean;
  selected?: boolean;
}
