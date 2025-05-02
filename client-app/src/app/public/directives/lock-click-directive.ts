import { Directive, HostListener, Input, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { ThankYouPopupServiceService } from '../services/thank-you-popup-service.service';
import { ActivityService } from 'src/app/core/services/activity.service';

import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';

@Directive({
  selector: '[neoLockClick]'
})
export class LockClickDirective {
  @Input('src') source: ActivityTypeEnum;
  @Input('data') data?: ActivityTypeEnum;
  @Output() lockClicked: EventEmitter<any> = new EventEmitter();

  constructor(
    private readonly thankYouPopupServiceService: ThankYouPopupServiceService,
    private readonly activityService: ActivityService
  ) {}

  @HostListener('click', ['$event.target'])
  onClick(target: any) {
    if (target.classList.contains('lock-icon') || target.closest('.lock-icon')) {
      if (this.source) {
        this.activityService.trackElementInteractionActivity(this.source, this.data)?.subscribe();
      }
      this.thankYouPopupServiceService.showStatus$.next(true);
    }
  }
}
