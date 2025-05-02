// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { CommunityComponent } from './community.component';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: CommunityComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CommunityRoutingModule {}
