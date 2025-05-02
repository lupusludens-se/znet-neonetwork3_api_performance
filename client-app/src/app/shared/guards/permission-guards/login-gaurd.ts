import { CanActivate, CanLoad, Router } from '@angular/router';
import { Injectable } from '@angular/core';

import { AuthService } from '../../../core/services/auth.service';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { UserInterface } from '../../interfaces/user/user.interface';
import { PermissionService } from 'src/app/core/services/permission.service';
import { Subject, takeUntil } from 'rxjs';
import { SpinnerService } from 'src/app/core/services/spinner.service';

@Injectable({
  providedIn: 'root'
})
export class LoginGaurd implements CanActivate {
  constructor(private router: Router, private authService: AuthService, private readonly spinnerService: SpinnerService) { }
  private unsubscribe$: Subject<void> = new Subject<void>();
  public hideLoader: boolean = false;


  canActivate(): boolean {
    this.spinnerService.onStarted(null);
    const currentUser: UserInterface = this.authService.currentUser$.getValue();
    const isPublicUser = currentUser == null ? true : false;
    if (isPublicUser) {
      this.authService.loginRedirect();
    }
    else {
      this.router.navigateByUrl('/dashboard');
    }
    return isPublicUser;
  }
}
