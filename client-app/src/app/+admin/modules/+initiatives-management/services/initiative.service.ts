import { Injectable } from '@angular/core';
import { BehaviorSubject, from } from 'rxjs';
import { HttpService } from 'src/app/core/services/http.service';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { InitiativeAdminResponse } from '../interfaces/initiative-admin';
import { InitiativeApiEnum } from 'src/app/initiatives/enums/initiative-api.enum';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import * as dayjs from 'dayjs';
import { environment } from 'src/environments/environment';
import { FileResponseInterface } from 'src/app/user-management/services/user.data.service';

@Injectable({
  providedIn: 'root'
})
export class InitiativeService {
  paging$: BehaviorSubject<PaginationInterface> = new BehaviorSubject<PaginationInterface>({
    take: 25,
    skip: 0,
    total: null
  });
orderBy$: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  searchStr$: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  fileObj: FileResponseInterface;
  constructor(private httpService: HttpService) {}

  getPaging() {
    return this.paging$.asObservable();
  }

  getOrderBy() {
    return this.orderBy$.asObservable();
  }

  setPaging(pageData: PaginationInterface) {
    this.paging$.next(pageData);
  }

  setOrderBy(orderBy: string) {
    this.orderBy$.next(orderBy);
  }

  fetchAllInitiatives(paging) {
    return this.httpService.get<PaginateResponseInterface<InitiativeAdminResponse>>(
      InitiativeApiEnum.GetAllInitiatives,
      paging
    );
  }

  getSearchStr() {
    return this.searchStr$.asObservable();
  }

  setSearchStr(searchStr: string) {
    this.searchStr$.next(searchStr);
  }

  exportInitiatives() {
    const key = Object.keys(localStorage).find(item => item.includes('accesstoken'));
    const token: string = JSON.parse(localStorage[key]).secret;
    const exportSearch = this.searchStr$.getValue() !== null ? `Search=${this.searchStr$.getValue()}&` : '';
    const exportFilter = '';
    const exportOrder = this.orderBy$.getValue() !== null ? `OrderBy=${this.orderBy$.getValue()}` : '';
    let todaysdate = new Date();
    const request = new Request(
      `${environment.apiUrl}/${
        InitiativeApiEnum.ExportInitiatives
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
            fileName: `User Initiatives Export file ${fileDate}.csv`
          };

          return this.fileObj;
        })
    );
  }

  clearAll() {
    this.paging$.next({
      take: 25,
      skip: 0,
      total: null
    });
    this.orderBy$.next(null);
    this.searchStr$.next('');
  }
}
