import { ResourceInterface } from '../../../core/interfaces/resource.interface';

export interface TechnologiesResourcesInterface {
  id: number;
  name: string;
  description: string;
  slug: string;
  categories: [
    {
      id: number;
      name: string;
      slug: string;
      solutionId: number;
    }
  ];
  resources: ResourceInterface[];
}
