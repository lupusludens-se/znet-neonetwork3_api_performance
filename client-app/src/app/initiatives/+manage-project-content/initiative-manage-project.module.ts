import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InitiativeManageProjectRoutingModule } from './initiative-manage-project-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { ProjectItemModule } from '../shared/contents/project-item/project-item.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';
import { ContentTagModule } from 'src/app/shared/modules/content-tag/content-tag.module';
import { ContentLocationModule } from 'src/app/shared/modules/content-location/content-location.module';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { InitiativeSharedService } from '../shared/services/initiative-shared.service';
import { InitiativeProjectRecommendedComponent } from './components/initiative-project-recommended-tab/initiative-project-recommended.component';
import { InitiativeProjectSavedComponent } from './components/initiative-project-saved-tab/initiative-project-saved.component';
import { InitiativeProjectComponent } from './components/initiative-project-form/initiative-project.component';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [InitiativeProjectComponent, InitiativeProjectRecommendedComponent, InitiativeProjectSavedComponent],
  imports: [
    CommonModule,
    InitiativeManageProjectRoutingModule,
    SharedModule,
    ProjectItemModule,
    ModalModule,
    ContentTagModule,
    ContentLocationModule,
    PaginationModule,
    PaginationModule,
    SharedModule,
    SvgIconsModule,
    TranslateModule
  ],
  providers: [InitiativeSharedService]
})
export class InitiativeManageProjectModule {}
