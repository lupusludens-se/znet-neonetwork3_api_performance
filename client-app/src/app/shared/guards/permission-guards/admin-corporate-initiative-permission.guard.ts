import { CanActivate, Router } from '@angular/router';
import { Injectable } from '@angular/core';

import { AuthService } from '../../../core/services/auth.service';
import { UserInterface } from '../../interfaces/user/user.interface';
import { PermissionService } from 'src/app/core/services/permission.service';
import { RolesEnum } from '../../enums/roles.enum';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';

@Injectable({
  providedIn: 'root'
})
export class AdminCorporateInitiativePermissionGuard implements CanActivate {
  userRoles = RolesEnum;
  constructor(private router: Router, private authService: AuthService, private permissionService: PermissionService) {}

  canActivate(): boolean {
    const currentUser: UserInterface = this.authService.currentUser$.getValue();

    let hasPermission: boolean = false;

    if (!currentUser) {
      const allPermissions = JSON.parse(localStorage.getItem(this.authService.currentUserKey));

      if (!allPermissions) {
        this.authService.loginRedirect();
        return false;
      }

      hasPermission =
        this.permissionService.hasPermission(allPermissions, PermissionTypeEnum.InitiativeManageOwn) ||
        this.permissionService.hasPermission(allPermissions, PermissionTypeEnum.InitiativeManageAll);
    } else {
      hasPermission =
        this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.InitiativeManageOwn) ||
        this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.InitiativeManageAll);
    }

    if (!hasPermission) this.router.navigate(['403']);

    return hasPermission;
  }
}
