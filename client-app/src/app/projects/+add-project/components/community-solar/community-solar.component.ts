import { ControlContainer, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

import { ProjectCommunitySolarDetailsInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { RadioControlInterface } from '../../../../shared/modules/radio-control/radio-control.component';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { AddProjectService } from '../../services/add-project.service';
import { ProjectTypesSteps } from '../../enums/project-types-name.enum';
import { INT32_MAX } from 'src/app/shared/constants/math.const';

@Component({
  selector: 'neo-community-solar',
  templateUrl: './community-solar.component.html',
  styleUrls: ['../../add-project.component.scss', './community-solar.component.scss']
})
export class CommunitySolarComponent implements OnInit {
  stepsList = AddProjectStepsEnum;
  projectTypes = ProjectTypesSteps;
  form: FormGroup = this.formBuilder.group({
    minimumAnnualMWh: ['', [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]],
    totalAnnualMWh: ['', [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]],
    utilityTerritory: ['', [Validators.required, Validators.maxLength(100)]],
    projectAvailable: [false, Validators.required],
    projectAvailabilityApproximateDate: [''],
    isInvestmentGradeCreditOfOfftakerRequired: ['', Validators.required],
    contractStructures: ['', Validators.required],
    minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
    valuesProvided: ['', Validators.required],
    timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
    additionalComments: ['', Validators.maxLength(200)]
  });
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
  showDraftModal: boolean;
  formSubmitted: boolean;

  constructor(
    public addProjectService: AddProjectService,
    public controlContainer: ControlContainer,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    if (this.addProjectService.currentFlowData?.projectDetails) {
      const formVal: ProjectCommunitySolarDetailsInterface = this.addProjectService.currentFlowData
        ?.projectDetails as ProjectCommunitySolarDetailsInterface;

      this.form.patchValue({
        timeAndUrgencyConsiderations: formVal.timeAndUrgencyConsiderations,
        additionalComments: formVal.additionalComments,
        minimumAnnualMWh: formVal.minimumAnnualMWh,
        totalAnnualMWh: formVal.totalAnnualMWh,
        utilityTerritory: formVal.utilityTerritory,
        projectAvailable: formVal.projectAvailable,
        projectAvailabilityApproximateDate: formVal.projectAvailabilityApproximateDate,
        isInvestmentGradeCreditOfOfftakerRequired: formVal.isInvestmentGradeCreditOfOfftakerRequired,
        contractStructures: formVal.contractStructures,
        minimumTermLength: formVal.minimumTermLength,
        valuesProvided: formVal.valuesProvided
      });

      formVal.contractStructures.forEach(cs => {
        this.structureOptions.forEach(csl => {
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
    this.form.controls['minimumAnnualMWh'].addValidators(Validators.required);
    this.form.controls['totalAnnualMWh'].addValidators(Validators.required);
    this.form.controls['utilityTerritory'].addValidators(Validators.required);
    this.form.controls['projectAvailable'].addValidators(Validators.required);
    this.form.controls['isInvestmentGradeCreditOfOfftakerRequired'].addValidators(Validators.required);
    this.form.controls['contractStructures'].addValidators(Validators.required);
    this.form.controls['valuesProvided'].addValidators(Validators.required);

    if (!this.form.controls['projectAvailable'].value) {
      this.form.get('projectAvailabilityApproximateDate')?.setValidators(Validators.required);
      this.form.get('projectAvailabilityApproximateDate')?.updateValueAndValidity();
    }

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

  chooseProjectAvailability(unselect: boolean, updateFormVal: boolean = false) {
    if (updateFormVal) this.form.patchValue({ projectAvailable: !unselect });

    if (unselect) {
      this.form.get('projectAvailabilityApproximateDate')?.setValidators(Validators.required);
      this.form.get('projectAvailabilityApproximateDate')?.updateValueAndValidity();
    } else {
      this.form.patchValue({ projectAvailabilityApproximateDate: null });
      this.form.get('projectAvailabilityApproximateDate')?.clearValidators();
      this.form.get('projectAvailabilityApproximateDate')?.updateValueAndValidity();
    }
  }

  saveDraft(): void {
    this.form.controls['minimumAnnualMWh'].removeValidators(Validators.required);
    this.form.controls['totalAnnualMWh'].removeValidators(Validators.required);
    this.form.controls['utilityTerritory'].removeValidators(Validators.required);
    this.form.controls['projectAvailable'].removeValidators(Validators.required);
    this.form.controls['isInvestmentGradeCreditOfOfftakerRequired'].removeValidators(Validators.required);
    this.form.controls['contractStructures'].removeValidators(Validators.required);
    this.form.controls['valuesProvided'].removeValidators(Validators.required);

    if (this.form.controls['projectAvailable'].value && this.form.controls['projectAvailable'].value === false) {
      this.form.patchValue({ projectAvailabilityApproximateDate: null });
      this.form.get('projectAvailabilityApproximateDate')?.clearValidators();
      this.form.get('projectAvailabilityApproximateDate')?.updateValueAndValidity();
    }

    this.formSubmitted = true;
    this.updateFormValidity();

    if (this.form.invalid) return;

    this.addProjectService.updateProjectTypeData(this.form.value);
    this.showDraftModal = true;
  }

  updateFormValidity(): void {
    this.form.controls['minimumAnnualMWh'].updateValueAndValidity();
    this.form.controls['totalAnnualMWh'].updateValueAndValidity();
    this.form.controls['utilityTerritory'].updateValueAndValidity();
    this.form.controls['isInvestmentGradeCreditOfOfftakerRequired'].updateValueAndValidity();
    this.form.controls['contractStructures'].updateValueAndValidity();
    this.form.controls['valuesProvided'].updateValueAndValidity();
    this.form.controls['projectAvailable'].updateValueAndValidity();
  }
}
