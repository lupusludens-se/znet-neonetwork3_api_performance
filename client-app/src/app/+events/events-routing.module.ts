// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { EventsComponent } from './events.component';
import { EventViewComponent } from './component/event-view/event-view.component';
import { EventDetailsViewOwnPermissionGuard } from '../shared/guards/permission-guards/event-details-view-own-permission.guard';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    pathMatch: 'full',
    component: EventsComponent
  }, 
  {
    path: ':id',
    data: { breadcrumb: 'Event Details' },
    component: EventViewComponent,
    pathMatch: 'full',
    canActivate: [EventDetailsViewOwnPermissionGuard]
  },   
  {
    path: ':id/:src',
    data: { breadcrumb: 'Event Details' },
    component: EventViewComponent,
    pathMatch: 'full',
    canActivate: [EventDetailsViewOwnPermissionGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EventsRoutingModule {}
