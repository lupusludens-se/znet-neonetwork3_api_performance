import { Pipe, PipeTransform } from '@angular/core';

import { TranslateService } from '@ngx-translate/core';
import { DatePipe } from '@angular/common';
import * as dayjs from 'dayjs';

@Pipe({
  name: 'timeAgo',
  pure: true
})
export class TimeAgoPipe implements PipeTransform {
  constructor(private readonly translateService: TranslateService, private readonly datePipe: DatePipe) { }

  transform(value: Date | string, format?: any): string | Date {
    if (!value) return;

    const currentDate = dayjs(new Date());
    const date = dayjs(value + 'Z');
    const minutes = currentDate.diff(date, 'minute');
    const hours = currentDate.diff(date, 'hour');

    if (minutes < 1) {
      return this.translateService.instant('general.justNowLabel');
    } else if (minutes <= 60) {
      return (
        minutes + this.translateService.instant(minutes === 1 ? 'general.minuteAgoLabel' : 'general.minutesAgoLabel')
      );
    } else if (hours <= 24) {
      return hours + this.translateService.instant(hours === 1 ? 'general.hourAgoLabel' : 'general.hoursAgoLabel');
    } 

    return this.datePipe.transform(value, format ? format : 'M/d/yy, h:mm a').toLowerCase();
  }
}
