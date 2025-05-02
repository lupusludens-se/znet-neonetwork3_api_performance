import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpService } from 'src/app/core/services/http.service';
import { InitiativeSavedContentListRequestInterface } from '../interfaces/initiative-saved-content-list-request.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { InitiativeProgress } from 'src/app/initiatives/interfaces/initiative-progress.interface';
import { InitiativeApiEnum } from 'src/app/initiatives/enums/initiative-api.enum';
import { BaseInitiativeInterface } from '../interfaces/base-initiative.interface';
import { RecommendationContentInterface } from 'src/app/initiatives/shared/models/recommendation-content.interface';
import {
  InitiativeArticleInterface,
  InitiativeCommunityInterface,
  InitiativeMessageInterface,
  InitiativeProjectInterface,
  InitiativeToolInterface
} from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';

@Injectable()
export class ViewInitiativeService {
  private currentPageSubject = new BehaviorSubject<number>(1);
  currentPageNumber$ = this.currentPageSubject.asObservable();
  selectedContents: RecommendationContentInterface = {
    InitiativeId: 0,
    LearnContent: [],
    ToolContent: [],
    MessageContent: [],
    CommunityContent: [],
    ProjectContent: []
  };

  constructor(private httpService: HttpService) { }

  changePage(page: number) {
    this.currentPageSubject.next(page);
  }

  private getRequest<T>(url: string, params?: any): Observable<T> {
    return this.httpService.get<T>(url, params);
  }

  getInitiativeDetailsByInitiativeId(id: number): Observable<BaseInitiativeInterface> {
    return this.getRequest<BaseInitiativeInterface>(`${InitiativeApiEnum.GetInitiativeAndProgressDetailsById}/${id}`);
  }

  pushSelectedContents(initiativeId: number, isSelected: boolean, ids: number, resourceType: InitiativeModulesEnum) {
    this.selectedContents.InitiativeId = initiativeId;
    const content = { id: ids, isSelected, initiativeId };
    switch (resourceType) {
      case InitiativeModulesEnum.Learn:
        this.selectedContents.LearnContent.push(content);
        break;
      case InitiativeModulesEnum.Tools:
        this.selectedContents.ToolContent.push(content);
        break;
      case InitiativeModulesEnum.Messages:
        this.selectedContents.MessageContent.push(content);
        break;
      case InitiativeModulesEnum.Community:
        this.selectedContents.CommunityContent.push(content);
        break;
    }
  }

  popSelectedContents(tileId: number, resourceType: InitiativeModulesEnum) {
    switch (resourceType) {
      case InitiativeModulesEnum.Learn:
        this.selectedContents.LearnContent = this.selectedContents.LearnContent.filter(x => x.id !== tileId);
        break;
      case InitiativeModulesEnum.Tools:
        this.selectedContents.ToolContent = this.selectedContents.ToolContent.filter(x => x.id !== tileId);
        break;
      case InitiativeModulesEnum.Messages:
        this.selectedContents.MessageContent = this.selectedContents.MessageContent.filter(x => x.id !== tileId);
        break;
      case InitiativeModulesEnum.Community:
        this.selectedContents.CommunityContent = this.selectedContents.CommunityContent.filter(x => x.id !== tileId);
        break;
    }
  }

  getSelectedTiles() {
    return this.selectedContents;
  }

  clearSelectedTiles(resourceType: InitiativeModulesEnum) {
    switch (resourceType) {
      case InitiativeModulesEnum.Learn:
        this.selectedContents.LearnContent = [];
        break;
      case InitiativeModulesEnum.Tools:
        this.selectedContents.ToolContent = [];
        break;
      case InitiativeModulesEnum.Messages:
        this.selectedContents.MessageContent = [];
        break;
      case InitiativeModulesEnum.Community:
        this.selectedContents.CommunityContent = [];
        break;
    }
  }

  getSavedArticlesForInitiative(
    initiativeId: number,
    requestData: InitiativeSavedContentListRequestInterface
  ): Observable<PaginateResponseInterface<InitiativeArticleInterface>> {
    return this.getRequest<PaginateResponseInterface<InitiativeArticleInterface>>(
      `${InitiativeApiEnum.GetSavedInitiativeArticle}/${initiativeId}`,
      requestData
    );
  }

  getSavedMessagesOfAnInitiative(
    initiativeId: number,
    requestData: InitiativeSavedContentListRequestInterface
  ): Observable<PaginateResponseInterface<InitiativeMessageInterface>> {
    return this.getRequest<PaginateResponseInterface<InitiativeMessageInterface>>(
      `${InitiativeApiEnum.GetSavedInitiativeConversations}/${initiativeId}`,
      requestData
    );
  }

  getInitiativeProgressDetails(initiativeId: number): Observable<InitiativeProgress> {
    return this.getRequest<InitiativeProgress>(
      `${InitiativeApiEnum.GetInitiativeAndProgressDetailsById}/${initiativeId}`
    );
  }

  updateInitiativeSubStep(payload: any): Observable<boolean> {
    return this.httpService.post<boolean>(InitiativeApiEnum.UpdateInitiativeSubStep, payload);
  }

  getSavedToolsForInitiative(
    initiativeId: number,
    requestData: InitiativeSavedContentListRequestInterface
  ): Observable<PaginateResponseInterface<InitiativeToolInterface>> {
    return this.getRequest<PaginateResponseInterface<InitiativeToolInterface>>(
      `${InitiativeApiEnum.GetSavedInitiativeTools}/${initiativeId}`,
      requestData
    );
  }

  getSavedProjectsForInitiative(
    initiativeId: number,
    requestData: InitiativeSavedContentListRequestInterface
  ): Observable<PaginateResponseInterface<InitiativeProjectInterface>> {
    return this.getRequest<PaginateResponseInterface<InitiativeProjectInterface>>(
      `${InitiativeApiEnum.GetSavedInitiativeProjects}/${initiativeId}`,
      requestData
    );
  }

  getSavedCommunityUsersForAnInitiative(
    initiativeId: number,
    requestData: InitiativeSavedContentListRequestInterface
  ): Observable<PaginateResponseInterface<InitiativeCommunityInterface>> {
    return this.getRequest<PaginateResponseInterface<InitiativeCommunityInterface>>(
      `${InitiativeApiEnum.GetSavedInitiativeCommunityUsers}/${initiativeId}`,
      requestData
    );
  }
}
