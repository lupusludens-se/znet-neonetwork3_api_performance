import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NewAndNoteworthyPostInterface } from 'src/app/+learn/interfaces/post.interface';
import { FirstClickInfoActivityDetailsInterface } from '../../../core/interfaces/activity-details/first-click-info-activity-details.interface';

@Component({
  selector: 'neo-public-discover-key-content',
  templateUrl: './public-discover-key-content.component.html',
  styleUrls: ['./public-discover-key-content.component.scss']
})
export class PublicDiscoverKeyContentComponent {
  @Input() publicOrPrivateContent: NewAndNoteworthyPostInterface;
  @Output() postClick: EventEmitter<void> = new EventEmitter<void>();
  @Input() isPublic: boolean;
  @Output() elementClick: EventEmitter<FirstClickInfoActivityDetailsInterface> =
    new EventEmitter<FirstClickInfoActivityDetailsInterface>();
  constructor() {}
}
