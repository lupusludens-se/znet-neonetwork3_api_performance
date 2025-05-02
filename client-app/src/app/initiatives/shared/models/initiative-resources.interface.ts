import { ConversationUserInterface } from 'src/app/+messages/interfaces/conversation-user.interface';
import { MessageInterface } from 'src/app/+messages/interfaces/message.interface';
import { PostTypeEnum } from 'src/app/core/enums/post-type.enum';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { DiscussionSourceTypeEnum } from 'src/app/shared/enums/discussion-source-type.enum';
import { ImageInterface } from 'src/app/shared/interfaces/image.interface';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';
import { CompanyInterface } from 'src/app/shared/interfaces/user/company.interface';
import { UserRoleInterface } from 'src/app/shared/interfaces/user/user-role.interface';
import { CountryInterface } from 'src/app/shared/interfaces/user/country.interface';

export interface BaseInitiativeContentInterface {
  id: number; //corresponding to ArticleId, ToolsId, etc...
  initiativeId: number;
  isSelected: boolean;
  isNew?: boolean;
}

export interface InitiativeArticleInterface extends BaseInitiativeContentInterface {
  title?: string;
  imageUrl?: string;
  categories?: TagInterface[];
  typeId?: PostTypeEnum;
  tagsTotalCount?: number;
}

export interface InitiativeMessageInterface extends BaseInitiativeContentInterface {
  unreadCount?: number;
  subject?: string;
  project?: ProjectInterface;
  lastMessage: MessageInterface;
  users: ConversationUserInterface[];
  sourceTypeId?: DiscussionSourceTypeEnum;
}

export interface InitiativeToolInterface extends BaseInitiativeContentInterface {
  title?: string;
  imageUrl?: ImageInterface;
  description?: string;
}

export interface InitiativeCommunityInterface extends BaseInitiativeContentInterface {
  jobTitle: string;
  typeId?: PostTypeEnum;
  tagsTotalCount?: number;
  firstName: string;
  lastName: string;
  companyName: string;
  imageName: string;
  image: ImageInterface;
  roles: UserRoleInterface[];
  categories: TagInterface[];
  regions: TagInterface[];
}

export interface InitiativeProjectInterface extends BaseInitiativeContentInterface {
  title: string;
  subTitle: string;
  company: CompanyInterface;
  category: TagInterface;
  regions: CountryInterface[];
  isNew: boolean;
}
