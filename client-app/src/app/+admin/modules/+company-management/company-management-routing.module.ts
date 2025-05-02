// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { CompanyManagementComponent } from './company-management.component';
import { CompanyManipulationComponent } from './components/company-manipulation/company-manipulation.component';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: CompanyManagementComponent
  },
  {
    path: 'add',
    data: { breadcrumb: 'Add Company' },
    component: CompanyManipulationComponent
  },
  {
    //* for creating company from review user
    path: 'add/:name',
    data: { breadcrumb: 'Add Company' },
    component: CompanyManipulationComponent
  },
  {
    path: 'edit/:id',
    data: { breadcrumb: 'Edit Company' },
    component: CompanyManipulationComponent,
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CompanyManagementRoutingModule {}
