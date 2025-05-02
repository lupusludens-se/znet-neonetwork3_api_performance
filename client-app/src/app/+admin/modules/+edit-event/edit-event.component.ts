import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserEventRoleConst, UserEventRoleInterface } from '../../constants/user-event-role.const';
import { EventUserInterface } from '../../../shared/interfaces/event/event-user.interface';
import { CreateEventInterface } from '../+create-event/interfaces/create-event.interface';
import { EventLocationType } from '../../../shared/enums/event/event-location-type.enum';
import { CustomValidator } from '../../../shared/validators/custom.validator';
import { EventsApiEnum } from '../../../shared/enums/api/events-api.enum';
import { RegionsService } from '../../../shared/services/regions.service';
import { EventsService } from '../../services/events.service';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { TitleService } from 'src/app/core/services/title.service';
import { EditEventService } from './services/edit-event.service';
import structuredClone from '@ungap/structured-clone';
import { take } from 'rxjs';
import { EventInviteType } from '../+create-event/enums/event-invite-type';
import { TranslateService } from '@ngx-translate/core';
import { RadioControlInterface } from 'src/app/shared/modules/radio-control/radio-control.component';
import { EventTypeEnum } from 'src/app/shared/interfaces/event/event.interface';

@Component({
  selector: 'neo-event-edit',
  templateUrl: 'edit-event.component.html',
  styleUrls: ['edit-event.component.scss']
})
export class EditEventComponent implements OnInit, OnDestroy {
  event: CreateEventInterface;
  eventInfoForm: FormGroup = this.formBuilder.group({
    subject: ['', [CustomValidator.required, Validators.maxLength(60)]],
    isHighlighted: [],
    showInPublicSite: [],
    eventType: [1, CustomValidator.required],
    highlights: [''],
    occurrences: this.formBuilder.array([], [CustomValidator.required, CustomValidator.checkOccurrences]),
    timeZoneId: [null],
    locationType: [],
    location: ['', CustomValidator.required],
    description: [],
    userRegistration: [],
    moderators: this.formBuilder.array([], [CustomValidator.required, CustomValidator.checkModerators]),
    recordings: this.formBuilder.array([]),
    categories: this.formBuilder.array([], CustomValidator.required), // * tags
    links: this.formBuilder.array([])
  });
  inviteForm: FormGroup = this.formBuilder.group({
    invitedRegions: [null],
    invitedCategories: [null]
  });
  eventLocationType = EventLocationType;
  linksError: boolean;
  showModal: boolean;
  moderatorsError: boolean;
  userRoles: UserEventRoleInterface[] = UserEventRoleConst.map(r => Object.assign({}, r));
  eventsApi = EventsApiEnum;
  showConfirmDelete: boolean;
  formSubmitted: boolean;
  formValue: Partial<CreateEventInterface>;
  modalTitle: string;
  showRadioButtons: boolean;
  showModalTitle: boolean;
  isInviviteesUpdated: boolean;
  confirmationMessages = [
    { id: 1, name: 'Save without sending updated invitations', type: EventInviteType.NoInvite },
    { id: 2, name: 'Save and send updated invitations to all attendees', type: EventInviteType.InviteAll },
    {
      id: 3,
      name: 'Save without sending updated invitations to existing attendees',
      type: EventInviteType.InviteNewlyAdded
    }
  ];
  confirmationMsgs: any;
  selectedConfirmationId: number;
  isPublicEvent: boolean;
  showInPublicModal: boolean;
  onlyVisibilityPropertyChanged: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private eventsService: EventsService,
    private activatedRoute: ActivatedRoute,
    public regionsService: RegionsService,
    private router: Router,
    public editEventService: EditEventService,
    private titleService: TitleService,
    private cdr: ChangeDetectorRef,
    private translateService: TranslateService
  ) {}

  ngOnInit(): void {
    this.regionsService.getContinentsList();
    const eventId: string = this.activatedRoute.snapshot.params.id;
    this.eventsService
      .getEditEvent(eventId)
      .pipe(take(1))
      .subscribe(event => {
        this.event = event as CreateEventInterface;
        this.isPublicEvent = event.eventType == EventTypeEnum.Public;
        this.eventsService.updateFormValue(event);

        this.eventInfoForm.patchValue({
          subject: event.subject,
          isHighlighted: event.isHighlighted,
          highlights: event.highlights,
          locationType: event.locationType,
          location: event.location,
          userRegistration: event.userRegistration,
          description: event.description,
          timeZoneId: event.timeZoneId,
          eventType: event.eventType,
          showInPublicSite: event.showInPublicSite
        });

        this.changeLocationType(event.locationType);
        this.eventInfoForm.markAllAsTouched();

        this.inviteForm.controls['invitedCategories'].patchValue(event.invitedCategories);

        this.userRoles.forEach(ur => {
          event.invitedRoles.forEach(ir => {
            if (ir.id === ur.id) ur.preSelected = true;
          });
        });
      });
    this.titleService.setTitle('events.editEventLabel');
    this.cdr.detectChanges();
  }

  ngOnDestroy(): void {
    this.eventsService.resetFromValue();
  }

  changeLocationType(type: number) {
    EventsService.changeLocationType(this.eventInfoForm, type);
    this.eventInfoForm.get('locationType').updateValueAndValidity();
  }

  showLinksError(): void {
    this.linksError =
      this.eventInfoForm.get('links').value?.length > 0 &&
      this.eventInfoForm.get('links').value?.some(r => (r.url && !r.name) || (r.name && !r.url));
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

  saveRole(id: number): void {
    let updatedRoles: { id: number }[] = [];

    this.userRoles.forEach(sr => {
      if (sr.id === id) sr.checked = !sr.checked;
      if (sr.checked || sr.preSelected) updatedRoles.push({ id: sr.id });
    });

    this.eventsService.updateFormValue({
      invitedRoles: updatedRoles
    });
  }

  addRegions(selectedRegions: TagInterface[]): void {
    this.inviteForm.controls['invitedRegions'].patchValue(selectedRegions);
    this.eventsService.updateFormValue({
      invitedRegions: this.inviteForm.controls['invitedRegions'].value
    });
  }

  onEdit(): void {
    this.linksError = false;
    this.formSubmitted = true;
    this.showRadioButtons = true;

    this.showLinksError();

    if (this.eventInfoForm.invalid || this.inviteForm.invalid || this.linksError) {
      return;
    }
    this.showModal = true;

    this.eventsService.checkHighlightsValue(this.eventInfoForm);

    const modIdsArray: number[] = this.eventInfoForm.value.moderators.map(mod => {
      return mod.userId;
    });

    const uniqueModerators = this.eventInfoForm.value.moderators.filter(
      (mod, index) => (mod.userId && !modIdsArray.includes(mod.userId, index + 1)) || (!mod.userId && mod.name)
    );

    this.eventsService.updateFormValue({
      ...this.eventInfoForm.getRawValue(),
      links: structuredClone(this.eventInfoForm.value.links)?.filter(l => !!l.name && !!l.url) || [],
      recordings: structuredClone(this.eventInfoForm.value.recordings)?.filter(r => !!r.url) || [],
      moderators: [...uniqueModerators]
    });
    let currentFormValue = this.eventsService.currentFormValue.value;
    // Date Time check
    let dateTimeChanged = false;

    if (
      currentFormValue.occurrences.length !== this.event.occurrences.length ||
      currentFormValue.timeZoneId !== this.event.timeZoneId
    ) {
      dateTimeChanged = true;
    } else {
      dateTimeChanged =
        this.compareIfArraysAreSame(currentFormValue.occurrences, this.event.occurrences, 'fromDate', [
          'fromDate',
          'toDate'
        ]) ||
        this.compareIfArraysAreSame(currentFormValue.occurrences, this.event.occurrences, 'toDate', [
          'fromDate',
          'toDate'
        ]);
    }

    // New Invities Check
    let newInvitiesAdded = this.event.invitedUsers.length !== currentFormValue.invitedUsers.length;

    // Location Change Check
    let locationChanged =
      currentFormValue.location !== this.event.location || currentFormValue.locationType !== this.event.locationType;

    // Other Properties Change Check
    let otherPropertiesExceptVisiblePropChanged =
      currentFormValue.subject !== this.event.subject ||
      currentFormValue.isHighlighted !== this.event.isHighlighted ||
      currentFormValue.userRegistration !== this.event.userRegistration ||
      currentFormValue.description !== this.event.description ||
      ((this.event.highlights || currentFormValue.highlights) &&
        currentFormValue.highlights !== this.event.highlights) ||
      currentFormValue.moderators.length !== this.event.moderators.length ||
      this.compareIfArraysAreSame(currentFormValue.moderators, this.event.moderators, 'name', ['name']) ||
      currentFormValue.categories.length !== this.event.categories.length ||
      this.compareIfArraysAreSame(currentFormValue.categories, this.event.categories, 'id', ['id']) ||
      currentFormValue.links.length !== this.event.links.length ||
      this.compareIfArraysAreSame(currentFormValue.links, this.event.links, 'name', ['name']) ||
      currentFormValue.recordings.length !== this.event.recordings.length ||
      this.compareIfArraysAreSame(currentFormValue.recordings, this.event.recordings, 'url', ['url']) ||
      currentFormValue.invitedRegions.length !== this.event.invitedRegions.length ||
      this.compareIfArraysAreSame(currentFormValue.invitedRegions, this.event.invitedRegions, 'name', ['name']) ||
      currentFormValue.invitedCategories.length !== this.event.invitedCategories.length ||
      this.compareIfArraysAreSame(currentFormValue.invitedCategories, this.event.invitedCategories, 'name', ['name']) ||
      currentFormValue.invitedRoles.length !== this.event.invitedRoles.length ||
      this.compareIfArraysAreSame(currentFormValue.invitedRoles, this.event.invitedRoles, 'id', ['id']);

    let otherPropertiesChanged =
      currentFormValue.showInPublicSite !== this.event.showInPublicSite || otherPropertiesExceptVisiblePropChanged;

    let onlyVisiblePropChanged =
      currentFormValue.showInPublicSite !== this.event.showInPublicSite && !otherPropertiesExceptVisiblePropChanged;

    this.eventsService.sortOccurrences();
    // Past event
    if (this.getNowUTC() > this.eventLastOccurenceDateUTC()) {
      this.showRadioButtons = false;
      this.showModalTitle = true;
      this.modalTitle = this.translateService.instant('events.noEmailsWillBeSentLabel');
      this.selectedConfirmationId = this.confirmationMessages.filter(e => e.type === EventInviteType.NoInvite)[0].id;
    }
    // Future event
    else {
      // (1+2+3) or (1+3) or (1+2)
      if (
        ((dateTimeChanged || locationChanged) && otherPropertiesChanged && newInvitiesAdded) ||
        ((dateTimeChanged || locationChanged) && newInvitiesAdded) ||
        ((dateTimeChanged || locationChanged) && otherPropertiesChanged)
      ) {
        this.showRadioButtons = false;
        this.showModalTitle = true;
        this.modalTitle = this.translateService.instant('events.updateAddedInviteesLabel');
        this.selectedConfirmationId = this.confirmationMessages.filter(e => e.type === EventInviteType.InviteAll)[0].id;
      }
      // (2+3)
      else if (otherPropertiesChanged && newInvitiesAdded) {
        this.showRadioButtons = true;
        this.showModalTitle = true;
        this.modalTitle = this.translateService.instant('events.addedInviteesLabel');
        let updatedConfirmationMsgs = JSON.parse(JSON.stringify(this.confirmationMessages));
        updatedConfirmationMsgs[updatedConfirmationMsgs.findIndex(e => e.type === EventInviteType.InviteAll)].name =
          'Also save and send updated invitations to existing attendees';
        this.confirmationMsgs = updatedConfirmationMsgs.filter(
          e => e.type === EventInviteType.InviteNewlyAdded || e.type === EventInviteType.InviteAll
        );
        this.selectedConfirmationId = this.confirmationMessages.filter(
          e => e.type === EventInviteType.InviteNewlyAdded
        )[0].id;
      }
      // Only date/time/location change (1)
      else if (dateTimeChanged || locationChanged) {
        this.showRadioButtons = false;
        this.showModalTitle = true;
        this.modalTitle = this.translateService.instant('events.updateAddedInviteesLabel');
        this.selectedConfirmationId = this.confirmationMessages.filter(e => e.type === EventInviteType.InviteAll)[0].id;
      }
      // Other then date/time/location changes (2)
      else if (otherPropertiesChanged) {
        this.showRadioButtons = true;
        this.showModalTitle = false;
        this.confirmationMsgs = this.confirmationMessages.filter(
          e => e.type === EventInviteType.NoInvite || e.type === EventInviteType.InviteAll
        );
        this.selectedConfirmationId = this.confirmationMessages.filter(e => e.type === EventInviteType.NoInvite)[0].id;
      }
      // Only new users added (3)
      else if (newInvitiesAdded) {
        this.showRadioButtons = false;
        this.showModalTitle = true;
        this.modalTitle = this.translateService.instant('events.invitationsToAddedInviteesLabel');
        this.selectedConfirmationId = this.confirmationMessages.filter(
          e => e.type === EventInviteType.InviteNewlyAdded
        )[0].id;
      } else {
        this.showRadioButtons = false;
        this.showModalTitle = true;
        this.modalTitle = this.translateService.instant('events.noChangesMadeLabel');
        this.selectedConfirmationId = this.confirmationMessages.filter(e => e.type === EventInviteType.NoInvite)[0].id;
      }
    }
    if (dateTimeChanged || newInvitiesAdded || locationChanged || otherPropertiesChanged) {
      this.onlyVisibilityPropertyChanged = false;
      if (otherPropertiesChanged && onlyVisiblePropChanged) {
        this.onlyVisibilityPropertyChanged = true;
      }
    }
  }

  goBack(): void {
    history.back();
  }

  deleteEvent(): void {
    this.eventsService.deleteEvent(this.event.id).subscribe(() => this.router.navigate(['events']));
  }

  addUsersToPayload(users: EventUserInterface[]): void {
    this.eventsService.updateFormValue({ invitedUsers: users });
  }

  Confirm() {
    this.eventsService.currentFormValue.value.inviteType = this.confirmationMessages.filter(
      e => e.id === this.selectedConfirmationId
    )[0].type;
    this.editEventService
      .editEvent(this.event.id, this.onlyVisibilityPropertyChanged, this.eventsService.currentFormValue.value)
      .pipe(take(1))
      .subscribe(() => {
        this.router.navigate(['events']);
      });
  }

  Cancel() {
    this.showModal = false;
  }

  confirmSelection(value: number) {
    this.selectedConfirmationId = value;
    this.eventsService.currentFormValue.value.inviteType = this.confirmationMessages.filter(
      e => e.id === value
    )[0].type;
  }

  getNowUTC() {
    const now = new Date();
    return now.getTime() + now.getTimezoneOffset() * 60000;
  }

  eventLastOccurenceDateUTC() {
    let date = new Date(this.event.occurrences[this.event.occurrences.length - 1].toDate);
    return date.getTime() + this.event.occurrences[this.event.occurrences.length - 1].timeZoneUtcOffset * 60000;
  }

  compareIfArraysAreSame(arr1, arr2, uniqueProperty, propertyArray) {
    var result = arr1
      .filter(function (o1) {
        // filter out (!) items in result2
        return !arr2.some(function (o2) {
          return o1[uniqueProperty] === o2[uniqueProperty]; // assumes unique id
        });
      })
      .map(function (o) {
        // use reduce to make objects with only the required properties
        // and map to apply this to the filtered array as a whole
        return propertyArray.reduce(function (newo, name) {
          newo[name] = o[name];
          return newo;
        }, {});
      });
    return result.length > 0;
  }

  showEventInPublic() {
    this.showInPublicModal = false;
  }

  togglePublicEventModal(): void {
    this.showInPublicModal = false;
    this.eventInfoForm.get('showInPublicSite').setValue(false);
  }

  changeVisibilityInPublicDashboard() {
    this.eventInfoForm.get('showInPublicSite').patchValue(!this.eventInfoForm.get('showInPublicSite').value);
    this.showInPublicModal = this.eventInfoForm.get('showInPublicSite').value;
  }
}
