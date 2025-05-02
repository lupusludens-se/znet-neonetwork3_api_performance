import { UserPermissionInterface } from './user-permission.interface';

export interface UserRoleInterface {
  id: number;
  isSpecial: boolean;
  name: string;
  permissions: UserPermissionInterface[];
  checked?: boolean;
  disabled?: boolean;
}
