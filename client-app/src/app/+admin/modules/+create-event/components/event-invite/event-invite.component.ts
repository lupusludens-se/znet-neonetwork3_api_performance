import { FormBuilder, FormGroup } from '@angular/forms';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { filter, take } from 'rxjs';
import { UserEventRoleConst } from '../../../../constants/user-event-role.const';
import { EventUserInterface } from '../../../../interfaces/event-user.interface';
import { RegionsService } from '../../../../../shared/services/regions.service';
import { TagInterface } from '../../../../../core/interfaces/tag.interface';
import { CommonService } from '../../../../../core/services/common.service';
import { CreateEventService } from '../../services/create-event.service';
import { CreateEventSteps } from '../../enums/create-event-steps.enum';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CustomValidator } from '../../../../../shared/validators/custom.validator';
import structuredClone from '@ungap/structured-clone';
import { EventLinkInterface } from '../../../../../shared/interfaces/event/event-link.interface';
import { EventsService } from 'src/app/+admin/services/events.service';
import { EventTypeEnum } from 'src/app/shared/interfaces/event/event.interface';

@UntilDestroy()
@Component({
  selector: 'neo-event-invite',
  templateUrl: 'event-invite.component.html',
  styleUrls: [
    '../../create-event.component.scss',
    '../event-side-panel/event-side-panel.component.scss',
    'event-invite.component.scss'
  ]
})
export class EventInviteComponent implements OnInit, OnDestroy {
  form: FormGroup = this.formBuilder.group({
    invitedRoles: [null, CustomValidator.required],
    invitedRegions: [null],
    invitedCategories: [null],
    invitedUsers: [null]
  });
  userRoles = UserEventRoleConst.map(r => Object.assign({}, r));
  categoriesList: TagInterface[];
  eventSteps = CreateEventSteps;
  formSubmitted: boolean;
  isPublicEvent: boolean;

  constructor(
    public regionsService: RegionsService,
    private formBuilder: FormBuilder,
    private router: Router,
    private createEventService: CreateEventService,
    private eventsService: EventsService,
    private commonService: CommonService
  ) {}
  ngOnDestroy(): void {
    this.eventsService.currentFormValue.next(null);
    this.createEventService.changeEventStep(this.eventSteps.CreateEventForm);
  }

  ngOnInit(): void {
    this.isPublicEvent = this.eventsService.currentFormValue.value.eventType == EventTypeEnum.Public;
    this.regionsService.getContinentsList();
    if (!this.form.controls['invitedRoles'].value)
      this.eventsService.updateFormValue({
        invitedRoles: []
      });
    this.isPublicEvent
      ? this.form.controls['invitedRoles'].removeValidators(CustomValidator.required)
      : this.form.controls['invitedRoles'].addValidators(CustomValidator.required);
    this.commonService
      .initialData()
      .pipe(
        filter(v => !!v),
        untilDestroyed(this)
      )
      .subscribe(data => {
        this.categoriesList = structuredClone(data)?.categories || [];
        this.setInitialFormData();
      });
  }

  saveRole(id: number) {
    let updatedRoles: { id: number }[] = [];

    this.userRoles.map(sr => {
      if (sr.id === id) {
        sr.checked = !sr.checked;
      }

      if (sr.checked || sr.preSelected) {
        updatedRoles.push({ id: sr.id });
      }
    });

    this.eventsService.updateFormValue({
      invitedRoles: updatedRoles
    });

    this.form.patchValue({ invitedRoles: updatedRoles });
  }

  addCategory(category: TagInterface[]) {
    const selectedCategories: TagInterface[] = category.filter(t => t.selected);
    this.form.controls['invitedCategories'].patchValue(selectedCategories);

    this.eventsService.updateFormValue({
      invitedCategories: selectedCategories
    });
  }

  addUsersToPayload(users: EventUserInterface[]): void {
    this.form.controls['invitedUsers'].patchValue(users);
    this.eventsService.updateFormValue({
      invitedUsers: users
    });
  }

  goBack() {
    this.createEventService.changeEventStep(this.eventSteps.CreateEventForm);
  }

  removeEmptyLinks(links: EventLinkInterface[]): void {
    if (links && links?.length !== 0) {
      const filteredLinks: EventLinkInterface[] = links.filter(l => !!l.url && !!l.name);

      if (filteredLinks?.length === 0) {
        delete this.eventsService.currentFormValue.value.links;
      } else {
        this.eventsService.updateFormValue({ links: filteredLinks });
      }
    }
  }

  removeEmptyRecordings(recordings: { url: string }[]): void {
    if (recordings && recordings?.length !== 0) {
      const filteredRecordings: EventLinkInterface[] = recordings.filter(r => !!r.url);

      if (filteredRecordings?.length === 0) {
        delete this.eventsService.currentFormValue.value.recordings;
      } else {
        this.eventsService.updateFormValue({ recordings: filteredRecordings });
      }
    }
  }

  saveEvent() {
    this.formSubmitted = true;

    if (!this.isPublicEvent && !this.form.value['invitedRoles']?.length) {
      return;
    }

    this.removeEmptyLinks(this.eventsService.currentFormValue.value.links);
    this.removeEmptyRecordings(this.eventsService.currentFormValue.value.recordings);

    if (
      this.eventsService.currentFormValue.value?.recordings?.length === 1 &&
      (!this.eventsService.currentFormValue.value?.recordings[0].url ||
        !this.eventsService.currentFormValue.value?.recordings[0])
    ) {
      delete this.eventsService.currentFormValue.value.recordings;
    }

    const modIdsArray: number[] = this.eventsService.currentFormValue.value.moderators.map(mod => mod.userId);

    const uniqueModerators = this.eventsService.currentFormValue.value.moderators.filter(
      (mod, index) => (mod.userId && !modIdsArray.includes(mod.userId, index + 1)) || (!mod.userId && mod.name)
    );

    this.eventsService.updateFormValue({
      moderators: [...uniqueModerators]
    });

    this.eventsService.sortOccurrences();

    const formValue = EventsService.transformSaveEventPayload(this.eventsService.currentFormValue.value);

    this.createEventService
      .createEvent(formValue)
      .pipe(take(1))
      .subscribe(resp => {
        this.router.navigate([`events/${resp.id}`]);
      });
  }

  onCancel(): void {
    this.form.reset();
    this.eventsService.resetFromValue();
    this.createEventService.changeEventStep(this.eventSteps.CreateEventForm);
    this.router.navigate(['./admin']);
  }

  private setInitialFormData(): void {
    this.eventsService.currentFormValue$
      .pipe(
        filter(v => !!v),
        take(1)
      )
      .subscribe(formVal => {
        if (formVal?.invitedCategories) {
          formVal.invitedCategories.forEach(ic => {
            this.categoriesList.forEach(cl => {
              if (ic.id === cl.id) {
                cl.selected = true;
              }
            });
          });
        }

        if (formVal?.invitedRoles) {
          this.form.controls['invitedRoles'].patchValue(formVal?.invitedRoles);

          this.userRoles.forEach(r => {
            formVal?.invitedRoles.forEach(ir => {
              if (ir.id === r.id) {
                r.checked = true;
              }
            });
          });
        }

        if (formVal.invitedRegions) {
          this.form.controls['invitedRegions'].patchValue(formVal.invitedRegions);
        }

        if (formVal.invitedUsers) {
          this.form.controls['invitedUsers'].patchValue(formVal.invitedUsers);
        }
      });
  }
}
