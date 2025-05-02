// modules
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ForumRoutingModule } from './forum-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ContentLocationModule } from '../shared/modules/content-location/content-location.module';
import { DotDecorModule } from '../shared/modules/dot-decor/dot-decor.module';
import { FilterHeaderModule } from '../shared/modules/filter-header/filter-header.module';
import { VerticalLineDecorModule } from '../shared/modules/vertical-line-decor/vertical-line-decor.module';
import { ContentTagModule } from '../shared/modules/content-tag/content-tag.module';
import { ForumTopicModule } from '../shared/modules/forum-topic/forum-topic.module';
import { FilterModule } from '../shared/modules/filter/filter.module';
import { PaginationModule } from '../shared/modules/pagination/pagination.module';
import { UserAvatarModule } from '../shared/modules/user-avatar/user-avatar.module';
import { FormFooterModule } from '../shared/modules/form-footer/form-footer.module';
import { MessageControlModule } from '../shared/modules/message-control/message-control.module';
import { NoResultsModule } from '../shared/modules/no-results/no-results.module';
import { BlueCheckboxModule } from '../shared/modules/blue-checkbox/blue-checkbox.module';
import { TextEditorModule } from '../shared/modules/text-editor/text-editor.module';

// components
import { RespondControlComponent } from './components/shared/respond-control/respond-control.component';
import { ForumThreadComponent } from './components/forum-thread/forum-thread.component';
import { SingleReplyComponent } from './components/shared/single-reply/single-reply.component';
import { ForumComponent } from './forum.component';
import { SelectItemComponent } from './components/shared/select-item/select-item.component';
import { StartADiscussionComponent } from './components/start-a-discussion/start-a-discussion.component';
import { DiscussionSearchComponent } from './components/shared/discussion-search/discussion-search.component';
import { ImageViewModule } from '../shared/modules/image-view/image-view.module';

// services
import { SaveContentService } from '../shared/services/save-content.service';
import { DatePipe } from '@angular/common';
import { EmptyPageModule } from '../shared/modules/empty-page/empty-page.module';
import { ModalModule } from '../shared/modules/modal/modal.module';
import { EditDiscussionComponent } from './components/edit-discussion/edit-discussion.component';
import { ForumDataService } from './services/forum-data.service';
import { EditCommentControlComponent } from './components/shared/edit-comments-control/edit-comments-control.component';
import { ForumUsersComponent } from './components/forum-users/forum-users.component';

@NgModule({
  declarations: [
    ForumComponent,
    ForumThreadComponent,
    RespondControlComponent,
    SingleReplyComponent,
    EditCommentControlComponent,
    StartADiscussionComponent,
    DiscussionSearchComponent,
    SelectItemComponent,
    EditDiscussionComponent,
    ForumUsersComponent
  ],
  imports: [
    SharedModule,
    ForumRoutingModule,
    DotDecorModule,
    ContentTagModule,
    ContentLocationModule,
    FilterHeaderModule,
    VerticalLineDecorModule,
    ForumTopicModule,
    FilterModule,
    PaginationModule,
    UserAvatarModule,
    FormsModule,
    FormFooterModule,
    ReactiveFormsModule,
    MessageControlModule,
    NoResultsModule,
    BlueCheckboxModule,
    TextEditorModule,
    ImageViewModule,
    EmptyPageModule,
    ModalModule
  ],
  providers: [DatePipe, SaveContentService, ForumDataService]
})
export class ForumModule {}
