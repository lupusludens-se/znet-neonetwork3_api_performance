import { ReactiveFormsModule } from '@angular/forms';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { UserAvatarModule } from '../user-avatar/user-avatar.module';
import { BlueCheckboxComponent } from './blue-checkbox.component';

@NgModule({
  declarations: [BlueCheckboxComponent],
  imports: [CommonModule, SvgIconsModule, ReactiveFormsModule, UserAvatarModule],
  exports: [BlueCheckboxComponent]
})
export class BlueCheckboxModule {}
