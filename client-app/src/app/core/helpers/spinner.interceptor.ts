import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

import { SpinnerService } from '../services/spinner.service';
import { CommonApiEnum } from '../enums/common-api.enum';
import { GeneralApiEnum } from '../../shared/enums/api/general-api.enum';
import { ActivityApiEnum } from '../enums/activity/activity-api.enum';
import { SavedContentApiEnum } from 'src/app/shared/modules/saved-content/enums/saved-content-api.enum';
import { FeedbackApiEnum } from 'src/app/+admin/modules/+feedback-management/enums/feedback.enum';
import { InitiativeApiEnum } from 'src/app/initiatives/enums/initiative-api.enum';

@Injectable()
export class SpinnerInterceptor implements HttpInterceptor {
  constructor(private spinnerService: SpinnerService) { }

  public intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // TODO: Create spinner api request black list black list
    const notifications = request.url.includes(CommonApiEnum.Notifications);
    const savedContents =
      request.url.includes(SavedContentApiEnum.SavedContent) && !location.pathname.includes('saved-content');
    const endsWithNumber = /\d+$/.test(location.pathname);   
    const savedContentsForInitiativeDashboard = request.url.includes('get-saved-') && location.pathname.includes('/initiatives/') && endsWithNumber;
    const unreadCounters = request.url.includes(CommonApiEnum.BadgeCounters);
    const logs = request.url.includes(CommonApiEnum.Logs);
    const activities = request.url.includes(ActivityApiEnum.Activity);
    const publicActivities = request.url.includes(ActivityApiEnum.PublicActivity);
    const submitFeedback = request.url.includes(FeedbackApiEnum.SubmitFeedback);
    const uploadMessageAttachments = location.pathname.includes('forum') && request.url.includes(GeneralApiEnum.Media);
    const authRedirect = location.pathname.includes('auth-redirect'); //don't show spinner on these pages - need to show only skeleton
    const unViewedRecomendations = request.url.includes(InitiativeApiEnum.GetNewRecommendationsCount);

    if (
      !unreadCounters &&
      !uploadMessageAttachments &&
      !notifications &&
      !savedContents &&
      !logs &&
      !savedContentsForInitiativeDashboard &&
      !activities &&
      !publicActivities &&
      !authRedirect &&
      !submitFeedback &&
      !unViewedRecomendations
    ) {
      this.spinnerService.onStarted(request);
    }

    return next.handle(request).pipe(finalize(() => this.spinnerService.onFinished(request)));
  }
}
