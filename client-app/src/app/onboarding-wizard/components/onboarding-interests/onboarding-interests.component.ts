import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';

import { OnboardingWizardService } from '../../services/onboarding-wizard.service';
import { OnboardingStepsEnum } from '../../enums/onboarding-steps.enum';
import { CommonService } from '../../../core/services/common.service';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'neo-onboarding-interests',
  templateUrl: 'onboarding-interests.component.html',
  styleUrls: ['../../onboarding-wizard.component.scss', 'onboarding-interests.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class OnboardingInterestsComponent implements OnInit {
  stepsList = OnboardingStepsEnum;
  categoriesList: TagInterface[];
  selectedAll: boolean;
  formSubmitted: boolean;

  constructor(
    public controlContainer: ControlContainer,
    private onboardingWizardService: OnboardingWizardService,
    private commonService: CommonService,
    private changeDetRef: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.commonService
      .initialData()
      .pipe(filter(v => !!v))
      .subscribe(data => {
        data.categories.forEach(c => (c.selected = false));
        this.categoriesList = data.categories;

        if (this.controlContainer.control?.get('categories')?.value) {
          this.controlContainer.control?.get('categories')?.value.forEach((v: TagInterface) => {
            this.categoriesList.forEach(c => {
              if (c.id === v.id) {
                c.selected = true;
              }
            });
          });
        }

        this.changeDetRef.markForCheck();
      });
  }

  goForward(step: OnboardingStepsEnum): void {
    this.formSubmitted = true;
    if (this.controlContainer.control!.get('categories')!.invalid) return;

    this.changeStep(step);
  }

  selectInterest(cat: TagInterface): void {
    this.categoriesList.forEach(c => {
      if (c.id === cat.id) {
        c.selected = !c.selected;
        this.changeDetRef.markForCheck();
      }
    });
    this.addDataToForm();

    this.selectedAll = !this.categoriesList.some(c => c.selected === false);
  }

  selectAll(): void {
    this.categoriesList.forEach(c => {
      c.selected = !this.selectedAll;
    });
    this.addDataToForm();
    this.changeDetRef.markForCheck();

    this.selectedAll = !this.selectedAll;
  }

  addDataToForm(): void {
    const selectedCategories: TagInterface[] = this.categoriesList.filter(c => c.selected === true);
    this.controlContainer.control?.get('categories')?.patchValue(selectedCategories);
  }

  changeStep(step: OnboardingStepsEnum) {
    this.onboardingWizardService.changeStep(step);
  }
}
