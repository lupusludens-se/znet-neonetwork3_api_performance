import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CorporateInitiativePermissionGuard } from 'src/app/shared/guards/permission-guards/corporate-initiative-permission.guard';
import { InitiativeLearnComponent } from './components/initiative-learn-form/initiative-learn.component';


const routes: Routes = [
  {
    path: 'learn',
    component: InitiativeLearnComponent,
    canActivate: [CorporateInitiativePermissionGuard],
    data: { breadcrumb: 'View All - Learn' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InitiativeManageLearnRoutingModule {}
