import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { PaginateResponseInterface } from '../../../../shared/interfaces/common/pagination-response.interface';
import { PatchPayloadInterface } from '../../../../shared/interfaces/common/patch-payload.interface';
import { DEFAULT_PER_PAGE } from '../../../../shared/modules/pagination/pagination.component';
import { AnnouncementInterface } from '../interfaces/announcement.interface';
import { HttpService } from '../../../../core/services/http.service';
import { ApiRoutes } from '../enums/announcement.enum';

@Injectable()
export class AnnouncementsService {
  defaultItemPerPage = DEFAULT_PER_PAGE;
  skip: number;
  orderBy: string;
  take: number;

  constructor(private httpService: HttpService) {}

  createAnnouncement(announcement: AnnouncementInterface, force?: boolean): Observable<void> {
    const requestUrl = force ? `${ApiRoutes.Announcements}?forceActivate=true` : ApiRoutes.Announcements;

    return this.httpService.post(requestUrl, announcement);
  }

  getAnnouncementsList(
    Skip: number = 0,
    OrderBy: string = '',
    Take: number = this.defaultItemPerPage
  ): Observable<PaginateResponseInterface<AnnouncementInterface>> {
    this.skip = Skip ?? this.skip;
    this.orderBy = OrderBy ?? this.orderBy;
    this.take = Take ?? this.take;

    return this.httpService.get<PaginateResponseInterface<AnnouncementInterface>>(ApiRoutes.Announcements, {
      IncludeCount: true,
      expand: 'audience,backgroundimage',
      Skip: this.skip,
      OrderBy: this.orderBy,
      Take: this.take
    });
  }

  getAnnouncement(id: string): Observable<AnnouncementInterface> {
    return this.httpService.get(ApiRoutes.Announcements + `/${id}`, {
      expand: 'audience,backgroundimage'
    });
  }

  updateAnnouncement(id: number, payloadValue: string, param: boolean): Observable<PatchPayloadInterface> {
    const payload: PatchPayloadInterface = {
      jsonPatchDocument: [
        {
          op: 'replace',
          value: payloadValue,
          path: '/IsActive'
        }
      ]
    };

    const baseRoute: string = `${ApiRoutes.Announcements}/${id}`;
    const route: string = param ? `${baseRoute}/?forceActivate=true` : baseRoute;

    return this.httpService.patch(route, payload);
  }

  deleteAnnouncement(id: number): Observable<void> {
    return this.httpService.delete(ApiRoutes.Announcements + `/${id}`, id);
  }

  editAnnouncement(
    announcement: AnnouncementInterface,
    announcementId: string,
    force?: boolean
  ): Observable<AnnouncementInterface> {
    const requestUrl = force
      ? `${ApiRoutes.Announcements}/${announcementId}?forceActivate=true`
      : `${ApiRoutes.Announcements}/${announcementId}`;

    return this.httpService.put(requestUrl, announcement);
  }
}
