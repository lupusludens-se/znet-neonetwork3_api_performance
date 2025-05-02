import { BaseInitiativeContentInterface } from "./initiative-resources.interface";


export interface RecommendationContentInterface {
  InitiativeId: number;
  LearnContent: BaseInitiativeContentInterface[];
  MessageContent: BaseInitiativeContentInterface[];
  ToolContent: BaseInitiativeContentInterface[];
  CommunityContent: BaseInitiativeContentInterface[];
  ProjectContent: BaseInitiativeContentInterface[];
}
