// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// pipes
import { SavedContentLoaderComponent } from './components/saved-content-loader/saved-content-loader.component';
import { SavedContentListComponent } from './components/saved-content-list/saved-content-list.component';
import { ContentTypeToIconPipe } from './pipes/content-type-to-icon.pipe';
import { SharedModule } from '../../shared.module';
import { PaginationModule } from '../pagination/pagination.module';
import { NoResultsModule } from '../no-results/no-results.module';
import { DotDecorModule } from '../dot-decor/dot-decor.module';
import { ContentTagModule } from '../content-tag/content-tag.module';
import { EmptyPageModule } from '../empty-page/empty-page.module';
import { ContentLocationModule } from '../content-location/content-location.module';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [SavedContentListComponent, ContentTypeToIconPipe, SavedContentLoaderComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule,
    PaginationModule,
    NoResultsModule,
    DotDecorModule,
    ContentTagModule,
    EmptyPageModule,
    ContentLocationModule
  ],
  exports: [SavedContentListComponent, SavedContentLoaderComponent]
})
export class SavedContentListModule {}
