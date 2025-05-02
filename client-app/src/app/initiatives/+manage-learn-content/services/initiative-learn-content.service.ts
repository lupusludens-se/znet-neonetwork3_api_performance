import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpService } from "src/app/core/services/http.service";
import { InitiativeApiEnum } from "src/app/initiatives/enums/initiative-api.enum";
import { RecommendationContentInterface } from "src/app/initiatives/shared/models/recommendation-content.interface";
import { PaginateResponseInterface } from "src/app/shared/interfaces/common/pagination-response.interface";
import { InitiativeArticleInterface } from "src/app/initiatives/shared/models/initiative-resources.interface";
import { InitiativeModulesEnum } from "src/app/initiatives/enums/initiative-modules.enum";
import { InitiativeSavedContentListRequestInterface } from "../../+initatives/+view-initiative/interfaces/initiative-saved-content-list-request.interface";
import { InitiativeContentsPaginationResponseInterface } from "../../interfaces/initiative-contents-pagination-response.interface";

@Injectable()
export class InitiativeLearnContentService {
  private selectedContents: RecommendationContentInterface = {
    InitiativeId: 0,
    LearnContent: [],
    ToolContent: [],
    MessageContent: [],
    CommunityContent: [],
    ProjectContent: []
  };

  constructor(private httpService: HttpService) {}

  pushSelectedContents(initiativeId: number, discussionId?: number): number {
    this.selectedContents.InitiativeId = initiativeId;
    this.selectedContents.LearnContent.push({ id: discussionId, isSelected: true, initiativeId });
    return this.selectedContents.LearnContent.length;
  }

  popSelectedContents(contentId: number): number {
    this.selectedContents.LearnContent = this.selectedContents.LearnContent.filter(x => x.id !== contentId);
    return this.selectedContents.LearnContent.length;
  }

  getSelectedTiles(): RecommendationContentInterface {
    return this.selectedContents;
  }

  clearSelectedTiles(): void {
    this.selectedContents.LearnContent = [];
  }

  getSavedArticlesOfAnInitiative(
    initiativeId: number,
    requestData: InitiativeSavedContentListRequestInterface
  ): Observable<PaginateResponseInterface<InitiativeArticleInterface>> {
    return this.httpService.get<PaginateResponseInterface<InitiativeArticleInterface>>(
      `${InitiativeApiEnum.GetSavedInitiativeArticle}/${initiativeId}`,
      requestData
    );
  }

  getRecommendedArticles(
    initiativeId: number,
    skip: number,
    take: number,
    isCreate: boolean
  ): Observable<InitiativeContentsPaginationResponseInterface<InitiativeArticleInterface>> {
    return this.httpService.get<InitiativeContentsPaginationResponseInterface<InitiativeArticleInterface>>(
      `${InitiativeApiEnum.GetRecommendedArticles}/${initiativeId}`,
      { skip, take, isCreate, includeCount: true }
    );
  }

  updateInitiativeContentLastViewedDate(
    initiativeId: number,
    resourceType: InitiativeModulesEnum
  ): Observable<boolean> {
    const requestData = { initiativeId, contentType: resourceType };
    return this.httpService.post(InitiativeApiEnum.UpdateInitiativeContentLastViewedDate, requestData);
  }
}
