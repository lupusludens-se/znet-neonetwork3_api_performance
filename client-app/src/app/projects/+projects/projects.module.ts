// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { ProjectsRoutingModule } from './projects-routing.module';
import { DotDecorModule } from '../../shared/modules/dot-decor/dot-decor.module';
import { ContentTagModule } from '../../shared/modules/content-tag/content-tag.module';
import { SortDropdownModule } from '../../shared/modules/sort-dropdown/sort-dropdown.module';
import { ContentLocationModule } from '../../shared/modules/content-location/content-location.module';
import { FilterHeaderModule } from '../../shared/modules/filter-header/filter-header.module';
import { FilterModule } from '../../shared/modules/filter/filter.module';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { NoResultsModule } from 'src/app/shared/modules/no-results/no-results.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';
import { EmptyPageModule } from 'src/app/shared/modules/empty-page/empty-page.module';

// components
import { CatalogTopPanelComponent } from './components/catalog-top-panel/catalog-top-panel.component';
import { ProjectsMapComponent } from './components/projects-map/projects-map.component';
import { ProjectItemComponent } from './components/project-item/project-item.component';
import { ProjectsComponent } from './projects.component';

// services
import { ProjectCatalogService } from './services/project-catalog.service';
import { SaveContentService } from '../../shared/services/save-content.service';
import { ProjectService } from './services/project.service';

@NgModule({
  declarations: [ProjectsComponent, CatalogTopPanelComponent, ProjectItemComponent, ProjectsMapComponent],
  imports: [
    SharedModule,
    ProjectsRoutingModule,
    DotDecorModule,
    ContentTagModule,
    ContentLocationModule,
    SortDropdownModule,
    FilterModule,
    PaginationModule,
    NoResultsModule,
    FilterHeaderModule,
    ModalModule,
    EmptyPageModule
  ],
  providers: [ProjectCatalogService, SaveContentService, ProjectService]
})
export class ProjectsModule {}
