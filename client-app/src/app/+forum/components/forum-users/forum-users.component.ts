import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable, take } from 'rxjs';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { HttpService } from 'src/app/core/services/http.service';
import { PermissionService } from 'src/app/core/services/permission.service';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { UserManagementApiEnum } from 'src/app/user-management/enums/user-management-api.enum';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import structuredClone from '@ungap/structured-clone';

@Component({
  selector: 'neo-forum-users',
  templateUrl: './forum-users.component.html',
  styleUrls: ['./forum-users.component.scss']
})
export class ForumUsersComponent implements OnInit {
  users: UserInterface[] = [];
  @Output() selectedUsersUpdated: EventEmitter<UserInterface[]> = new EventEmitter<UserInterface[]>();
  private usersDuplicateList: UserInterface[] = [];
  permissionTypes = PermissionTypeEnum;
  selectedUsers: UserInterface[];
  @Input() forumUserIds: number[];
  currentUser$: Observable<UserInterface> = this.authService.currentUser();

  constructor(
    public readonly permissionService: PermissionService,
    private readonly authService: AuthService,
    private httpService: HttpService
  ) {}

  ngOnInit(): void {
    this.getUsers();
  }

  filterUsers(search: string = ''): void {
    this.users = this.usersDuplicateList.filter(
      user =>
        user.company.name.toLowerCase().includes(search.toLowerCase()) ||
        user.firstName.toLowerCase().includes(search.toLowerCase()) ||
        user.lastName.toLowerCase().includes(search.toLowerCase())
    );
  }

  private getUsers(): void {
    this.httpService
      .get<PaginateResponseInterface<UserInterface>>(UserManagementApiEnum.Users, {
        expand: 'company,image',
        filterBy: `statusids=${UserStatusEnum.Active}`,
        orderBy: 'lastname.asc'
      })
      .pipe(take(1))
      .subscribe(users => {
        this.selectedUsers = [];
        this.users = users?.dataList || [];
        this.usersDuplicateList = structuredClone(this.users);

        if (this.usersDuplicateList.length) {
          const userIds = this.forumUserIds;
          this.selectedUsers = this.usersDuplicateList?.filter(user => userIds?.some(id => id === user.id));
        }
      });
  }

  setUser(user: UserInterface) {
    const userIndex = this.selectedUsers.findIndex(su => su.id === user.id);

    if (userIndex >= 0) {
      this.selectedUsers.splice(userIndex, 1);
    } else {
      this.selectedUsers.push(user);
    }
    this.selectedUsersUpdated.emit(this.selectedUsers);
  }

  clearSelectedUsers(): void {
    this.selectedUsers = [];
    this.selectedUsersUpdated.emit(this.selectedUsers);
  }

  isChecked(selectedUsers: UserInterface[], user: UserInterface): boolean {
    if (
      selectedUsers.filter(function (e) {
        return e.id === user.id;
      }).length > 0
    ) {
      return true;
    }

    return false;
  }
}
