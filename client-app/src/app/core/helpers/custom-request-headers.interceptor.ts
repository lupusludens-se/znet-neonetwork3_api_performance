import { HttpEvent, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MsalService } from '@azure/msal-angular';
import { AuthService } from '../services/auth.service';
import { ApiRoutes } from 'src/app/+admin/modules/announcements/enums/announcement.enum';
import { BYPASS_MSAL } from './msal-custom.interceptor';
import { ToolsApiEnum } from 'src/app/shared/enums/api/tools-api.enum';
import { EventsApiEnum } from 'src/app/shared/enums/api/events-api.enum';
import { ActivityApiEnum } from '../enums/activity/activity-api.enum';
import { CommonApiEnum } from '../enums/common-api.enum';
import { DashboardManagement } from 'src/app/+dashboard/enums/dashboard-management.enum';

@Injectable()
export class CustomRequestHeadersInterceptor implements HttpInterceptor {
  auth = AuthService;
  constructor(private authService: AuthService, private msalAuthService: MsalService) { }
  intercept(request: HttpRequest<unknown>, next: any): Observable<HttpEvent<unknown>> {

    if (
      !(request.url.includes(CommonApiEnum.Solutions) ||
        request.url.includes(CommonApiEnum.Technologies) ||
        request.url.includes(CommonApiEnum.Categories) ||
        request.url.includes(CommonApiEnum.Regions))) {
      request = request.clone({
        setHeaders: {
          'Pragma': 'no-cache',
          'Cache-Control': 'no-cache'
        }
      });
    }


    if (
      (request.url.includes(ApiRoutes.AnnouncementLatest) ||
        request.url.includes(ToolsApiEnum.Tools) ||
        request.url.includes(EventsApiEnum.Events) ||
        request.url.includes(ActivityApiEnum.PublicActivity) ||
        request.url.includes(CommonApiEnum.Articles) ||
        request.url.includes(CommonApiEnum.Solutions) ||
        request.url.includes(CommonApiEnum.Categories) ||
        request.url.includes(CommonApiEnum.Articles) ||
        request.url.includes(CommonApiEnum.ScheduleDemo) ||
        request.url.includes(DashboardManagement.PublicDiscoverabilityData) ||
        request.url.includes(ActivityApiEnum.Activity)) &&
      !this.auth.isLoggedIn()
    ) {
      request.context.set(BYPASS_MSAL, true);
      return next.handle(request);
    }
    return next.handle(request);
  }
}
