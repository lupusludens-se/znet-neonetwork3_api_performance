import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { DashboardClickElementActionTypeEnum } from 'src/app/core/enums/activity/dashboard-click-element-action-type.enum';
import { FirstClickInfoActivityDetailsInterface } from 'src/app/core/interfaces/activity-details/first-click-info-activity-details.interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { NewTrendingProjectResponse } from 'src/app/shared/interfaces/projects/project.interface';
import { ActivityService } from 'src/app/core/services/activity.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'neo-public-project-carousel',
  templateUrl: './public-project-carousel.component.html',
  styleUrls: ['./public-project-carousel.component.scss']
})
export class PublicProjectCarouselComponent {
  @Input() postData: NewTrendingProjectResponse[];
  @Input() title: string = '';
  @Input() subTitle: string = '';
  @Input() loading: boolean = false;
  @Input() postsCountPerSlide: number = 3;
  @Input() totalSlides: number = 3;
  @Output() forwardClick: EventEmitter<void> = new EventEmitter<void>();
  @Input() recentlyViewed: boolean = false;
  activityEnum = ActivityTypeEnum.FirstDashboardClick;
  projectActionType = DashboardClickElementActionTypeEnum.ProjectDiscoverabilityItemView;

  elementClick$: Subject<FirstClickInfoActivityDetailsInterface> =
    new Subject<FirstClickInfoActivityDetailsInterface>();
  public position: number = 0;
  @Input() circlesArr: number[] = [0, -3, -6];
  constructor(private readonly activityService: ActivityService) {}

  forward(): void {
    this.position = this.position - this.postsCountPerSlide;
    if (this.circlesArr.findIndex(y => y == this.position) == -1) {
      this.position = this.circlesArr[0];
    }
    this.forwardClick.emit();
  }

  backward(): void {
    this.position = this.position + this.postsCountPerSlide;
    if (this.circlesArr.findIndex(y => y == this.position) == -1) {
      this.position = this.circlesArr[this.circlesArr.length - 1];
    }
  }
  onProjectsAllClick(): void {
    this.elementClick$.next({ actionType: DashboardClickElementActionTypeEnum.ProjectView });
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ViewAllClick, { viewAllType: 'Project' })
      ?.subscribe();
  }
}
