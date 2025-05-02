import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, from, Observable, throwError } from 'rxjs';
import { HttpService } from '../../core/services/http.service';
import { UserInterface } from '../../shared/interfaces/user/user.interface';
import { UserManagementApiEnum } from '../enums/user-management-api.enum';
import { PaginateResponseInterface } from '../../shared/interfaces/common/pagination-response.interface';
import { DEFAULT_PER_PAGE } from '../../shared/modules/pagination/pagination.component';
import { TitleService } from '../../core/services/title.service';
import { environment } from '../../../environments/environment';
import * as dayjs from 'dayjs';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { CoreService } from 'src/app/core/services/core.service';

export interface FileResponseInterface {
  fileData: string;
  fileName: string;
}

@Injectable()
export class UserDataService {
  apiRoutes = UserManagementApiEnum;
  defaultItemPerPage = DEFAULT_PER_PAGE;
  searchStr: string;
  skip: number;
  orderBy: string;
  filterBy: string;
  fileObj: FileResponseInterface;

  private userData: BehaviorSubject<UserInterface | null> = new BehaviorSubject<UserInterface | null>(null);

  userData$: Observable<UserInterface | null> = this.userData.asObservable();

  constructor(
    private httpService: HttpService,
    private titleService: TitleService,
    private readonly router: Router,
    private readonly coreService: CoreService
  ) { }

  setUserData(userdata: UserInterface) {
    this.userData.next(userdata);
  }

  getUserData(id: string, expand: string = null) {
    let expandOptions = expand != null ? `,${expand}` : '';
    return this.httpService
      .get(this.apiRoutes.Users + `/${id}`, {
        expand: 'permissions,country,company,userprofiles,image' + expandOptions
      })
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            this.router.navigate(['/community']);
            this.coreService.elementNotFoundData$.next({
              iconKey: 'user-unavailable',
              mainTextTranslate: 'userProfile.notFoundText',
              buttonTextTranslate: 'userProfile.notFoundButton',
              buttonLink: '/community'
            });
          }
          else if (error.status === 401) {
            this.router.navigate(['403']);
          }

          return throwError(error);
        })
      );
  }

  getSPAdminByCompany(companyId: number, userId: number): Observable<UserInterface> {
    const formData = {
      companyId: companyId,
      userId: userId
    };
    return this.httpService.post(this.apiRoutes.GetSPAdminByCompany, formData);
  }

  getUserList(
    search: string,
    skip: number,
    orderBy: string,
    filterBy: string,
    preserveFilters: boolean = true
  ): Observable<PaginateResponseInterface<UserInterface>> {
    this.searchStr = search ?? this.searchStr;
    this.orderBy = orderBy ?? this.orderBy;
    this.skip = skip ?? this.skip;

    if (preserveFilters) {
      this.filterBy = filterBy ?? this.filterBy;
    }

    const paramsObj: Record<string, any> = {
      Search: this.searchStr,
      Skip: this.skip,
      OrderBy: this.orderBy,
      FilterBy: this.filterBy
    };

    for (let prop in paramsObj) {
      if (!paramsObj[prop]) {
        delete paramsObj[prop];
      }
    }

    return this.httpService.get(this.apiRoutes.Users, {
      expand: 'company,permissions,image',
      IncludeCount: true,
      Take: this.defaultItemPerPage,
      ...paramsObj
    });
  }

  getCompanyUserList(skip: number, orderBy: string): Observable<PaginateResponseInterface<UserInterface>> {
    this.orderBy = orderBy ?? this.orderBy;
    this.skip = skip ?? this.skip;

    const paramsObj: Record<string, any> = {
      Skip: this.skip,
      OrderBy: this.orderBy
    };

    for (let prop in paramsObj) {
      if (!paramsObj[prop]) {
        delete paramsObj[prop];
      }
    }

    return this.httpService.get(this.apiRoutes.CompanyUsers, {
      expand: 'userprofile',
      IncludeCount: true,
      Take: this.defaultItemPerPage,
      ...paramsObj
    });
  }

  exportUsers(isCompanyUsersExport: boolean): Observable<FileResponseInterface> {
    const key = Object.keys(localStorage).find(item => item.includes('accesstoken'));
    const token: string = JSON.parse(localStorage[key]).secret;
    const exportSearch = this.searchStr ? `Search=${this.searchStr}&` : '';
    const exportFilter = this.filterBy ? `FilterBy=${encodeURIComponent(this.filterBy)}&` : '';
    const exportOrder = this.orderBy ? `OrderBy=${this.orderBy}` : '';
    let todaysdate = new Date();
    const request = new Request(
      `${environment.apiUrl}/${isCompanyUsersExport ? this.apiRoutes.ExportCompanyUsers : this.apiRoutes.ExportUsers
      }?v=${todaysdate.getTime()}&${exportSearch}${exportFilter}${exportOrder}`,
      {
        method: 'GET',
        headers: new Headers({
          authorization: `Bearer ${token}`
        })
      }
    );

    return from(
      fetch(request)
        .then(response => response.text())
        .then(data => {
          const fileDate: string = dayjs().format('D_MM_YYYY');
          this.fileObj = {
            fileData: data,
            fileName: `Export file ${fileDate}.csv`
          };

          return this.fileObj;
        })
    );
  }

  clearRequestParams(): void {
    this.searchStr = null;
    this.skip = null;
    this.orderBy = null;
    this.filterBy = null;
  }
}
