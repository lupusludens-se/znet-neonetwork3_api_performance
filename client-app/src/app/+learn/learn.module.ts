// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { LearnRoutingModule } from './learn-routing.module';
import { ContentTagModule } from '../shared/modules/content-tag/content-tag.module';
import { DotDecorModule } from '../shared/modules/dot-decor/dot-decor.module';
import { FilterHeaderModule } from '../shared/modules/filter-header/filter-header.module';
import { PostLinkModule } from '../shared/modules/post-link/post-link.module';
import { TopPanelModule } from '../shared/modules/top-panel/top-panel.module';
import { FilterModule } from '../shared/modules/filter/filter.module';
import { NoResultsModule } from '../shared/modules/no-results/no-results.module';
import { PaginationModule } from '../shared/modules/pagination/pagination.module';
import { ModalModule } from '../shared/modules/modal/modal.module';
import { EmptyPageModule } from '../shared/modules/empty-page/empty-page.module';
import { ContentLocationModule } from '../shared/modules/content-location/content-location.module';

// components
import { LearnComponent } from './learn.component';
import { PostComponent } from './components/post/post.component';
import { PostsCarouselComponent } from './components/posts-carousel/posts-carousel.component';

// services
import { SaveContentService } from '../shared/services/save-content.service';
import { ForYouComponent } from './components/for-you/for-you.component';
import { CommunityModule } from '../+community/community.module';
import { InitiativeSharedService } from '../initiatives/shared/services/initiative-shared.service';

@NgModule({
  declarations: [LearnComponent, PostComponent, PostsCarouselComponent, ForYouComponent],
  imports: [
    SharedModule,
    LearnRoutingModule,
    ContentTagModule,
    DotDecorModule,
    FilterHeaderModule,
    PostLinkModule,
    TopPanelModule,
    FilterModule,
    NoResultsModule,
    PaginationModule,
    ModalModule,
    EmptyPageModule,
    ContentLocationModule,
    CommunityModule
  ],
  providers: [SaveContentService, InitiativeSharedService]
})
export class LearnModule { }
