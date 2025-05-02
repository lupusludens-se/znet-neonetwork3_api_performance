import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CorporateInitiativePermissionGuard } from 'src/app/shared/guards/permission-guards/corporate-initiative-permission.guard';
import { InitiativeProjectComponent } from './components/initiative-project-form/initiative-project.component';

const routes: Routes = [
  {
    path: 'projects',
    data: { breadcrumb: 'View All - Projects' },
    component: InitiativeProjectComponent,
    canActivate: [CorporateInitiativePermissionGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InitiativeManageProjectRoutingModule {}
