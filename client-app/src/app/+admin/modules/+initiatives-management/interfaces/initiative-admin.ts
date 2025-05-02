import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';

export interface InitiativeAdminResponse {
  id: number;
  title: string;
  userName: string;
  category: TagInterface;
  companyName: string;
  phase: string;
  statusId: number;
  statusName: string;
  modifiedOn: Date;
  regions: TagInterface[];
  regionNames: string;
  user: UserInterface;
}
