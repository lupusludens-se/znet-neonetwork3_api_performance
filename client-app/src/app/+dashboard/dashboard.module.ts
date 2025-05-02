// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { TopPanelModule } from '../shared/modules/top-panel/top-panel.module';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { PostLinkModule } from '../shared/modules/post-link/post-link.module';
import { ForumTopicModule } from '../shared/modules/forum-topic/forum-topic.module';
import { ContentTagModule } from '../shared/modules/content-tag/content-tag.module';
import { ModalModule } from '../shared/modules/modal/modal.module';
import { DotDecorModule } from '../shared/modules/dot-decor/dot-decor.module';
import { ToolModule } from '../shared/modules/tool/tool.module';

// components
import { DashboardComponent } from './dashboard.component';
import { AnnouncementComponent } from './components/layout/announcement/announcement.component';
import { UpcomingEventsComponent } from './components/layout/upcoming-events/upcoming-events.component';
import { CorporationDashboardComponent } from './components/corporation-dashboard/corporation-dashboard.component';
import { SolutionProviderDashboardComponent } from './components/solution-provider-dashboard/solution-provider-dashboard.component';
import { SuggestionComponent } from './components/layout/suggestion/suggestion.component';
import { ContentComponent } from './components/layout/content/content.component';
import { TitleSectionComponent } from './components/layout/title-section/title-section.component';
import { CompanyComponent } from './components/layout/company/company.component';
import { InternalDashboardComponent } from './components/internal-dashboard/internal-dashboard.component';
import { PinnedToolsRequestsService } from './services/pinned-tools-requests.service';
import { SaveContentService } from '../shared/services/save-content.service';
import { ProjectComponent } from './components/layout/project/project.component';
import { NewTrendingProjectTileComponent } from './components/corporation-dashboard/corporate-new-trending-project-tile/corporate-new-trending-project-tile.component';
import { ProjectCarouselComponent } from './components/project-carousel/project-carousel.component';
import { NewAndNoteworthyComponent } from './components/new-and-noteworthy/new-and-noteworthy.component';
import { DisablePipe } from './pipes/disabled.pipe';
import { PublicCorporationDashboardComponent } from './components/public-corporation-dashboard/public-corporation-dashboard.component';
import { PublicProjectCarouselComponent } from './components/public-project-carousel/public-project-carousel.component';
import { PublicModule } from '../public/public.module';
import { NetworkStatsModule } from '../shared/modules/network-stats/network-stats.module';
import { CommunityModule } from '../+community/community.module';
import { PublicDiscoverKeyContentComponent } from './components/public-discover-key-content/public-discover-key-content.component';
import { TextareaControlModule } from '../shared/modules/controls/textarea-control/textarea-control.module';
import { ReactiveFormsModule } from '@angular/forms';
import { SpProjectTileComponent } from './components/sp-project-tile/sp-project-tile.component';
import { SpProjectCarousalComponent } from './components/sp-project-carousal/sp-project-carousal.component';
import { InitiativesComponent } from './components/corporation-dashboard/initiatives/initiatives.component';
import { ProgressStepperModule } from '../shared/modules/progress-stepper/progress-stepper.module';
import { ContentLocationModule } from '../shared/modules/content-location/content-location.module';
import { DashboardInitiativeService } from './components/corporation-dashboard/initiatives/services/initiative-information.service';
import { DatePipe } from '@angular/common';
import { InitiativeSharedService } from '../initiatives/shared/services/initiative-shared.service';
import { PublicDecarbonizationInitiativesComponent } from './components/public-decarbonization-initiatives/public-decarbonization-initiatives.component';
import { InitiativeInformationModule } from '../shared/modules/initiatives/initiative-information.module';
import { DecarbonizationInitiativeService } from '../initiatives/+decarbonization-initiatives/services/decarbonization-initiatives.service';

@NgModule({
  declarations: [
    DashboardComponent,
    AnnouncementComponent,
    UpcomingEventsComponent,
    CorporationDashboardComponent,
    SolutionProviderDashboardComponent,
    SuggestionComponent,
    ContentComponent,
    TitleSectionComponent,
    CompanyComponent,
    InternalDashboardComponent,
    ProjectComponent,
    NewTrendingProjectTileComponent,
    ProjectCarouselComponent,
    NewAndNoteworthyComponent,
    DisablePipe,
    PublicCorporationDashboardComponent,
    PublicProjectCarouselComponent,
    PublicDiscoverKeyContentComponent,
    SpProjectTileComponent,
    SpProjectCarousalComponent,
    InitiativesComponent,
    PublicDecarbonizationInitiativesComponent
  ],
  exports: [SuggestionComponent],
  providers: [
    PinnedToolsRequestsService,
    SaveContentService,
    DashboardInitiativeService,
    DatePipe,
    InitiativeSharedService
  ],
  imports: [
    SharedModule,
    DashboardRoutingModule,
    TopPanelModule,
    PostLinkModule,
    ForumTopicModule,
    ContentTagModule,
    ContentLocationModule,
    ToolModule,
    ModalModule,
    DotDecorModule,
    PublicModule,
    CommunityModule,
    NetworkStatsModule,
    TextareaControlModule,
    ReactiveFormsModule,
    ProgressStepperModule,
    InitiativeInformationModule
  ]
})
export class DashboardModule {}
