import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { UsersSearchModule } from '../../../shared/modules/users-search/users-search.module';
import { SharedModule } from '../../../shared/shared.module';
import { ModeratorsComponent } from './moderators.component';
import { ControlErrorModule } from '../../../shared/modules/controls/control-error/control-error.module';

@NgModule({
  declarations: [ModeratorsComponent],
  exports: [ModeratorsComponent, UsersSearchModule],
  imports: [SharedModule, FormsModule, ReactiveFormsModule, UsersSearchModule, ControlErrorModule]
})
export class ModeratorsModule {}
