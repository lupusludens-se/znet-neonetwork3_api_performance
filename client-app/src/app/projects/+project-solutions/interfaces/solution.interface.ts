import { ResourceInterface } from '../../../core/interfaces/resource.interface';
import { CategoryInterface } from './category.interface';

export interface SolutionsResourcesInterface {
  id: number;
  name: string;
  description: string;
  slug: string;
  scope: string;
  categories: [
    {
      id: number;
      name: string;
      slug: string;
      solutionId: number;
    }
  ];
  resources: ResourceInterface[] | CategoryInterface[];
}
