import { UserRoleInterface } from './user/user-role.interface';
import { CompanyInterface } from './user/company.interface';
import { ImageInterface } from './image.interface';

export class ToolInterface {
  id?: number;
  title: string;
  description?: string;
  icon: ImageInterface;
  iconName: string;
  toolUrl?: string;
  roles: UserRoleInterface[];
  companies: CompanyInterface[];
  isPinned: boolean;
  isActive?: boolean;
  toolHeight: number;
}
