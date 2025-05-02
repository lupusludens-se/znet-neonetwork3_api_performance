// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { ModalModule } from '../../shared/modules/modal/modal.module';

// components
import { ProjectDetailsComponent } from './project-details.component';
import { ProjectDetailsService } from './services/project-details.service';
import { ContentTagModule } from '../../shared/modules/content-tag/content-tag.module';
import { ProjectSidePanelComponent } from './components/project-side-panel/project-side-panel.component';
import { ContactModalComponent } from './components/contact-modal/contact-modal.component';
import { SaveContentService } from '../../shared/services/save-content.service';
import { FormsModule } from '@angular/forms';
import { ProjectSidePanelBatteryStorageComponentComponent } from './components/project-side-panel-battery-storage-component/project-side-panel-battery-storage-component.component';
import { ProjectSidePanelCarbonOffsetComponentComponent } from './components/project-side-panel-carbon-offset-component/project-side-panel-carbon-offset-component.component';
import { ProjectSidePanelCommunitySolarComponentComponent } from './components/project-side-panel-community-solar-component/project-side-panel-community-solar-component.component';
import { ProjectSidePanelEacPurchasingComponentComponent } from './components/project-side-panel-eac-purchasing-component/project-side-panel-eac-purchasing-component.component';
import { ProjectSidePanelEfficiencyAuditComponentComponent } from './components/project-side-panel-efficiency-audit-component/project-side-panel-efficiency-audit-component.component';
import { ProjectSidePanelEmergingTechnologyComponentComponent } from './components/project-side-panel-emerging-technology-component/project-side-panel-emerging-technology-component.component';
import { ProjectSidePanelEvChargingComponentComponent } from './components/project-side-panel-ev-charging-component/project-side-panel-ev-charging-component.component';
import { ProjectSidePanelFuellCellsComponentComponent } from './components/project-side-panel-fuell-cells-component/project-side-panel-fuell-cells-component.component';
import { ProjectSidePanelOnsiteSolarComponentComponent } from './components/project-side-panel-onsite-solar-component/project-side-panel-onsite-solar-component.component';
import { ProjectSidePanelRenewableElectricityComponentComponent } from './components/project-side-panel-renewable-electricity-component/project-side-panel-renewable-electricity-component.component';
import { ProjectSidePanelGreenTariffComponentComponent } from './components/project-side-panel-green-tariff-component/project-side-panel-green-tariff-component.component';
import { ProjectSidePanelOffsitePpaComponentComponent } from './components/project-side-panel-offsite-ppa-component/project-side-panel-offsite-ppa-component.component';
import { ProjectPrivateDetailsComponent } from './components/project-private-details/project-private-details.component';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: ProjectDetailsComponent
  }
];

@NgModule({
  imports: [SharedModule, RouterModule.forChild(routes), ContentTagModule, ModalModule, FormsModule],
  declarations: [
    ProjectDetailsComponent,
    ProjectSidePanelComponent,
    ContactModalComponent,
    ProjectSidePanelBatteryStorageComponentComponent,
    ProjectSidePanelCarbonOffsetComponentComponent,
    ProjectSidePanelCommunitySolarComponentComponent,
    ProjectSidePanelEacPurchasingComponentComponent,
    ProjectSidePanelEfficiencyAuditComponentComponent,
    ProjectSidePanelEmergingTechnologyComponentComponent,
    ProjectSidePanelEvChargingComponentComponent,
    ProjectSidePanelFuellCellsComponentComponent,
    ProjectSidePanelOnsiteSolarComponentComponent,
    ProjectSidePanelRenewableElectricityComponentComponent,
    ProjectSidePanelGreenTariffComponentComponent,
    ProjectSidePanelOffsitePpaComponentComponent,
    ProjectPrivateDetailsComponent
  ],
  providers: [ProjectDetailsService, SaveContentService, InitiativeSharedService]
})
export class ProjectDetailsModule { }
