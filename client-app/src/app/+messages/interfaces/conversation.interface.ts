import { MessageInterface } from './message.interface';
import { ProjectInterface } from '../../shared/interfaces/projects/project.interface';
import { ConversationUserInterface } from './conversation-user.interface';
import { DiscussionSourceTypeEnum } from '../../shared/enums/discussion-source-type.enum';

export interface ConversationInterface {
  id: number;
  unreadCount?: number;
  subject?: string;
  project?: ProjectInterface;
  lastMessage: MessageInterface;
  users: ConversationUserInterface[];
  sourceTypeId?: DiscussionSourceTypeEnum;
}

export interface NewConversationMessageInterface {
  subject: string;
  projectId: number;
  sourceTypeId?: DiscussionSourceTypeEnum;
  companyId?: number;
  message: {
    text: string;
    attachments?: [
      {
        text: string;
        link: string;
        type: number;
        imageName: string;
      }
    ];
  };
  users: [
    {
      id: number;
    }
  ];
}
