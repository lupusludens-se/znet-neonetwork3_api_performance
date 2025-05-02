import { Component, Input, OnInit } from '@angular/core';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { ProjectEmergingTechnologiesDetailsInterface } from 'src/app/shared/interfaces/projects/project-creation.interface';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';

@Component({
  selector: 'neo-project-side-panel-emerging-technology-component',
  templateUrl: './project-side-panel-emerging-technology-component.component.html',
  styleUrls: ['../project-side-panel/project-side-panel.component.scss']
})
export class ProjectSidePanelEmergingTechnologyComponentComponent implements OnInit {
  @Input() projectDetails: ProjectInterface;
  technologiesList: string;
  contractStructutesList: string;
  valuesProvidedList: string;
  energyUnitName: string;
  energyUnits: TagInterface[] = [
    { name: 'KW', id: 1 },
    { name: 'KWh', id: 2 },
    { name: 'MW', id: 3 },
    { name: 'MWh', id: 4 },
    { name: 'Gallons', id: 5 }
  ];

  constructor() {}

  ngOnInit(): void {
    const details = this.projectDetails?.projectDetails as ProjectEmergingTechnologiesDetailsInterface;
    this.energyUnitName = this.energyUnits.filter(s => s.id === details?.energyUnitId)[0].name;
    this.technologiesList = this.projectDetails?.technologies?.map(t => t.name).join(', ');
    this.contractStructutesList = details.contractStructures?.map(t => t.name).join(', ');
    this.valuesProvidedList = details.valuesProvided?.map(t => t.name).join(', ');
  }
}
