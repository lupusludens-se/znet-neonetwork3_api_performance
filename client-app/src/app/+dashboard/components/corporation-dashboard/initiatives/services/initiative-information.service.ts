import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from 'src/app/core/services/http.service';
import { InitiativeViewSource } from 'src/app/initiatives/+decarbonization-initiatives/enums/initiative-view-source';
import { NewRecommendationCounterInterface } from 'src/app/initiatives/+initatives/+view-initiative/interfaces/new-recommendation-counter';
import { InitiativeApiEnum } from 'src/app/initiatives/enums/initiative-api.enum';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { InitiativeProgress } from 'src/app/initiatives/interfaces/initiative-progress.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { PaginationIncludeCountInterface } from 'src/app/shared/modules/pagination/pagination.component';

@Injectable()
export class DashboardInitiativeService {
  constructor(private httpService: HttpService) {}
  getInitiativesForUser(initiativeType: InitiativeViewSource, paging?: PaginationIncludeCountInterface) {
    return this.httpService.get<PaginateResponseInterface<InitiativeProgress>>(
      `${InitiativeApiEnum.GetInitiativesAndProgressDetailsByUserId}/${initiativeType}`, paging
    );
  }
  getNewRecommendationsCount(
    initiativeIds: number[] = [],
    initiativeModulesEnum: InitiativeModulesEnum
  ): Observable<NewRecommendationCounterInterface[]> {
    const requestBody = { initiativeIds, InitiativeContentType: initiativeModulesEnum };
    return this.httpService.post<NewRecommendationCounterInterface[]>(
      InitiativeApiEnum.GetNewRecommendationsCount,
      requestBody
    );
  }
}
