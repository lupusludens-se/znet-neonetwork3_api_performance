import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { ProjectCarbonOffsetDetailsInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { ProjectInterface } from '../../../../shared/interfaces/projects/project.interface';
import { ProjectApiRoutes } from '../../../shared/constants/project-api-routes.const';
import { TagInterface } from '../../../../core/interfaces/tag.interface';

@Component({
  selector: 'neo-edit-project-carbon-offset',
  templateUrl: 'edit-project-carbon-offset.component.html',
  styleUrls: ['../../edit-project.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class EditProjectCarbonOffsetComponent implements OnInit {
  @Output() closeSection: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() project: ProjectInterface;
  @Input() formSubmitted: boolean;
  valueProvidedList: TagInterface[] = [
    { name: 'Greenhouse Gas Emission Reduction Offset', id: 11 },
    { name: 'Other', id: 5 }
  ];
  stripLengthList: TagInterface[] = [
    { name: '12 months', id: 1 },
    { name: '24 months', id: 2 },
    { name: '36 months', id: 3 },
    { name: '>36 months', id: 4 }
  ];
  apiRoutes = ProjectApiRoutes;

  constructor(public controlContainer: ControlContainer) {}

  ngOnInit(): void {
    const projectDetails: ProjectCarbonOffsetDetailsInterface = this.project
      .projectDetails as ProjectCarbonOffsetDetailsInterface;

    this.controlContainer.control.patchValue({
      minimumPurchaseVolume: projectDetails.minimumPurchaseVolume,
      stripLengths: projectDetails.stripLengths,
      valuesProvided: projectDetails.valuesProvided,
      timeAndUrgencyConsiderations: projectDetails.timeAndUrgencyConsiderations,
      additionalComments: projectDetails.additionalComments
    });

    if (projectDetails.stripLengths) {
      projectDetails.stripLengths.forEach(sl => {
        this.stripLengthList.map(v => {
          if (v.id === sl.id) {
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

  chooseStripLength(val: TagInterface, unselect: boolean) {
    if (unselect) {
      const updatedVal: number[] = [...this.controlContainer.control.value.stripLengths].filter(o => o.id !== val.id);

      this.controlContainer.control.patchValue({
        stripLengths: [...updatedVal]
      });
    } else {
      this.controlContainer.control.patchValue({
        stripLengths: [...this.controlContainer.control.value.stripLengths, val]
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
        [controlName]: this.controlContainer.value[controlName]
          ? [...this.controlContainer.value[controlName], val]
          : [val]
      });
    }
  }
}
