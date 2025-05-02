// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { NotificationsComponent } from './notifications.component';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: NotificationsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NotificationsRoutingModule {}
