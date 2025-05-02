import { TagInterface } from './tag.interface';

export interface CompanyAnnouncementInterface {
  id?: number;
  title: string;
  link: string;
  scaleId: number;
  regionIds: number[];
  regions?: TagInterface[];
  companyId?: number;
  modifiedOn?: Date;
}
