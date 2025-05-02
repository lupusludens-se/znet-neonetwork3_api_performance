import { ImageInterface } from '../../../../shared/interfaces/image.interface';

export interface AnnouncementInterface {
  backgroundImageName: string;
  oldBackgroundImageName: string;
  backgroundImage: ImageInterface;
  name: string;
  buttonText: string;
  buttonUrl: string;
  isActive: boolean;
  audienceId: number;
  audience: string;
  id: number;
}
