// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { FilterHeaderModule } from '../../shared/modules/filter-header/filter-header.module';
import { RouterModule, Routes } from '@angular/router';
import { TableModule } from '../../shared/modules/table/table.module';
import { FilterModule } from '../../shared/modules/filter/filter.module';
import { ModalModule } from '../../shared/modules/modal/modal.module';
import { UserAvatarModule } from '../../shared/modules/user-avatar/user-avatar.module';
import { PaginationModule } from '../../shared/modules/pagination/pagination.module';
import { NoResultsModule } from '../../shared/modules/no-results/no-results.module';
import { MenuModule } from '../../shared/modules/menu/menu.module';

// components
import { UsersListComponent } from './users-list.component';
import { TableRowComponent } from './components/table-row/table-row.component';
import { ExportModalComponent } from '../../shared/modules/export-modal/export-modal.component';

// utils
import { UserDataService } from '../services/user.data.service';
import { MsalGuard } from '@azure/msal-angular';
import { UserAccessManagementPermissionGuard } from 'src/app/shared/guards/permission-guards/user-access-management-permission.guard';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: UsersListComponent,
    canActivate: [UserAccessManagementPermissionGuard]
  },
  {
    path: 'add',
    data: { breadcrumb: 'Add User' },
    loadChildren: () => import('../+add-user/add-user.module').then(m => m.AddUserModule),
    canActivate: [MsalGuard, UserAccessManagementPermissionGuard],
    pathMatch: 'full'
  },
  {
    path: 'edit/:id',
    data: { breadcrumb: 'Edit User' },
    loadChildren: () => import('../+edit-user/edit-user.module').then(m => m.EditUserModule),
    canActivate: [MsalGuard, UserAccessManagementPermissionGuard],
    pathMatch: 'full'
  }
];

@NgModule({
  declarations: [UsersListComponent, TableRowComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    FilterHeaderModule,
    TableModule,
    ModalModule,
    FilterModule,
    UserAvatarModule,
    PaginationModule,
    NoResultsModule,
    MenuModule
  ],
  providers: [UserAccessManagementPermissionGuard, UserDataService]
})
export class UsersListModule {}
