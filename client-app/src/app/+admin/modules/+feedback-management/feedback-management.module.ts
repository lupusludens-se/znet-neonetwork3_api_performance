import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeedbackManagementComponent } from './feedback-management.component';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { FeedbackTableRowComponent } from './components/feedback-table-row/feedback-table-row.component';
import { UserAvatarModule } from 'src/app/shared/modules/user-avatar/user-avatar.module';
import { DatePipe } from '@angular/common';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { EmptyPageModule } from 'src/app/shared/modules/empty-page/empty-page.module';
import { UserFeedbackDetailsComponent } from './components/user-feedback-details.component';
import { TextInputModule } from 'src/app/shared/modules/controls/text-input/text-input.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MessageControlModule } from 'src/app/shared/modules/message-control/message-control.module';
import { AdminAllPermissionGuard } from 'src/app/shared/guards/permission-guards/admin-all-permission.guard';
import { UserDataService } from 'src/app/user-management/services/user.data.service';
const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: FeedbackManagementComponent,
    canActivate: [AdminAllPermissionGuard]
  },
  {
    path: ':id',
    data: { breadcrumb: 'User Feedback Details' },
    component: UserFeedbackDetailsComponent,
    canActivate: [AdminAllPermissionGuard]
  }
];

@NgModule({
  declarations: [FeedbackManagementComponent, FeedbackTableRowComponent, UserFeedbackDetailsComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    SharedModule,
    UserAvatarModule,
    PaginationModule,
    EmptyPageModule,
    TextInputModule,
    MessageControlModule,
    ReactiveFormsModule
  ],
  providers: [DatePipe, UserDataService]
})
export class FeedbackManagementModule {}
