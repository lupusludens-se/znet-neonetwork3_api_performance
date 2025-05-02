import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

import { Observable, tap } from 'rxjs';

import { HttpService } from '../../../core/services/http.service';
import { TranslateService } from '@ngx-translate/core';
import { SnackbarService } from '../../../core/services/snackbar.service';

import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { EmailAlertApiEnum, EmailAlertFrequencyEnum, EmailAlertInterface } from './models/email-alert';
import { DROPDOWN_OPTIONS } from '../../../shared/constants/email-alert-options.const';

@Component({
  selector: 'neo-email-alerts',
  templateUrl: './email-alerts.component.html',
  styleUrls: ['./email-alerts.component.scss']
})
export class EmailAlertsComponent {
  formGroup: FormGroup = new FormGroup({});
  frequency: FormControl = new FormControl(0);

  emailAlerts: PaginateResponseInterface<EmailAlertInterface>;

  emailAlerts$: Observable<PaginateResponseInterface<EmailAlertInterface>> = this.httpService
    .get<PaginateResponseInterface<EmailAlertInterface>>(EmailAlertApiEnum.EmailAlerts)
    .pipe(
      tap(response => {
        this.emailAlerts = response;
        this.createControls();
      })
    );

  private frequencies: { id: number; frequency: EmailAlertFrequencyEnum }[] = [];

  constructor(
    private readonly httpService: HttpService,
    private readonly translateService: TranslateService,
    private readonly router: Router,
    private readonly snackbarService: SnackbarService
  ) {}

  updateSetting(alertSetting: EmailAlertInterface, frequency: TagInterface): void {
    const frequencyIndex = this.frequencies.findIndex(fr => fr.id === alertSetting.id);

    if (frequencyIndex >= 0) {
      this.frequencies[frequencyIndex].frequency = frequency.id;
    } else {
      this.frequencies.push({ id: alertSetting.id, frequency: frequency.id });
    }
  }

  getControlName(title: string): string {
    return title.toLowerCase().replace(/\s/g, '');
  }

  getOptionName(frequency: EmailAlertFrequencyEnum): string {
    switch (frequency) {
      case EmailAlertFrequencyEnum.Off:
        return this.translateService.instant('settings.offLabel');
      case EmailAlertFrequencyEnum.Immediately:
        return this.translateService.instant('settings.immediatelyLabel');
      case EmailAlertFrequencyEnum.Daily:
        return this.translateService.instant('settings.dailyLabel');
      case EmailAlertFrequencyEnum.Weekly:
        return this.translateService.instant('settings.weeklyLabel');
      case EmailAlertFrequencyEnum.Monthly:
        return this.translateService.instant('settings.monthlyLabel');
    }
  }

  goBack(): void {
    this.router.navigate(['../']).then();
  }

  saveData(): void {
    this.httpService
      .put<unknown>(EmailAlertApiEnum.EmailAlerts, { emailAlertsData: this.frequencies })
      .subscribe(() => this.snackbarService.showSuccess('settings.emailAlertsSavedLabel'));
  }

  dropdownOptions(title: string): TagInterface[] {
    const options = DROPDOWN_OPTIONS.map(option => ({
      ...option,
      name: this.translateService.instant(option.name)
    }));

    if (title.toLowerCase().includes('summary')) {
      options.splice(1, 1);
    }

    return options;
  }

  private createControls(): void {
    this.emailAlerts.dataList.forEach(setting =>
      this.formGroup.addControl(
        this.getControlName(setting?.title),
        new FormControl({ id: setting.frequency, name: this.getOptionName(setting.frequency) })
      )
    );
  }
}
