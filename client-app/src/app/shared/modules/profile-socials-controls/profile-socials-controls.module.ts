// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { RouterModule } from '@angular/router';

// components
import { ProfileSocialsControlsComponent } from './profile-socials-controls.component';

@NgModule({
  declarations: [ProfileSocialsControlsComponent],
  imports: [SharedModule, RouterModule],
  exports: [ProfileSocialsControlsComponent]
})
export class ProfileSocialsControlsModule {}
