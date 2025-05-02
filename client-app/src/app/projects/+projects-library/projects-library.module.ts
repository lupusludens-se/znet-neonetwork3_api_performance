// modules
import { NgModule } from '@angular/core';
import { MsalGuard } from '@azure/msal-angular';
import { SharedModule } from '../../shared/shared.module';
import { ModalModule } from '../../shared/modules/modal/modal.module';
import { PaginationModule } from '../../shared/modules/pagination/pagination.module';
import { NoResultsModule } from '../../shared/modules/no-results/no-results.module';
import { MenuModule } from '../../shared/modules/menu/menu.module';
import { EmptyPageModule } from 'src/app/shared/modules/empty-page/empty-page.module';

// components
import { ProjectsLibraryComponent } from './projects-library.component';
import { RouterModule, Routes } from '@angular/router';
import { ProjectsTableRowComponent } from './projects-table-row/projects-table-row.component';

// services
import { ProjectCatalogService } from '../+projects/services/project-catalog.service';
import { ProjectManagePermissionGuard } from 'src/app/shared/guards/permission-guards/project-manage.permission.guard';
import { ExportProjectModalComponent } from '../export-modal/project-export-modal.component';
import { TextInputModule } from 'src/app/shared/modules/controls/text-input/text-input.module';
import { ReactiveFormsModule } from '@angular/forms';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: ProjectsLibraryComponent
  },
  {
    path: 'add-project',
    data: { breadcrumb: 'Create a New Project' },
    loadChildren: () => import('../+add-project/add-project.module').then(m => m.AddProjectModule),
    canActivate: [MsalGuard, ProjectManagePermissionGuard]
  },
  {
    path: 'edit-project/:id',
    data: { breadcrumb: 'Edit Project' },
    pathMatch: 'full',
    loadChildren: () => import('../+edit-project/edit-project.module').then(m => m.EditProjectModule),
    canActivate: [MsalGuard]
  }
];

@NgModule({
  declarations: [ProjectsLibraryComponent, ProjectsTableRowComponent, ExportProjectModalComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    NoResultsModule,
    ModalModule,
    PaginationModule,
    EmptyPageModule,
    MenuModule,
    TextInputModule,
    ReactiveFormsModule
  ],
  providers: [ProjectCatalogService, ProjectManagePermissionGuard]
})
export class ProjectsLibraryModule {}
