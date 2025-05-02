import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Router } from '@angular/router';

import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { ProjectApiRoutes } from 'src/app/projects/shared/constants/project-api-routes.const';

import { AuthService } from '../services/auth.service';
import { LogService } from 'src/app/shared/services/log.service';

import { CommonApiEnum } from '../enums/common-api.enum';
import { EventsApiEnum } from 'src/app/shared/enums/api/events-api.enum';
import { AdmitUserEnum } from 'src/app/admit/emuns/admit-user.enum';
import { HTTPType } from '../enums/http-type.enum';
import { ForumApiEnum } from 'src/app/+forum/enums/forum-api.enum';
import { UserProfileApiEnum } from 'src/app/shared/enums/api/user-profile-api.enum';
import { ArticleDetailsViewOwnPermissionGuard } from 'src/app/shared/guards/permission-guards/article-details-view-own-permission.guard';
import { CanActivateGaurdTypeService } from 'src/app/shared/services/can-activate-gaurd-type.service';
import { EventDetailsViewOwnPermissionGuard } from 'src/app/shared/guards/permission-guards/event-details-view-own-permission.guard';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private readonly neoAuthService: AuthService,
    private readonly router: Router,
    private readonly canActivateGaurdTypeService: CanActivateGaurdTypeService,
    private readonly logService: LogService
  ) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (request.url.includes(CommonApiEnum.Logs)) {
          return;
        }

        if (400 <= error.status && error.status < 500) {
          this.logService.error(`Error during HTTP ${request.method} to ${request.url}.`, error, {
            request: request.body,
            headers: request.headers
          });
        }

        if (error.status === 401) {
          if (this.neoAuthService.currentUser$.getValue() !== null) {
            this.neoAuthService.logout();
            return;
          }
          else {
            if (this.canActivateGaurdTypeService.getCanActivateGaurdType() != typeof (ArticleDetailsViewOwnPermissionGuard)
               || this.canActivateGaurdTypeService.getCanActivateGaurdType() != typeof (EventDetailsViewOwnPermissionGuard)
            ) {
              this.neoAuthService.loginRedirect();
            }
          }

        } else if (error.status === 403) {
          this.router.navigate(['403']);
        } else if (error.status === 404) {
          this.handleNotFound(request);
        } else if (error.status === 423) {
          this.router.navigate(['423']);
        }

        return throwError(() => error);
      })
    );
  }

  private handleNotFound(request: HttpRequest<unknown>): void {
    if (
      request.method.toLowerCase() === HTTPType.GET &&
      (request.url.includes(EventsApiEnum.Events) ||
        request.url.includes(AdmitUserEnum.UserPendings) ||
        (request.url.includes(ProjectApiRoutes.projectsList) &&
          (location.href.includes('projects-library') || location.href.includes('projects'))) ||
        request.url.includes(CommonApiEnum.Users) ||
        request.url.includes(UserProfileApiEnum.UserProfiles) ||
        request.url.includes(CommonApiEnum.Companies) ||
        request.url.includes(ForumApiEnum.Forum) ||
        request.url.includes(CommonApiEnum.Articles))
    ) {
      return;
    } else {
      this.router.navigate(['404']);
    }
  }
}
