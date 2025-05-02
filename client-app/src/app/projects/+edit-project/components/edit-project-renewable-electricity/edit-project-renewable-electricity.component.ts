import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';

import { ProjectRenewableElectricityDetailsInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { ProjectInterface } from '../../../../shared/interfaces/projects/project.interface';
import { ProjectApiRoutes } from '../../../shared/constants/project-api-routes.const';
import { TagInterface } from '../../../../core/interfaces/tag.interface';

@Component({
  selector: 'neo-edit-project-renewable-electricity',
  templateUrl: 'edit-project-renewable-electricity.component.html',
  styleUrls: ['../../edit-project.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class EditProjectRenewableElectricityComponent implements OnInit {
  @Input() project: ProjectInterface;
  @Input() formSubmitted: boolean;
  purchaseOptions: TagInterface[] = [
    { name: 'Behind the Meter', id: 1 },
    { name: 'In Front of the Meter', id: 2 }
  ];
  valueProvidedList: TagInterface[] = [
    { name: 'Cost Savings', id: 1 },
    { name: 'Renewable Attributes', id: 6 },
    { name: 'Other', id: 5 }
  ];
  apiRoutes = ProjectApiRoutes;

  constructor(public controlContainer: ControlContainer) {}

  ngOnInit(): void {
    const projectDetails: ProjectRenewableElectricityDetailsInterface = this.project
      .projectDetails as ProjectRenewableElectricityDetailsInterface;

    this.controlContainer.control.patchValue({
      minimumAnnualSiteKWh: projectDetails.minimumAnnualSiteKWh,
      purchaseOptions: projectDetails.purchaseOptions,
      minimumTermLength: projectDetails.minimumTermLength,
      valuesProvided: projectDetails.valuesProvided,
      timeAndUrgencyConsiderations: projectDetails.timeAndUrgencyConsiderations,
      additionalComments: projectDetails.additionalComments
    });

    if (projectDetails.purchaseOptions) {
      projectDetails.purchaseOptions.forEach(po => {
        this.purchaseOptions.map(v => {
          if (v.id === po.id) {
            v.selected = true;
          }
        });
      });
    }

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

  choosePurchaseOptions(val: TagInterface, unselect: boolean) {
    if (unselect) {
      const updatedVal: TagInterface[] = [...this.controlContainer.control.value.purchaseOptions].filter(
        o => o.id !== val.id
      );
      this.controlContainer.control.patchValue({
        purchaseOptions: [...updatedVal]
      });
    } else {
      this.controlContainer.control.patchValue({
        purchaseOptions: [...this.controlContainer.control.value.purchaseOptions, val]
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
