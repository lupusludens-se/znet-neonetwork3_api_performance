// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { SaveCancelControlsModule } from '../../../../shared/modules/save-cancel-controls/save-cancel-controls.module';
import { FormFooterModule } from '../../../../shared/modules/form-footer/form-footer.module';
import { RadioControlModule } from '../../../../shared/modules/radio-control/radio-control.module';
import { ModalModule } from '../../../../shared/modules/modal/modal.module';
import { ConfirmAnnouncementModule } from '../modules/confirm-announcement/confirm-announcement.module';

// components
import { EditAnnouncementComponent } from './edit-announcement.component';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { UserDataService } from '../../../../user-management/services/user.data.service';
import { AnnouncementsService } from '../services/announcements.service';
import { ImageUploadService } from '../../../../shared/services/image-upload.service';
import { TextInputModule } from '../../../../shared/modules/controls/text-input/text-input.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: EditAnnouncementComponent,
    canActivate: [MsalGuard]
  }
];

@NgModule({
  declarations: [EditAnnouncementComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    RadioControlModule,
    ReactiveFormsModule,
    SaveCancelControlsModule,
    FormFooterModule,
    ModalModule,
    ConfirmAnnouncementModule,
    TextInputModule
  ],
  providers: [UserDataService, AnnouncementsService, ImageUploadService]
})
export class EditAnnouncementModule {}
