import { ControlContainer, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

import { ProjectCarbonOffsetDetailsInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { AddProjectService } from '../../services/add-project.service';
import { ProjectTypesSteps } from '../../enums/project-types-name.enum';
import { INT32_MAX } from 'src/app/shared/constants/math.const';

@Component({
  selector: 'neo-carbon-offset',
  templateUrl: './carbon-offset.component.html',
  styleUrls: ['../../add-project.component.scss']
})
export class CarbonOffsetComponent implements OnInit {
  stepsList = AddProjectStepsEnum;
  projectTypes = ProjectTypesSteps;
  form: FormGroup = this.formBuilder.group({
    minimumPurchaseVolume: ['', [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]],
    stripLengths: [[], Validators.required],
    valuesProvided: ['', Validators.required],
    timeAndUrgencyConsiderations: ['', Validators.maxLength(200)],
    additionalComments: ['', Validators.maxLength(200)]
  });
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
  showDraftModal: boolean;
  formSubmitted: boolean;

  constructor(
    public addProjectService: AddProjectService,
    public controlContainer: ControlContainer,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    if (this.addProjectService.currentFlowData?.projectDetails) {
      const formVal: ProjectCarbonOffsetDetailsInterface = this.addProjectService.currentFlowData
        ?.projectDetails as ProjectCarbonOffsetDetailsInterface;

      this.form.patchValue({
        minimumPurchaseVolume: formVal.minimumPurchaseVolume,
        stripLengths: formVal.stripLengths,
        valuesProvided: formVal.valuesProvided,
        timeAndUrgencyConsiderations: formVal.timeAndUrgencyConsiderations,
        additionalComments: formVal.additionalComments
      });

      if (formVal.stripLengths) {
        formVal.stripLengths.forEach(sl => {
          this.stripLengthList.map(v => {
            if (v.id === sl.id) {
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
    this.form.controls['minimumPurchaseVolume'].addValidators(Validators.required);
    this.form.controls['stripLengths'].addValidators(Validators.required);
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

  chooseStripLength(val: TagInterface, unselect: boolean) {
    if (unselect) {
      const updatedVal: TagInterface[] = [...this.form.value.stripLengths].filter(o => o.id !== val.id);

      this.form.patchValue({
        stripLengths: [...updatedVal]
      });
    } else {
      this.form.patchValue({
        stripLengths: this.form.value?.stripLengths ? [...this.form.value.stripLengths, val] : [val]
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
    this.form.controls['minimumPurchaseVolume'].removeValidators(Validators.required);
    this.form.controls['stripLengths'].removeValidators(Validators.required);
    this.form.controls['valuesProvided'].removeValidators(Validators.required);
    this.formSubmitted = true;
    this.updateFormValidity();

    if (this.form.invalid) return;

    this.addProjectService.updateProjectTypeData(this.form.value);
    this.showDraftModal = true;
  }

  updateFormValidity(): void {
    this.form.controls['minimumPurchaseVolume'].updateValueAndValidity();
    this.form.controls['stripLengths'].updateValueAndValidity();
    this.form.controls['valuesProvided'].updateValueAndValidity();
  }
}
