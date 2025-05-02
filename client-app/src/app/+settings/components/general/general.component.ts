import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { combineLatest, filter, Subject, takeUntil, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpService } from '../../../core/services/http.service';
import { AuthService } from '../../../core/services/auth.service';
import { SnackbarService } from '../../../core/services/snackbar.service';

import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { TimezoneInterface } from '../../../shared/interfaces/common/timezone.interface';
import { TagInterface } from '../../../core/interfaces/tag.interface';

import { FormControlStatusEnum } from '../../../shared/enums/form-control-status.enum';
import { UserManagementApiEnum } from '../../../user-management/enums/user-management-api.enum';
import { CommonApiEnum } from '../../../core/enums/common-api.enum';
import { UserProfileApiEnum } from '../../../shared/enums/api/user-profile-api.enum';

import { CustomValidator } from '../../../shared/validators/custom.validator';

@Component({
  selector: 'neo-general',
  templateUrl: './general.component.html',
  styleUrls: ['./general.component.scss', '../../settings.component.scss']
})
export class GeneralComponent implements OnInit, OnDestroy {
  showModal: boolean;
  emailCustomError: string;
  currentUser: UserInterface;
  timeZones: TagInterface[];

  formGroup: FormGroup = new FormGroup({
    email: new FormControl(null, [Validators.required, CustomValidator.email, Validators.maxLength(70)]),
    timeZoneId: new FormControl('', Validators.required)
  });

  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly httpService: HttpService,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly snackbarService: SnackbarService
  ) {}

  ngOnInit(): void {
    combineLatest([this.httpService.get<TimezoneInterface[]>(CommonApiEnum.Timezones), this.authService.currentUser()])
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(([timezones, currentUser]) => !!timezones && !!currentUser)
      )
      .subscribe(([timezones, currentUser]) => {
        this.currentUser = currentUser;
        this.timeZones = timezones.map(timezone => ({
          id: timezone.id,
          name: timezone.displayName
        }));

        this.formGroup.patchValue({
          email: this.currentUser.email,
          timeZoneId: this.timeZones.find(item => item.id === this.currentUser.timeZoneId)
        });
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  hasError(controlName: string): boolean {
    const control = this.formGroup?.get(controlName);

    return control?.invalid && control?.touched && control?.dirty;
  }

  updateAccount(): void {
    if (this.formGroup.status === FormControlStatusEnum.Invalid) {
      Object.keys(this.formGroup.value).map(key => {
        this.formGroup.get(key).markAsDirty();
        this.formGroup.get(key).markAsTouched();
      });

      return;
    }

    this.httpService
      .put(`${UserManagementApiEnum.Users}/${this.currentUser.id}`, this.getRequestBody())
      .pipe(
        takeUntil(this.unsubscribe$),
        catchError(error => {
          this.emailCustomError = error?.error?.errors?.email[0];

          if (this.emailCustomError) {
            this.formGroup.get('email').markAsDirty();
            this.formGroup.get('email').markAsTouched();
            this.formGroup.get('email').setErrors(Validators.email);
            this.emailCustomError = 'general.emailExistLabel';
          }

          return throwError(error);
        })
      )
      .subscribe(() => {
        this.snackbarService.showSuccess('settings.generalInfoSavedLabel');
        this.authService.getCurrentUser$.next(true);
      });
  }

  deleteUser(): void {
    this.httpService.delete(UserProfileApiEnum.UsersCurrent).subscribe(() => this.authService.logout());
  }

  cancelClick(): void {
    this.router.navigate(['/']).then();
  }

  private getRequestBody(): unknown {
    return {
      email: this.currentUser.email,
      timeZoneId: this.formGroup.value.timeZoneId.id,
      firstName: this.currentUser.firstName,
      lastName: this.currentUser.lastName,
      username: this.currentUser.userName ? this.currentUser.userName : this.currentUser.email,
      statusId: this.currentUser.statusId,
      companyId: this.currentUser.companyId,
      imageName: this.currentUser.imageName,
      countryId: this.currentUser.countryId,
      heardViaId: this.currentUser.heardViaId,
      roles: this.currentUser.roles.map(role => ({ id: role.id })),
      permissions: this.currentUser.permissions.map(permission => ({
        id: permission.id
      }))
    };
  }
}
