import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/core/services/http.service';
import { InitiativeApiEnum } from '../../enums/initiative-api.enum';
import { InitiativeModulesEnum } from '../../enums/initiative-modules.enum';
import { Observable } from 'rxjs';
import { NewRecommendationCounterInterface } from '../../+initatives/+view-initiative/interfaces/new-recommendation-counter';
import { InitiativeAttachedContent } from '../../interfaces/initiative-attached.interface';
import { UserManagementApiEnum } from 'src/app/user-management/enums/user-management-api.enum';
import { UserWrapperDataInterface } from 'src/app/shared/interfaces/user/user.interface';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { BaseInitiativeInterface } from '../../+initatives/+view-initiative/interfaces/base-initiative.interface';

@Injectable()
export class InitiativeSharedService {
  constructor(private httpService: HttpService) { }

  deleteSavedContent(id: number, contentId: number, contentType: InitiativeModulesEnum): Observable<boolean> {
    return this.httpService.delete<boolean>(
      `${InitiativeApiEnum.RemoveSavedContent}/${id}/content/${contentId}/contentType/${contentType}`
    );
  }

  saveSelectedContentsForAnInitiative(
    initiativeId: number,
    isNew: boolean,
    articleIds?: number[],
    communityUserIds?: number[],
    projectIds?: number[],
    toolIds?: number[],
    discussionIds?: number[]
  ): Observable<boolean> {
    const requestData = {
      isNew,
      initiativeId,
      articleIds,
      projectIds,
      communityUserIds,
      toolIds,
      discussionIds
    };
    return this.httpService.post<boolean>(InitiativeApiEnum.SaveInitiativeContentItem, requestData);
  }

  getNewRecommendationsCount(
    initiativeId: number,
    initiativeModulesEnum: InitiativeModulesEnum
  ): Observable<NewRecommendationCounterInterface[]> {
    const requestBody = { initiativeIds: [initiativeId], initiativeContentType: initiativeModulesEnum };
    return this.httpService.post<NewRecommendationCounterInterface[]>(
      InitiativeApiEnum.GetNewRecommendationsCount,
      requestBody
    );
  }

  displayCounter(counter: number): string {
    if (counter <= 0) return '';
    if (counter < 10) return counter.toString();
    if (counter < 100) return '9+';
    return '99+';
  }

  getInitiativeAttachedDetails(contentId: string, contentType: string) {
    return this.httpService.get<InitiativeAttachedContent[]>(`${InitiativeApiEnum.GetInitiativeAttachedDetails}/${contentId}/contentType/${contentType}`);
  }

  getCompanyUsers(): Observable<UserWrapperDataInterface> {
    return this.httpService.get<UserWrapperDataInterface>(UserManagementApiEnum.CompanyUsers, {
      expand: 'company,image,roles',
      filterBy: `statusids=${UserStatusEnum.Active}`,
      orderBy: 'lastname.asc'
      });
  }

  updateInitiativeContentLastViewedDate(
    initiativeId: number,
    resourceType: InitiativeModulesEnum
  ): Observable<boolean> {
    const requestData = { initiativeId, contentType: resourceType };
    return this.httpService.post(InitiativeApiEnum.UpdateInitiativeContentLastViewedDate, requestData);
  }

  getInitiativeDetailsByInitiativeId(id: number): Observable<BaseInitiativeInterface> {
    return this.httpService.get<BaseInitiativeInterface>(
      `${InitiativeApiEnum.GetInitiativeAndProgressDetailsById}/${id}/true`
    );
  }
}
