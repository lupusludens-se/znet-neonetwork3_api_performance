// modules
import { APP_INITIALIZER, Injector, NgModule } from '@angular/core';
import { APP_BASE_HREF, CommonModule, LOCATION_INITIALIZED, PlatformLocation } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslateLoader, TranslateModule, TranslateService } from '@ngx-translate/core';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { MenuModule } from '../shared/modules/menu/menu.module';
import { UserAvatarModule } from '../shared/modules/user-avatar/user-avatar.module';
import { NotificationModule } from '../shared/modules/notification/notification.module';
import { FooterModule } from '../shared/modules/footer/footer.module';
import { GlobalSearchModule } from '../shared/modules/global-search/global-search.module';

// components
import { ForbiddenComponent } from './components/forbidden/forbidden.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { HeaderComponent } from './components/header/header.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { SnackbarComponent } from './components/snackbar/snackbar.component';
import { NotificationsComponent } from './components/notifications/notifications.component';
import { InactiveComponent } from './components/inactive/inactive.component';
import { HeaderSkeletonComponent } from './components/header/components/header-skeleton/header-skeleton.component';
import { NavbarSkeletonComponent } from './components/navbar/components/navbar-skeleton/navbar-skeleton.component';

// constants
import { icons } from '../shared/svg-icons';

// providers
import { SpinnerInterceptor } from './helpers/spinner.interceptor';
import { loginRequest, msalConfig, protectedResources, unprotectedResources } from './configs/auth.config';
import { InteractionType, IPublicClientApplication, PublicClientApplication } from '@azure/msal-browser';
import {
  MSAL_GUARD_CONFIG,
  MSAL_INSTANCE,
  MSAL_INTERCEPTOR_CONFIG,
  MsalBroadcastService,
  MsalGuard,
  MsalGuardConfiguration,
  MsalInterceptorConfiguration,
  MsalService
} from '@azure/msal-angular';
import { AuthService } from './services/auth.service';
import { ErrorInterceptor } from './helpers/error.interceptor';
import { CommonService } from './services/common.service';
import { TrackingInterceptor } from './helpers/tracking.interceptor';
import { ElementNotFoundComponent } from './components/element-not-found/element-not-found.component';
import { VerticalLineDecorModule } from '../shared/modules/vertical-line-decor/vertical-line-decor.module';
import { take } from 'rxjs';
import { SavedContentWidgetComponent } from './components/saved-content-widget/saved-content-widget.component';
import { SavedContentListModule } from '../shared/modules/saved-content/saved-content-list.module';
import { CustomRequestHeadersInterceptor } from './helpers/custom-request-headers.interceptor';
import { ModalModule } from "../shared/modules/modal/modal.module";
import { PublicModule } from '../public/public.module';
import { MsalCustomInterceptor } from './helpers/msal-custom.interceptor';
import { PublicHeaderSkeletonComponent } from './components/header/components/public-header-skeleton/public-header-skeleton.component';
import { SharedModule } from '../shared/shared.module';

export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication(msalConfig);
}

export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string> | null>();

  protectedResourceMap.set(protectedResources.pendingApi.endpoint, protectedResources.pendingApi.scopes);

  protectedResourceMap.set(unprotectedResources.heardViaApi.endpoint, unprotectedResources.heardViaApi.scopes);

  protectedResourceMap.set(unprotectedResources.countriesApi.endpoint, unprotectedResources.countriesApi.scopes);

  protectedResourceMap.set(unprotectedResources.userPendingApi.endpoint, unprotectedResources.userPendingApi.scopes);

  protectedResourceMap.set(unprotectedResources.contactUsApi.endpoint, unprotectedResources.contactUsApi.scopes);


  protectedResourceMap.set(
    unprotectedResources.unSubscribeGetApi.endpoint,
    unprotectedResources.unSubscribeGetApi.scopes
  );

  protectedResourceMap.set(
    unprotectedResources.unSubscribePostApi.endpoint,
    unprotectedResources.unSubscribePostApi.scopes
  );

  protectedResourceMap.set(
    unprotectedResources.networkStatsCountersApi.endpoint,
    unprotectedResources.networkStatsCountersApi.scopes
  );
  protectedResourceMap.set(protectedResources.getApi.endpoint, protectedResources.getApi.scopes);

  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap
  };
}

export function MSALGuardConfigFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: loginRequest
  };
}

export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  let todaysdate = new Date();
  todaysdate.setHours(0, 0, 0, 0);
  return new TranslateHttpLoader(http, 'assets/i18n/', '.json' + '?v=' + todaysdate.getTime());
}

export function ApplicationInitializerFactory(translate: TranslateService, injector: Injector) {
  return () =>
    new Promise<any>((resolve: any) => {
      const locationInitialized = injector.get(LOCATION_INITIALIZED, Promise.resolve(null));
      const languages: string[] = translate.getLangs();
      const defaultLang: string = languages !== null && languages.length > 0 ? languages[0] : 'en';
      locationInitialized.then(() => {
        translate
          .use(defaultLang)
          .pipe(take(1))
          .subscribe({
            next: () => {},
            error: err => console.error(err),
            complete: () => resolve(null)
          });
      });
    });
}

export function getBaseHref(platformLocation: PlatformLocation): string {
  return platformLocation.getBaseHrefFromDOM();
}

@NgModule({
  declarations: [
    ForbiddenComponent,
    NotFoundComponent,
    HeaderComponent,
    HeaderSkeletonComponent,
    NavbarComponent,
    NavbarSkeletonComponent,
    BreadcrumbComponent,
    SpinnerComponent,
    SnackbarComponent,
    NotificationsComponent,
    InactiveComponent,
    ElementNotFoundComponent,
    SavedContentWidgetComponent,
    PublicHeaderSkeletonComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FooterModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      },
      useDefaultLang: false
    }),
    RouterModule,
    SvgIconsModule.forRoot({
      icons: icons,
      sizes: {
        xs: '10px',
        sm: '12px',
        md: '14px',
        lg: '16px',
        xl: '18px',
        xxl: '20px',
        xxxl: '30px'
      },
      defaultSize: 'lg'
    }),
    MenuModule,
    UserAvatarModule,
    NotificationModule,
    GlobalSearchModule,
    VerticalLineDecorModule,
    SavedContentListModule,
    ModalModule,
    PublicModule,
    SharedModule
  ],
  exports: [
    CommonModule,
    TranslateModule,
    SvgIconsModule,
    ForbiddenComponent,
    NotFoundComponent,
    HeaderComponent,
    NavbarComponent,
    SpinnerComponent,
    SnackbarComponent,
    ElementNotFoundComponent
  ],
  providers: [
    CommonService,
    {
      provide: APP_INITIALIZER,
      useFactory: ApplicationInitializerFactory,
      deps: [TranslateService, Injector],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: CustomRequestHeadersInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: SpinnerInterceptor,
      multi: true
    },

    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalCustomInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TrackingInterceptor,
      multi: true
    },
    {
      provide: MSAL_INSTANCE,
      useFactory: MSALInstanceFactory
    },
    {
      provide: MSAL_GUARD_CONFIG,
      useFactory: MSALGuardConfigFactory
    },
    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useFactory: MSALInterceptorConfigFactory
    },
    {
      provide: APP_BASE_HREF,
      useFactory: getBaseHref,
      deps: [PlatformLocation]
    },
    AuthService,
    MsalService,
    MsalGuard,
    MsalBroadcastService
  ]
})
export class CoreModule {}
