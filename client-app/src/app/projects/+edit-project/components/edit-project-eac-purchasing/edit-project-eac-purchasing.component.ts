import { ProjectInterface } from '../../../../shared/interfaces/projects/project.interface';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ControlContainer, FormGroupDirective } from '@angular/forms';

import { ProjectEacPurchasingDetailsInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { ProjectApiRoutes } from '../../../shared/constants/project-api-routes.const';
import { TagInterface } from '../../../../core/interfaces/tag.interface';

@Component({
  selector: 'neo-edit-project-eac-purchasing',
  templateUrl: 'edit-project-eac-purchasing.component.html',
  styleUrls: ['../../edit-project.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class EditProjectEacPurchasingComponent implements OnInit {
  @Output() closeSection: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() project: ProjectInterface;
  @Input() formSubmitted: boolean;
  valueProvidedList: TagInterface[] = [
    { name: 'Renewable Attributes', id: 6 },
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
    const projectDetails: ProjectEacPurchasingDetailsInterface = this.project
      .projectDetails as ProjectEacPurchasingDetailsInterface;

    this.controlContainer.control.patchValue({
      minimumPurchaseVolume: projectDetails.minimumPurchaseVolume,
      stripLengths: projectDetails.stripLengths,
      minimumTermLength: projectDetails.minimumTermLength,
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
      const updatedVal: TagInterface[] = [...this.controlContainer.control.value.stripLengths].filter(
        o => o.id !== val.id
      );

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
        [controlName]: this.controlContainer.control.value[controlName]
          ? [...this.controlContainer.control.value[controlName], val]
          : [val]
      });
    }
  }
}
