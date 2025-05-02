import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommunityItemComponent } from './community-item.component';
import { UserAvatarModule } from 'src/app/shared/modules/user-avatar/user-avatar.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { TranslateModule } from '@ngx-translate/core';
import { ContentTagModule } from 'src/app/shared/modules/content-tag/content-tag.module';
import { RouterModule } from '@angular/router';
import { BlueCheckboxModule } from 'src/app/shared/modules/blue-checkbox/blue-checkbox.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MenuModule } from 'src/app/shared/modules/menu/menu.module';

@NgModule({
  declarations: [CommunityItemComponent],
  imports: [
    CommonModule,
    SharedModule,
    TranslateModule,
    ContentTagModule,
    UserAvatarModule,
    RouterModule,
    MenuModule,
    BlueCheckboxModule,
    ReactiveFormsModule
  ],
  exports: [CommunityItemComponent]
})
export class CommunityItemModule {}
