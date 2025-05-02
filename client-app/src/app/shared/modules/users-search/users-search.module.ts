import { NgModule } from '@angular/core';
import { UsersSearchComponent } from './users-search.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule } from '@angular/forms';
import { UserAvatarModule } from '../user-avatar/user-avatar.module';

@NgModule({
  declarations: [UsersSearchComponent],
  exports: [UsersSearchComponent],
  imports: [SharedModule, FormsModule, UserAvatarModule]
})
export class UsersSearchModule {}
