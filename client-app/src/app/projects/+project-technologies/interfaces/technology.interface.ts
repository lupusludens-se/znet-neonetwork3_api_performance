import { ProjectTagInterface } from 'src/app/shared/interfaces/projects/project-tag.interface';

export interface TechnologyInterface {
  id: number;
  slug: string;
  titleText: string;
  descriptionText: string;
  techologyTags: ProjectTagInterface[];
}
