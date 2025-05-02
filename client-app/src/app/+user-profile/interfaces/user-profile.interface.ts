import { UserInterface } from '../../shared/interfaces/user/user.interface';
import { UserLinksInterface } from '../../shared/interfaces/user/user-links.interface';
import { CountryInterface } from '../../shared/interfaces/user/country.interface';
import { TagInterface } from '../../core/interfaces/tag.interface';
import { MemberInterface } from 'src/app/shared/modules/members-list/interfaces/member.interface';
import { SkillsByCategoryInterface } from 'src/app/shared/interfaces/user/skills-by-category.interface';

export interface UserProfileInterface {
  about: string;
  categories: TagInterface[];
  followersCount: number;
  isFollowed: boolean;
  id: number;
  jobTitle: string;
  linkedInUrl: string;
  regions: CountryInterface[];
  responsibilityId: number;
  responsibilityName: string;
  solutions: string[];
  state: CountryInterface; // TODO: add specific type returned from api
  stateId: number;
  technologies: [];
  urlLinks: UserLinksInterface[];
  user: UserInterface;
  userId: number;
  followers: MemberInterface[];
  skillsByCategory:SkillsByCategoryInterface[];
}
