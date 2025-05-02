import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

import { HttpService } from '../../../../core/services/http.service';

import { UserInterface } from '../../../../shared/interfaces/user/user.interface';
import { PatchPayloadInterface } from '../../../../shared/interfaces/common/patch-payload.interface';

import { PATCH_PAYLOAD } from '../../../../shared/constants/patch-payload.const';
import { MENU_OPTIONS, MenuOptionInterface } from '../../../../shared/modules/menu/interfaces/menu-option.interface';
import structuredClone from '@ungap/structured-clone';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { UserManagementApiEnum } from 'src/app/user-management/enums/user-management-api.enum';
import { Observable, Subject, catchError, of, takeUntil, throwError } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'neo-table-row',
  templateUrl: 'table-row.component.html',
  styleUrls: ['../sp-users-list.component.scss', 'table-row.component.scss']
})
export class TableRowComponent implements OnInit {
  @Output() updateUsers: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() user: UserInterface;
  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  private unsubscribe$: Subject<void> = new Subject<void>();

  userStatuses = UserStatusEnum;
  showModal: boolean;
  menuOptions: MenuOptionInterface[] = structuredClone(MENU_OPTIONS);

  private patchPayload: PatchPayloadInterface = PATCH_PAYLOAD;

  constructor(
    private router: Router,
    private httpService: HttpService,
    private authService: AuthService,
    private snackbarService: SnackbarService,
    private translateService: TranslateService
  ) {}

  ngOnInit(): void {}

  deleteUserClick(): void {
    this.showModal = true;
  }

  private deactivateUser(): void {
    if (this.user?.statusId === UserStatusEnum.Onboard) {
      this.deleteUser();
      return;
    }
    this.patchPayload.jsonPatchDocument[0].value = UserStatusEnum.Inactive;
    this.patchPayload.jsonPatchDocument[0].op = 'replace';
    this.patchPayload.jsonPatchDocument[0].path = '/StatusId';

    this.httpService
      .patch(UserManagementApiEnum.Users + `/${this.user.id}`, this.patchPayload)
      .pipe(
        takeUntil(this.unsubscribe$),
        catchError(error => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(error);
        })
      )
      .subscribe(() => {
        this.updateUsers.emit(true); // !! should be handled through getUsers service
        this.snackbarService.showSuccess(
          this.translateService.instant('general.inactiveModal.UserDeleteSuccessMessage')
        );
      });
  }

  deleteUser(): void {
    this.httpService
      .delete(UserManagementApiEnum.Users + `/${this.user.id}`)
      .pipe(
        takeUntil(this.unsubscribe$),
        catchError(error => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(error);
        })
      )
      .subscribe(() => {
        this.updateUsers.emit(true);
        this.snackbarService.showSuccess(
          this.translateService.instant('general.inactiveModal.UserDeleteSuccessMessage')
        );
      });
  }
}
