import { UserProfileInterface } from 'src/app/+user-profile/interfaces/user-profile.interface';
import { ImageInterface } from 'src/app/shared/interfaces/image.interface';
import { CountryInterface } from 'src/app/shared/interfaces/user/country.interface';

export interface MemberInterface {
  id: number;
  firstName: string;
  lastName: string;
  company?: string;
  isFollowed: boolean;
  image?: ImageInterface;
  statusId: number;
  followersCount?: number;
  userProfile?: UserProfileInterface;
  country?: CountryInterface;
}
