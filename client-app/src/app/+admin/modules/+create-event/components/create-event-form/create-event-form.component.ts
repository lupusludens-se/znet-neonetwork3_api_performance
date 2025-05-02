import { ControlContainer, FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { filter, Observable } from 'rxjs';
import { Router } from '@angular/router';

import { EventLocationType } from '../../../../../shared/enums/event/event-location-type.enum';
import { CustomValidator } from '../../../../../shared/validators/custom.validator';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { CreateEventService } from '../../services/create-event.service';
import { CreateEventSteps } from '../../enums/create-event-steps.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { EventsService } from '../../../../services/events.service';
import { RadioControlInterface } from 'src/app/shared/modules/radio-control/radio-control.component';
import { EventTypeEnum } from 'src/app/shared/interfaces/event/event.interface';
import { TranslateService } from '@ngx-translate/core';

@UntilDestroy()
@Component({
  selector: 'neo-create-event-form',
  templateUrl: 'create-event-form.component.html',
  styleUrls: ['../../create-event.component.scss', 'create-event-form.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class CreateEventFormComponent implements OnInit {
  createEventForm: FormGroup = this.formBuilder.group({
    subject: ['', CustomValidator.required],
    eventType: [1, CustomValidator.required],
    isHighlighted: [false],
    showInPublicSite: [false],
    occurrences: this.formBuilder.array([], [CustomValidator.required, CustomValidator.checkOccurrences]),
    timeZoneId: [1, Validators.required],
    locationType: [EventLocationType.Virtual],
    location: ['', [CustomValidator.required, CustomValidator.url]],
    userRegistration: [''],
    description: [''],
    highlights: [''],
    moderators: this.formBuilder.array([], CustomValidator.checkModerators),
    categories: this.formBuilder.array([], CustomValidator.required), // * tags
    recordings: this.formBuilder.array([]),
    links: this.formBuilder.array([])
  });

  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  eventLocationType = EventLocationType;
  eventSteps = CreateEventSteps;
  linksError: boolean;
  formSubmitted: boolean;
  publicEventModal: boolean;
  showInPublicModal: boolean;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private createEventService: CreateEventService,
    private eventsService: EventsService,
    private readonly translateService: TranslateService,
    private readonly authService: AuthService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.eventsService.currentFormValue$
      .pipe(
        filter(v => !!v),
        untilDestroyed(this)
      )
      .subscribe(formVal => {
        if (formVal?.location && formVal.locationType !== this.createEventForm.value.locationType) {
          this.changeLocationType(formVal.locationType);
        }

        this.createEventForm.patchValue({
          subject: formVal.subject,
          isHighlighted: formVal.isHighlighted,
          locationType: formVal.locationType,
          location: formVal.location,
          userRegistration: formVal.userRegistration,
          description: formVal.description,
          highlights: formVal.highlights,
          eventType: formVal.eventType,
          showInPublicSite: formVal.showInPublicSite
        });
      });

    this.cdr.detectChanges();
  }

  eventTypes: RadioControlInterface[] = [
    {
      id: EventTypeEnum.Private,
      name: this.translateService.instant('events.eventTypes.visibleToInviteesOnly')
    },
    {
      id: EventTypeEnum.Public,
      name: this.translateService.instant('events.eventTypes.visibleToAllMembers')
    }
  ];

  changeLocationType(type: number) {
    EventsService.changeLocationType(this.createEventForm, type);
    this.createEventForm.get('locationType').updateValueAndValidity();
  }

  onCancel() {
    this.createEventForm.reset();
    this.eventsService.resetFromValue();
    this.router.navigate(['./admin']);
  }

  showLinksError(): void {
    this.linksError =
      this.createEventForm.get('links').value?.length > 0 &&
      this.createEventForm.get('links').value?.some(r => (r.url && !r.name) || (r.name && !r.url));
  }

  goToInvite(): void {
    this.formSubmitted = true;
    this.showLinksError();

    if (this.createEventForm.invalid || this.linksError) {
      return;
    }

    this.linksError = false;
    this.eventsService.checkHighlightsValue(this.createEventForm);
    this.eventsService.updateFormValue(this.createEventForm.getRawValue());
    this.createEventService.changeEventStep(this.eventSteps.EventInvite);
  }

  changeEventType(control: RadioControlInterface) {
    this.publicEventModal = control.id === EventTypeEnum.Public;
  }

  makeEventPublic() {
    this.createEventForm.get('eventType').setValue(EventTypeEnum.Public);
    this.publicEventModal = false;
  }

  toggleModal(): void {
    this.publicEventModal = false;
    this.createEventForm.get('eventType').setValue(EventTypeEnum.Private);
  }

  showEventInPublic() {
    this.showInPublicModal = false;
  }

  togglePublicEventModal(): void {
    this.showInPublicModal = false;
    this.createEventForm.get('showInPublicSite').setValue(false);
  }

  changeVisibilityInPublicDashboard() {
    this.createEventForm.get('showInPublicSite').patchValue(!this.createEventForm.get('showInPublicSite').value);
    this.showInPublicModal = this.createEventForm.get('showInPublicSite').value;
  }
}
