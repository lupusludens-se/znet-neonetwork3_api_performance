import { AttachmentInterface } from '../../shared/interfaces/attachment.interface';
import { ForumUserResponseInterface } from './forum-user-response.interface';

export interface FirstMessageInterface {
  id: number;
  text?: string;
  textContent?: string;
  createdOn: Date;
  modifiedOn: Date;
  isPinned: boolean;
  isLiked: boolean;
  likesCount: number;
  repliesCount: number;
  user: ForumUserResponseInterface;
  attachments: AttachmentInterface[];
}
