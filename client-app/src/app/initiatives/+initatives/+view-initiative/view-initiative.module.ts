import { CommonModule, DatePipe } from '@angular/common';
import { ViewInitiativeComponent } from './view-initiative.component';
import { ProgressTrackerComponent } from './components/progress-tracker/progress-tracker.component';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { ProgressStepperModule } from 'src/app/shared/modules/progress-stepper/progress-stepper.module';
import { NgModule } from '@angular/core';
import { LearnSectionComponent } from './components/learn-section/learn-section.component';
import { ViewInitiativeService } from './services/view-initiative.service';
import { InitiativeSubStepsComponent } from './components/initiative-sub-steps/initiative-sub-steps.component';
import { BlueCheckboxModule } from 'src/app/shared/modules/blue-checkbox/blue-checkbox.module';
import { LearnItemModule } from '../../shared/contents/learn-item/learn-item.module';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { EmptyPageModule } from 'src/app/shared/modules/empty-page/empty-page.module';import { TopPanelModule } from 'src/app/shared/modules/top-panel/top-panel.module';
import { MenuModule } from 'src/app/shared/modules/menu/menu.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';
import { FileSectionComponent } from './components/file-section/file-section.component';
import { FileService } from 'src/app/shared/services/file.service';
import { ToolsSectionComponent } from './components/tools-section/tools-section.component';
import { MessageSectionComponent } from './components/message-section/message-section.component';
import { MessageItemModule } from '../../shared/contents/message-item/message-item.module';
import { ProjectSectionComponent } from './components/project-section/project-section.component';
import { ContentTagModule } from 'src/app/shared/modules/content-tag/content-tag.module';
import { ContentLocationModule } from 'src/app/shared/modules/content-location/content-location.module';
import { ToolItemModule } from '../../shared/contents/tool-item/tool-item.module';
import { InitiativeNoContentComponent } from './components/initiative-no-content/initiative-no-content.component';
import { InitiativeSharedService } from '../../shared/services/initiative-shared.service';
import { CommunitySectionComponent } from './components/community-section/community-section.component';
import { CommunityItemModule } from '../../shared/contents/community-item/community-item.module';
import { SectionSpinnerModule } from 'src/app/shared/modules/section-spinner/section-spinner.module';
const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: ViewInitiativeComponent
  }
];

@NgModule({
  declarations: [
    ViewInitiativeComponent,
    ProgressTrackerComponent,
    LearnSectionComponent,
    InitiativeSubStepsComponent,
    FileSectionComponent,
    MessageSectionComponent,
    ToolsSectionComponent,
    ProjectSectionComponent,
    InitiativeNoContentComponent,
    CommunitySectionComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ProgressStepperModule,
    BlueCheckboxModule,
    RouterModule.forChild(routes),
    LearnItemModule,
    ToolItemModule,
    PaginationModule,
    EmptyPageModule,
    TopPanelModule,
    MenuModule,
    ModalModule,
    MessageItemModule,
    ContentTagModule,
    ContentLocationModule,
    CommunityItemModule,
    SectionSpinnerModule
  ],
  providers: [ViewInitiativeService, DatePipe, FileService, InitiativeSharedService]
})
export class ViewInitiativeModule {}
