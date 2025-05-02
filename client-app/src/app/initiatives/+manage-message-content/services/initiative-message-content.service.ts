import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpService } from "src/app/core/services/http.service";
import { InitiativeApiEnum } from "src/app/initiatives/enums/initiative-api.enum";
import { InitiativeMessageInterface } from "src/app/initiatives/shared/models/initiative-resources.interface";
import { RecommendationContentInterface } from "src/app/initiatives/shared/models/recommendation-content.interface";
import { PaginateResponseInterface } from "src/app/shared/interfaces/common/pagination-response.interface";
import { InitiativeSavedContentListRequestInterface } from "../../+initatives/+view-initiative/interfaces/initiative-saved-content-list-request.interface";

@Injectable()
export class InitiativeMessageContentService {
  selectedContents: RecommendationContentInterface = {
    InitiativeId: 0,
    LearnContent: [],
    ToolContent: [],
    MessageContent: [],
    CommunityContent: [],
    ProjectContent: []
  };

  constructor(private httpService: HttpService) {}

  pushSelectedContents = (initiativeId: number, discussionId?: number): number => {
    this.selectedContents.InitiativeId = initiativeId;
    this.selectedContents.MessageContent.push({ id: discussionId, isSelected: true, initiativeId });
    return this.selectedContents.MessageContent.length;
  };

  popSelectedContents = (contentId: number): number => {
    this.selectedContents.MessageContent = this.selectedContents.MessageContent.filter(x => x.id !== contentId);
    return this.selectedContents.MessageContent.length;
  };

  getSelectedTiles = (): RecommendationContentInterface => this.selectedContents;

  clearSelectedTiles = (): void => {
    this.selectedContents.MessageContent = [];
  };

  getSavedMessages = (
    initiativeId: number,
    requestData: InitiativeSavedContentListRequestInterface
  ): Observable<PaginateResponseInterface<InitiativeMessageInterface>> => {
    return this.httpService.get<PaginateResponseInterface<InitiativeMessageInterface>>(
      `${InitiativeApiEnum.GetSavedInitiativeConversations}/${initiativeId}`,
      requestData
    );
  };

  getRecommendedMessages = (
    initiativeId: number,
    skip: number,
    take: number,
    isCreate: boolean
  ): Observable<PaginateResponseInterface<InitiativeMessageInterface>> => {
    return this.httpService.get<PaginateResponseInterface<InitiativeMessageInterface>>(
      `${InitiativeApiEnum.GetRecommendedConversations}/${initiativeId}`, {
      skip,
      take,
      isCreate,
      includeCount: true
    });
  };
}
