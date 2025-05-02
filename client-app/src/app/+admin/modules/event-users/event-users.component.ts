import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime, Subject } from 'rxjs';

import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import { EventInterface } from '../../../shared/interfaces/event/event.interface';
import { EventUserInterface } from '../../interfaces/event-user.interface';
import { EventsApiEnum } from '../../../shared/enums/api/events-api.enum';
import { HttpService } from '../../../core/services/http.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CreateEventInterface } from '../+create-event/interfaces/create-event.interface';
import { SearchValidatorService } from 'src/app/shared/services/search-validator.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { EventsService } from '../../services/events.service';

@UntilDestroy()
@Component({
  selector: 'neo-event-users',
  templateUrl: 'event-users.component.html',
  styleUrls: ['../+create-event/create-event.component.scss', 'event-users.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EventUsersComponent implements OnChanges, OnDestroy, OnInit {
  @Input() disabled: boolean;
  @Input() event: EventInterface;

  @Output() selectedUsersUpdated: EventEmitter<EventUserInterface[]> = new EventEmitter<EventUserInterface[]>();

  searchStrSubject: Subject<string> = new Subject<string>();
  apiRoutes = EventsApiEnum;
  matchedUsers: EventUserInterface[] = [];
  selectedUsers: EventUserInterface[] = [];
  initialMatchedUsers: EventUserInterface[] = [];
  searchInput: FormControl = new FormControl();
  usersRequested: boolean;
  matchByParam: string;
  allUsersSelected: boolean;

  eventRegions = [];
  eventCategories = [];
  eventRoles = [];

  constructor(
    private httpService: HttpService,
    private changeDetRef: ChangeDetectorRef,
    private eventsService: EventsService,
    private snackbarService: SnackbarService
  ) {}

  ngOnInit(): void {
    this.searchStrSubject.pipe(debounceTime(400)).subscribe(searchStr => {
      this.searchUsers(searchStr);
    });

    this.eventsService.currentFormValue.pipe(untilDestroyed(this)).subscribe((value: CreateEventInterface) => {
      if (
        value &&
        (this.eventRegions?.length !== value.invitedRegions?.length ||
          this.eventCategories?.length !== value.invitedCategories?.length ||
          this.eventRoles?.length !== value.invitedRoles?.length)
      ) {
        value.id ? this.resetUsersList() : this.initSelectedUsers(value);
        this.eventRegions = value.invitedRegions || [];
        this.eventCategories = value.invitedCategories || [];
        this.eventRoles = value.invitedRoles || [];
      }
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    // * for edit page
    if (changes['event']?.currentValue !== changes['event']?.previousValue && !!changes['event']?.currentValue) {
      this.loadMatchingUsers();
    }
  }

  ngOnDestroy(): void {
    this.searchStrSubject.unsubscribe();
  }

  loadMatchingUsers(): void {
    this.searchInput.setValue('', { emitEvent: false });
    this.usersRequested = false;
    this.matchedUsers = this.matchedUsers.filter(mu => mu.isInvited);
    this.initialMatchedUsers = [];

    // !! refactor to one function
    if (this.eventsService.currentFormValue.value.invitedRoles?.length) {
      let selectedRoles: string;

      this.eventsService.currentFormValue.value.invitedRoles.forEach(role => {
        selectedRoles = selectedRoles ? `${selectedRoles},${role.id}` : `${role.id}`;
      });

      this.matchByParam = `roleids=${selectedRoles}`;
    }

    if (this.eventsService.currentFormValue.value.invitedRegions?.length) {
      let selectedRegions: string;

      this.eventsService.currentFormValue.value.invitedRegions.forEach(region => {
        selectedRegions = selectedRegions ? `${selectedRegions},${region.id}` : `${region.id}`;
      });

      this.matchByParam = `${this.matchByParam}&regionids=${selectedRegions}`;
    }

    if (this.eventsService.currentFormValue.value.invitedCategories?.length) {
      let selectedCategories: string;

      this.eventsService.currentFormValue.value.invitedCategories.forEach(category => {
        selectedCategories = selectedCategories ? `${selectedCategories},${category.id}` : `${category.id}`;
      });

      this.matchByParam = `${this.matchByParam}&categoryids=${selectedCategories}`;
    }

    const eventId = this.event?.id || 0;

    this.httpService
      .get<PaginateResponseInterface<EventUserInterface>>(`${this.apiRoutes.Events}/${eventId}/users`, {
        matchBy: this.matchByParam
      })
      .subscribe(users => {
        this.matchedUsers = users.dataList;
        this.initialMatchedUsers = [...this.matchedUsers];

        this.selectedUsers.forEach(su => {
          this.matchedUsers.forEach(mu => {
            if (su.id === mu.id) {
              mu.selected = true;
            }
          });
        });

        this.matchedUsers.forEach(mu => {
          if (mu.isInvited && !this.selectedUsers.some(su => su.id === mu.id)) {
            this.selectedUsers.push(mu);
          }
        });

        this.selectedUsersUpdated.emit(this.selectedUsers);
        this.allUsersSelected = this.matchedUsers.every(mu => mu.selected);
        this.changeDetRef.detectChanges();

        this.usersRequested = true;
      });
  }

  checkSelectedUsers(): void {
    this.matchedUsers.forEach(mu => {
      this.selectedUsers.forEach(su => {
        if (su.id === mu.id) {
          mu.selected = true;
        }
      });
    });

    this.allUsersSelected = this.matchedUsers.every(mu => mu.selected);
    this.changeDetRef.detectChanges();
  }

  chooseUser(user: EventUserInterface): void {
    user.selected = !user.selected;

    const selectedUserIndex = this.selectedUsers.findIndex(c => c.id === user.id);
    selectedUserIndex >= 0 ? this.selectedUsers.splice(selectedUserIndex, 1) : this.selectedUsers.push(user);

    this.allUsersSelected = this.matchedUsers.every(mu => mu.selected);

    this.selectedUsersUpdated.emit(this.selectedUsers);
    this.changeDetRef.detectChanges();
  }

  removeUser(user: EventUserInterface) {
    this.matchedUsers.forEach(m => {
      if (m.id === user.id) {
        m.selected = false;
      }
    });

    this.selectedUsers = this.selectedUsers.filter(su => su.isInvited || su.id !== user.id);

    this.selectedUsersUpdated.emit(this.selectedUsers);
    this.allUsersSelected = this.matchedUsers.every(mu => mu.selected);
  }

  searchUsers(searchStr: string): void {
    if (!SearchValidatorService.validateSearch(searchStr)) {
      this.snackbarService.showError('general.searchErrorLabel');
      return;
    }
    const eventId = this.event?.id || 0;

    this.httpService
      .get<PaginateResponseInterface<EventUserInterface>>(this.apiRoutes.Events + `/${eventId}/users`, {
        search: searchStr,
        matchBy: this.matchByParam
      })
      .subscribe(users => {
        this.matchedUsers = users.dataList || [];
        this.checkSelectedUsers();
        this.usersRequested = true;
        this.allUsersSelected = this.matchedUsers.every(mu => mu.selected);
      });
  }

  clearAllUsers() {
    this.selectedUsers = this.selectedUsers.filter(su => su.isInvited);

    this.matchedUsers = [
      ...this.matchedUsers.map(c => {
        c.selected = false;
        return c;
      })
    ];

    this.selectedUsersUpdated.emit(this.selectedUsers);
    this.allUsersSelected = false;
  }

  resetUsersList(): void {
    this.initialMatchedUsers = [];
    this.matchedUsers = this.matchedUsers?.filter(mu => mu.isInvited) ?? [];
    this.changeDetRef.detectChanges();
  }

  anySelectedUser(): boolean {
    return this.selectedUsers.filter(su => su.selected && !su.isInvited)?.length > 0;
  }

  selectAllMatchingUsers() {
    this.matchedUsers.forEach(user => {
      if (!user.isInvited) {
        user.selected = true;

        if (!this.selectedUsers.find(su => user.id === su.id)) {
          this.selectedUsers.push({ ...user });
        }

        this.changeDetRef.detectChanges();
      }
    });

    this.selectedUsersUpdated.emit(this.selectedUsers);
    this.allUsersSelected = true;
  }

  initSelectedUsers(value: CreateEventInterface): void {
    this.selectedUsers =
      value.invitedUsers?.map(
        iu =>
          ({
            id: iu.id,
            name: iu.name,
            isInvited: iu.isInvited,
            selected: iu.selected,
            company: iu.company
          } as EventUserInterface)
      ) || [];
  }
}
