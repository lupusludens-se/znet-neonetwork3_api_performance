import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { FormFooterModule } from '../shared/modules/form-footer/form-footer.module';
import { ReactiveFormsModule } from '@angular/forms';
import { DropdownModule } from '../shared/modules/controls/dropdown/dropdown.module';
import { UserAvatarModule } from '../shared/modules/user-avatar/user-avatar.module';
import { SharedModule } from '../shared/shared.module';
import { MessageControlModule } from '../shared/modules/message-control/message-control.module';
import { TranslateModule } from '@ngx-translate/core';
import { SaveCancelControlsModule } from '../shared/modules/save-cancel-controls/save-cancel-controls.module';

// components
import { EditProfileComponent } from './edit-profile.component';
import { ImageUploadService } from '../shared/services/image-upload.service';
import { BlueCheckboxModule } from '../shared/modules/blue-checkbox/blue-checkbox.module';
import { TextInputModule } from '../shared/modules/controls/text-input/text-input.module';
import { ControlErrorModule } from '../shared/modules/controls/control-error/control-error.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: EditProfileComponent
  }
];

@NgModule({
  declarations: [EditProfileComponent],
  imports: [
    CommonModule,
    TranslateModule,
    RouterModule.forChild(routes),
    SvgIconsModule,
    SaveCancelControlsModule,
    ReactiveFormsModule,
    DropdownModule,
    UserAvatarModule,
    SharedModule,
    FormFooterModule,
    MessageControlModule,
    BlueCheckboxModule,
    TextInputModule,
    ControlErrorModule
  ],
  providers: [ImageUploadService]
})
export class EditProfileModule {}
