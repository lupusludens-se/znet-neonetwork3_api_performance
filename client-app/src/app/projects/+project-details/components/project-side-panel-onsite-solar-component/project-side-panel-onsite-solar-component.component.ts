import { Component, Input, OnInit } from '@angular/core';
import { ProjectOnsiteSolarDetailsInterface } from 'src/app/shared/interfaces/projects/project-creation.interface';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';

@Component({
  selector: 'neo-project-side-panel-onsite-solar-component',
  templateUrl: './project-side-panel-onsite-solar-component.component.html',
  styleUrls: ['../project-side-panel/project-side-panel.component.scss']
})
export class ProjectSidePanelOnsiteSolarComponentComponent implements OnInit {
  @Input() projectDetails: ProjectInterface;
  technologiesList: string;
  contractStructutesList: string;
  valuesProvidedList: string;
  constructor() {}

  ngOnInit(): void {
    const details = this.projectDetails?.projectDetails as ProjectOnsiteSolarDetailsInterface;

    this.technologiesList = this.projectDetails?.technologies?.map(t => t.name).join(', ');
    this.contractStructutesList = details.contractStructures?.map(t => t.name).join(', ');
    this.valuesProvidedList = details.valuesProvided?.map(t => t.name).join(', ');
  }
}
