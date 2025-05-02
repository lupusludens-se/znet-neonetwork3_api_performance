// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { PaginationModule } from '../shared/modules/pagination/pagination.module';
import { NoResultsModule } from '../shared/modules/no-results/no-results.module';
import { DotDecorModule } from '../shared/modules/dot-decor/dot-decor.module';
import { EmptyPageModule } from '../shared/modules/empty-page/empty-page.module';

// components
import { TopicsComponent } from './topics.component';

// pipes
import { TopicTypeToIconPipe } from './pipes/topic-type-to-icon.pipe';
import { ContentTagModule } from '../shared/modules/content-tag/content-tag.module';
import { ContentLocationModule } from '../shared/modules/content-location/content-location.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: TopicsComponent
  }
];

@NgModule({
  declarations: [TopicsComponent, TopicTypeToIconPipe],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    SharedModule,
    PaginationModule,
    NoResultsModule,
    DotDecorModule,
    EmptyPageModule,
    ContentTagModule,
    ContentLocationModule
  ]
})
export class TopicsModule {}
