import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CompanyManagementApiEnum } from '../enums/company-management-api.enum';
import { CompanyInterface } from 'src/app/shared/interfaces/user/company.interface';
import { DEFAULT_COMPANIES_PER_PAGE } from '../constants/parameter.const';
import { CompanyStatusEnum } from 'src/app/shared/enums/company-status.enum';
import { HttpService } from 'src/app/core/services/http.service';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';

@Injectable()
export class CompanyDataService {
  apiRoutes = CompanyManagementApiEnum;
  defaultItemPerPage: number = DEFAULT_COMPANIES_PER_PAGE;
  searchStr: string;
  orderBy: string;

  constructor(private httpService: HttpService) {}

  getCompanyList(
    Search: string = '',
    Skip: number = 0,
    OrderBy: string
  ): Observable<PaginateResponseInterface<CompanyInterface>> {
    this.searchStr = Search;
    this.orderBy = OrderBy;

    return this.httpService.get(this.apiRoutes.Companies, {
      IncludeCount: true,
      Take: this.defaultItemPerPage,
      Search,
      Skip,
      OrderBy,
      FilterBy: `statusids=${CompanyStatusEnum.Active},${CompanyStatusEnum.Inactive}`,
      Expand: 'image'
    });
  }
}
