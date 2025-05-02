import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { MSAL_GUARD_CONFIG, MsalBroadcastService, MsalGuardConfiguration, MsalService } from '@azure/msal-angular';
import {
  AuthenticationResult,
  EventMessage,
  InteractionStatus,
  PopupRequest,
  RedirectRequest,
  SilentRequest
} from '@azure/msal-browser';
import { b2cPolicies } from 'src/app/core/configs/auth.config';

import { BehaviorSubject, filter, Observable, Subject, switchMap, throwError } from 'rxjs';

import { HttpService } from './http.service';
import { SnackbarService } from './snackbar.service';
import { CommonService } from './common.service';

import { UserInterface } from '../../shared/interfaces/user/user.interface';
import { UserRoleInterface } from '../../shared/interfaces/user/user-role.interface';
import { PayloadInterface } from '../interfaces/payload.interface';

import { UserProfileApiEnum } from '../../shared/enums/api/user-profile-api.enum';
import { CommonApiEnum } from '../enums/common-api.enum';
import { UserStatusEnum } from '../../user-management/enums/user-status.enum';
import { StorageKeysEnum } from '../enums/storage-keys.enum';
import { APP_BASE_HREF } from '@angular/common';
import { catchError, share, takeUntil } from 'rxjs/operators';
import * as dayjs from 'dayjs';
import { environment } from 'src/environments/environment';
import { HttpErrorResponse } from '@angular/common/http';
import { SkillsByCategoryInterface } from 'src/app/shared/interfaces/user/skills-by-category.interface';

// TODO: get rid of unnecessary logic
@Injectable()
export class AuthService {
  currentUser$: BehaviorSubject<UserInterface> = new BehaviorSubject<UserInterface>(null);
  readonly currentUserKey: string = 'dfsdgsdfiuysd67f5sda76';
  getCurrentUser$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  userStatuses = UserStatusEnum;
  private userData: object;
  private getCurrentUserSend: boolean = false;
  private isNewUser: boolean = !!sessionStorage.getItem(StorageKeysEnum.IsNewUser);

  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    @Inject(MSAL_GUARD_CONFIG)
    private readonly msalGuardConfig: MsalGuardConfiguration,
    private readonly msalBroadcastService: MsalBroadcastService,
    private readonly authService: MsalService,
    private readonly httpService: HttpService,
    private readonly snackbarService: SnackbarService,
    private readonly commonService: CommonService,
    private readonly router: Router,
    @Inject(APP_BASE_HREF) public baseHref: string
  ) {}

  //# region support methods
  static isLoggedIn(): boolean {
    const dateNow = Date.now().toString();
    const expiredIn = localStorage.getItem(StorageKeysEnum.ExpiresAt);

    return dateNow < expiredIn;
  }

  static needSilentLogIn(): boolean {
    if (!localStorage.getItem(StorageKeysEnum.ExpiresAt)) {
      return false;
    }
    const expirationDate = dayjs(parseInt(localStorage.getItem(StorageKeysEnum.ExpiresAt) + '000'));
    const nowDate = dayjs();
    // if now date is later than expiration date of access token (1h), but it's not later when expiration date of refresh token (24h) then we need silent login (to refresh access token)
    return nowDate > expirationDate && nowDate.diff(expirationDate) < 82800000; // 23h for refresh token (+1h for access token)
  }

  private static getUserData(userData: object): UserInterface {
    return {
      firstName: userData['given_name'],
      lastName: userData['family_name'],
      email: userData['email'],
      userName: userData['email'],
      statusId: UserStatusEnum.Active,
      companyId: 4,
      country: null,
      countryId: 2,
      image: null,
      imageName: null,
      heardViaId: 1,
      heardViaName: null,
      roles: [{ id: 1 }] as UserRoleInterface[]
    };
  }

  currentUser(): Observable<UserInterface> {
    return this.currentUser$.pipe(share());
  }

  getCurrentUser(): Observable<boolean> {
    return this.getCurrentUser$.pipe(share());
  }

  listenToAuthSubjects(): void {
    this.listenToMsalSubject();
    this.listenToMsalInProgress();
    this.listenForCurrentUser();

    this.authService
      .handleRedirectObservable()
      .pipe(catchError(error => throwError(error)))
      .subscribe(() => this.checkAndSetActiveAccount());

    if (AuthService.isLoggedIn() || AuthService.needSilentLogIn()) {
      this.getCurrentUser$.next(true);

      if (location.pathname === this.baseHref) {
        this.router.navigate(['/auth-redirect']);
      }
    } else {
      const path = location.pathname === this.baseHref ? '' : location.pathname.replace(this.baseHref, '');
      this.router.navigate([path]);
    }
  }

  logout(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this.authService.logoutRedirect();
    localStorage.clear();
    sessionStorage.clear();
  }

  //# endregion

  //# region current user
  // TODO: update user create flow when ready.
  //  Add route guards and right redirection for each type of user

  loginRedirect(returnUrl: string = ''): void {
    let loginDirect = {
      scopes: ['openid'],
      authority: b2cPolicies.authorities.signUpSignIn.authority
    };

    this.login(loginDirect, false, returnUrl);
  }

  passwordReset() {
    let passwordResetRequest = {
      scopes: ['openid'],
      authority: b2cPolicies.authorities.passwordReset.authority
    };
    this.login(passwordResetRequest);
  }

  // passwordReset() {
  //   let passwordResetRequest = {
  //     scopes: ['openid'],
  //     authority: b2cPolicies.authorities.passwordReset.authority
  //   };

  //   const request = {
  //     redirectStartPage: environment.passwordReset.redirectUri,
  //     ...this.msalGuardConfig.authRequest,
  //     ...passwordResetRequest
  //   } as RedirectRequest;

  //   this.authService.loginRedirect(request);
  // }

  changePassword() {
    let changePasswordRequest = {
      scopes: ['openid'],
      authority: b2cPolicies.authorities.changePassword.authority,
      redirectUri: b2cPolicies.authorities.changePassword.redirectUri
    };

    this.login(changePasswordRequest, true);
  }

  //# endregion

  //# region msal subject subscriptions
  private listenToMsalInProgress(): void {
    this.msalBroadcastService.inProgress$
      .pipe(filter((status: InteractionStatus) => status === InteractionStatus.None))
      .subscribe(() => this.checkAndSetActiveAccount());
  }

  private listenToMsalSubject(): void {
    this.msalBroadcastService.msalSubject$.pipe(takeUntil(this.unsubscribe$)).subscribe((result: EventMessage) => {
      if (result.error != undefined && result.error.message.indexOf('AADB2C90118') > 0) {
        this.passwordReset();
      } else if (result.error != undefined && result.error.message.indexOf('AADB2C90091') > 0) {
        this.login();
      } else if (result.error != undefined) {
        this.logout();
      } else if (result.payload != undefined) {
        const payload: PayloadInterface = <AuthenticationResult>result.payload;

        if (payload.idTokenClaims != undefined) {
          this.userData = (<SilentRequest>result.payload).account.idTokenClaims;
          localStorage.setItem(StorageKeysEnum.ExpiresAt, payload.idTokenClaims.exp.toString());
          const isNew = !!(<Record<string, boolean>>this.userData)?.newUser;

          if (!this.isNewUser && isNew && !this.getCurrentUserSend) {
            this.createUser(this.userData);
          }

          if ((!this.getCurrentUserSend && !isNew) || (!this.getCurrentUserSend && this.isNewUser)) {
            this.getCurrentUser$.next(true);
          }
        }
      }
    });
  }

  private listenForCurrentUser(): void {
    this.getCurrentUser$
      .pipe(
        filter(response => response),
        switchMap(() =>
          this.httpService.get<UserInterface>(
            `${CommonApiEnum.CurrentUser}?expand=timezone, company, country, roles, permissions, image, userprofile, userprofile.state, userprofile.urllinks, userprofile.categories,userprofile.regions,,userprofile.skills`
          )
        ),
        catchError((error: HttpErrorResponse) => {
          if (location.pathname === environment.redirect) {
            this.router.navigate([this.baseHref]);
          }

          return throwError(error);
        })
      )
      .subscribe(currentUser => {
        if (currentUser != null) {
          if (currentUser.statusId === UserStatusEnum.Deleted || currentUser.statusId === UserStatusEnum.Inactive) {
            this.logout();
            return;
          }

          this.currentUser$.next(currentUser);
          this.getCurrentUserSend = true;

          const allPermissions = [
            ...new Set(
              []
                .concat(...currentUser.roles.map(r => r.permissions).filter(x => x.length > 0), currentUser.permissions)
                .map(p => p.id)
            )
          ];

          localStorage.setItem(this.currentUserKey, JSON.stringify(allPermissions));

          if (currentUser.statusId !== this.userStatuses.Active) {
            this.commonService.loadInitialData(false, false);
          } else {
            this.commonService.loadInitialData(true, true);
          }
        }
      });
  }

  private createUser(userData: object): void {
    this.httpService.post<any>(UserProfileApiEnum.Users, AuthService.getUserData(userData)).subscribe(() => {
      sessionStorage.setItem(StorageKeysEnum.IsNewUser, 'true');
      this.getCurrentUser$.next(true);
    });
  }

  private checkAndSetActiveAccount(): void {
    const activeAccount = this.authService.instance.getActiveAccount();

    if (!activeAccount && this.authService.instance.getAllAccounts().length > 0) {
      const accounts = this.authService.instance.getAllAccounts();
      this.authService.instance.setActiveAccount(accounts[0]);
    } else if (activeAccount && !AuthService.isLoggedIn() && AuthService.needSilentLogIn()) {
      this.login();
    }
  }

  private login(
    userFlowRequest?: RedirectRequest | PopupRequest,
    isChangePasswordRequest: boolean = false,
    returnUrl: string = ''
  ): void {
    const request = {
      redirectStartPage:
        returnUrl == ''
          ? isChangePasswordRequest
            ? environment.changePassword.redirectUri
            : environment.redirect
          : `${environment.baseAppUrl}${returnUrl.substring(1)}`,
      ...this.msalGuardConfig.authRequest,
      ...userFlowRequest
    } as RedirectRequest;

    this.authService.loginRedirect(request);
  }

  public getSkillsByCategories() {
    return this.httpService.get<SkillsByCategoryInterface[]>(`${UserProfileApiEnum.getSkillsByCategory}`);
  }

  public getSkillsByCategoriesForOnboardUser(roleIds: number[], userId: number) {
    return this.httpService.get<SkillsByCategoryInterface[]>(
      `${UserProfileApiEnum.getSkillsByCategory}` + `/${userId}`,
      { roleIds }
    );
  }
}
