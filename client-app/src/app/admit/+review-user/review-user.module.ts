// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { SvgIconsModule } from '@ngneat/svg-icon';

// components
import { ReviewUserComponent } from './review-user.component';
import { RadioControlModule } from '../../shared/modules/radio-control/radio-control.module';
import { BlueCheckboxModule } from '../../shared/modules/blue-checkbox/blue-checkbox.module';
import { DropdownModule } from '../../shared/modules/controls/dropdown/dropdown.module';
import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { CountriesService } from '../../shared/services/countries.service';
import { HeardViaService } from '../../user-management/services/heard-via.service';
import { FormFooterModule } from '../../shared/modules/form-footer/form-footer.module';
import { TextInputModule } from '../../shared/modules/controls/text-input/text-input.module';
import { ControlErrorModule } from '../../shared/modules/controls/control-error/control-error.module';
import { MessageControlModule } from 'src/app/shared/modules/message-control/message-control.module';
import { TextareaControlModule } from 'src/app/shared/modules/controls/textarea-control/textarea-control.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';
import { UserDataService } from 'src/app/user-management/services/user.data.service';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: ReviewUserComponent
  }
];

@NgModule({
  declarations: [ReviewUserComponent],
  imports: [
    CommonModule,
    TranslateModule,
    RouterModule.forChild(routes),
    SvgIconsModule,
    RadioControlModule,
    BlueCheckboxModule,
    DropdownModule,
    SharedModule,
    ReactiveFormsModule,
    FormFooterModule,
    TextInputModule,
    ControlErrorModule,
    MessageControlModule,
    TextareaControlModule,
    ModalModule
  ],
  providers: [CountriesService, HeardViaService, UserDataService]
})
export class ReviewUserModule {}
