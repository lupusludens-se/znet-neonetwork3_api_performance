// modules
import { NgModule } from '@angular/core';
import { DotDecorModule } from '../dot-decor/dot-decor.module';
import { SharedModule } from '../../shared.module';
import { ContentTagModule } from '../content-tag/content-tag.module';
import { ContentLocationModule } from '../content-location/content-location.module';
import { UserAvatarModule } from '../user-avatar/user-avatar.module';
import { RouterModule } from '@angular/router';
import { ModalModule } from '../modal/modal.module';

// components
import { ForumTopicComponent } from './components/forum-topic/forum-topic.component';
import { VerticalLineDecorModule } from '../vertical-line-decor/vertical-line-decor.module';

@NgModule({
  declarations: [ForumTopicComponent],
  imports: [
    DotDecorModule,
    VerticalLineDecorModule,
    SharedModule,
    ContentTagModule,
    ContentLocationModule,
    UserAvatarModule,
    RouterModule,
    ModalModule
  ],
  exports: [ForumTopicComponent]
})
export class ForumTopicModule {}
