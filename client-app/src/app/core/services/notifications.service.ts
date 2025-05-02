import { Injectable } from '@angular/core';
import { NOTIFICATION_LINK_MIN_LENGTH, NOTIFICATION_MAX_LENGTH } from '../../shared/constants/maxlength.const';
import { Observable } from 'rxjs';
import { CommonApiEnum } from '../enums/common-api.enum';
import { HttpService } from './http.service';
import { PaginateResponseInterface } from '../../shared/interfaces/common/pagination-response.interface';
import { NotificationInterface } from '../../shared/interfaces/notification.interface';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {
  constructor(private httpService: HttpService) {}

  getNotifications(scrollState: Record<string, number>, includeCount: boolean): Observable<any> {
    return this.httpService.get<PaginateResponseInterface<NotificationInterface>>(CommonApiEnum.Notifications, {
      ...scrollState,
      includeCount
    });
  }

  markNotificationAsRead(id: number): Observable<any> {
    return this.httpService.put<boolean>(CommonApiEnum.Notifications, null, {
      id,
      readSeen: 1
    });
  }

  markNotificationsAsRead(readSeen: number): Observable<any> {
    return this.httpService.put<number>(CommonApiEnum.Notifications, null, { readSeen });
  }

  getTruncatedName(nameString: string = '', wholeTextLength: number = 0, staticTextLength: number = 0): string {
    if (nameString.length > NOTIFICATION_LINK_MIN_LENGTH && wholeTextLength > NOTIFICATION_MAX_LENGTH) {
      // truncate for 2 rows with padding or get substring with constant number of symbols
      let subStringLength =
        wholeTextLength - nameString.length === staticTextLength
          ? NOTIFICATION_MAX_LENGTH - staticTextLength - 10
          : NOTIFICATION_LINK_MIN_LENGTH;

      // static text should be visible
      if (staticTextLength && staticTextLength + subStringLength > NOTIFICATION_MAX_LENGTH) {
        subStringLength = NOTIFICATION_MAX_LENGTH - staticTextLength;
      }

      nameString = nameString.slice(0, subStringLength) + '...';
    }

    return nameString;
  }
}
