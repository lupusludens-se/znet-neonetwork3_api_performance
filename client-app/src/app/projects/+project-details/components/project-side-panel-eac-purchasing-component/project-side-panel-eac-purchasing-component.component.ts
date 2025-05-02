import { Component, Input, OnInit } from '@angular/core';
import { ProjectEacPurchasingDetailsInterface } from 'src/app/shared/interfaces/projects/project-creation.interface';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';

@Component({
  selector: 'neo-project-side-panel-eac-purchasing-component',
  templateUrl: './project-side-panel-eac-purchasing-component.component.html',
  styleUrls: ['../project-side-panel/project-side-panel.component.scss']
})
export class ProjectSidePanelEacPurchasingComponentComponent implements OnInit {
  @Input() projectDetails: ProjectInterface;
  technologiesList: string;
  valuesProvidedList: string;
  stripLengthList: string;
  constructor() {}

  ngOnInit(): void {
    const details = this.projectDetails?.projectDetails as ProjectEacPurchasingDetailsInterface;

    this.technologiesList = this.projectDetails?.technologies?.map(t => t.name).join(', ');
    this.valuesProvidedList = details.valuesProvided?.map(t => t.name).join(', ');
    this.stripLengthList = details.stripLengths?.map(t => t.name).join(', ');
  }
}
