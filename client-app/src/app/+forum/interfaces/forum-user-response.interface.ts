import { ImageInterface } from '../../shared/interfaces/image.interface';

export interface ForumUserResponseInterface {
  id: number;
  name: string;
  company: string;
  jobTitle: string;
  isFollowed: boolean;
  image: ImageInterface;
  statusId: number;
}
