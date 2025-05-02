// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from 'src/app/shared/shared.module';
import { FilterHeaderModule } from 'src/app/shared/modules/filter-header/filter-header.module';
import { TableModule } from 'src/app/shared/modules/table/table.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';
import { FilterModule } from 'src/app/shared/modules/filter/filter.module';
import { UserAvatarModule } from 'src/app/shared/modules/user-avatar/user-avatar.module';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { NoResultsModule } from 'src/app/shared/modules/no-results/no-results.module';
import { MenuModule } from 'src/app/shared/modules/menu/menu.module';

// components
import { SPUsersListComponent } from './sp-users-list.component';

// utils
import { UserDataService } from 'src/app/user-management/services/user.data.service';
import { TableRowComponent } from './table-row/table-row.component';
import { MsalGuard } from '@azure/msal-angular';
import { TranslateModule } from '@ngx-translate/core';
import { SPAdminPermissionGuard } from 'src/app/shared/guards/permission-guards/sp-admin-permission.guard';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: SPUsersListComponent,
    canActivate: [MsalGuard, SPAdminPermissionGuard]
  }
];

@NgModule({
  declarations: [SPUsersListComponent, TableRowComponent],
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
    MenuModule,
    TranslateModule
  ],
  providers: [UserDataService]
})
export class SPUsersListModule {}
