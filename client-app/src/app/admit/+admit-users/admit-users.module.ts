// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { PaginationModule } from '../../shared/modules/pagination/pagination.module';
import { UserAvatarModule } from '../../shared/modules/user-avatar/user-avatar.module';
import { MsalGuard } from '@azure/msal-angular';
import { ModalModule } from '../../shared/modules/modal/modal.module';

// components
import { AdmitUsersComponent } from './admit-users.component';
import { UserAccessManagementPermissionGuard } from 'src/app/shared/guards/permission-guards/user-access-management-permission.guard';
import { EmptyPageModule } from 'src/app/shared/modules/empty-page/empty-page.module';
import { UserDataService } from 'src/app/user-management/services/user.data.service';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: AdmitUsersComponent,
    canActivate: [UserAccessManagementPermissionGuard]
  },
  {
    path: 'review-user/:id',
    data: {
      breadcrumb: 'Review User'
    },
    loadChildren: () => import('../../admit/+review-user/review-user.module').then(m => m.ReviewUserModule),
    canActivate: [MsalGuard, UserAccessManagementPermissionGuard],
    pathMatch: 'full'
  }
];

@NgModule({
  declarations: [AdmitUsersComponent],
  imports: [
    CommonModule,
    TranslateModule,
    RouterModule.forChild(routes),
    SvgIconsModule,
    PaginationModule,
    UserAvatarModule,
    ModalModule,
    EmptyPageModule,
  ],
  providers: [UserAccessManagementPermissionGuard, UserDataService]
})
export class AdmitUsersModule { }
