import { Component, Input, OnInit } from '@angular/core';
import { ProjectBatteryStorageDetailsInterface } from 'src/app/shared/interfaces/projects/project-creation.interface';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';

@Component({
  selector: 'neo-project-side-panel-battery-storage-component',
  templateUrl: './project-side-panel-battery-storage-component.component.html',
  styleUrls: ['../project-side-panel/project-side-panel.component.scss']
})
export class ProjectSidePanelBatteryStorageComponentComponent implements OnInit {
  @Input() projectDetails: ProjectInterface;
  technologiesList: string;
  valuesProvidedList: string;

  constructor() {}

  ngOnInit(): void {
    const details = this.projectDetails?.projectDetails as ProjectBatteryStorageDetailsInterface;

    this.technologiesList = this.projectDetails?.technologies?.map(t => t.name).join(', ');
    this.valuesProvidedList = details.valuesProvided?.map(t => t.name).join(', ');
  }
}
