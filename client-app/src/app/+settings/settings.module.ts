// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { SaveCancelControlsModule } from '../shared/modules/save-cancel-controls/save-cancel-controls.module';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { DropdownModule } from '../shared/modules/controls/dropdown/dropdown.module';
import { FormFooterModule } from '../shared/modules/form-footer/form-footer.module';
import { ModalModule } from '../shared/modules/modal/modal.module';

// components
import { SettingsComponent } from './settings.component';
import { NotificationsComponent } from './components/notifications/notifications.component';
import { GeneralComponent } from './components/general/general.component';
import { ResetCompleteComponent } from './components/reset-complete/reset-complete.component';
import { FooterModule } from '../shared/modules/footer/footer.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: SettingsComponent
  },
  {
    path: 'resetcomplete',
    pathMatch: 'full',
    component: ResetCompleteComponent
  },
  {
    path: ':tab',
    pathMatch: 'full',
    component: SettingsComponent
  },
];

@NgModule({
  declarations: [SettingsComponent, GeneralComponent, NotificationsComponent, ResetCompleteComponent],
  imports: [
    RouterModule.forChild(routes),
    SharedModule,
    SaveCancelControlsModule,
    ReactiveFormsModule,
    DropdownModule,
    FormFooterModule,
    ModalModule,
    FooterModule
  ]
})
export class SettingsModule { }
