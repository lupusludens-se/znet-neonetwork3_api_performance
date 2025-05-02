import { ResourceTypeEnum } from '../enums/resource-type.enum';
import { TagInterface } from './tag.interface';

export interface ResourceInterface {
  id: ResourceTypeEnum;
  contentTitle?: string;
  //TODO  Remove after all resource types will be defined
  referenceUrl?: string;
  typeId?: number;
  type?: string;
  technologies?: TagInterface[];
  categories?: TagInterface[];
  toolId?: number;
  articleId?: number;
}
