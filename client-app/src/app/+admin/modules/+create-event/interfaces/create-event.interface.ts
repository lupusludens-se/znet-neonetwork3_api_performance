import { EventOccurrenceInterface } from 'src/app/shared/interfaces/event/event-occurrence.interface';
import { EventLinkInterface } from '../../../../shared/interfaces/event/event-link.interface';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { UserInterface } from '../../../../shared/interfaces/user/user.interface';
import { EventInviteType } from '../enums/event-invite-type';
import { EventTypeEnum } from 'src/app/shared/interfaces/event/event.interface';

export interface CreateEventInterface {
  id?: number;
  subject: string;
  description: string;
  highlights: string;
  isHighlighted: boolean;
  location: string;
  locationType: number;
  userRegistration: string;
  timeZoneId: number;
  recordings: { url: string }[];
  links: EventLinkInterface[];
  categories: [{ id: number }];
  occurrences: EventOccurrenceInterface[];
  moderators: {
    name: string;
    company: string;
    userId: number;
    user?: UserInterface;
  }[];
  invitedRoles: { id: number }[];
  invitedRegions: TagInterface[];
  invitedCategories: TagInterface[];
  invitedUsers: TagInterface[];
  inviteType: EventInviteType;
  eventType: EventTypeEnum;
  showInPublicSite: boolean;
}
