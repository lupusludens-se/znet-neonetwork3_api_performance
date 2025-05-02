import { ProjectInterface } from '../../../../shared/interfaces/projects/project.interface';
import { ControlContainer, FormGroupDirective, Validators } from '@angular/forms';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import * as dayjs from 'dayjs';

import { ProjectCommunitySolarDetailsInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { RadioControlInterface } from '../../../../shared/modules/radio-control/radio-control.component';
import { ProjectApiRoutes } from '../../../shared/constants/project-api-routes.const';
import { TagInterface } from '../../../../core/interfaces/tag.interface';

@Component({
  selector: 'neo-edit-project-community-solar',
  templateUrl: 'edit-project-community-solar.component.html',
  styleUrls: ['../../edit-project.component.scss', 'edit-project-community-solar.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class EditProjectCommunitySolarComponent implements OnInit {
  @Output() closeSection: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() project: ProjectInterface;
  @Input() formSubmitted: boolean;
  projectAvailable: RadioControlInterface[] = [
    { name: 'Yes', id: 1 },
    { name: 'No', id: 0 }
  ];
  investmentsOptions: RadioControlInterface[] = [
    { name: 'Yes', id: 1 },
    { name: 'No', id: 0 }
  ];
  structureOptions: TagInterface[] = [
    { name: 'Discount to Tariff', id: 7 },
    { name: 'Other', id: 3 }
  ];
  valueProvidedList: TagInterface[] = [
    { name: 'Cost Savings', id: 1 },
    { name: 'Renewable Attributes', id: 6 },
    { name: 'Visible Commitment to Mitigating Climate Action', id: 8 },
    { name: 'Community Benefits', id: 9 },
    { name: 'Other', id: 5 }
  ];
  apiRoutes = ProjectApiRoutes;

  constructor(public controlContainer: ControlContainer) {}

  ngOnInit(): void {
    const projectDetails: ProjectCommunitySolarDetailsInterface = this.project
      .projectDetails as ProjectCommunitySolarDetailsInterface;

    this.controlContainer.control.patchValue({
      timeAndUrgencyConsiderations: projectDetails.timeAndUrgencyConsiderations,
      additionalComments: projectDetails.additionalComments,
      minimumAnnualMWh: projectDetails.minimumAnnualMWh,
      totalAnnualMWh: projectDetails.totalAnnualMWh,
      utilityTerritory: projectDetails.utilityTerritory,
      projectAvailable: projectDetails.projectAvailable,
      projectAvailabilityApproximateDate: projectDetails.projectAvailabilityApproximateDate
        ? dayjs(projectDetails.projectAvailabilityApproximateDate).format('YYYY-MM-DD')
        : null,
      isInvestmentGradeCreditOfOfftakerRequired:
        projectDetails.isInvestmentGradeCreditOfOfftakerRequired === true
          ? 1
          : projectDetails.isInvestmentGradeCreditOfOfftakerRequired === false
          ? 0
          : null,
      contractStructures: projectDetails.contractStructures,
      minimumTermLength: projectDetails.minimumTermLength,
      valuesProvided: projectDetails.valuesProvided
    });

    if (projectDetails.projectAvailable) {
      this.controlContainer.control.get('projectAvailabilityApproximateDate').clearValidators();
      this.controlContainer.control.get('projectAvailabilityApproximateDate').updateValueAndValidity();
    }

    if (projectDetails.contractStructures) {
      projectDetails.contractStructures.forEach(vto => {
        this.structureOptions.map(v => {
          if (v.id === vto.id) {
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

  chooseProjectAvailability(unselect: boolean) {
    this.controlContainer.control.patchValue({ projectAvailable: !unselect });

    if (unselect) {
      this.controlContainer.control.get('projectAvailabilityApproximateDate')?.setValidators(Validators.required);
      this.controlContainer.control.get('projectAvailabilityApproximateDate')?.updateValueAndValidity();
    } else {
      this.controlContainer.control.patchValue({ projectAvailabilityApproximateDate: null });
      this.controlContainer.control.get('projectAvailabilityApproximateDate')?.clearValidators();
      this.controlContainer.control.get('projectAvailabilityApproximateDate')?.updateValueAndValidity();
    }
  }
}
