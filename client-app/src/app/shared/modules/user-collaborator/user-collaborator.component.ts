import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { UserCollaboratorInterface } from '../../interfaces/user/user.interface';
import { FormControl } from '@angular/forms';
import { CoreService } from 'src/app/core/services/core.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'neo-user-collaborator',
  templateUrl: './user-collaborator.component.html',
  styleUrls: ['./user-collaborator.component.scss']
})
export class UserCollaboratorComponent implements OnInit {
  @Input() userList: UserCollaboratorInterface[];
  @Input() title: string;
  @Input() subTitle: string;

  @Output() selectedUsersUpdated: EventEmitter<UserCollaboratorInterface[]> = new EventEmitter<UserCollaboratorInterface[]>();

  selectedUsers: UserCollaboratorInterface[] = [];
  allUsers: UserCollaboratorInterface[];
  searchInput: FormControl = new FormControl();

  constructor(
    private snackbarService: SnackbarService,
    private changeDetRef: ChangeDetectorRef,
    private router: Router,
    private coreService: CoreService
  ) {}

  ngOnInit(): void {
    if (this.userList) {
      this.allUsers = [...this.userList];
      this.selectedUsers = this.userList?.filter(u => u.selected) ?? [];
    }
  }

  chooseUser(user: UserCollaboratorInterface): void {
    user.selected = !user.selected;
    const selectedUserIndex = this.selectedUsers.findIndex(c => c.id === user.id);
    selectedUserIndex >= 0 ? this.selectedUsers.splice(selectedUserIndex, 1) : this.selectedUsers.push(user);
    this.selectedUsersUpdated.emit(this.selectedUsers);
    this.changeDetRef.detectChanges();
  }

  removeUser(user: UserCollaboratorInterface) {
    user.selected = !user.selected;
    const selectedUserIndex = this.selectedUsers.findIndex(c => c.id === user.id);
    selectedUserIndex >= 0 ? this.selectedUsers.splice(selectedUserIndex, 1) : this.selectedUsers.push(user);
    this.selectedUsersUpdated.emit(this.selectedUsers);
    this.changeDetRef.detectChanges();
  }

  searchUsers(searchStr: string): void {
    this.userList = searchStr
      ? this.allUsers?.filter(
          c =>
            c.firstName.toLowerCase().includes(searchStr.toLowerCase()) ||
            c.lastName.toLowerCase().includes(searchStr.toLowerCase()) ||
            c.email.toLowerCase().includes(searchStr.toLowerCase())
        )
      : [...this.allUsers];
  }

  clearAllUsers() {
    this.selectedUsers = [];
    this.allUsers.forEach(user => {
      user.selected = false;
    });
    this.selectedUsersUpdated.emit(this.selectedUsers);
  }

  onLinkButtonClick(): void {
    const serializedUrl = `${location.origin}${environment.baseAppUrl}` + 'sign-up';
    this.coreService.copyTextToClipboard(serializedUrl);
    this.snackbarService.showSuccess('general.linkCopiedLabel');
  }
}
