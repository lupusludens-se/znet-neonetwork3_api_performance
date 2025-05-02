import { LocationStrategy } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { PermissionService } from 'src/app/core/services/permission.service';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { SPCompanyProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';

@Component({
  selector: 'neo-company-live-projects-section',
  templateUrl: './company-live-projects-section.component.html',
  styleUrls: ['./company-live-projects-section.component.scss']
})
export class CompanyLiveProjectsSectionComponent {
  @Input() projectsList: SPCompanyProjectInterface[];
  @Input() user: UserInterface;
  type = TaxonomyTypeEnum;
  @Input() companyId: number;
  @Output() removeProjectFromSavedEvent: EventEmitter<SPCompanyProjectInterface> =
    new EventEmitter<SPCompanyProjectInterface>();
  @Output() saveProjectEvent: EventEmitter<SPCompanyProjectInterface> = new EventEmitter<SPCompanyProjectInterface>();
  constructor(
    private permissionService: PermissionService,
    private activityService: ActivityService,
    private locationStrategy: LocationStrategy
  ) {}

  hasPermissionToSave(user: UserInterface): boolean {
    return this.permissionService.userHasPermission(user, PermissionTypeEnum.ProjectCatalogView);
  }

  onProjectClick(projectId: number): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ProjectView, {
        companyId: this.companyId,
        projectId: projectId
      })
      ?.subscribe();
    const getBaseHref = location.origin + this.locationStrategy.getBaseHref();
    const url = getBaseHref + 'projects/' + projectId;
    window.open(url, '_blank');
  }

  saveProject(project: SPCompanyProjectInterface): void {
    if (project?.isSaved) {
      this.removeProjectFromSavedEvent.emit(project);
    } else {
      this.saveProjectEvent.emit(project);
    }
  }
}
