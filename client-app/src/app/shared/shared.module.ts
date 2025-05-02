// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

//components
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { ProjectTagComponent } from './components/project-tag/project-tag.component';
import { GlobalSearchBarComponent } from './components/global-search-bar/global-search-bar.component';

// pipes
import { NeoTypeToIconPipe } from './pipes/neo-type-to-icon.pipe';
import { FindByPipe } from './pipes/find-by.pipe';
import { SafeHtmlPipe } from './pipes/safe-html.pipe';
import { TimeAgoPipe } from './pipes/time-ago.pipe';
import { TaxonomyIdFromEnumPipe } from './pipes/taxonomy-id-from-enum.pipe';
import { ResourceTypeToIconPipe } from './pipes/resource-type-to-icon.pipe';
import { RouterModule } from '@angular/router';
import { ClickOutsideDirective } from './directives/click-outside.directive';
import { TopicTypeToStringPipe } from './pipes/topic-type-to-string.pipe';
import { DashboardContentSkeletonComponent } from './components/dashboard-skeleton/dashboard-content-skeleton/dashboard-content-skeleton.component';
import { DashboardUpcomingSkeletonComponent } from './components/dashboard-skeleton/dashboard-upcoming-skeleton/dashboard-upcoming-skeleton.component';
import { CtrlClickDirective } from './directives/ctrl-click.directive';
import { ScrollLoaderComponent } from './modules/scroll-loader/scroll-loader/scroll-loader.component';
import { DashboardPublicSkeletonComponent } from './components/dashboard-skeleton/dashboard-public-skeleton/dashboard-public-skeleton.component';
import { ExportModalComponent } from './modules/export-modal/export-modal.component';
import { DashboardPrivateSkeletonComponent } from './components/dashboard-skeleton/dashboard-private-skeleton/dashboard-private-skeleton.component';
import { InfiniteSectionScrollDirective } from './directives/infinite-section-scroll.directive';
import { FileSizePipe } from './pipes/file-size.pipe';

@NgModule({
  declarations: [
    SearchBarComponent,
    ProjectTagComponent,
    DashboardContentSkeletonComponent,
    DashboardPublicSkeletonComponent,
    DashboardUpcomingSkeletonComponent,
    NeoTypeToIconPipe,
    ResourceTypeToIconPipe,
    FindByPipe,
    SafeHtmlPipe,
    TimeAgoPipe,
    TaxonomyIdFromEnumPipe,
    ClickOutsideDirective,
    TopicTypeToStringPipe,
    CtrlClickDirective,
    ScrollLoaderComponent,
    ExportModalComponent,
    DashboardPrivateSkeletonComponent,
    InfiniteSectionScrollDirective,
    GlobalSearchBarComponent,
    FileSizePipe
  ],
  imports: [CommonModule, RouterModule, TranslateModule, SvgIconsModule, FormsModule, ReactiveFormsModule],
  exports: [
    CommonModule,
    TranslateModule,
    SvgIconsModule,
    SearchBarComponent,
    ProjectTagComponent,
    DashboardContentSkeletonComponent,
    DashboardPublicSkeletonComponent,
    DashboardUpcomingSkeletonComponent,
    NeoTypeToIconPipe,
    ResourceTypeToIconPipe,
    FindByPipe,
    SafeHtmlPipe,
    TimeAgoPipe,
    TaxonomyIdFromEnumPipe,
    ClickOutsideDirective,
    TopicTypeToStringPipe,
    CtrlClickDirective,
    ScrollLoaderComponent,
    ExportModalComponent,
    DashboardPrivateSkeletonComponent,
    InfiniteSectionScrollDirective,
    GlobalSearchBarComponent,
    FileSizePipe
  ]
})
export class SharedModule {}
