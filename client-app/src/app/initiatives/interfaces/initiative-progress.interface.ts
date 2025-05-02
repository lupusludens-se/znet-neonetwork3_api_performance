import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { NewRecommendationCounterInterface } from '../+initatives/+view-initiative/interfaces/new-recommendation-counter';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
export interface InitiativeProgress {
  initiativeId: number;
  currentStepId: number;
  title: string;
  regions: TagInterface[];
  category: TagInterface;
  steps: InitiativeStep[];
  collaborators: UserInterface[];
  allNewExceptMessageRecommendationsCount: string;
  allNewMessageUnreadCount: string;
  recommendationsCount?: NewRecommendationCounterInterface;
  user?: UserInterface;
}

export interface InitiativeStep {
  stepId: number;
  name: string;
  description: string;
  completed?: number;
  isActive?: boolean;
  subSteps: InitiativeSubStep[];
}

export interface InitiativeSubStep {
  subStepId: number;
  title: string;
  content: string;
  order: number;
  isChecked: boolean;
}
