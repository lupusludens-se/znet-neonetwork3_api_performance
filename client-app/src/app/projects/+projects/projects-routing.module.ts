// modules
import { NgModule } from '@angular/core';
import { MsalGuard } from '@azure/msal-angular';
import { RouterModule, Routes } from '@angular/router';

// components
import { ProjectsComponent } from './projects.component';

import { ProjectDetailsViewOwnPermissionGuard } from 'src/app/shared/guards/permission-guards/project-details-view-own-permission.guard';
import { ProjectsAllPermissionGuard } from 'src/app/shared/guards/permission-guards/all-projects-permission.guard';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: ProjectsComponent,
   canActivate: [ProjectsAllPermissionGuard]
  },
  {
    path: ':id',
    data: { breadcrumb: 'Project Details' },
    loadChildren: () => import('../+project-details/project-details.module').then(m => m.ProjectDetailsModule),
    canActivate: [ProjectDetailsViewOwnPermissionGuard, MsalGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectsRoutingModule {}
