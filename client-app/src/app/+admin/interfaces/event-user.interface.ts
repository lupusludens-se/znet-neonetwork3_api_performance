import { ImageInterface } from '../../shared/interfaces/image.interface';

export interface EventUserInterface {
  company: string;
  id: number;
  image: ImageInterface;
  isInvited: boolean;
  isMatching: boolean;
  name: string;
  selected?: boolean;
}
