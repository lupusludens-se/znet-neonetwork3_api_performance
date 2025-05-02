import { ControlContainer, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

import { ProjectEmergingTechnologiesDetailsInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { AddProjectService } from '../../services/add-project.service';
import { ProjectTypesSteps } from '../../enums/project-types-name.enum';
import { INT32_MAX } from 'src/app/shared/constants/math.const';

@Component({
  selector: 'neo-emerging-technologies',
  templateUrl: './emerging-technologies.component.html',
  styleUrls: ['../../add-project.component.scss']
})
export class EmergingTechnologiesComponent implements OnInit {
  stepsList = AddProjectStepsEnum;
  projectTypes = ProjectTypesSteps;
  form: FormGroup = this.formBuilder.group({
    contractStructures: ['', Validators.required],
    minimumAnnualValue: ['', [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]],
    energyUnit: ['', [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]],
    energyUnitId: ['', [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]],
    minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
    valuesProvided: ['', Validators.required],
    timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
    additionalComments: ['', Validators.maxLength(200)]
  });
  contractStructure: TagInterface[] = [
    { name: 'PPA', id: 2 },
    { name: 'Lease', id: 4 },
    { name: 'Cash Purchase', id: 1 },
    { name: 'Shared Savings', id: 5 },
    { name: 'Guaranteed Savings', id: 6 }
  ];
  valueProvidedList: TagInterface[] = [
    { name: 'Cost Savings', id: 1 },
    { name: 'Visible Commitment to Climate Action', id: 8 },
    { name: 'Resiliency', id: 4 },
    { name: 'Other', id: 5 }
  ];
  energyUnits: TagInterface[] = [
    { name: 'KW', id: 1 },
    { name: 'KWh', id: 2 },
    { name: 'MW', id: 3 },
    { name: 'MWh', id: 4 },
    { name: 'Gallons', id: 5 }
  ];
  showDraftModal: boolean;
  formSubmitted: boolean;
  energyUnitId: number = this.energyUnits[0].id;

  constructor(
    public addProjectService: AddProjectService,
    public controlContainer: ControlContainer,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    if (this.addProjectService.currentFlowData?.projectDetails) {
      const formVal: ProjectEmergingTechnologiesDetailsInterface = this.addProjectService.currentFlowData
        ?.projectDetails as ProjectEmergingTechnologiesDetailsInterface;

      this.form.patchValue({
        contractStructures: formVal.contractStructures,
        minimumAnnualValue: formVal.minimumAnnualValue,
        energyUnit: this.energyUnits.filter(s => s.id === formVal.energyUnitId)[0] ?? '',
        minimumTermLength: formVal.minimumTermLength,
        valuesProvided: formVal.valuesProvided,
        timeAndUrgencyConsiderations: formVal.timeAndUrgencyConsiderations,
        additionalComments: formVal.additionalComments,
        energyUnitId: formVal.energyUnitId
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
    this.form.controls['contractStructures'].addValidators(Validators.required);
    this.form.controls['minimumAnnualValue'].addValidators(Validators.required);
    this.form.controls['valuesProvided'].addValidators(Validators.required);
    this.form.controls['energyUnit'].addValidators(Validators.required);
    this.updateFormValidity();

    this.formSubmitted = true;
    if (this.form.invalid) return;

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
    this.form.controls['contractStructures'].removeValidators(Validators.required);
    this.form.controls['minimumAnnualValue'].removeValidators(Validators.required);
    this.form.controls['valuesProvided'].removeValidators(Validators.required);
    this.form.controls['energyUnit'].removeValidators(Validators.required);
    this.formSubmitted = true;
    this.updateFormValidity();

    if (this.form.invalid) return;

    this.addProjectService.updateProjectTypeData(this.form.value);
    this.showDraftModal = true;
  }

  updateFormValidity(): void {
    this.form.controls['contractStructures'].updateValueAndValidity();
    this.form.controls['minimumAnnualValue'].updateValueAndValidity();
    this.form.controls['valuesProvided'].updateValueAndValidity();
  }

  setControlValue(dropdownValId: number, controlName: string) {
    this[controlName] = dropdownValId;
    this.form.patchValue({ [controlName]: dropdownValId });
  }
}
