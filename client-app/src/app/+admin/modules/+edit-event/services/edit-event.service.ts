import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { CreateEventInterface } from '../../+create-event/interfaces/create-event.interface';
import { EventsApiEnum } from '../../../../shared/enums/api/events-api.enum';
import { HttpService } from '../../../../core/services/http.service';
import { EventsService } from 'src/app/+admin/services/events.service';

@Injectable()
export class EditEventService {
  apiRoutes = EventsApiEnum;

  constructor(private httpService: HttpService) {}

  editEvent(
    eventId: number,
    onlyVisibilityPropertyChanged: boolean,
    payload: Partial<CreateEventInterface>
  ): Observable<any> {
    return this.httpService.put(
      this.apiRoutes.Events + `/${eventId}`,
      EventsService.transformSaveEventPayload(payload),
      { onlyVisibilityPropertyChanged: onlyVisibilityPropertyChanged }
    );
  }
}
