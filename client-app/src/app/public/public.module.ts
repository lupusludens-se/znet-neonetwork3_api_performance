import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScheduleDemoComponent } from './schedule-demo/schedule-demo.component';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ControlErrorModule } from '../shared/modules/controls/control-error/control-error.module';
import { TextInputModule } from '../shared/modules/controls/text-input/text-input.module';
import { TextareaControlModule } from '../shared/modules/controls/textarea-control/textarea-control.module';
import { BlueCheckboxModule } from '../shared/modules/blue-checkbox/blue-checkbox.module';
import { DropdownModule } from '../shared/modules/controls/dropdown/dropdown.module';
import { SharedModule } from '../shared/shared.module';
import { ThankYouInterestModalPopupComponent } from './thank-you-interest-modal-popup/thank-you-interest-modal-popup.component';
import { LockClickDirective } from './directives/lock-click-directive';
import { RouterLink, RouterModule } from '@angular/router';
import { InitiativeProgressTrackerComponent } from './initiative-progress-tracker/initiative-progress-tracker.component';
import { InitiativeDetailsComponent } from './initiative-details/initiative-details.component';



@NgModule({
  declarations: [ScheduleDemoComponent, ThankYouInterestModalPopupComponent, LockClickDirective, InitiativeProgressTrackerComponent, InitiativeDetailsComponent],
  imports: [
    CommonModule,
    TranslateModule,
    ReactiveFormsModule,
    TextInputModule,
    DropdownModule,
    BlueCheckboxModule,
    ControlErrorModule,
    TextareaControlModule,
    FormsModule,
    SharedModule,
    RouterModule
  ],
  exports: [
    ScheduleDemoComponent,
    ThankYouInterestModalPopupComponent,
    LockClickDirective,
    InitiativeProgressTrackerComponent,
    InitiativeDetailsComponent
  ]
})
export class PublicModule { }
