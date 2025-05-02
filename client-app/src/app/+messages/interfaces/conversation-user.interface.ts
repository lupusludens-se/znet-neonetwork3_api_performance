import { ImageInterface } from '../../shared/interfaces/image.interface';

export interface ConversationUserInterface {
  id: number;
  name?: string;
  company?: string;
  image?: ImageInterface;
  isSolutionProvider?: boolean;
  statusId: number;
}
