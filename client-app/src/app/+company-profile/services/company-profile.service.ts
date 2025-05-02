import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { FileTabEnum } from 'src/app/shared/enums/file-type.enum';
import { HttpService } from '../../core/services/http.service';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { CompanyAnnouncementInterface } from 'src/app/core/interfaces/company-announcement-interface';
import { CompanyApiEnum } from 'src/app/shared/enums/api/company-api-enum';

@Injectable()
export class CompanyProfileService {
  selectedTab$: BehaviorSubject<number> = new BehaviorSubject<number>(FileTabEnum['Public']);
  constructor(private httpService: HttpService) {}

  getCompanyAnnouncements(companyId: number): Observable<PaginateResponseInterface<CompanyAnnouncementInterface>> {
    return this.httpService.get<PaginateResponseInterface<CompanyAnnouncementInterface>>(
      `${CompanyApiEnum.GetCompanyAnnouncements}/${companyId}`
    );
  }

  getCompanyAnnouncementById(announcementId: number): Observable<CompanyAnnouncementInterface> {
    return this.httpService.get<CompanyAnnouncementInterface>(
      CompanyApiEnum.GetCompanyAnnouncementById + `/${announcementId}`
    );
  }

  createOrUpdateAnnouncement(announcementData: CompanyAnnouncementInterface): Observable<any> {
    return this.httpService.post(
      CompanyApiEnum.CreateUpdateAnnouncement + `/${announcementData.id ?? 0}`,
      announcementData
    );
  }

  deleteAnnouncement(announcementId: number): Observable<boolean> {
    return this.httpService.delete<boolean>(CompanyApiEnum.DeleteAnnouncement + `/${announcementId}`);
  }

  getTab() {
    return this.selectedTab$.asObservable();
  }

  setSource(sourceId: number) {
    this.selectedTab$.next(sourceId);
  }
}
