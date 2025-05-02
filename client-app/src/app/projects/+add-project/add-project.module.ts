// modules
import { CalendarModule } from '@syncfusion/ej2-angular-calendars';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { GeographicInterestModule } from '../../shared/modules/geographic-interest/geographic-interest.module';
import { BlueCheckboxModule } from '../../shared/modules/blue-checkbox/blue-checkbox.module';
import { RadioControlModule } from '../../shared/modules/radio-control/radio-control.module';
import { DropdownModule } from '../../shared/modules/controls/dropdown/dropdown.module';
import { FormFooterModule } from '../../shared/modules/form-footer/form-footer.module';

// components
import { ProjectTechnologiesComponent } from './components/project-technologies/project-technologies.component';
import { OffsitePpaPrivateComponent } from './components/offsite-ppa-private/offsite-ppa-private.component';
import { ProjectOverviewComponent } from './components/project-overview/project-overview.component';
import { BatteryStorageComponent } from './components/battery-storage/battery-storage.component';
import { ProjectRegionsComponent } from './components/project-regions/project-regions.component';
import { CommunitySolarComponent } from './components/community-solar/community-solar.component';
import { ProjectTypeComponent } from './components/project-type/project-type.component';
import { OffsitePpaComponent } from './components/offsite-ppa/offsite-ppa.component';
import { FuelCellsComponent } from './components/fuel-cells/fuel-cells.component';
import { AddProjectComponent } from './add-project.component';
import { CarbonOffsetComponent } from './components/carbon-offset/carbon-offset.component';
import { OnsiteSolarComponent } from './components/onsite-solar/onsite-solar.component';
import { EacPurchasingComponent } from './components/eac-purchasing/eac-purchasing.component';
import { EfficiencyMeasuresComponent } from './components/efficiency-measures/efficiency-measures.component';
import { EfficiencyAuditComponent } from './components/efficiency-audit/efficiency-audit.component';
import { EmergingTechnologiesComponent } from './components/emerging-technologies/emerging-technologies.component';
import { EvChargingComponent } from './components/ev-charging/ev-charging.component';
import { RenewableElectricityComponent } from './components/renewable-electricity/renewable-electricity.component';
import { GreenTariffComponent } from './components/green-tariff/green-tariff.component';
// services
import { RegionsService } from '../../shared/services/regions.service';
import { AddProjectService } from './services/add-project.service';
import { NumberInputModule } from '../../shared/modules/controls/number-input/number-input.module';
import { DraftModalComponent } from './components/draft-modal/draft-modal.component';
import { ModalModule } from '../../shared/modules/modal/modal.module';
import { ModeratorsModule } from '../../+admin/modules/moderators/moderators.module';
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
    component: AddProjectComponent
  }
];

@NgModule({
  declarations: [
    AddProjectComponent,
    ProjectTypeComponent,
    ProjectTechnologiesComponent,
    ProjectRegionsComponent,
    CommunitySolarComponent,
    ProjectOverviewComponent,
    BatteryStorageComponent,
    FuelCellsComponent,
    OffsitePpaComponent,
    OffsitePpaPrivateComponent,
    CarbonOffsetComponent,
    OnsiteSolarComponent,
    EacPurchasingComponent,
    EfficiencyMeasuresComponent,
    EfficiencyAuditComponent,
    EmergingTechnologiesComponent,
    EvChargingComponent,
    RenewableElectricityComponent,
    GreenTariffComponent,
    DraftModalComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    FormFooterModule,
    GeographicInterestModule,
    BlueCheckboxModule,
    DropdownModule,
    RadioControlModule,
    CalendarModule,
    NumberInputModule,
    ModalModule,
    ModeratorsModule,
    UsersSearchModule,
    MessageControlModule,
    TextInputModule,
    TextareaControlModule,
    ControlErrorModule,
    DateInputModule
  ],
  providers: [AddProjectService, RegionsService]
})
export class AddProjectModule {}
