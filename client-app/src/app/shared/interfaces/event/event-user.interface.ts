import { ImageInterface } from 'src/app/shared/interfaces/image.interface';

export interface EventUserInterface {
  id: number;
  firstName: string;
  lastName: string;
  name: string;
  company: string;
  isFollowed?: boolean;
  image?: ImageInterface;
  statusId: number;
}
