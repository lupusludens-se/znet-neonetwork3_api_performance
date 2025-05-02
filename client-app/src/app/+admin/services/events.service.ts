import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { CreateEventInterface } from '../modules/+create-event/interfaces/create-event.interface';
import { HttpService } from '../../core/services/http.service';
import { EventsApiEnum } from '../../shared/enums/api/events-api.enum';
import { FormGroup } from '@angular/forms';
import { BULLET_SYMBOL } from '../../shared/constants/symbols.const';
import { EventLocationType } from '../../shared/enums/event/event-location-type.enum';
import { CustomValidator } from '../../shared/validators/custom.validator';

@Injectable()
export class EventsService {
  eventsApiRoutes = EventsApiEnum;
  currentFormValue: BehaviorSubject<Partial<CreateEventInterface>> = new BehaviorSubject<Partial<CreateEventInterface>>(
    null
  );
  currentFormValue$: Observable<Partial<CreateEventInterface>> = this.currentFormValue.asObservable();

  constructor(private httpService: HttpService) {}

  updateFormValue(formVal: Partial<CreateEventInterface>): void {
    this.currentFormValue.next({ ...this.currentFormValue.value, ...formVal });
  }

  resetFromValue(): void {
    this.currentFormValue.next(null);
  }

  sortOccurrences(): void {
    this.currentFormValue.value.occurrences.sort((a, b) => {
      return a.fromDate.localeCompare(b.fromDate);
    });
  }

  checkHighlightsValue(formGroup: FormGroup): void {
    let trimmedValue = formGroup.get('highlights').value?.trimEnd();
    const highlights = trimmedValue?.length
      ? trimmedValue
          .replace('\r\n', '\n')
          .split('\n')
          .map(el => el.trim())
      : [];

    for (let i = 0; i < highlights.length; i++) {
      if (highlights[i].startsWith(BULLET_SYMBOL) && highlights[i].length === 1) {
        highlights[i] = '';
      }

      if (highlights[i].length > 0 && !highlights[i].startsWith(BULLET_SYMBOL)) {
        highlights[i] = BULLET_SYMBOL + ' ' + highlights[i];
      }
    }

    trimmedValue = highlights.join('\n').replace(/\n\s*\n/g, '\n');
    formGroup.get('highlights').patchValue(trimmedValue);
  }

  getEditEvent(eventId: string): Observable<Partial<CreateEventInterface>> {
    return this.httpService.get(this.eventsApiRoutes.Events + `/${eventId}`, {
      expand:
        'links,categories,occurrences,moderators,moderators.company,moderators.image,invitedcategories,invitedregions,invitedroles,invitedusers,invitedusers.company,invitedusers.image',
      eventTimeZoneOffset: true
    });
  }

  deleteEvent(eventId: number): Observable<any> {
    return this.httpService.delete(`${this.eventsApiRoutes.Events}/${eventId}`);
  }

  static changeLocationType(form: FormGroup, type: number): void {
    form.patchValue({ locationType: type });

    if (type === EventLocationType.InPerson) {
      form.get('location').removeValidators(CustomValidator.url);
      form.get('location').updateValueAndValidity();
    } else {
      form.get('location').addValidators(CustomValidator.url);
      form.get('location').updateValueAndValidity();
    }
  }

  static transformSaveEventPayload(saveEventPayload) {
    const payload = { ...saveEventPayload };
    payload.categories = <[{ id: number }]>payload.categories.map(category => ({ id: category.id }));
    payload.invitedCategories = payload.invitedCategories?.map(category => ({ id: category.id }));
    payload.invitedRegions = payload.invitedRegions?.map(region => ({ id: region.id }));
    payload.invitedRoles = payload.invitedRoles?.map(role => ({ id: role.id }));
    payload.invitedUsers = payload.invitedUsers?.map(user => ({ id: user.id }));
    payload.moderators = payload.moderators?.map(moderator => ({
      name: moderator.name,
      company: moderator.company,
      userId: moderator.userId
    }));
    delete payload.attendees;
    delete payload.timeZone;
    return payload;
  }
}
