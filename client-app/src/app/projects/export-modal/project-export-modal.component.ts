import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ProjectCatalogService } from '../+projects/services/project-catalog.service';


@Component({
  selector: 'neo-project-export-modal',
  templateUrl: 'project-export-modal.component.html',
  styleUrls: ['project-export-modal.component.scss']
})
export class ExportProjectModalComponent {
  @Input() projectCount: number;
  @Input() params: string;
  @Output() closeModal = new EventEmitter<boolean>();
  fileIsReady: boolean;
  progress: boolean;
  fileData: string;
  fileName: string;
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(public projectsService: ProjectCatalogService) {}

  requestFile(): void {
    this.progress = true;

    this.projectsService
      .exportProjects()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(ProjectsData => {
        this.fileIsReady = true;
        this.fileData = ProjectsData.fileData;
        this.fileName = ProjectsData.fileName;
        this.progress = false;
      });
  }

  downloadFile(): void {
    const blob = new Blob([this.fileData]);
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = this.fileName;
    link.click();
    this.closeModal.emit(true);
  }
}
