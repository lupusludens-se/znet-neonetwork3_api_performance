import { Component } from '@angular/core';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';

@Component({
  selector: 'neo-catalog-top-panel',
  templateUrl: './catalog-top-panel.component.html',
  styleUrls: ['./catalog-top-panel.component.scss']
})
export class CatalogTopPanelComponent {
  contactUsModal: boolean;

  constructor(private readonly activityService: ActivityService) {}

  onContactNEONetworkClick(): void {
    this.contactUsModal = true;
    this.activityService.trackElementInteractionActivity(ActivityTypeEnum.ConnectWithNEOClick)?.subscribe();
  }

  onTechnologiesSolutionsClick(name: string): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.TechnologiesSolutionsClick, { buttonName: name })
      ?.subscribe();
  }
}
