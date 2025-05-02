// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../../shared/shared.module';
import { DropdownModule } from '../../../shared/modules/controls/dropdown/dropdown.module';
import { FormFooterModule } from '../../../shared/modules/form-footer/form-footer.module';

// components
import { EmailAlertsComponent } from './email-alerts.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    data: { breadcrumbSkip: true },
    component: EmailAlertsComponent
  }
];

@NgModule({
  declarations: [EmailAlertsComponent],
  imports: [SharedModule, RouterModule.forChild(routes), DropdownModule, ReactiveFormsModule, FormFooterModule]
})
export class EmailAlertsModule {}
