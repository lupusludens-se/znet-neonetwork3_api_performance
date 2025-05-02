import { Component, Input, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { DashboardClickElementActionTypeEnum } from 'src/app/core/enums/activity/dashboard-click-element-action-type.enum';
import { FirstClickInfoActivityDetailsInterface } from 'src/app/core/interfaces/activity-details/first-click-info-activity-details.interface';
import { ActivityService } from 'src/app/core/services/activity.service';
@Component({
  selector: 'neo-public-decarbonization-initiatives',
  templateUrl: './public-decarbonization-initiatives.component.html',
  styleUrls: ['./public-decarbonization-initiatives.component.scss']
})
export class PublicDecarbonizationInitiativesComponent implements OnInit {
  @Input() title: string = '';
  @Input() subTitle: string = '';
  @Input() loading: boolean = false;
  elementClick$: Subject<FirstClickInfoActivityDetailsInterface> =
    new Subject<FirstClickInfoActivityDetailsInterface>();
  constructor(private readonly activityService: ActivityService) { }

  ngOnInit(): void {
  }
  onLearnMoreClick() {
    this.elementClick$.next({ actionType: DashboardClickElementActionTypeEnum.Initiatives });
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ViewAllClick, { viewAllType: 'Initiative' })
      ?.subscribe();
  }
}
