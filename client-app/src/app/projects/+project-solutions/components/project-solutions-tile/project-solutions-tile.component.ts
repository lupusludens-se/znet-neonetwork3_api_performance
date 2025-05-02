import { Component, Input } from '@angular/core';
import { SolutionsResourcesInterface } from '../../interfaces/solution.interface';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';

@Component({
  selector: 'neo-project-solutions-tile',
  templateUrl: './project-solutions-tile.component.html',
  styleUrls: ['./project-solutions-tile.component.scss']
})
export class ProjectSolutionsTileComponent {
  @Input() solution: SolutionsResourcesInterface;

  constructor(private readonly activityService: ActivityService) {}

  onSolutionClick(id: number): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.SolutionDetailsView, { solutionId: id })
      ?.subscribe();
  }
}
