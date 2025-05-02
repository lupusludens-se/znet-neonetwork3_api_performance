import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SVG_CONFIG } from '@ngneat/svg-icon/lib/types';

import { ActivityService } from '../../../../core/services/activity.service';

import { FirstClickInfoActivityDetailsInterface } from '../../../../core/interfaces/activity-details/first-click-info-activity-details.interface';

import { ActivityTypeEnum } from '../../../../core/enums/activity/activity-type.enum';
import { DashboardClickElementActionTypeEnum } from '../../../../core/enums/activity/dashboard-click-element-action-type.enum';

@Component({
  selector: 'neo-title-section',
  templateUrl: './title-section.component.html',
  styleUrls: ['./title-section.component.scss']
})
export class TitleSectionComponent {
  @Input() title: string;
  @Input() subTitle: string;
  @Input() linkText: string;
  @Input() link: string;
  @Input() icon: string = 'right-arrow';
  @Input() iconPosition: 'left' | 'right' = 'right';
  @Input() iconSize: keyof SVG_CONFIG['sizes'] = 'xs';
  @Input() queryParams: Record<string, string | number>;

  @Output() elementClick: EventEmitter<FirstClickInfoActivityDetailsInterface> =
    new EventEmitter<FirstClickInfoActivityDetailsInterface>();
  @Output() buttonClick: EventEmitter<void> = new EventEmitter<void>();

  constructor(private readonly activityService: ActivityService) {}

  onButtonClick(): void {
    if (this.link === '/learn') {
      this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.LearnViewAll });
      this.activityService
        .trackElementInteractionActivity(ActivityTypeEnum.ViewAllClick, { viewAllType: 'Learn' })
        ?.subscribe();
    } else if (this.link === '/forum') {
      this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.ForumsViewAll });
      this.activityService
        .trackElementInteractionActivity(ActivityTypeEnum.ViewAllClick, { viewAllType: 'Forum' })
        ?.subscribe();
    } else if (this.link === '/community') {
      this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.CompaniesViewAll });
      this.activityService
        .trackElementInteractionActivity(ActivityTypeEnum.ViewAllClick, { viewAllType: 'Company' })
        ?.subscribe();
    }

    if (this.linkText && !this.link) {
      this.buttonClick.emit();
    }
  }
}
