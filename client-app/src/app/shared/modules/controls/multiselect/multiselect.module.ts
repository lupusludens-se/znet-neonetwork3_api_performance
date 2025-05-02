// modules
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../../shared.module';
import { UserAvatarModule } from '../../user-avatar/user-avatar.module';

// components
import { MultiselectComponent } from './multiselect.component';

@NgModule({
  declarations: [MultiselectComponent],
  exports: [MultiselectComponent],
  imports: [CommonModule, SharedModule, FormsModule, UserAvatarModule]
})
export class MultiselectModule {}
