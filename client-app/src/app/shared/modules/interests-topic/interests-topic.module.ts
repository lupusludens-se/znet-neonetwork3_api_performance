import { SvgIconsModule } from '@ngneat/svg-icon';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { InterestsTopicComponent } from './interests-topic.component';

@NgModule({
  declarations: [InterestsTopicComponent],
  imports: [CommonModule, SvgIconsModule],
  exports: [InterestsTopicComponent]
})
export class InterestsTopicModule {}
