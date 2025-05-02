import { CountryInterface } from 'src/app/shared/interfaces/user/country.interface';
import { CompanyInterface } from '../../shared/interfaces/user/company.interface';
import { UserRoleInterface } from '../../shared/interfaces/user/user-role.interface';

export interface PendingUserInterface {
  email: string;
  company: CompanyInterface;
  companyId: number;
  companyName: string;
  country: CountryInterface;
  countryId: number;
  createdDate: string;
  firstName: string;
  id: number;
  isDenied: boolean;
  heardViaId: number;
  heardViaName: string;
  lastName: string;
  role: UserRoleInterface;
  roleId: number;
  adminComments: string;
  joiningInterestDetails?: string;
}
