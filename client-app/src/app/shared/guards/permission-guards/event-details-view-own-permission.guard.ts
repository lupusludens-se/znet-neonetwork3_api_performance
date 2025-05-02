import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { MsalService, MsalBroadcastService } from '@azure/msal-angular';
import { InteractionStatus } from '@azure/msal-browser';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Observable, filter, switchMap, map, catchError, of } from 'rxjs';
import { PostInterface } from 'src/app/+learn/interfaces/post.interface';
import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { CommonService } from 'src/app/core/services/common.service';
import { HttpService } from 'src/app/core/services/http.service';
import { SpinnerService } from 'src/app/core/services/spinner.service';
import { CanActivateGaurdTypeService } from '../../services/can-activate-gaurd-type.service';
import { EventsApiEnum } from '../../enums/api/events-api.enum';

@UntilDestroy()
@Injectable({
	providedIn: 'root'
})
export class EventDetailsViewOwnPermissionGuard implements CanActivate {
	constructor(
		private msalService: MsalService,
		private canActivateGaurdTypeService: CanActivateGaurdTypeService,
		private authService: AuthService,
		private router: Router,
		private commonService: CommonService,
		private httpService: HttpService,
		private spinnerService: SpinnerService,
		private msalBroadcastService: MsalBroadcastService
	) { }

	canActivate(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot
	): Observable<boolean> | Promise<boolean> | boolean {
		return this.msalBroadcastService.inProgress$.pipe(
			filter((status: InteractionStatus) => status === InteractionStatus.None),
			switchMap(() => {
				if (this.msalService.instance.getAllAccounts().length == 0) {
					this.canActivateGaurdTypeService.setCanActivateGaurdType(typeof EventDetailsViewOwnPermissionGuard);
					return this.httpService
						.get<PostInterface>(`${EventsApiEnum.Events}/${route.params['id']}`, { expand: '' })
						.pipe(
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
			})
		);
	}
}
