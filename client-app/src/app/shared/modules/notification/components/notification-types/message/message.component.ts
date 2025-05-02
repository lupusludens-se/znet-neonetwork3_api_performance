import { Component, Input } from '@angular/core';

import { NotificationInterface } from '../../../../../interfaces/notification.interface';

@Component({
  selector: 'neo-message',
  templateUrl: './message.component.html'
})
export class MessageComponent {
  @Input() notification: NotificationInterface;
}
