import { ImageInterface } from 'src/app/shared/interfaces/image.interface';
import { UserRoleInterface } from 'src/app/shared/interfaces/user/user-role.interface';

export interface FeedbackInterface {
  id?: number;
  feedbackUser: FeedbackUserInterface;
  rating: number;
  comments: string;
  createdOn: Date;
  modifiedOn: Date;
  roles: UserRoleInterface[];
}
export interface FeedbackUserInterface {
  name: string;
  firstName: string;
  lastName: string;
  statusName?: string;
  statusId: number;
  requestDeleteDate?: Date;
  companyId: number;
  company?: string;
  image: ImageInterface;
  imageName: string;
  roles: UserRoleInterface[];
}
