import { InitiativeInformationComponent } from './initiative-information.component';
import { SharedModule } from '../../shared.module';
import { NgModule } from '@angular/core';
import { ProgressStepperModule } from '../progress-stepper/progress-stepper.module';
import { ContentTagModule } from '../content-tag/content-tag.module';
import { ContentLocationModule } from '../content-location/content-location.module';
import { DashboardInitiativeService } from 'src/app/+dashboard/components/corporation-dashboard/initiatives/services/initiative-information.service';
import { PaginationModule } from '../pagination/pagination.module';
import { UserAvatarModule } from '../user-avatar/user-avatar.module';

@NgModule({
  declarations: [InitiativeInformationComponent],
  exports: [InitiativeInformationComponent],
  providers: [DashboardInitiativeService],
  imports: [
    SharedModule,
    ProgressStepperModule,
    ContentTagModule,
    ContentLocationModule,
    PaginationModule,
    UserAvatarModule
  ]
})
export class InitiativeInformationModule {}
