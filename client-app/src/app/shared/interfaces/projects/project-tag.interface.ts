import { ResourceTypeEnum } from 'src/app/core/enums/resource-type.enum';

export interface ProjectTagInterface {
  tagTitle: string;
  slug: string;
  tagType: ResourceTypeEnum;
  link: string;
}
