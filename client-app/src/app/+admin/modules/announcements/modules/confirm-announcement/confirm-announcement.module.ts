// modules
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { CommonModule } from '@angular/common';

// components
import { ConfirmAnnouncementComponent } from './confirm-announcement.component';

@NgModule({
  declarations: [ConfirmAnnouncementComponent],
  imports: [CommonModule, TranslateModule, SvgIconsModule],
  exports: [ConfirmAnnouncementComponent]
})
export class ConfirmAnnouncementModule {}
