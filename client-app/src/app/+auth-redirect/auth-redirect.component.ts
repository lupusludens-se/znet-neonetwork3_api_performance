import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subject, takeUntil } from 'rxjs';
import { AuthService } from '../core/services/auth.service';
import { HttpService } from '../core/services/http.service';
import { PATCH_PAYLOAD } from '../shared/constants/patch-payload.const';
import { UserProfileApiEnum } from '../shared/enums/api/user-profile-api.enum';
import { UserStatusEnum } from '../user-management/enums/user-status.enum';

@UntilDestroy()
@Component({
  selector: 'neo-auth-redirect',
  templateUrl: './auth-redirect.component.html',
  styleUrls: ['../../../src/app/core/components/spinner/spinner.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AuthRedirectComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  public hideLoader: boolean = false;

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly httpService: HttpService
  ) {}

  ngOnInit(): void {
    this.authService
      .getCurrentUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(value => {
        if (!value) {
          this.authService.getCurrentUser$.next(true);
        }
      });

    this.authService
      .currentUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(user => {
        if (user) {
          if (user.statusId === UserStatusEnum.Active) {
            if (user.requestDeleteDate) {
              const patchPayload = PATCH_PAYLOAD;
              patchPayload.jsonPatchDocument[0].value = null;
              patchPayload.jsonPatchDocument[0].op = 'replace';
              patchPayload.jsonPatchDocument[0].path = '/RequestDeleteDate';

              this.httpService.patch(UserProfileApiEnum.UsersCurrent, patchPayload).subscribe(() => {});
            }
            this.router.navigate(['/dashboard']);
          } else {
            this.hideLoader = true;
            console.warn(`User's status is not Active.`);
          }
        } else if (AuthService.needSilentLogIn()) {
          this.authService.loginRedirect();
        } else {
          console.warn(`User is not available and User's access token can't be refreshed.`);
        }
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
