// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { NoResultsModule } from 'src/app/shared/modules/no-results/no-results.module';
import { CompanyManagementRoutingModule } from './company-management-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { FormFooterModule } from 'src/app/shared/modules/form-footer/form-footer.module';
import { DropdownModule } from 'src/app/shared/modules/controls/dropdown/dropdown.module';
import { RadioControlModule } from 'src/app/shared/modules/radio-control/radio-control.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';
import { BlueCheckboxModule } from 'src/app/shared/modules/blue-checkbox/blue-checkbox.module';
import { StatusChangeModule } from 'src/app/shared/modules/status-change/status-change.module';
import { MessageControlModule } from '../../../shared/modules/message-control/message-control.module';
import { MenuModule } from '../../../shared/modules/menu/menu.module';

// components
import { CompanyTableRowComponent } from './components/company-table-row/company-table-row.component';
import { CompanyManipulationComponent } from './components/company-manipulation/company-manipulation.component';
import { CompanyManagementComponent } from './company-management.component';

// providers
import { CompanyDataService } from './services/company.data.service';
import { CountriesService } from 'src/app/shared/services/countries.service';
import { CompanyManagementPermissionGuard } from 'src/app/shared/guards/permission-guards/company-management-permission.guard';
import { ControlErrorModule } from '../../../shared/modules/controls/control-error/control-error.module';
import { TextInputModule } from '../../../shared/modules/controls/text-input/text-input.module';
import { CompanyLogoModule } from 'src/app/shared/modules/company-logo/company-logo.module';
import { CompanyProfileComponent } from './components/company-profile/company-profile.component';
import { ImageUploadService } from 'src/app/shared/services/image-upload.service';

@NgModule({
  declarations: [CompanyManagementComponent, CompanyTableRowComponent, CompanyManipulationComponent, CompanyProfileComponent],
  imports: [
    CompanyManagementRoutingModule,
    ReactiveFormsModule,
    FormFooterModule,
    ModalModule,
    CommonModule,
    SharedModule,
    NoResultsModule,
    PaginationModule,
    DropdownModule,
    BlueCheckboxModule,
    RadioControlModule,
    StatusChangeModule,
    MessageControlModule,
    MenuModule,
    ControlErrorModule,
    TextInputModule,
    CompanyLogoModule
  ],
  providers: [CompanyDataService, CountriesService, CompanyManagementPermissionGuard, ImageUploadService]
})
export class CompanyManagementModule {}
