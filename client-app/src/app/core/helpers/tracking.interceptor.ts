import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { ActivityService } from '../services/activity.service';
import { ActivityApiEnum } from '../enums/activity/activity-api.enum';
import { CommonApiEnum } from '../enums/common-api.enum';
import { EventsApiEnum } from 'src/app/shared/enums/api/events-api.enum';
import { AuthService } from '../services/auth.service';
import { DashboardManagement } from 'src/app/+dashboard/enums/dashboard-management.enum';

@Injectable()
export class TrackingInterceptor implements HttpInterceptor {
  private readonly untrackedEndpointUrls: string[] = [
    ActivityApiEnum.Activity,
    CommonApiEnum.BadgeCounters,
    CommonApiEnum.Logs,
    CommonApiEnum.ArticlesSync,
    CommonApiEnum.Articles,
    CommonApiEnum.ScheduleDemo,
    ActivityApiEnum.PublicActivity,
    DashboardManagement.PublicDiscoverabilityData
  ];
  private readonly untrackEndpointUrlsIfAuthNotLoggedIn: string[] = [EventsApiEnum.Events];

  constructor(private activityService: ActivityService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (
      request.url.indexOf('/api/') === -1 ||
      this.shouldTrackEndpoint(request.url) === false ||
      request.urlWithParams.includes('skipActivities=true')
    ) {
      return next.handle(request);
    }
    return next.handle(request).pipe(
      tap((response: HttpResponse<any>) => {
        if (response instanceof HttpResponse) {
          this.activityService
            .trackRequestActivity(request.urlWithParams.toLowerCase(), request.method, request.body, response.body)
            ?.subscribe();
        }
      })
    );
  }

  private shouldTrackEndpoint(url: string): boolean {
    for (const untrackedEndpointUrl of this.untrackedEndpointUrls) {
      if (
        url.indexOf(untrackedEndpointUrl) >= 0 ||
        (this.untrackEndpointUrlsIfAuthNotLoggedIn.includes(untrackedEndpointUrl) && !AuthService.isLoggedIn())
      ) {
        return false;
      }
    }
    return true;
  }
  // private shouldTrackEndpoint(url: string): boolean {
  //   for (const untrackedEndpointUrl of this.untrackedEndpointUrls) {
  //     if (url.indexOf(untrackedEndpointUrl) >= 0) {
  //       return false;
  //     }
  //   }
  //   return true;
  // }
}
