import { Component, Input } from '@angular/core';
import { CreateEventInterface } from '../../interfaces/create-event.interface';

@Component({
  selector: 'neo-event-side-panel',
  templateUrl: 'event-side-panel.component.html',
  styleUrls: ['event-side-panel.component.scss']
})
export class EventSidePanelComponent {
  @Input() stepsValue: CreateEventInterface;
}
