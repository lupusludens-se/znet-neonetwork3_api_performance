import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { HttpService } from 'src/app/core/services/http.service';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { InitiativeApiEnum } from '../../enums/initiative-api.enum';
import { BaseInitiativeInterface } from '../../+initatives/+view-initiative/interfaces/base-initiative.interface';
import { RecommendationContentInterface } from '../../shared/models/recommendation-content.interface';
import {
  InitiativeArticleInterface,
  InitiativeMessageInterface,
  InitiativeToolInterface,
  InitiativeCommunityInterface,
  InitiativeProjectInterface
} from '../../shared/models/initiative-resources.interface';
import { InitiativeAutoAttachedDetails } from '../../interfaces/initiative-attached.interface';
import { InitiativeModulesEnum } from '../../enums/initiative-modules.enum';

@Injectable({
  providedIn: 'root'
})
export class CreateInitiativeService {
  autoAttached: InitiativeAutoAttachedDetails;
  hasContentLoaded$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  selectedContents: RecommendationContentInterface = {
    InitiativeId: 0,
    LearnContent: [],
    ToolContent: [],
    MessageContent: [],
    CommunityContent: [],
    ProjectContent: []
  };

  constructor(private httpService: HttpService) {}

  saveInitiative(formData: any): Observable<any> {
    return this.httpService.post(InitiativeApiEnum.CreateUpdateInitiative, formData);
  }

  private getRecommendedContent<T>(
    apiUrl: string,
    initiativeId: number,
    skip: number,
    take: number,
    isCreate: boolean,
    attachedContentId: number = 0,
    expand: string = ''
  ): Observable<PaginateResponseInterface<T>> {
    return this.httpService.get<PaginateResponseInterface<T>>(`${apiUrl}/${initiativeId}`, {
      expand,
      skip,
      take,
      isCreate,
      attachedContentId,
      includeCount: true
    });
  }

  getRecommendedArticles(initiativeId: number, skip: number, take: number, isCreate: boolean) {
    const learnId =
      this.autoAttached?.contentType.toString() == InitiativeModulesEnum[InitiativeModulesEnum.Learn]
        ? this.autoAttached.contentId
        : 0;
    const expand = 'categories,regions,solutions,technologies,contenttags';
    return this.getRecommendedContent<InitiativeArticleInterface>(
      InitiativeApiEnum.GetRecommendedArticles,
      initiativeId,
      skip,
      take,
      isCreate,
      learnId,
      expand
    );
  }

  getRecommendedMessages(initiativeId: number, skip: number, take: number, isCreate: boolean) {
    const discussionId =
      this.autoAttached?.contentType.toString() == InitiativeModulesEnum[InitiativeModulesEnum.Messages]
        ? this.autoAttached.contentId
        : 0;
    return this.getRecommendedContent<InitiativeMessageInterface>(
      InitiativeApiEnum.GetRecommendedConversations,
      initiativeId,
      skip,
      take,
      isCreate,
      discussionId
    );
  }

  getRecommendedCommunityUsers(initiativeId: number, skip: number, take: number, isCreate: boolean) {
    const userId =
      this.autoAttached?.contentType.toString() == InitiativeModulesEnum[InitiativeModulesEnum.Community]
        ? this.autoAttached.contentId
        : 0;
    return this.getRecommendedContent<InitiativeCommunityInterface>(
      InitiativeApiEnum.GetRecommendedCommunityUsers,
      initiativeId,
      skip,
      take,
      isCreate,
      userId
    );
  }

  getRecommendedTools(initiativeId: number, skip: number, take: number, isCreate: boolean) {
    const toolId =
      this.autoAttached?.contentType.toString() == InitiativeModulesEnum[InitiativeModulesEnum.Tools]
        ? this.autoAttached.contentId
        : 0;
    const expand = 'categories,regions,solutions,technologies,contenttags';
    return this.getRecommendedContent<InitiativeToolInterface>(
      InitiativeApiEnum.GetRecommendedTools,
      initiativeId,
      skip,
      take,
      isCreate,
      toolId,
      expand
    );
  }

  getRecommendedProjects(initiativeId: number, skip: number, take: number, isCreate: boolean) {
    const projectId =
      this.autoAttached?.contentType.toString() == InitiativeModulesEnum[InitiativeModulesEnum.Projects]
        ? this.autoAttached.contentId
        : 0;
    const expand = 'categories,regions,solutions,technologies,contenttags';
    return this.getRecommendedContent<InitiativeProjectInterface>(
      InitiativeApiEnum.GetRecommendedProjects,
      initiativeId,
      skip,
      take,
      isCreate,
      projectId,
      expand
    );
  }

  pushSelectedContents(initiativeId: number, isSelected: boolean, ids: number, resourceType: InitiativeModulesEnum) {
    this.selectedContents.InitiativeId = initiativeId;
    const contentMap = {
      [InitiativeModulesEnum.Learn]: this.selectedContents.LearnContent,
      [InitiativeModulesEnum.Tools]: this.selectedContents.ToolContent,
      [InitiativeModulesEnum.Messages]: this.selectedContents.MessageContent,
      [InitiativeModulesEnum.Community]: this.selectedContents.CommunityContent,
      [InitiativeModulesEnum.Projects]: this.selectedContents.ProjectContent
    };
    contentMap[resourceType].push({ id: ids, isSelected, initiativeId });
  }

  popSelectedContents(tileId: number, resourceType: InitiativeModulesEnum) {
    switch (resourceType) {
      case InitiativeModulesEnum.Learn:
        this.selectedContents.LearnContent = this.selectedContents.LearnContent.filter(x => x.id !== tileId);
        break;
      case InitiativeModulesEnum.Tools:
        this.selectedContents.ToolContent = this.selectedContents.ToolContent.filter(x => x.id !== tileId);
        break;
      case InitiativeModulesEnum.Community:
        this.selectedContents.CommunityContent = this.selectedContents.CommunityContent.filter(x => x.id !== tileId);
        break;
      case InitiativeModulesEnum.Messages:
        this.selectedContents.MessageContent = this.selectedContents.MessageContent.filter(x => x.id !== tileId);
        break;
      case InitiativeModulesEnum.Projects:
        this.selectedContents.ProjectContent = this.selectedContents.ProjectContent.filter(x => x.id !== tileId);
    }
  }

  popAllSelectedContents(resourceType: InitiativeModulesEnum) {
    const contentMap = {
      [InitiativeModulesEnum.Learn]: this.selectedContents.LearnContent,
      [InitiativeModulesEnum.Tools]: this.selectedContents.ToolContent,
      [InitiativeModulesEnum.Messages]: this.selectedContents.MessageContent,
      [InitiativeModulesEnum.Community]: this.selectedContents.CommunityContent,
      [InitiativeModulesEnum.Projects]: this.selectedContents.ProjectContent
    };
    contentMap[resourceType].length = 0;
  }

  getSelectedTiles() {
    return this.selectedContents;
  }

  getInitiativeDetailsByInitiativeId(id: number): Observable<BaseInitiativeInterface> {
    return this.httpService.get<BaseInitiativeInterface>(
      `${InitiativeApiEnum.GetInitiativeAndProgressDetailsById}/${id}/true`
    );
  }
}
