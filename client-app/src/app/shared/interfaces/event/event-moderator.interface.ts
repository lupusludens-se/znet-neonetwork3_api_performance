import { EventUserInterface } from './event-user.interface';

export interface EventModeratorInterface {
  id: number;
  name?: string;
  company?: string;
  userId?: number;
  user?: EventUserInterface;
  statusId: number;
}
