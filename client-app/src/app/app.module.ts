// modules
import { ErrorHandler, NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from './core/core.module';
import { OnboardingWizardModule } from './onboarding-wizard/onboarding-wizard.module';

// components
import { AppComponent } from './app.component';
import { UserProfileGuard } from './shared/guards/user-profile.guard';
import { APP_BASE_HREF, PlatformLocation } from '@angular/common';
import { FooterModule } from './shared/modules/footer/footer.module';
import { UserProfileEditPermissionGuard } from './shared/guards/permission-guards/user-profile-edit-permission.guard';
import { AdminAllPermissionGuard } from './shared/guards/permission-guards/admin-all-permission.guard';
import { ProjectManagePermissionGuard } from './shared/guards/permission-guards/project-manage.permission.guard';
import { ProjectsAllPermissionGuard } from './shared/guards/permission-guards/all-projects-permission.guard';
import { CountriesService } from './shared/services/countries.service';
import { LogService } from './shared/services/log.service';
import { LogPublishersService } from './shared/services/log-publisher.service';
import { NeoLoggingErrorHandler } from './shared/interfaces/logging/neo-logging-error-handler';
import { NgxMaskModule } from 'ngx-mask';
import { PublicModule } from './public/public.module';
import { ModalModule } from './shared/modules/modal/modal.module';
import { LoginComponent } from './login/login.component';
import { SPAdminPermissionGuard } from './shared/guards/permission-guards/sp-admin-permission.guard';
import { UserFeedbackComponent } from './+dashboard/components/layout/user-feedback/user-feedback.component';
import { ReactiveFormsModule } from '@angular/forms';


export function getBaseHref(platformLocation: PlatformLocation): string {
  return platformLocation.getBaseHrefFromDOM();
}

@NgModule({
  declarations: [AppComponent, LoginComponent, UserFeedbackComponent],
  imports: [CoreModule, AppRoutingModule, ModalModule, OnboardingWizardModule, ReactiveFormsModule,
    FooterModule, PublicModule, NgxMaskModule.forRoot()],
  bootstrap: [AppComponent],
  providers: [
    UserProfileGuard,
    UserProfileEditPermissionGuard,
    AdminAllPermissionGuard,
    SPAdminPermissionGuard,
    ProjectsAllPermissionGuard,
    ProjectManagePermissionGuard,
    CountriesService,
    LogService,
    LogPublishersService,
    {
      provide: APP_BASE_HREF,
      useFactory: getBaseHref,
      deps: [PlatformLocation]
    },
    {
      provide: ErrorHandler,
      useClass: NeoLoggingErrorHandler
    }
  ]
})
export class AppModule { }
