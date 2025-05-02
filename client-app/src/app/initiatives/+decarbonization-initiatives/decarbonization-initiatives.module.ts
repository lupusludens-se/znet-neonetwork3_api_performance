import { NgModule } from '@angular/core';
import { DecarbonizationInitiativesComponent } from './decarbonization-initiatives.component';
import { RouterModule, Routes } from '@angular/router';
import { TopPanelModule } from 'src/app/shared/modules/top-panel/top-panel.module';
import { InitiativeInformationModule } from 'src/app/shared/modules/initiatives/initiative-information.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { InitiativeSharedService } from '../shared/services/initiative-shared.service';
import { DatePipe } from '@angular/common';
import { AdminCorporateInitiativePermissionGuard } from 'src/app/shared/guards/permission-guards/admin-corporate-initiative-permission.guard';
import { CorporateInitiativePermissionGuard } from 'src/app/shared/guards/permission-guards/corporate-initiative-permission.guard';
import { DecarbonizationInitiativeService } from './services/decarbonization-initiatives.service';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: DecarbonizationInitiativesComponent
  },
  {
    path: ':id',
    data: { breadcrumb: 'View Initiative' },
    loadChildren: () =>
      import('../../initiatives/+initatives/+view-initiative/view-initiative.module').then(m => m.ViewInitiativeModule),
    canActivate: [AdminCorporateInitiativePermissionGuard]
  },
  {
    path: ':id',
    data: { breadcrumb: 'View Initiative' },
    loadChildren: () =>
      import('../../initiatives/+manage-files-content/manage-files.module').then(m => m.ManageFilesModule),
    canActivate: [AdminCorporateInitiativePermissionGuard]
  },
  {
    path: ':id',
    data: { breadcrumb: 'View Initiative' },
    loadChildren: () =>
      import('../../initiatives/+manage-message-content/initiative-manage-message.module').then(
        m => m.InitiativeManageMessageModule
      ),
    canActivate: [CorporateInitiativePermissionGuard]
  },
  {
    path: ':id',
    data: { breadcrumb: 'View Initiative' },
    loadChildren: () =>
      import('../../initiatives/+manage-tools-content/initiative-manage-tools.module').then(
        m => m.InitiativeManageToolsModule
      ),
    canActivate: [CorporateInitiativePermissionGuard]
  },
  {
    path: ':id',
    data: { breadcrumb: 'View Initiative' },
    loadChildren: () =>
      import('../../initiatives/+manage-learn-content/initiative-manage-learn.module').then(
        m => m.InitiativeManageLearnModule
      ),
    canActivate: [CorporateInitiativePermissionGuard]
  },
  {
    path: ':id',
    data: { breadcrumb: 'View Initiative' },
    loadChildren: () =>
      import('../../initiatives/+manage-community-content/initiative-manage-community.module').then(
        m => m.InitiativeManageCommunityModule
      ),
    canActivate: [CorporateInitiativePermissionGuard]
  },
  {
    path: ':id',
    data: { breadcrumb: 'View Initiative' },
    loadChildren: () =>
      import('../../initiatives/+manage-project-content/initiative-manage-project.module').then(
        m => m.InitiativeManageProjectModule
      ),
    canActivate: [CorporateInitiativePermissionGuard]
  }
];

@NgModule({
  declarations: [DecarbonizationInitiativesComponent],
  imports: [RouterModule.forChild(routes), TopPanelModule, SharedModule, InitiativeInformationModule],
  providers: [InitiativeSharedService, DatePipe, DecarbonizationInitiativeService],
  exports: [DecarbonizationInitiativesComponent]
})
export class DecarbonizationInitiativesModule {}
