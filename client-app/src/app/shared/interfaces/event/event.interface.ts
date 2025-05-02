import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { EventLocationType } from '../../enums/event/event-location-type.enum';
import { TimezoneInterface } from '../common/timezone.interface';
import { EventLinkInterface } from './event-link.interface';
import { EventModeratorInterface } from './event-moderator.interface';
import { EventOccurrenceInterface } from './event-occurrence.interface';
import { EventUserInterface } from './event-user.interface';

export interface EventInterface {
  id: number;
  subject: string;
  description: string;
  highlights?: string;
  isAttending?: boolean;
  isHighlighted: string;
  location: string;
  timeZoneId: number;
  timeZone: TimezoneInterface;
  locationType: EventLocationType;
  userRegistration?: string;
  recordings: { url: string }[];
  links: EventLinkInterface[];
  moderators: EventModeratorInterface[];
  categories: TagInterface[];
  occurrences: EventOccurrenceInterface[];
  attendees: EventUserInterface[];
  shortDates?: string;
  shortTimes?: string;
  moreDatesCount?: number;
  moreOccurrencesCount?: number;
  eventType: EventTypeEnum;
  showInPublicSite: boolean;
}

export enum EventTypeEnum {
  Private = 1,
  Public
}
