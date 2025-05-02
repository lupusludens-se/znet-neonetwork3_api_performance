import { ControlContainer, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

import { ProjectRenewableElectricityDetailsInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { AddProjectService } from '../../services/add-project.service';
import { ProjectTypesSteps } from '../../enums/project-types-name.enum';
import { INT32_MAX } from 'src/app/shared/constants/math.const';

@Component({
  selector: 'neo-renewable-electricity',
  templateUrl: './renewable-electricity.component.html',
  styleUrls: ['../../add-project.component.scss']
})
export class RenewableElectricityComponent implements OnInit {
  stepsList = AddProjectStepsEnum;
  projectTypes = ProjectTypesSteps;
  form: FormGroup = this.formBuilder.group({
    minimumAnnualSiteKWh: ['', [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]],
    purchaseOptions: [''],
    minimumTermLength: ['', [Validators.min(1), Validators.max(INT32_MAX)]],
    valuesProvided: ['', Validators.required],
    timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
    additionalComments: ['', Validators.maxLength(200)]
  });
  purchaseOptions: TagInterface[] = [
    { name: 'Behind the Meter', id: 1 },
    { name: 'In Front of the Meter', id: 2 }
  ];
  valueProvidedList: TagInterface[] = [
    { name: 'Cost Savings', id: 1 },
    { name: 'Renewable Attributes', id: 6 },
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
      const formVal: ProjectRenewableElectricityDetailsInterface = this.addProjectService.currentFlowData
        ?.projectDetails as ProjectRenewableElectricityDetailsInterface;

      this.form.patchValue({
        minimumAnnualSiteKWh: formVal.minimumAnnualSiteKWh,
        purchaseOptions: formVal.purchaseOptions,
        minimumTermLength: formVal.minimumTermLength,
        valuesProvided: formVal.valuesProvided,
        timeAndUrgencyConsiderations: formVal.timeAndUrgencyConsiderations,
        additionalComments: formVal.additionalComments
      });

      if (formVal.purchaseOptions) {
        formVal.purchaseOptions.forEach(po => {
          this.purchaseOptions.map(v => {
            if (v.id === po.id) {
              v.selected = true;
            }
          });
        });
      }

      formVal.valuesProvided.forEach(vp => {
        this.valueProvidedList.forEach(vpl => {
          if (vp.id === vpl.id) vpl.selected = true;
        });
      });
    }
  }

  changeStep(step: number): void {
    this.form.controls['minimumAnnualSiteKWh'].addValidators(Validators.required);
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

  choosePurchaseOptions(val: TagInterface, unselect: boolean) {
    if (unselect) {
      const updatedVal: TagInterface[] = [...this.form.value.purchaseOptions].filter(o => o.id !== val.id);
      this.form.patchValue({
        purchaseOptions: [...updatedVal]
      });
    } else {
      this.form.patchValue({
        purchaseOptions: [...this.form.value.purchaseOptions, val]
      });
    }
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
    this.form.controls['valuesProvided'].removeValidators(Validators.required);
    this.formSubmitted = true;
    this.updateFormValidity();

    if (this.form.invalid) return;

    this.addProjectService.updateProjectTypeData(this.form.value);
    this.showDraftModal = true;
  }

  updateFormValidity(): void {
    this.form.controls['minimumAnnualSiteKWh'].updateValueAndValidity();
    this.form.controls['valuesProvided'].updateValueAndValidity();
  }
}
