import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CorporateInitiativePermissionGuard } from 'src/app/shared/guards/permission-guards/corporate-initiative-permission.guard';
import { InitiativeMessageComponent } from './components/initiative-message-form/initiative-message.component';

const routes: Routes = [
  {
    path: 'messages',
    component: InitiativeMessageComponent,
    canActivate: [CorporateInitiativePermissionGuard],
    data: { breadcrumb: 'View All - Messages' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InitiativeManageMessageRoutingModule {}
