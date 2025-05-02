import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { TaxonomyTypeEnum } from '../../../../shared/enums/taxonomy-type.enum';
import { SaveContentService } from '../../../../shared/services/save-content.service';

@Component({
  selector: 'neo-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent {
  @Input() image: string;
  @Input() title: string;
  @Input() tags: TagInterface[];
  @Input() projectId: number;
  @Input() isSavedProject: boolean;

  @Output() projectClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() tagClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() saveClick: EventEmitter<boolean> = new EventEmitter<boolean>();

  type = TaxonomyTypeEnum;
  constructor(private readonly saveContentService: SaveContentService) {}

  saveProject(): void {
    if (!this.isSavedProject) {
      this.saveContentService.saveProject(this.projectId).subscribe(() => {
        this.isSavedProject = true;
        this.saveClick.emit(true);
      });
    } else {
      this.saveContentService.deleteProject(this.projectId).subscribe(() => {
        this.isSavedProject = false;
        this.saveClick.emit(false);
      });
    }
  }
}
