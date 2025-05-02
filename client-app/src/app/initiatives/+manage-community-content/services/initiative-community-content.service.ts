import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from 'src/app/core/services/http.service';
import { InitiativeApiEnum } from 'src/app/initiatives/enums/initiative-api.enum';
import { RecommendationContentInterface } from 'src/app/initiatives/shared/models/recommendation-content.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { InitiativeSavedContentListRequestInterface } from '../../+initatives/+view-initiative/interfaces/initiative-saved-content-list-request.interface';
import { InitiativeCommunityInterface } from '../../shared/models/initiative-resources.interface';
import { InitiativeContentsPaginationResponseInterface } from '../../interfaces/initiative-contents-pagination-response.interface';

@Injectable()
export class InitiativeCommunityContentService {
  private selectedContents: RecommendationContentInterface = {
    InitiativeId: 0,
    LearnContent: [],
    ToolContent: [],
    MessageContent: [],
    CommunityContent: [],
    ProjectContent: []
  };

  constructor(private httpService: HttpService) {}

  pushSelectedContents(initiativeId: number, userId?: number): number {
    this.selectedContents.InitiativeId = initiativeId;
    this.selectedContents.CommunityContent.push({ id: userId, isSelected: true, initiativeId });
    return this.selectedContents.CommunityContent.length;
  }

  popSelectedContents(contentId: number): number {
    this.selectedContents.CommunityContent = this.selectedContents.CommunityContent.filter(x => x.id !== contentId);
    return this.selectedContents.CommunityContent.length;
  }

  getSelectedTiles(): RecommendationContentInterface {
    return this.selectedContents;
  }

  clearSelectedTiles(): void {
    this.selectedContents.CommunityContent = [];
  }

  getSavedCommunityUsersOfAnInitiative(
    initiativeId: number,
    requestData: InitiativeSavedContentListRequestInterface
  ): Observable<PaginateResponseInterface<InitiativeCommunityInterface>> {
    return this.httpService.get<PaginateResponseInterface<InitiativeCommunityInterface>>(
      `${InitiativeApiEnum.GetSavedInitiativeCommunityUsers}/${initiativeId}`,
      requestData
    );
  }

  getRecommendedCommunityUsers(
    initiativeId: number,
    skip: number,
    take: number,
    isCreate: boolean
  ): Observable<InitiativeContentsPaginationResponseInterface<InitiativeCommunityInterface>> {
    return this.httpService.get<InitiativeContentsPaginationResponseInterface<InitiativeCommunityInterface>>(
      `${InitiativeApiEnum.GetRecommendedCommunityUsers}/${initiativeId}`,
      { skip, take, isCreate, includeCount: true }
    );
  }
}
