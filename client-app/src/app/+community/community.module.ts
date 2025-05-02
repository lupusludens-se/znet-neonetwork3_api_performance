// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { CommunityRoutingModule } from './community-routing.module';
import { FilterHeaderModule } from '../shared/modules/filter-header/filter-header.module';
import { FilterModule } from '../shared/modules/filter/filter.module';
import { UserAvatarModule } from '../shared/modules/user-avatar/user-avatar.module';
import { PaginationModule } from '../shared/modules/pagination/pagination.module';
import { NoResultsModule } from '../shared/modules/no-results/no-results.module';
import { ContentTagModule } from '../shared/modules/content-tag/content-tag.module';
import { ReactiveFormsModule } from '@angular/forms';

// components
import { CommunityComponent } from './community.component';
import { CommunityItemComponent } from './components/community-item/community-item.component';
import { CommunityCompanyComponent } from './components/community-company/community-company.component';
import { CommunityUserComponent } from './components/community-user/community-user.component';

// providers
import { CommunityDataService } from './services/community.data.service';
import { EmptyPageModule } from '../shared/modules/empty-page/empty-page.module';
import { CompanyLogoModule } from '../shared/modules/company-logo/company-logo.module';
import { TopPanelModule } from '../shared/modules/top-panel/top-panel.module';
import { NetworkStatsModule } from '../shared/modules/network-stats/network-stats.module';
import { CommunityPublicComponent } from './components/community-public/community-public.component';
import { CommunityTestimonialComponent } from './components/community-testimonial/community-testimonial.component';
import { ClientListComponent } from './components/client-list/client-list.component';
import { PublicModule } from '../public/public.module';
import { SortDropdownModule } from '../shared/modules/sort-dropdown/sort-dropdown.module';

@NgModule({
  declarations: [
    CommunityComponent,
    CommunityItemComponent,
    CommunityCompanyComponent,
    CommunityUserComponent,
    CommunityPublicComponent,
    CommunityTestimonialComponent,
    ClientListComponent
  ],
  imports: [
    SharedModule,
    CommunityRoutingModule,
    FilterHeaderModule,
    FilterModule,
    PaginationModule,
    UserAvatarModule,
    NoResultsModule,
    ContentTagModule,
    ReactiveFormsModule,
    EmptyPageModule,
    TopPanelModule,
    CompanyLogoModule,
    NetworkStatsModule,
    PublicModule,
    SortDropdownModule
  ],
  providers: [CommunityDataService]
})
export class CommunityModule {}
