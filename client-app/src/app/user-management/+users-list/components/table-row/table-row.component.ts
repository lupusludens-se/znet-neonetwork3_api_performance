import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

import { HttpService } from '../../../../core/services/http.service';

import { UserInterface } from '../../../../shared/interfaces/user/user.interface';
import { PatchPayloadInterface } from '../../../../shared/interfaces/common/patch-payload.interface';

import { TableCrudEnum } from '../../../../shared/modules/table/enums/table-crud.enum';
import { UserStatusEnum } from '../../../enums/user-status.enum';
import { UserManagementApiEnum } from '../../../enums/user-management-api.enum';

import { PATCH_PAYLOAD } from '../../../../shared/constants/patch-payload.const';
import { MENU_OPTIONS, MenuOptionInterface } from '../../../../shared/modules/menu/interfaces/menu-option.interface';
import structuredClone from '@ungap/structured-clone';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'neo-table-row',
  templateUrl: 'table-row.component.html',
  styleUrls: ['../../users-list.component.scss', 'table-row.component.scss']
})
export class TableRowComponent implements OnInit {
  @Output() updateUsers: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() user: UserInterface;

  userStatuses = UserStatusEnum;
  rolesList = RolesEnum;
  showModal: boolean;
  menuOptions: MenuOptionInterface[] = structuredClone(MENU_OPTIONS);
  currentUser: UserInterface;

  private patchPayload: PatchPayloadInterface = PATCH_PAYLOAD;

  constructor(private router: Router, private httpService: HttpService, private authService: AuthService) { }

  ngOnInit(): void {
    this.authService.currentUser().subscribe(user => {
      if (user) {
        this.currentUser = user;
      }
    });
    this.formatRoles();
  }

  deleteUser(): void {
    this.httpService.delete(UserManagementApiEnum.Users + `/${this.user.id}`).subscribe(() => {
      this.updateUsers.emit(true);
    });
  }

  optionClick(option: MenuOptionInterface): void {
    switch (option.operation) {
      case TableCrudEnum.Edit:
        this.goToUser(this.user.id);
        break;
      case TableCrudEnum.Delete:
        this.showModal = true;
        break;
      case TableCrudEnum.Status:
        this.updateUserStatus(this.user.statusId);
        break;
    }
  }

  getOptions(): MenuOptionInterface[] {
		this.menuOptions = this.menuOptions.map(option => {
			if (this.user?.statusId === UserStatusEnum.Active) {
				this.menuOptions.map(opt => {
					if (opt.name === 'actions.previewLabel') opt.hidden = true;
					if (opt.name === 'actions.deactivateLabel') opt.hidden = false;
					if (opt.name === 'actions.activateLabel') opt.hidden = true;
				});
			} else if (this.user?.statusId === UserStatusEnum.Inactive) {
				this.menuOptions.map(opt => {
					if (opt.name === 'actions.previewLabel') opt.hidden = true;
					if (opt.name === 'actions.deactivateLabel') opt.hidden = true;
					if (opt.name === 'actions.activateLabel') opt.hidden = false;
				});
			} else if (this.user?.statusId === UserStatusEnum.Onboard || this.user?.statusId === UserStatusEnum.Expired) {
				this.menuOptions.map(opt => {
					if (opt.name === 'actions.previewLabel') opt.hidden = true;
					if (opt.name === 'actions.deactivateLabel') opt.hidden = true;
					if (opt.name === 'actions.activateLabel') opt.hidden = true;
				});
			}

			if (option.operation === TableCrudEnum.Delete) {
				option.hidden = this.user.statusId === UserStatusEnum.Deleted;
			}

			if ((this.currentUser?.roles?.some(r => r.id === this.rolesList.Admin) && !this.currentUser?.roles?.some(r => r.id === this.rolesList.SystemOwner)) 
        && (option.operation === TableCrudEnum.Edit || option.name === 'actions.activateLabel' || option.name === 'actions.deactivateLabel' || option.operation === TableCrudEnum.Delete)) {
        option.disabled = this.user.roles?.some(r => r.id === this.rolesList.SystemOwner) ? true : false;
      }

      return option;
    });

    return this.menuOptions;
  }

  private updateUserStatus(status: UserStatusEnum): void {
    this.patchPayload.jsonPatchDocument[0].value =
      status === UserStatusEnum.Active ? UserStatusEnum.Inactive : UserStatusEnum.Active;
    this.patchPayload.jsonPatchDocument[0].op = 'replace';
    this.patchPayload.jsonPatchDocument[0].path = '/StatusId';

    this.httpService.patch(UserManagementApiEnum.Users + `/${this.user.id}`, this.patchPayload).subscribe(() => {
      this.updateUsers.emit(true); // !! should be handled through getUsers service
    });
  }

  private goToUser(id: number): void {
    this.router.navigate([`admin/user-management/edit/${id}`]).then();
  }

  private formatRoles(): void {
    this.user.roles = this.user.roles.filter(r => {
      return r.name !== 'All';
    });

    this.user.roles = this.user.roles.map((r, index) => {
      if (index !== this.user.roles.length - 1) {
        return { ...r, roleName: `${r.name}, ` };
      } else {
        return { ...r };
      }
    });
  }

  isRowDisable(userRow: UserInterface) {
    return userRow.roles.some(role => role.id === RolesEnum.SystemOwner) && (this.currentUser.roles.some(role => role.id === RolesEnum.Admin) && !this.currentUser.roles.some(role => role.id === RolesEnum.SystemOwner));
  }
}
