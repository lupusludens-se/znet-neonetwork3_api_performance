import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { AuthService } from '../../../core/services/auth.service';

import { UserStatusEnum } from '../../../user-management/enums/user-status.enum';

@UntilDestroy()
@Component({
  selector: 'neo-complete',
  templateUrl: './complete.component.html',
  styleUrls: ['../../sign-up.component.scss','./complete.component.scss']
})
export class CompleteComponent implements OnInit {
  termOfUseModal: boolean;

  constructor(private readonly authService: AuthService, private readonly router: Router) {}

  ngOnInit(): void {
    this.authService
      .currentUser()
      .pipe(untilDestroyed(this))
      .subscribe(user => {
        if (user) {
          if (user.statusId === UserStatusEnum.Active) {
            this.router.navigate(['/dashboard']);
          }
        } else if (AuthService.needSilentLogIn()) {
          this.authService.loginRedirect();
        }
      });
  }

  login(): void {
    this.authService.loginRedirect();
  }
}
