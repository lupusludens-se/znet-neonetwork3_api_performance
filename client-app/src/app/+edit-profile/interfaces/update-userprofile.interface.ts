import { SkillsByCategoryInterface } from 'src/app/shared/interfaces/user/skills-by-category.interface';
import { TagInterface } from '../../core/interfaces/tag.interface';
import { UserLinksInterface } from '../../shared/interfaces/user/user-links.interface';

export interface UpdateUserprofileInterface {
  jobTitle: string;
  userId: number;
  linkedInUrl: string;
  about: string;
  countryId: number;
  stateId: number;
  categories: TagInterface[];
  solutions: TagInterface[];
  technologies: TagInterface[];
  regions: TagInterface[];
  urlLinks: UserLinksInterface[];
  skillsByCategory: SkillsByCategoryInterface[];
}
