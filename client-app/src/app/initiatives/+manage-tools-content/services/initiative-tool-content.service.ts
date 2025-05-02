import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from 'src/app/core/services/http.service';
import { InitiativeApiEnum } from 'src/app/initiatives/enums/initiative-api.enum';
import { InitiativeToolInterface } from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { RecommendationContentInterface } from 'src/app/initiatives/shared/models/recommendation-content.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { InitiativeSavedContentListRequestInterface } from '../../+initatives/+view-initiative/interfaces/initiative-saved-content-list-request.interface';
import { InitiativeContentsPaginationResponseInterface } from '../../interfaces/initiative-contents-pagination-response.interface';
@Injectable()
export class InitiativeToolContentService {
  readonly selectedContents: RecommendationContentInterface = {
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
    this.selectedContents.ToolContent.push({ id: discussionId, isSelected: true, initiativeId });
    return this.selectedContents.ToolContent.length;
  };

  popSelectedContents = (contentId: number): number => {
    this.selectedContents.ToolContent = this.selectedContents.ToolContent.filter(x => x.id !== contentId);
    return this.selectedContents.ToolContent.length;
  };

  getSelectedTiles = (): RecommendationContentInterface => this.selectedContents;

  clearSelectedTiles = (): void => {
    this.selectedContents.ToolContent = [];
  };

  getSavedToolsOfAnInitiative = (
    initiativeId: number,
    requestData: InitiativeSavedContentListRequestInterface
  ): Observable<PaginateResponseInterface<InitiativeToolInterface>> => {
    return this.httpService.get<PaginateResponseInterface<InitiativeToolInterface>>(
      `${InitiativeApiEnum.GetSavedInitiativeTools}/${initiativeId}`,
      requestData
    );
  };

  getRecommendedTools = (
    initiativeId: number,
    skip: number,
    take: number,
    isCreate: boolean
  ): Observable<InitiativeContentsPaginationResponseInterface<InitiativeToolInterface>> => {
    return this.httpService.get<InitiativeContentsPaginationResponseInterface<InitiativeToolInterface>>(
      `${InitiativeApiEnum.GetRecommendedTools}/${initiativeId}`,
      {
        skip,
        take,
        isCreate,
        includeCount: true
      }
    );
  };
}
