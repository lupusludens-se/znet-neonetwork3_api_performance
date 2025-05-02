// modules
import { NgModule } from '@angular/core';
import { AdminRoutingModule } from './admin-routing.module';
import { SharedModule } from '../shared/shared.module';
import { VerticalLineDecorModule } from '../shared/modules/vertical-line-decor/vertical-line-decor.module';

// components
import { AdminComponent } from './admin.component';
import { EventManagementPermissionGuard } from '../shared/guards/permission-guards/event-management-permission.guard';
import { AdminAllPermissionGuard } from '../shared/guards/permission-guards/admin-all-permission.guard';
import { AnnouncementManagementPermissionGuard } from '../shared/guards/permission-guards/announcement-event-management-permission.guard';
import { ModalModule } from '../shared/modules/modal/modal.module';

@NgModule({
  declarations: [AdminComponent],
  imports: [SharedModule, AdminRoutingModule, VerticalLineDecorModule, ModalModule],
  providers: [EventManagementPermissionGuard, AnnouncementManagementPermissionGuard, AdminAllPermissionGuard]
})
export class AdminModule {}
