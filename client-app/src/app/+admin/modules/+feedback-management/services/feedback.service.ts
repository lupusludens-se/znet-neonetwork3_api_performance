import { Injectable } from '@angular/core';
import { BehaviorSubject, from } from 'rxjs';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { FeedbackInterface } from '../interfaces/feedback.interface';
import { HttpService } from 'src/app/core/services/http.service';
import { FeedbackApiEnum } from '../enums/feedback.enum';
import { FileResponseInterface } from 'src/app/user-management/services/user.data.service';
import { CoreService } from 'src/app/core/services/core.service';
import * as dayjs from 'dayjs';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root'
})
export class FeedbackService {
  paging$: BehaviorSubject<PaginationInterface> = new BehaviorSubject<PaginationInterface>({
    take: 25,
    skip: 0,
    total: null
  });
  orderBy$: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  fileObj: FileResponseInterface;
  constructor(private httpService: HttpService, private coreService: CoreService) {}

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

  fetchFeedbacks(paging) {
    return this.httpService.get<PaginateResponseInterface<FeedbackInterface>>(FeedbackApiEnum.FeedbackList, paging);
  }

  fetchFeedback(feedbackId: number) {
    return this.httpService.get<FeedbackInterface>(FeedbackApiEnum.FeedbackList + '/' + feedbackId);
  }

  exportFeedbacks() {
    const key = Object.keys(localStorage).find(item => item.includes('accesstoken'));
    const token: string = JSON.parse(localStorage[key]).secret;
    const exportSearch = '';
    const exportFilter = '';
    const exportOrder = this.orderBy$.getValue() !== null ? `OrderBy=${this.orderBy$.getValue()}` : '';
    let todaysdate = new Date();
    const request = new Request(
      `${environment.apiUrl}/${
        FeedbackApiEnum.ExportFeedbacks
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
            fileName: `User Feedbacks Export file ${fileDate}.csv`
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
  }
}
