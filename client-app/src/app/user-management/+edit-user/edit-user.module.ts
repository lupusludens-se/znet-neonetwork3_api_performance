// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';
import { UserAvatarModule } from '../../shared/modules/user-avatar/user-avatar.module';
import { BlueCheckboxModule } from '../../shared/modules/blue-checkbox/blue-checkbox.module';
import { ReactiveFormsModule } from '@angular/forms';
import { FormFooterModule } from '../../shared/modules/form-footer/form-footer.module';
import { StatusChangeModule } from '../../shared/modules/status-change/status-change.module';

// components
import { EditUserComponent } from './edit-user.component';
import { RadioControlModule } from '../../shared/modules/radio-control/radio-control.module';
import { DropdownModule } from '../../shared/modules/controls/dropdown/dropdown.module';
import { CountriesService } from '../../shared/services/countries.service';
import { HeardViaService } from '../services/heard-via.service';
import { TextInputModule } from '../../shared/modules/controls/text-input/text-input.module';
import { ControlErrorModule } from '../../shared/modules/controls/control-error/control-error.module';
import { ImageUploadService } from '../../shared/services/image-upload.service';
import { MessageControlModule } from 'src/app/shared/modules/message-control/message-control.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: EditUserComponent
  }
];

@NgModule({
  declarations: [EditUserComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    UserAvatarModule,
    BlueCheckboxModule,
    RadioControlModule,
    DropdownModule,
    FormFooterModule,
    StatusChangeModule,
    TextInputModule,
    ControlErrorModule,
    MessageControlModule,
    ModalModule
  ],
  providers: [CountriesService, HeardViaService, ImageUploadService]
})
export class EditUserModule {}
