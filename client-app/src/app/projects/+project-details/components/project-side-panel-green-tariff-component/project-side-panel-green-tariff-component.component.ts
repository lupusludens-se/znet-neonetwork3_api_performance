import { Component, Input, OnInit } from '@angular/core';
import { ProjectGreenTariffDetailsInterface } from 'src/app/shared/interfaces/projects/project-creation.interface';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';

@Component({
  selector: 'neo-project-side-panel-green-tariff-component',
  templateUrl: './project-side-panel-green-tariff-component.component.html',
  styleUrls: ['../project-side-panel/project-side-panel.component.scss']
})
export class ProjectSidePanelGreenTariffComponentComponent implements OnInit {
  @Input() projectDetails: ProjectInterface;
  technologiesList: string;
  valuesProvidedList: string;
  constructor() {}

  ngOnInit(): void {
    const details = this.projectDetails?.projectDetails as ProjectGreenTariffDetailsInterface;

    this.technologiesList = this.projectDetails?.technologies?.map(t => t.name).join(', ');
    this.valuesProvidedList = details.valuesProvided?.map(t => t.name).join(', ');
  }
}
