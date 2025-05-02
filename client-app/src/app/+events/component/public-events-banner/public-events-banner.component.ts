import { Component } from '@angular/core';
import { SignTrackingSourceEnum } from '../../../core/enums/sign-tracking-source-enum';
@Component({
  selector: 'neo-public-events-banner',
  templateUrl: './public-events-banner.component.html',
  styleUrls: ['./public-events-banner.component.scss']
})
export class PublicEventsBannerComponent {
  signTrackingSourceEnum = SignTrackingSourceEnum.ZeigoNetwork;
  constructor() {}
}
