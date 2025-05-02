// modules
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { RadioControlModule } from '../shared/modules/radio-control/radio-control.module';
import { UnsubscribeRoutingModule } from './unsubscribe-routing.module';
import { UnsubscribeComponent } from './unsubscribe.component';
import { FooterModule } from '../shared/modules/footer/footer.module';
import { UnsubscribeService } from '../shared/services/unsubscribe.service';



@NgModule({
  declarations: [UnsubscribeComponent],
  imports: [
    UnsubscribeRoutingModule,
    SharedModule,
    RadioControlModule,
    ReactiveFormsModule,
    FormsModule,
    FooterModule
  ],
  providers: [
    UnsubscribeService
  ]
})
export class UnsubscribeModule {}
