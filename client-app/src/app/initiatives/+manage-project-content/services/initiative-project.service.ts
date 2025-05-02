import { Injectable } from '@angular/core';
import { RecommendationContentInterface } from '../../shared/models/recommendation-content.interface';
import { HttpService } from 'src/app/core/services/http.service';
import { InitiativeSavedContentListRequestInterface } from '../../+initatives/+view-initiative/interfaces/initiative-saved-content-list-request.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { Observable } from 'rxjs';
import { InitiativeProjectInterface } from '../../shared/models/initiative-resources.interface';
import { InitiativeApiEnum } from '../../enums/initiative-api.enum';
import { InitiativeContentsPaginationResponseInterface } from '../../interfaces/initiative-contents-pagination-response.interface';

@Injectable({
  providedIn: 'root'
})
export class InitiativeProjectService {
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
    this.selectedContents.ProjectContent.push({ id: discussionId, isSelected: true, initiativeId });
    return this.selectedContents.ProjectContent.length;
  };

  popSelectedContents = (contentId: number): number => {
    this.selectedContents.ProjectContent = this.selectedContents.ProjectContent.filter(x => x.id !== contentId);
    return this.selectedContents.ProjectContent.length;
  };

  getSelectedTiles = () => this.selectedContents;

  clearSelectedTiles = () => {
    this.selectedContents.ProjectContent = [];
  };

  getSavedPojectsOfAnInitiative = (
    initiativeId: number,
    requestData: InitiativeSavedContentListRequestInterface
  ): Observable<PaginateResponseInterface<InitiativeProjectInterface>> => {
    return this.httpService.get<PaginateResponseInterface<InitiativeProjectInterface>>(
      `${InitiativeApiEnum.GetSavedInitiativeProjects}/${initiativeId}`,
      requestData
    );
  };

  getRecommendedProjects = (initiativeId: number, skip: number, take: number, isCreate: boolean) => {
    return this.httpService.get<InitiativeContentsPaginationResponseInterface<InitiativeProjectInterface>>(
      `${InitiativeApiEnum.GetRecommendedProjects}/${initiativeId}`,
      {
        skip,
        take,
        isCreate,
        includeCount: true
      }
    );
  };
}
