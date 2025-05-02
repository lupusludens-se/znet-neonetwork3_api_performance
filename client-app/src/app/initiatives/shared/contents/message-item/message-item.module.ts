import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { TranslateModule } from '@ngx-translate/core';
import { MenuModule } from 'src/app/shared/modules/menu/menu.module';
import { MessageItemComponent } from './message-item.component';
import { UserAvatarModule } from 'src/app/shared/modules/user-avatar/user-avatar.module';

@NgModule({
  declarations: [MessageItemComponent],
  imports: [CommonModule, SharedModule, TranslateModule, MenuModule, UserAvatarModule],
  providers: [DatePipe],
  exports: [MessageItemComponent]
})
export class MessageItemModule { }
