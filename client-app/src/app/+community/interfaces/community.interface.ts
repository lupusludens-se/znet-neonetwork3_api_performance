import { UserRoleInterface } from 'src/app/shared/interfaces/user/user-role.interface';
import { ImageInterface } from '../../shared/interfaces/image.interface';
import { CommunityCompanyInterface } from './community.category.interface';

export interface CommunityInterface {
  id: number;
  firstName: string;
  lastName: string;
  companyName: string;
  image: ImageInterface;
  type: number;
  jobTitle: string;
  memberCount: number;
  categories: CommunityCompanyInterface[];
  isFollowed: boolean;
  statusId: number;
  role: UserRoleInterface;
}
