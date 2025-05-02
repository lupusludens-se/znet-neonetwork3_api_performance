import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { EventsService } from '../../../../services/events.service';

@Component({
  selector: 'neo-sections-panel',
  templateUrl: 'sections-panel.component.html',
  styleUrls: ['sections-panel.component.scss']
})
export class SectionsPanelComponent {
  @Input() createEventForm: FormGroup;
  @Input() inviteForm: FormGroup;

  constructor(public eventsService: EventsService) {}
}
