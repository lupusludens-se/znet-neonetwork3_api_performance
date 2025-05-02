import { ProjectTagInterface } from 'src/app/shared/interfaces/projects/project-tag.interface';

export interface CategoryInterface {
  id: number;
  slug: string;
  titleText: string;
  descriptionText: string;
  categoryTags: ProjectTagInterface[];
}
