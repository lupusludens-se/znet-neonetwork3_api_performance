import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { InitiativeToolInterface } from '../../models/initiative-resources.interface';

@Component({
  selector: 'neo-tool-item',
  templateUrl: './tool-item.component.html',
  styleUrls: ['./tool-item.component.scss']
})
export class ToolItemComponent {
  @Input() readonly isSavedTool: boolean = false;
  @Input() initiativeId: number;
  @Input() readonly tool: InitiativeToolInterface;
  @Output() readonly selectedTool = new EventEmitter<number>();

  options: MenuOptionInterface[] = [
    {
      icon: 'trash-can-red',
      name: 'initiative.viewInitiative.deleteSavedContentLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];

  constructor(private readonly activityService: ActivityService) {}

  optionClick(): void {
    this.selectedTool.emit(this.tool.id);
  }

  trackToolActivity(): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ToolClick, {
        toolId: this.tool.id,
        initiativeId: this.initiativeId
      })
      ?.subscribe();
  }

  openTool(): void {
    if (this.isSavedTool) {
      this.trackToolActivity();
      window.open(`tools/${this.tool.id}`, '_blank');
    }
  }
}
