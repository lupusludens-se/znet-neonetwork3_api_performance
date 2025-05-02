import { CanLoad, Router } from '@angular/router';
import { Injectable } from '@angular/core';

import { UserStatusEnum } from '../../user-management/enums/user-status.enum';
import { AuthService } from '../../core/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class UserProfileGuard implements CanLoad {
  userStatuses = UserStatusEnum;

  constructor(private router: Router, private authsService: AuthService) {}

  canLoad(): any {
    const userStatus: number = this.authsService.currentUser$.getValue()?.statusId;
    if (userStatus === this.userStatuses.Active) return true;
    else {
      this.authsService.loginRedirect();
      return false;
    }
  }
}
