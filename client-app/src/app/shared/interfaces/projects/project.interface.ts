import { CompanyInterface } from '../user/company.interface';
import { UserInterface } from '../user/user.interface';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { CountryInterface } from '../user/country.interface';
import {
  ProjectBatteryStorageDetailsInterface,
  ProjectCarbonOffsetDetailsInterface,
  ProjectCommunitySolarDetailsInterface,
  ProjectEacPurchasingDetailsInterface,
  ProjectEfficiencyAuditsDetailsInterface,
  ProjectEfficiencyMeasuresDetailsInterface,
  ProjectEmergingTechnologiesDetailsInterface,
  ProjectEvChargingDetailsInterface,
  ProjectFuelCellsDetailsInterface,
  ProjectGreenTariffDetailsInterface,
  ProjectOffsitePpaInterface,
  ProjectOnsiteSolarDetailsInterface,
  ProjectRenewableElectricityDetailsInterface
} from './project-creation.interface';
import { ProjectStatusEnum } from '../../enums/projects/project-status.enum';

export interface ProjectInterface {
  id: number;
  title: string;
  subTitle: string;
  description: string;
  descriptionText?: string;
  company: CompanyInterface;
  isPinned: boolean;
  isSaved: boolean;
  category: TagInterface;
  categoryId: number;
  companyId: number;
  owner: UserInterface;
  ownerId: number;
  opportunity?: string;
  opportunityText?: string;
  publishedOn: string;
  changedOn: string;
  publishedBy?: UserInterface;
  regions: CountryInterface[];
  statusId: ProjectStatusEnum;
  statusName: string;
  technologies: TagInterface[];
  projectDetails:
    | ProjectBatteryStorageDetailsInterface
    | ProjectFuelCellsDetailsInterface
    | ProjectOffsitePpaInterface
    | Partial<ProjectOffsitePpaInterface>
    | Partial<ProjectCarbonOffsetDetailsInterface>
    | Partial<ProjectCommunitySolarDetailsInterface>
    | Partial<ProjectEacPurchasingDetailsInterface>
    | Partial<ProjectEfficiencyAuditsDetailsInterface>
    | Partial<ProjectEfficiencyMeasuresDetailsInterface>
    | Partial<ProjectEmergingTechnologiesDetailsInterface>
    | Partial<ProjectEvChargingDetailsInterface>
    | Partial<ProjectOnsiteSolarDetailsInterface>
    | Partial<ProjectRenewableElectricityDetailsInterface>
    | Partial<ProjectGreenTariffDetailsInterface>;
  projectCategoryImage?: string;
}

export interface ProjectResourceInterface {
  id: number;
  technologies: TagInterface[];
  category: TagInterface;
}

export interface NewTrendingProjectResponse {
  id: number;
  title: string;
  subTitle: string;
  description: string;
  tag: string;
  companyImage: string;
  imageUrl?: string;
  projectCategory: TagInterface;
  projectCategoryImage: string;
  projectCategorySlug: string;
  trendingOrNewTag?: string;
  geography?: string;
  technologies: string[];
  technologyImageSlug?: string;
}
export interface SPDashboardProjectDetails {
  id?: number;
  title?: string;
  subTitle?: string;
  description?: string;
  status?: string;
  imageUrl?: string;
  projectCategory?: TagInterface;
  projectCategoryImage?: string;
  projectCategorySlug?: string;
  changedOn?: string;
  technologies?: TagInterface[];
}

export interface SPCompanyProjectInterface {
  id: number;
  title: string;
  subTitle: string;
  description: string;
  company: CompanyInterface;
  isSaved: boolean;
  category: TagInterface;
  categoryId: number;
  companyId: number;
  owner: UserInterface;
  ownerId: number;
  changedOn: string;
  regions: CountryInterface[];
  imageUrl?: string;
  statusId: ProjectStatusEnum;
  statusName: string;
  technologies: TagInterface[];
  projectCategoryImage?: string;
  projectCategorySlug?: string;
}
