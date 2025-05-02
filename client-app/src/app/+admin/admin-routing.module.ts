// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { AdminAllPermissionGuard } from '../shared/guards/permission-guards/admin-all-permission.guard';
import { AnnouncementManagementPermissionGuard } from '../shared/guards/permission-guards/announcement-event-management-permission.guard';
import { CompanyManagementPermissionGuard } from '../shared/guards/permission-guards/company-management-permission.guard';
import { EventManagementPermissionGuard } from '../shared/guards/permission-guards/event-management-permission.guard';
import { ToolManagementPermissionGuard } from '../shared/guards/permission-guards/tool-management-permission.guard';

// components
import { AdminComponent } from './admin.component';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: AdminComponent,
    canActivate: [MsalGuard, AdminAllPermissionGuard]
  },
  {
    path: 'tool-management',
    data: { breadcrumb: 'Tool Management' },
    loadChildren: () =>
      import('./modules/+tools-management/tools-management.module').then(m => m.ToolsManagementModule),
    canActivate: [MsalGuard],
    canLoad: [ToolManagementPermissionGuard]
  },
  {
    path: 'initiatives',
    data: { breadcrumb: 'Initiatives' },
    loadChildren: () =>
      import('./modules/+initiatives-management/initiatives-management.module').then(
        m => m.InitiativesManagementModule
      ),
    canActivate: [MsalGuard, AdminAllPermissionGuard]
  },
  {
    path: 'announcements',
    data: { breadcrumb: 'Announcements' },
    loadChildren: () =>
      import('./modules/announcements/+announcement/announcement.module').then(m => m.AnnouncementModule),
    canActivate: [MsalGuard],
    canLoad: [AnnouncementManagementPermissionGuard]
  },
  {
    path: 'events/create-an-event',
    data: { breadcrumb: 'Create an Event' },
    loadChildren: () => import('./modules/+create-event/create-event.module').then(m => m.CreateEventModule),
    canActivate: [MsalGuard],
    canLoad: [EventManagementPermissionGuard]
  },
  {
    path: 'events/edit-event/:id',
    data: { breadcrumb: 'Edit Event' },
    loadChildren: () => import('./modules/+edit-event/edit-event.module').then(m => m.EditEventModule),
    pathMatch: 'full',
    canActivate: [MsalGuard],
    canLoad: [EventManagementPermissionGuard]
  },
  {
    path: 'admit-users',
    data: { breadcrumb: 'Admit Users' },
    loadChildren: () => import('../admit/+admit-users/admit-users.module').then(m => m.AdmitUsersModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'user-management',
    data: { breadcrumb: 'User Management' },
    loadChildren: () => import('../user-management/+users-list/users-list.module').then(m => m.UsersListModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'company-management',
    data: { breadcrumb: 'Company Management' },
    loadChildren: () =>
      import('./modules/+company-management/company-management.module').then(m => m.CompanyManagementModule),
    canActivate: [MsalGuard],
    canLoad: [CompanyManagementPermissionGuard]
  },
  {
    path: 'email-alerts',
    data: { breadcrumb: 'Email Alert Settings' },
    loadChildren: () => import('./modules/+email-alerts/email-alerts.module').then(m => m.EmailAlertsModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'user-feedback',
    data: { breadcrumb: 'User Feedback' },
    loadChildren: () =>
      import('./modules/+feedback-management/feedback-management.module').then(m => m.FeedbackManagementModule),
    canActivate: [MsalGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
