import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';

import { AuthService } from '../../../core/services/auth.service';
import { Observable, catchError, filter, map, of, switchMap } from 'rxjs';
import { SpinnerService } from 'src/app/core/services/spinner.service';
import { HttpService } from 'src/app/core/services/http.service';
import { PostInterface } from 'src/app/+learn/interfaces/post.interface';
import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { UntilDestroy } from '@ngneat/until-destroy';
import { CommonService } from 'src/app/core/services/common.service';
import { MsalService, MsalBroadcastService } from '@azure/msal-angular';
import { AuthenticationResult, InteractionStatus } from '@azure/msal-browser';
import { CanActivateGaurdTypeService } from '../../services/can-activate-gaurd-type.service';

@UntilDestroy()
@Injectable({
  providedIn: 'root'
})

export class ArticleDetailsViewOwnPermissionGuard implements CanActivate {
  constructor(private msalService: MsalService,
    private canActivateGaurdTypeService: CanActivateGaurdTypeService,
    private authService: AuthService,
    private router: Router,
    private commonService: CommonService,
    private httpService: HttpService,
    private spinnerService: SpinnerService,
    private msalBroadcastService: MsalBroadcastService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None),
        switchMap(() => {
          if (this.msalService.instance.getAllAccounts().length == 0) {
            this.canActivateGaurdTypeService.setCanActivateGaurdType(typeof (ArticleDetailsViewOwnPermissionGuard));
            return this.httpService.get<PostInterface>(`${CommonApiEnum.Articles}/${route.params['id']}`, { expand: '' }).pipe(
              map(() => {
                if (route.params["src"] == "email") {
                  this.canActivateGaurdTypeService.setCanActivateGaurdType(null);
                  this.authService.loginRedirect(state.url.replace("/email", ""));
                  this.spinnerService.onStarted(null);
                  return false;
                }
                return true;
              }),
              catchError(error => {
                this.canActivateGaurdTypeService.setCanActivateGaurdType(null);
                this.authService.loginRedirect(state.url);
                this.spinnerService.onStarted(null);
                return of(false);
              })
            );
          } else {
            const payload = this.msalService.instance.getActiveAccount();
            this.msalService.instance.setActiveAccount(payload);
            this.commonService.hideSkeleton$.next(false);
            if (route.params["src"] == "email") {
              this.canActivateGaurdTypeService.setCanActivateGaurdType(null);
              this.router.navigateByUrl(state.url.replace("/email", ""));
              return of(false);
            }
            return of(true);
          }
        }));
  }
}