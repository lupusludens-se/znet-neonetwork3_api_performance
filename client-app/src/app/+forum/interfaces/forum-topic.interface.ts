import { DiscussionTypeEnum } from '../enums/discussion-type.enum';
import { FirstMessageInterface } from './first-message.interface';
import { ForumUserResponseInterface } from './forum-user-response.interface';
import { TagInterface } from '../../core/interfaces/tag.interface';

export interface ForumTopicInterface {
  id: number;
  subject: string;
  type: DiscussionTypeEnum;
  isPrivate: boolean;
  isPinned: boolean;
  isSaved: boolean;
  responsesCount: number;
  isFollowed: boolean;
  firstMessage: FirstMessageInterface;
  users: ForumUserResponseInterface[];
  categories: TagInterface[];
  regions: TagInterface[];
}
