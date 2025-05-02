import { NgModule } from '@angular/core';
import { VerticalLineDecorModule } from '../shared/modules/vertical-line-decor/vertical-line-decor.module';
import { SharedModule } from '../shared/shared.module';
import { SPAdminRoutingModule } from './sp-admin-routing.module';
import { SpAdminComponent } from './sp-admin.component';
import { SPAdminPermissionGuard } from '../shared/guards/permission-guards/sp-admin-permission.guard';
import { SPUsersListModule } from './user-management/+users-list/sp-users-list.module';

@NgModule({
  declarations: [SpAdminComponent],
  imports: [SharedModule, SPAdminRoutingModule, VerticalLineDecorModule, SPUsersListModule],
  providers: [SPAdminPermissionGuard]
})
export class SPAdminModule {}
