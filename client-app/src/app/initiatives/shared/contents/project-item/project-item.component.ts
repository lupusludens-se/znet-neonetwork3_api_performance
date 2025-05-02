import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { InitiativeProjectInterface } from '../../models/initiative-resources.interface';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
@Component({
  selector: 'neo-project-item',
  templateUrl: './project-item.component.html',
  styleUrls: ['./project-item.component.scss'] 
})
export class ProjectItemComponent {
  showDeleteModal: boolean = false;
  @Output() selectedProject = new EventEmitter<number>();
  @Input() project: InitiativeProjectInterface;
  @Input() initiativeId: number;
  @Input() readonly isSavedProject: boolean = false;
  type = TaxonomyTypeEnum;
  constructor(private router: Router, private readonly activityService: ActivityService) {}

  options: MenuOptionInterface[] = [
    {
      icon: 'trash-can-red',
      name: 'initiative.viewInitiative.deleteSavedContentLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];

  optionClick(): void {
    this.selectedProject.emit(this.project.id);
  }

  openProject(): void {
    if (this.isSavedProject) {
      this.trackProjectActivity();
      window.open(`projects/${this.project.id}`, '_blank');
    }
  }

  trackProjectActivity(): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ProjectView, {
        projectId: this.project.id,
        initiativeId: this.initiativeId
      })
      ?.subscribe();
  }
}
