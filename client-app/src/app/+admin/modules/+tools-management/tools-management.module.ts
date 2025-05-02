// modules
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ToolsManagementRoutingModule } from './tools-management-routing.module';
import { SharedModule } from '../../../shared/shared.module';
import { TableModule } from '../../../shared/modules/table/table.module';
import { ModalModule } from '../../../shared/modules/modal/modal.module';
import { FilterHeaderModule } from '../../../shared/modules/filter-header/filter-header.module';
import { SaveCancelControlsModule } from '../../../shared/modules/save-cancel-controls/save-cancel-controls.module';
import { BlueCheckboxModule } from '../../../shared/modules/blue-checkbox/blue-checkbox.module';
import { FormFooterModule } from '../../../shared/modules/form-footer/form-footer.module';
import { AdminModule } from '../../admin.module';
import { PaginationModule } from '../../../shared/modules/pagination/pagination.module';
import { NoResultsModule } from '../../../shared/modules/no-results/no-results.module';
import { StatusChangeModule } from '../../../shared/modules/status-change/status-change.module';

// components
import { ToolsManagementComponent } from './tools-management.component';
import { ToolsManipulationComponent } from './components/tools-manipulation/tools-manipulation.component';
import { ToolManagementPermissionGuard } from 'src/app/shared/guards/permission-guards/tool-management-permission.guard';
import { EmptyPageModule } from 'src/app/shared/modules/empty-page/empty-page.module';
import { TextInputModule } from '../../../shared/modules/controls/text-input/text-input.module';
import { NumberInputModule } from '../../../shared/modules/controls/number-input/number-input.module';
import { ImageUploadService } from 'src/app/shared/services/image-upload.service';

@NgModule({
  declarations: [ToolsManagementComponent, ToolsManipulationComponent],
  imports: [
    ToolsManagementRoutingModule,
    SharedModule,
    TableModule,
    ModalModule,
    FilterHeaderModule,
    SaveCancelControlsModule,
    ReactiveFormsModule,
    BlueCheckboxModule,
    FormFooterModule,
    AdminModule,
    PaginationModule,
    NoResultsModule,
    StatusChangeModule,
    EmptyPageModule,
    TextInputModule,
    NumberInputModule
  ],
  providers: [ToolManagementPermissionGuard, ImageUploadService]
})
export class ToolsManagementModule {}
