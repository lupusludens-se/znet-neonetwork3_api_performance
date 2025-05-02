// modules
import { NgModule } from '@angular/core';
import { InitiativesManagementComponent } from './initiatives-management.component';
import { InitiativesManagementRoutingModule } from './initiatives-management-routing.module';
import { InitiativeTableRowComponent } from './components/initiative-table-row/initiative-table-row.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { UserAvatarModule } from 'src/app/shared/modules/user-avatar/user-avatar.module';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { EmptyPageModule } from 'src/app/shared/modules/empty-page/empty-page.module';
import { TextInputModule } from 'src/app/shared/modules/controls/text-input/text-input.module';
import { MessageControlModule } from 'src/app/shared/modules/message-control/message-control.module';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { UserDataService } from 'src/app/user-management/services/user.data.service';

@NgModule({
  declarations: [InitiativesManagementComponent, InitiativeTableRowComponent],
  imports: [
    InitiativesManagementRoutingModule,
    SharedModule,
    UserAvatarModule,
    PaginationModule,
    EmptyPageModule,
    TextInputModule,
    MessageControlModule,
    ReactiveFormsModule,
    CommonModule
  ],
  providers: [DatePipe,UserDataService]
})
export class InitiativesManagementModule {}
