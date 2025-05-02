// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InitiativesManagementComponent } from './initiatives-management.component';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: InitiativesManagementComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InitiativesManagementRoutingModule {}
