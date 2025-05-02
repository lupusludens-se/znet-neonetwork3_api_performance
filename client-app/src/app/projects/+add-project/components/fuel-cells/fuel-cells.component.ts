import { ControlContainer, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

import { ProjectFuelCellsDetailsInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { AddProjectService } from '../../services/add-project.service';
import { ProjectTypesSteps } from '../../enums/project-types-name.enum';
import { INT32_MAX } from 'src/app/shared/constants/math.const';

@Component({
  selector: 'neo-fuel-cells',
  templateUrl: './fuel-cells.component.html',
  styleUrls: ['../../add-project.component.scss']
})
export class FuelCellsComponent implements OnInit {
  stepsList = AddProjectStepsEnum;
  projectTypes = ProjectTypesSteps;
  form: FormGroup = this.formBuilder.group({
    minimumAnnualSiteKWh: ['', [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]],
    contractStructures: ['', Validators.required],
    minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
    valuesProvided: ['', Validators.required],
    timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
    additionalComments: ['', Validators.maxLength(200)]
  });
  contractStructure: TagInterface[] = [
    { name: 'PPA', id: 2 },
    { name: 'Lease', id: 4 },
    { name: 'Cash Purchase', id: 1 },
    { name: 'Shared Savings ', id: 5 },
    { name: 'Guaranteed Savings ', id: 6 },
    { name: 'Other Financing Structures  ', id: 3 }
  ];
  valueProvidedList: TagInterface[] = [
    { name: 'Cost Savings', id: 1 },
    { name: 'Renewable Attributes', id: 6 },
    { name: 'Resiliency', id: 4 },
    { name: 'Other', id: 5 }
  ];
  showDraftModal: boolean;
  formSubmitted: boolean;

  constructor(
    public addProjectService: AddProjectService,
    public controlContainer: ControlContainer,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    if (this.addProjectService.currentFlowData?.projectDetails) {
      const formVal: ProjectFuelCellsDetailsInterface = this.addProjectService.currentFlowData
        ?.projectDetails as ProjectFuelCellsDetailsInterface;

      this.form.patchValue({
        minimumAnnualSiteKWh: formVal.minimumAnnualSiteKWh,
        contractStructures: formVal.contractStructures,
        minimumTermLength: formVal.minimumTermLength,
        valuesProvided: formVal.valuesProvided,
        timeAndUrgencyConsiderations: formVal.timeAndUrgencyConsiderations,
        additionalComments: formVal.additionalComments
      });

      formVal.contractStructures.forEach(cs => {
        this.contractStructure.forEach(csl => {
          if (cs.id === csl.id) csl.selected = true;
        });
      });

      formVal.valuesProvided.forEach(vp => {
        this.valueProvidedList.forEach(vpl => {
          if (vp.id === vpl.id) vpl.selected = true;
        });
      });
    }
  }

  changeStep(step: number): void {
    this.form.controls['minimumAnnualSiteKWh'].addValidators(Validators.required);
    this.form.controls['contractStructures'].addValidators(Validators.required);
    this.form.controls['valuesProvided'].addValidators(Validators.required);
    this.updateFormValidity();

    this.formSubmitted = true;
    if (this.form.invalid) return;

    this.addProjectService.updateProjectTypeData(this.form.value);
    this.goBack(step);
  }

  goBack(step: number): void {
    this.addProjectService.updateProjectTypeData(this.form.value);
    this.addProjectService.changeStep(step);
  }

  chooseCheckboxControlVal(val: TagInterface, unselect: boolean, controlName: string) {
    if (unselect) {
      const updatedVal: number[] = [...this.form.value[controlName]].filter(o => o.id !== val.id);

      this.form.patchValue({
        [controlName]: [...updatedVal]
      });
    } else {
      this.form.patchValue({
        [controlName]: this.form.value[controlName] ? [...this.form.value[controlName], val] : [val]
      });
    }
  }

  saveDraft(): void {
    this.form.controls['minimumAnnualSiteKWh'].removeValidators(Validators.required);
    this.form.controls['contractStructures'].removeValidators(Validators.required);
    this.form.controls['valuesProvided'].removeValidators(Validators.required);
    this.formSubmitted = true;
    this.updateFormValidity();

    if (this.form.invalid) return;

    this.addProjectService.updateProjectTypeData(this.form.value);
    this.showDraftModal = true;
  }

  updateFormValidity(): void {
    this.form.controls['minimumAnnualSiteKWh'].updateValueAndValidity();
    this.form.controls['contractStructures'].updateValueAndValidity();
    this.form.controls['valuesProvided'].updateValueAndValidity();
  }
}
