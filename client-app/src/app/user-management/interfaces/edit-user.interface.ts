import { UserRoleInterface } from '../../shared/interfaces/user/user-role.interface';
import { UserPermissionInterface } from '../../shared/interfaces/user/user-permission.interface';

export interface EditUserInterface {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  companyId: number;
  isActive: boolean;
  isOnboarded: boolean;
  imageUrl: string;
  timeZone: string;
  roles: UserRoleInterface[];
  permissions: UserPermissionInterface[];
  userName: string;
}
