import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CorporateInitiativePermissionGuard } from 'src/app/shared/guards/permission-guards/corporate-initiative-permission.guard';
import { InitiativeCommunityFormComponent } from './components/initiative-community-form/initiative-community-form.component';

const routes: Routes = [
  {
    path: 'community',
    component: InitiativeCommunityFormComponent,
    canActivate: [CorporateInitiativePermissionGuard],
    data: { breadcrumb: 'View All - Community' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InitiativeManageCommunityRoutingModule {}
