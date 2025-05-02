import { Component, OnDestroy, OnInit } from '@angular/core';
import { CreateEventService } from './services/create-event.service';
import { CreateEventSteps } from './enums/create-event-steps.enum';
import { TitleService } from 'src/app/core/services/title.service';
import { EventsService } from '../../services/events.service';

@Component({
  selector: 'neo-create-event',
  templateUrl: 'create-event.component.html',
  styleUrls: ['create-event.component.scss']
})
export class CreateEventComponent implements OnInit, OnDestroy {
  eventSteps = CreateEventSteps;

  constructor(
    public createEventStepsService: CreateEventService,
    private eventsService: EventsService,
    private titleService: TitleService
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle('events.createAnEventLabel');
  }

  ngOnDestroy(): void {
    this.createEventStepsService.changeEventStep(this.eventSteps.CreateEventForm);
    this.eventsService.resetFromValue();
  }
}
