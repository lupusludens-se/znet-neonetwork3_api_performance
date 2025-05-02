// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { SpAdminComponent } from './sp-admin.component';
import { SPAdminPermissionGuard } from '../shared/guards/permission-guards/sp-admin-permission.guard';

// components

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: SpAdminComponent,
    canActivate: [MsalGuard, SPAdminPermissionGuard]
  },
  {
    path: 'users',
    data: { breadcrumb: 'Users' },
    loadChildren: () => import('./user-management/+users-list/sp-users-list.module').then(m => m.SPUsersListModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'company-profile',
    data: { breadcrumb: 'Company Profile' },
    loadChildren: () =>
      import('../+admin/modules/+company-management/components/company-profile/company-profile.module').then(
        m => m.CompanyProfileModule
      ),
    canActivate: [MsalGuard, SPAdminPermissionGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SPAdminRoutingModule {}
