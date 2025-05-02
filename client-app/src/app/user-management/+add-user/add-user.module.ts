// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { RadioControlModule } from '../../shared/modules/radio-control/radio-control.module';
import { BlueCheckboxModule } from '../../shared/modules/blue-checkbox/blue-checkbox.module';
import { UserDataService } from '../services/user.data.service';
import { FormFooterModule } from '../../shared/modules/form-footer/form-footer.module';

// components
import { AddUserComponent } from './add-user.component';
import { CountriesService } from '../../shared/services/countries.service';
import { DropdownModule } from '../../shared/modules/controls/dropdown/dropdown.module';
import { HeardViaService } from '../services/heard-via.service';
import { TextInputModule } from '../../shared/modules/controls/text-input/text-input.module';
import { ControlErrorModule } from '../../shared/modules/controls/control-error/control-error.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: AddUserComponent
  }
];

@NgModule({
  declarations: [AddUserComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    RadioControlModule,
    BlueCheckboxModule,
    FormFooterModule,
    DropdownModule,
    TextInputModule,
    ControlErrorModule,
    ModalModule
  ],
  exports: [AddUserComponent],
  providers: [UserDataService, CountriesService, HeardViaService]
})
export class AddUserModule {}
