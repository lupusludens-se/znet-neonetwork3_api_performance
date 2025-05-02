import { AttachmentTypeEnum } from '../../+messages/enums/attachment-type.enum';
import { ImageInterface } from './image.interface';

export interface AttachmentInterface {
  id: number;
  messageId: number;
  text: string;
  link: string;
  type: AttachmentTypeEnum;
  imageName?: string;
  image: ImageInterface;
}
