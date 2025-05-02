import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { NgModule } from '@angular/core';

import { ForbiddenComponent } from './core/components/forbidden/forbidden.component';
import { NotFoundComponent } from './core/components/not-found/not-found.component';
import { InactiveComponent } from './core/components/inactive/inactive.component';

import { ProjectManagePermissionGuard } from './shared/guards/permission-guards/project-manage.permission.guard';
import { UserProfileResolverService } from './+user-profile/services/user-profile.resolver.service';
import { ProjectViewPermissionGuard } from './shared/guards/permission-guards/project-view-permission.guard';
import { LoginComponent } from './login/login.component';
import { LoginGaurd } from './shared/guards/permission-guards/login-gaurd';
import { CorporateInitiativePermissionGuard } from './shared/guards/permission-guards/corporate-initiative-permission.guard';
import { AdminCorporateInitiativePermissionGuard } from './shared/guards/permission-guards/admin-corporate-initiative-permission.guard';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    pathMatch: 'full',
    redirectTo: '/'
  },
  {
    path: 'login',
    data: { breadcrumbSkip: true },
    component: LoginComponent,
    canActivate: [LoginGaurd]
  },
  {
    path: '',
    loadChildren: () => import('./+landing/landing.module').then(m => m.LandingModule)
  },
  {
    path: 'unsubscribe/:token',
    loadChildren: () => import('./+unsubscribe/unsubscribe.module').then(m => m.UnsubscribeModule)
  },
  {
    path: 'sign-up',
    loadChildren: () => import('./+sign-up/sign-up.module').then(m => m.SignUpModule)
  },
  {
    path: 'auth-redirect',
    loadChildren: () => import('./+auth-redirect/auth-redirect.module').then(m => m.AuthRedirectModule)
  },
  {
    path: 'dashboard',
    data: { breadcrumb: 'Dashboard' },
    loadChildren: () => import('./+dashboard/dashboard.module').then(m => m.DashboardModule)
  },
  {
    path: 'projects',
    data: { breadcrumb: 'Projects' },
    loadChildren: () => import('./projects/+projects/projects.module').then(m => m.ProjectsModule)
  },
  {
    path: 'projects-library',
    data: { breadcrumb: 'Project Library' },
    loadChildren: () =>
      import('./projects/+projects-library/projects-library.module').then(m => m.ProjectsLibraryModule),
    canActivate: [MsalGuard, ProjectManagePermissionGuard]
  },
  {
    path: 'learn',
    data: { breadcrumb: 'Learn' },
    loadChildren: () => import('./+learn/learn.module').then(m => m.LearnModule)
  },
  {
    path: 'tools',
    data: { breadcrumb: 'Tools' },
    loadChildren: () => import('./+tools/tools.module').then(m => m.ToolsModule)
  },
  {
    path: 'community',
    data: { breadcrumb: 'Community' },
    loadChildren: () => import('./+community/community.module').then(m => m.CommunityModule)
  },
  {
    path: 'forum',
    data: { breadcrumb: 'Forum' },
    loadChildren: () => import('./+forum/forum.module').then(m => m.ForumModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'create-initiative',
    data: { breadcrumb: 'Create a new initiative' },
    loadChildren: () =>
      import('./initiatives/+create-initiative/create-initiative.module').then(m => m.CreateInitiativeModule),
    pathMatch: 'full',
    canActivate: [CorporateInitiativePermissionGuard]
  },
  {
    path: 'decarbonization-initiatives',
    data: { breadcrumb: 'Decarbonization Initiatives' },
    loadChildren: () =>
      import('../app/initiatives/+decarbonization-initiatives/decarbonization-initiatives.module').then(
        m => m.DecarbonizationInitiativesModule
      ),
    canActivate: [AdminCorporateInitiativePermissionGuard]
  },
  {
    path: 'edit-initiative/:id',
    data: { breadcrumb: 'Edit Initiative' },
    loadChildren: () =>
      import('./initiatives/+edit-initiative/edit-initiative.module').then(m => m.EditInitiativeModule),
    canActivate: [CorporateInitiativePermissionGuard]
  },
  {
    path: 'messages',
    data: { breadcrumb: 'Messages' },
    loadChildren: () => import('./+messages/messages.module').then(m => m.MessagesModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'user-profile/:id',
    loadChildren: () => import('./+user-profile/user-profile.module').then(m => m.UserProfileModule),
    data: {
      breadcrumb: (data: any) => {
        return `${data.user.firstName} ${data.user.lastName}`;
      }
    },
    resolve: { user: UserProfileResolverService },
    canActivate: [MsalGuard]
  },
  {
    path: 'company-profile/:id',
    data: { breadcrumb: 'Company Profile' },
    loadChildren: () => import('./+company-profile/company-profile.module').then(m => m.CompanyProfileModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'notifications',
    data: { breadcrumb: 'Notifications' },
    loadChildren: () => import('./+notifications/notifications.module').then(m => m.NotificationsModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'settings',
    data: { breadcrumb: 'Account Settings' },
    loadChildren: () => import('./+settings/settings.module').then(m => m.SettingsModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'saved-content',
    data: { breadcrumb: 'Saved' },
    loadChildren: () => import('./+saved-content/saved-content.module').then(m => m.SavedContentModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'topics',
    data: { breadcrumb: 'Topics' },
    loadChildren: () => import('./+topics/topics.module').then(m => m.TopicsModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'search',
    data: { breadcrumb: 'Search Results' },
    loadChildren: () => import('./+topics/topics.module').then(m => m.TopicsModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'admin',
    data: { breadcrumb: 'Admin' },
    loadChildren: () => import('./+admin/admin.module').then(m => m.AdminModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'manage',
    data: { breadcrumb: 'Manage' },
    loadChildren: () => import('./+sp-admin/sp-admin.module').then(m => m.SPAdminModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'events',
    data: { breadcrumb: 'Events' },
    loadChildren: () => import('./+events/events.module').then(m => m.EventsModule)
  },
  {
    path: 'solutions',
    data: { breadcrumb: 'Solutions' },
    loadChildren: () =>
      import('./projects/+project-solutions/project-solutions.module').then(m => m.ProjectSolutionsModule)
  },
  {
    path: 'technologies',
    data: { breadcrumb: 'Technologies' },
    loadChildren: () =>
      import('./projects/+project-technologies/project-technologies.module').then(m => m.ProjectTechnologiesModule),
    canActivate: [MsalGuard, ProjectViewPermissionGuard]
  },
  {
    path: 'initiatives',
    data: { breadcrumb: 'View Initiative' },
    loadChildren: () =>
      import('./public/initiative-details/initiative-details.module').then(m => m.InitiativeDetailsModule)
 
  },
  {
    path: '403',
    component: ForbiddenComponent
  },
  {
    path: '404',
    component: NotFoundComponent
  },
  {
    path: '423',
    component: InactiveComponent
  },
  {
    path: '**',
    redirectTo: '404'
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      enableTracing: false,
      scrollPositionRestoration: 'top',
      onSameUrlNavigation: 'reload'
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
