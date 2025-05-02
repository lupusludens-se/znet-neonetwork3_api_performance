// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { AuthRedirectComponent } from './auth-redirect.component';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: AuthRedirectComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRedirectRoutingModule {}
