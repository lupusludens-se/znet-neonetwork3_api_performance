import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ControlContainer, FormGroupDirective } from '@angular/forms';

import { ProjectEfficiencyAuditsDetailsInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { RadioControlInterface } from '../../../../shared/modules/radio-control/radio-control.component';
import { ProjectInterface } from '../../../../shared/interfaces/projects/project.interface';
import { ProjectApiRoutes } from '../../../shared/constants/project-api-routes.const';
import { TagInterface } from '../../../../core/interfaces/tag.interface';

@Component({
  selector: 'neo-edit-project-efficiency-audit',
  templateUrl: 'edit-project-efficiency-audit.component.html',
  styleUrls: ['../../edit-project.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class EditProjectEfficiencyAuditComponent implements OnInit {
  @Output() closeSection: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() project: ProjectInterface;
  @Input() formSubmitted: boolean;
  contractStructure: TagInterface[] = [
    { name: 'Cash Purchase', id: 1 },
    { name: 'As-a-Service or Alternative Financing', id: 8 },
    { name: 'Shared Savings', id: 5 },
    { name: 'Guaranteed Savings', id: 6 }
  ];
  requiresInvestmentOptions: RadioControlInterface[] = [
    { name: 'Yes', id: 1 },
    { name: 'No', id: 0 }
  ];
  valueProvidedList: TagInterface[] = [
    { name: 'Cost Savings', id: 1 },
    { name: 'Energy Savings/Reduction', id: 10 },
    { name: 'Resiliency', id: 4 },
    { name: 'Other', id: 5 }
  ];
  apiRoutes = ProjectApiRoutes;

  constructor(public controlContainer: ControlContainer) {}

  ngOnInit(): void {
    const projectDetails: ProjectEfficiencyAuditsDetailsInterface = this.project
      .projectDetails as ProjectEfficiencyAuditsDetailsInterface;

    this.controlContainer.control.patchValue({
      contractStructures: projectDetails.contractStructures,
      isInvestmentGradeCreditOfOfftakerRequired: projectDetails.isInvestmentGradeCreditOfOfftakerRequired ? 1 : 0,
      minimumTermLength: projectDetails.minimumTermLength,
      valuesProvided: projectDetails.valuesProvided,
      timeAndUrgencyConsiderations: projectDetails.timeAndUrgencyConsiderations,
      additionalComments: projectDetails.additionalComments
    });

    if (projectDetails.contractStructures) {
      projectDetails.contractStructures.forEach(vto => {
        this.contractStructure.map(v => {
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
}
