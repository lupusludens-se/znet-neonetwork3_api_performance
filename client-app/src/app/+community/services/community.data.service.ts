import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { HttpService } from 'src/app/core/services/http.service';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { DEFAULT_PER_PAGE } from 'src/app/shared/modules/pagination/pagination.component';
import { UserProfileApiEnum } from 'src/app/shared/enums/api/user-profile-api.enum';
import { CommunityInterface } from '../interfaces/community.interface';
import { CommunitySearchInterface } from '../interfaces/community.search.interface';
import { CoreService } from '../../core/services/core.service';
import { CompanyApiEnum } from 'src/app/shared/enums/api/company-api-enum';
import { FollowResultInterface } from '../interfaces/follow-result.interface';
import { NetwrokStatsInterface } from 'src/app/shared/interfaces/netwrok-stats.interface';
import { CommunityComponentEnum } from '../enums/community-component.enum';

@Injectable()
export class CommunityDataService {
  defaultItemPerPage: number = DEFAULT_PER_PAGE;
  filterBy: string;

  userProfileApiRoutes = UserProfileApiEnum;
  companyApiRoutes = CompanyApiEnum;

  constructor(private httpService: HttpService, private readonly coreService: CoreService) {}

  getCommunityList(searchObject: CommunitySearchInterface): Observable<PaginateResponseInterface<CommunityInterface>> {
    this.filterBy = encodeURIComponent(searchObject.filterBy);

    searchObject.includeCount = true;
    searchObject.take = this.defaultItemPerPage;

    return this.httpService.get(`${CommonApiEnum.Community}`, this.coreService.deleteEmptyProps(searchObject));
  }

  getNetorkStats(): Observable<NetwrokStatsInterface> {
    return this.httpService.get(`${CommunityComponentEnum.NetworkStats}`);
  }

  followUser(userId: number, httpMethod: string): Observable<FollowResultInterface> {
    return this.httpService[httpMethod](`${this.userProfileApiRoutes.Followers}/${userId}`, {
      id: userId
    });
  }

  followCompany(companyId: number, httpMethod: string): Observable<FollowResultInterface> {
    return this.httpService[httpMethod](`${this.companyApiRoutes.Companies}/${companyId}/followers`, {
      id: companyId
    });
  }
}
