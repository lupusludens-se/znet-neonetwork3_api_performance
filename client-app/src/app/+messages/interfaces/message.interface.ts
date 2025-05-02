import { AttachmentInterface } from '../../shared/interfaces/attachment.interface';
import { ConversationUserInterface } from './conversation-user.interface';

export interface MessageInterface {
  id?: number;
  text: string;
  createdOn?: Date;
  user?: ConversationUserInterface;
  attachments: AttachmentInterface[];
  showActions?: boolean;
  showEdit?: boolean;
  statusId?: number;
  modifiedOn?: Date;
  showRequiredMessage?: boolean;
}
