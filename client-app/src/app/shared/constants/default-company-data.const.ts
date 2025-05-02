import { CompanyRolesEnum } from '../enums/company-roles.enum';
import { CompanyStatusEnum } from '../enums/company-status.enum';
import { CompanyInterface } from '../interfaces/user/company.interface';
import { TagInterface } from '../../core/interfaces/tag.interface';

export const DEFAULT_COMPANY: Partial<CompanyInterface> = {
  id: 1,
  name: 'Schneider Electric',
  statusId: CompanyStatusEnum.Active,
  typeId: CompanyRolesEnum.Corporation
};

export const ZEIGO_NETWORK_TEAM: Partial<CompanyInterface> = {
  name: 'Zeigo Network Team',
  statusId: CompanyStatusEnum.Active,
  typeId: CompanyRolesEnum.SolutionProvider
};

export const DEFAULT_HEARD_VIA: TagInterface = {
  id: 8,
  name: 'I am an Employee'
};
export const DEFAULT_TIMEZONE:any={
  
  name:'(UTC-05:00) Eastern Time (US & Canada)'
}
