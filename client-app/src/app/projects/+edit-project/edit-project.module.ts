// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { StatusChangeModule } from '../../shared/modules/status-change/status-change.module';
import { GeographicInterestModule } from '../../shared/modules/geographic-interest/geographic-interest.module';

// components
import { EditProjectComponent } from './edit-project.component';
import { EditProjectTypeComponent } from './components/edit-project-type/edit-project-type.component';
import { EditProjectTechnologiesComponent } from './components/edit-project-technologies/edit-project-technologies.component';

// services
import { RegionsService } from '../../shared/services/regions.service';
import { EditProjectBatteryStorageComponent } from './components/edit-project-battery-storage/edit-project-battery-storage.component';
import { BlueCheckboxModule } from '../../shared/modules/blue-checkbox/blue-checkbox.module';
import { NumberInputModule } from '../../shared/modules/controls/number-input/number-input.module';
import { EditProjectCarbonOffsetComponent } from './components/edit-project-carbon-offset/edit-project-carbon-offset.component';
import { RadioControlModule } from '../../shared/modules/radio-control/radio-control.module';
import { EditProjectCommunitySolarComponent } from './components/edit-project-community-solar/edit-project-community-solar.component';
import { EditProjectEacPurchasingComponent } from './components/edit-project-eac-purchasing/edit-project-eac-purchasing.component';
import { EditProjectEfficiencyAuditComponent } from './components/edit-project-efficiency-audit/edit-project-efficiency-audit.component';
import { EditProjectEfficiencyMeasuresComponent } from './components/edit-project-efficiency-measures/edit-project-efficiency-measures.component';
import { EditProjectEmergingTechnologiesComponent } from './components/edit-project-emerging-technologies/edit-project-emerging-technologies.component';
import { EditProjectEvChargingComponent } from './components/edit-project-ev-charging/edit-project-ev-charging.component';
import { EditProjectFuelCellsComponent } from './components/edit-project-fuel-cells/edit-project-fuel-cells.component';
import { EditProjectOnsilteSolarComponent } from './components/edit-project-onsilte-solar/edit-project-onsilte-solar.component';
import { EditProjectRenewableElectricityComponent } from './components/edit-project-renewable-electricity/edit-project-renewable-electricity.component';
import { EditProjectGreenTariffComponent } from './components/edit-project-green-tariff/edit-project-green-tariff.component';
import { FormFooterModule } from '../../shared/modules/form-footer/form-footer.module';
import { EditProjectPpaComponent } from './components/edit-project-ppa/edit-project-ppa.component';
import { DropdownModule } from '../../shared/modules/controls/dropdown/dropdown.module';
import { EditProjectPrivatePpaComponent } from './components/edit-project-private-ppa/edit-project-private-ppa.component';
import { UsersSearchModule } from '../../shared/modules/users-search/users-search.module';
import { MessageControlModule } from '../../shared/modules/message-control/message-control.module';
import { TextInputModule } from '../../shared/modules/controls/text-input/text-input.module';
import { TextareaControlModule } from '../../shared/modules/controls/textarea-control/textarea-control.module';
import { ControlErrorModule } from '../../shared/modules/controls/control-error/control-error.module';
import { DateInputModule } from '../../shared/modules/controls/date-input/date-input.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: EditProjectComponent
  }
];

@NgModule({
  declarations: [
    EditProjectComponent,
    EditProjectTypeComponent,
    EditProjectTechnologiesComponent,
    EditProjectBatteryStorageComponent,
    EditProjectCarbonOffsetComponent,
    EditProjectCommunitySolarComponent,
    EditProjectEacPurchasingComponent,
    EditProjectEfficiencyAuditComponent,
    EditProjectEfficiencyMeasuresComponent,
    EditProjectEmergingTechnologiesComponent,
    EditProjectEvChargingComponent,
    EditProjectFuelCellsComponent,
    EditProjectOnsilteSolarComponent,
    EditProjectRenewableElectricityComponent,
    EditProjectGreenTariffComponent,
    EditProjectPpaComponent,
    EditProjectPrivatePpaComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    StatusChangeModule,
    ReactiveFormsModule,
    GeographicInterestModule,
    BlueCheckboxModule,
    NumberInputModule,
    RadioControlModule,
    FormFooterModule,
    DropdownModule,
    UsersSearchModule,
    MessageControlModule,
    TextInputModule,
    TextareaControlModule,
    ControlErrorModule,
    DateInputModule
  ],
  providers: [RegionsService]
})
export class EditProjectModule {}
