import { Component, Input } from '@angular/core';

@Component({
  selector: 'neo-notification-loader',
  templateUrl: './notification-loader.component.html',
  styleUrls: ['./notification-loader.component.scss']
})
export class NotificationLoaderComponent {
  @Input() showWrapper: boolean;
}
