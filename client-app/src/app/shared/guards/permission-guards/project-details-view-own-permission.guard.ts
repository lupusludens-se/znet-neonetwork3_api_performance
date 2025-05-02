import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';

import { AuthService } from '../../../core/services/auth.service';
import { Observable, catchError, filter, map, of, switchMap } from 'rxjs';
import { SpinnerService } from 'src/app/core/services/spinner.service';
import { UntilDestroy } from '@ngneat/until-destroy';
import { CommonService } from 'src/app/core/services/common.service';
import { MsalService, MsalBroadcastService } from '@azure/msal-angular';
import { InteractionStatus } from '@azure/msal-browser';
import { PermissionService } from 'src/app/core/services/permission.service';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';

@UntilDestroy()
@Injectable({
  providedIn: 'root'
})

export class ProjectDetailsViewOwnPermissionGuard implements CanActivate {
  constructor(private msalService: MsalService,
    private authService: AuthService,
    private commonService: CommonService,
    private permissionService: PermissionService,
    private spinnerService: SpinnerService,
    private msalBroadcastService: MsalBroadcastService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    let hasPermission: boolean = false;
    return this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None),
        switchMap(() => {
          if (this.msalService.instance.getAllAccounts().length == 0) { 
            return  this.authService.currentUser$.pipe(
              map((currentUser) => {
                if (currentUser == null) {
                  const allPermissions = JSON.parse(localStorage.getItem(this.authService.currentUserKey));

                  if (!allPermissions) {
                    this.authService.loginRedirect(state.url);
                    this.spinnerService.onStarted(null);
                    return false;
                  }

                  hasPermission =
                    this.permissionService.hasPermission(allPermissions, PermissionTypeEnum.ProjectsManageOwn) ||
                    this.permissionService.hasPermission(allPermissions, PermissionTypeEnum.ProjectCatalogView);
                } else {
                  hasPermission =
                    this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.ProjectsManageOwn) ||
                    this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.ProjectCatalogView);
                }
              }),
              catchError(error => {
                this.authService.loginRedirect(state.url);
                this.spinnerService.onStarted(null);
                return of(false);
              })
            );
          } else {
            const payload = this.msalService.instance.getActiveAccount();
            this.msalService.instance.setActiveAccount(payload);
            this.commonService.hideSkeleton$.next(false);
            return of(true);
          }
        }));
  }
}