import { CanActivate, Router, UrlTree } from '@angular/router';
import { Injectable } from '@angular/core';

import { AuthService } from '../../../core/services/auth.service';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { UserInterface } from '../../interfaces/user/user.interface';
import { Observable } from 'rxjs';
import { PermissionService } from 'src/app/core/services/permission.service';

@Injectable({
  providedIn: 'root'
})
export class UserAccessManagementPermissionGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService, private permissionService: PermissionService) {}

  canActivate(): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    const currentUser: UserInterface = this.authService.currentUser$.getValue();

    let hasPermission: boolean = false;

    if (!currentUser) {
      const allPermissions = JSON.parse(localStorage.getItem(this.authService.currentUserKey));

      if (!allPermissions) {
        this.authService.loginRedirect();
        return false;
      }

      hasPermission = this.permissionService.hasPermission(allPermissions, PermissionTypeEnum.UserAccessManagement);
    } else {
      hasPermission = this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.UserAccessManagement);
    }

    if (!hasPermission) this.router.navigate(['403']);

    return hasPermission;
  }
}
