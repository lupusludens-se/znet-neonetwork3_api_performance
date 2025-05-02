import { UserRoleInterface } from '../../shared/interfaces/user/user-role.interface';
import { UserPermissionInterface } from '../../shared/interfaces/user/user-permission.interface';
import { UserLinksInterface } from '../../shared/interfaces/user/user-links.interface';

export interface UpdateUserInterface {
  firstName: string;
  lastName: string;
  email: string;
  username: string;
  companyId: number;
  countryId: number;
  isActive: boolean;
  isOnboarded: boolean;
  isApproved: boolean;
  imageUrl: string;
  timeZone: string;
  roles: UserRoleInterface[];
  permissions: UserPermissionInterface[];
  urlLinks: UserLinksInterface[];
}
