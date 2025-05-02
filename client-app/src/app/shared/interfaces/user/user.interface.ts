import { UserRoleInterface } from './user-role.interface';
import { CompanyInterface } from './company.interface';
import { UserPermissionInterface } from './user-permission.interface';
import { UserProfileInterface } from '../../../+user-profile/interfaces/user-profile.interface';
import { ImageInterface } from '../image.interface';
import { CountryInterface } from './country.interface';
import { TimezoneInterface } from '../common/timezone.interface';
import {
  EmailAlertInterface,
  EmailAlertRequestInterface
} from 'src/app/+admin/modules/+email-alerts/models/email-alert';

export interface UserInterface {
  id?: number;
  firstName: string;
  lastName: string;
  email: string;
  statusId?: number;
  statusName?: string;
  companyId: number;
  company?: CompanyInterface;
  country: CountryInterface;
  countryId: number;
  image: ImageInterface;
  imageName: string;
  heardViaId: number;
  heardViaName: string;
  timeZone?: TimezoneInterface;
  roles: UserRoleInterface[];
  permissions?: UserPermissionInterface[];
  userProfile?: UserProfileInterface;
  userName: string;
  timeZoneId?: number;
  requestDeleteDate?: string;
  isPrivateUser?: boolean;
  adminComments?: string;
  approvedBy?: string;
  emailAlerts?: EmailAlertInterface[] | EmailAlertRequestInterface[];
}

export interface UserCollaboratorInterface {
  id?: number;
  firstName: string;
  lastName: string;
  name: string;
  email: string;
  statusId?: number;
  statusName?: string;
  companyId: number;
  selected: boolean;
  roles: UserRoleInterface[];
}

export interface UserWrapperDataInterface {
  count: number;
  dataList: UserCollaboratorInterface[];
}
