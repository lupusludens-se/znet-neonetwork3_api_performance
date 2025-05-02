import { Component, Input, OnInit } from '@angular/core';
import { ProjectEfficiencyAuditsDetailsInterface } from 'src/app/shared/interfaces/projects/project-creation.interface';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';

@Component({
  selector: 'neo-project-side-panel-efficiency-audit-component',
  templateUrl: './project-side-panel-efficiency-audit-component.component.html',
  styleUrls: ['../project-side-panel/project-side-panel.component.scss']
})
export class ProjectSidePanelEfficiencyAuditComponentComponent implements OnInit {
  @Input() projectDetails: ProjectInterface;
  technologiesList: string;
  contractStructutesList: string;
  valuesProvidedList: string;
  constructor() {}

  ngOnInit(): void {
    const details = this.projectDetails?.projectDetails as ProjectEfficiencyAuditsDetailsInterface;

    this.technologiesList = this.projectDetails?.technologies?.map(t => t.name).join(', ');
    this.contractStructutesList = details.contractStructures?.map(t => t.name).join(', ');
    this.valuesProvidedList = details.valuesProvided?.map(t => t.name).join(', ');
  }
}
