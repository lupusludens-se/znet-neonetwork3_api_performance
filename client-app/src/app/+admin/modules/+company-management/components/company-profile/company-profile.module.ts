import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { SPAdminPermissionGuard } from 'src/app/shared/guards/permission-guards/sp-admin-permission.guard';
import { CompanyProfileComponent } from 'src/app/+admin/modules/+company-management/components/company-profile/company-profile.component';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: CompanyProfileComponent,
    canActivate: [MsalGuard, SPAdminPermissionGuard]
  }
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forChild(routes)]
})
export class CompanyProfileModule {}
