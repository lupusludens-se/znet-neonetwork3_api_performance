import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';

@Component({
  selector: 'neo-project-private-details',
  templateUrl: './project-private-details.component.html',
  styleUrls: ['./project-private-details.component.scss']
})
export class ProjectPrivateDetailsComponent {
  @Input() projectDetails: ProjectInterface;
  @Output() closeModal: EventEmitter<void> = new EventEmitter<void>();
  constructor() {}
}
