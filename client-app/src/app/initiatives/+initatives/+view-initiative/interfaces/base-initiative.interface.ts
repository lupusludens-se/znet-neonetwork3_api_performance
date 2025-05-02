import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { UserCollaboratorInterface } from 'src/app/shared/interfaces/user/user.interface';

export interface BaseInitiativeInterface {
  initiativeId: number;
  title: string;
  regions: TagInterface[];
  category: string;
  scaleId: number;
  user: UserCollaboratorInterface;
  collaborators: UserCollaboratorInterface[];
}
 