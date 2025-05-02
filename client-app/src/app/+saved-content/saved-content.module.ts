// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { PaginationModule } from '../shared/modules/pagination/pagination.module';
import { NoResultsModule } from '../shared/modules/no-results/no-results.module';
import { DotDecorModule } from '../shared/modules/dot-decor/dot-decor.module';
import { EmptyPageModule } from '../shared/modules/empty-page/empty-page.module';
import { ContentTagModule } from '../shared/modules/content-tag/content-tag.module';
import { ContentLocationModule } from '../shared/modules/content-location/content-location.module';

// components
import { SavedContentComponent } from './saved-content.component';
import { SavedContentListModule } from '../shared/modules/saved-content/saved-content-list.module';

// pipes

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: SavedContentComponent
  }
];

@NgModule({
  declarations: [SavedContentComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    SharedModule,
    PaginationModule,
    NoResultsModule,
    DotDecorModule,
    ContentTagModule,
    EmptyPageModule,
    ContentLocationModule,
    SavedContentListModule
  ],
  exports: [SavedContentComponent]
})
export class SavedContentModule {}
