import { Component, Input } from '@angular/core';
import { NotificationInterface } from 'src/app/shared/interfaces/notification.interface';

@Component({
  selector: 'neo-initiative',
  templateUrl: './initiative.component.html'
})
export class InitiativeComponent {
  @Input() notification: NotificationInterface;
}
