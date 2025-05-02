import { Component, Input } from '@angular/core';
import { NotificationInterface } from 'src/app/shared/interfaces/notification.interface';

@Component({
  selector: 'neo-feedback',
  templateUrl: './feedback.component.html'
})
export class FeedbackComponent {
  @Input() notification: NotificationInterface;
}
