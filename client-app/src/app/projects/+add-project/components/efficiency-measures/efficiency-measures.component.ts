import { ControlContainer, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

import { ProjectEfficiencyMeasuresDetailsInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { RadioControlInterface } from '../../../../shared/modules/radio-control/radio-control.component';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { AddProjectService } from '../../services/add-project.service';
import { ProjectTypesSteps } from '../../enums/project-types-name.enum';
import { INT32_MAX } from 'src/app/shared/constants/math.const';

@Component({
  selector: 'neo-efficiency-measures',
  templateUrl: './efficiency-measures.component.html',
  styleUrls: ['../../add-project.component.scss']
})
export class EfficiencyMeasuresComponent implements OnInit {
  stepsList = AddProjectStepsEnum;
  projectTypes = ProjectTypesSteps;
  form: FormGroup = this.formBuilder.group({
    contractStructures: ['', Validators.required],
    isInvestmentGradeCreditOfOfftakerRequired: [null, Validators.required],
    minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
    valuesProvided: ['', Validators.required],
    timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
    additionalComments: ['', Validators.maxLength(200)]
  });
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
  showDraftModal: boolean;
  formSubmitted: boolean;

  constructor(
    public addProjectService: AddProjectService,
    public controlContainer: ControlContainer,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    if (this.addProjectService.currentFlowData?.projectDetails) {
      const formVal: ProjectEfficiencyMeasuresDetailsInterface = this.addProjectService.currentFlowData
        ?.projectDetails as ProjectEfficiencyMeasuresDetailsInterface;

      this.form.patchValue({
        contractStructures: formVal.contractStructures,
        isInvestmentGradeCreditOfOfftakerRequired: formVal.isInvestmentGradeCreditOfOfftakerRequired,
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
    this.form.controls['contractStructures'].addValidators(Validators.required);
    this.form.controls['isInvestmentGradeCreditOfOfftakerRequired'].addValidators(Validators.required);
    this.form.controls['valuesProvided'].addValidators(Validators.required);
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
    this.form.controls['isInvestmentGradeCreditOfOfftakerRequired'].removeValidators(Validators.required);
    this.form.controls['valuesProvided'].removeValidators(Validators.required);
    this.formSubmitted = true;
    this.updateFormValidity();

    if (this.form.invalid) return;

    this.addProjectService.updateProjectTypeData(this.form.value);
    this.showDraftModal = true;
  }

  updateFormValidity(): void {
    this.form.controls['contractStructures'].updateValueAndValidity();
    this.form.controls['isInvestmentGradeCreditOfOfftakerRequired'].updateValueAndValidity();
    this.form.controls['valuesProvided'].updateValueAndValidity();
  }
}
