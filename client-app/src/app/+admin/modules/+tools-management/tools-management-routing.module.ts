// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { ToolsManagementComponent } from './tools-management.component';
import { ToolsManipulationComponent } from './components/tools-manipulation/tools-manipulation.component';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: ToolsManagementComponent
  },
  {
    path: 'add',
    data: { breadcrumb: 'Add Tool' },
    component: ToolsManipulationComponent
  },
  {
    path: 'edit/:id',
    data: { breadcrumb: 'Edit Tool' },
    component: ToolsManipulationComponent,
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ToolsManagementRoutingModule {}
