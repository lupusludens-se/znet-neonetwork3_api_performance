import { ImageInterface } from '../../shared/interfaces/image.interface';
import { ProjectInterface } from '../../shared/interfaces/projects/project.interface';
import { UserInterface } from '../../shared/interfaces/user/user.interface';
import { CategoryResponseInterface } from './category-response.interface';
import { CountryInterface } from '../../shared/interfaces/user/country.interface';
import { MemberInterface } from 'src/app/shared/modules/members-list/interfaces/member.interface';

export interface CompanyProfileInterface {
  id: number;
  name: string;
  statusId: number;
  statusName: string;
  typeId: CompanyType;
  typeName: string;
  imageLogo: string;
  image: ImageInterface;
  companyUrl: string;
  linkedInUrl: string;
  about: string;
  countryId: number;
  country: CountryInterface;
  industryId: number;
  industryName: string;
  isFollowed: boolean;
  followersCount: number;
  followers?: CompanyFollowerInterface[];
  projects: ProjectInterface[];
  users: UserInterface[];
  urlLinks: { urlLink: string; urlName: string }[];
  offsitePPAs: { id: number }[];
  categories: CategoryResponseInterface[];
}

export interface CompanyFollowerInterface {
  id: number;
  followerId: number;
  follower: MemberInterface;
  companyId: number;
}

export enum CompanyType {
  Corporation = 1,
  SolutionProvider = 2
}
