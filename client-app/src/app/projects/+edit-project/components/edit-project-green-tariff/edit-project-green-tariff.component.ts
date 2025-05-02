import { Component, Input, OnInit } from '@angular/core';
import { ControlContainer, FormGroupDirective } from '@angular/forms';

import { ProjectGreenTariffDetailsInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { ProjectInterface } from '../../../../shared/interfaces/projects/project.interface';
import { ProjectApiRoutes } from '../../../shared/constants/project-api-routes.const';
import { TagInterface } from '../../../../core/interfaces/tag.interface';

@Component({
  selector: 'neo-edit-project-green-tariff',
  templateUrl: 'edit-project-green-tariff.component.html',
  styleUrls: ['../../edit-project.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class EditProjectGreenTariffComponent implements OnInit {
  @Input() project: ProjectInterface;
  @Input() formSubmitted: boolean;

  valueProvidedList: TagInterface[] = [
    { name: 'Cost Savings', id: 1 },
    { name: 'Renewable Attributes', id: 6 },
    { name: 'Other', id: 5 }
  ];

  termLength: TagInterface[] = [
    { name: '12 months', id: 1 },
    { name: '24 months', id: 2 },
    { name: '36 months', id: 3 },
    { name: '>36 months', id: 4 }
  ];

  apiRoutes = ProjectApiRoutes;

  constructor(public controlContainer: ControlContainer) {}

  ngOnInit(): void {
    const projectDetails: ProjectGreenTariffDetailsInterface = this.project
      .projectDetails as ProjectGreenTariffDetailsInterface;

    this.controlContainer.control.patchValue({
      utilityName: projectDetails.utilityName,
      programWebsite: projectDetails.programWebsite,
      minimumPurchaseVolume: projectDetails.minimumPurchaseVolume,
      termLengthId: projectDetails.termLengthId,
      valuesProvided: projectDetails.valuesProvided,
      timeAndUrgencyConsiderations: projectDetails.timeAndUrgencyConsiderations,
      additionalComments: projectDetails.additionalComments
    });

    if (projectDetails.valuesProvided) {
      projectDetails.valuesProvided.forEach(vp => {
        this.valueProvidedList.map(vpl => {
          if (vpl.id === vp.id) {
            vpl.selected = true;
          }
        });
      });
    }
  }

  chooseCheckboxControlVal(val: TagInterface, unselect: boolean, controlName: string) {
    if (unselect) {
      const updatedVal: number[] = [...this.controlContainer.control.value[controlName]].filter(o => o.id !== val.id);

      this.controlContainer.control.patchValue({
        [controlName]: [...updatedVal]
      });
    } else {
      this.controlContainer.control.patchValue({
        [controlName]: this.controlContainer.control.value[controlName]
          ? [...this.controlContainer.control.value[controlName], val]
          : [val]
      });
    }
  }
}
