import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { CreateEventSteps } from '../enums/create-event-steps.enum';
import { EventInterface } from '../../../../shared/interfaces/event/event.interface';
import { HttpService } from '../../../../core/services/http.service';
import { EventsApiEnum } from '../../../../shared/enums/api/events-api.enum';

@Injectable()
export class CreateEventService {
  private currentEventStep: BehaviorSubject<number> = new BehaviorSubject<number>(CreateEventSteps.CreateEventForm);
  currentEventStep$: Observable<CreateEventSteps> = this.currentEventStep.asObservable();
  eventsApi = EventsApiEnum;

  constructor(private readonly httpService: HttpService) {}

  changeEventStep(section: number): void {
    this.currentEventStep.next(section);

    if (window) {
      window.scrollTo(0, 0);
    }
  }

  createEvent(payload): Observable<any> {
    return this.httpService.post<EventInterface>(this.eventsApi.Events, payload);
  }
}
