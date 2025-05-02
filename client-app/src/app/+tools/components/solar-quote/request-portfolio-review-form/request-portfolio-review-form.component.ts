import { Component, Input, OnDestroy } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { HttpService } from '../../../../core/services/http.service';
import { SnackbarService } from '../../../../core/services/snackbar.service';

import { QuoteInterestsTypeEnum } from '../../../enums/quote-interests-type.enum';
import { FormControlStatusEnum } from '../../../../shared/enums/form-control-status.enum';
import { ToolsApiEnum } from '../../../../shared/enums/api/tools-api.enum';
import { Subject } from 'rxjs';

@Component({
  selector: 'neo-request-portfolio-review-form',
  templateUrl: './request-portfolio-review-form.component.html',
  styleUrls: ['./../indicative-quote-request-form/indicative-quote-request-form.component.scss']
})
export class RequestPortfolioReviewFormComponent implements OnDestroy {
  @Input() toolId: number;

  formGroup: FormGroup = new FormGroup({
    interests: new FormArray([], Validators.required),
    additionalComments: new FormControl(null, Validators.maxLength(4000))
  });

  quoteInterestsOptions = QuoteInterestsTypeEnum;
  formControlStatus = FormControlStatusEnum;
  formSubmitted: boolean;
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly router: Router,
    private readonly httpService: HttpService,
    private readonly snackbarService: SnackbarService
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  onCancel(): void {
    this.router.navigate(['/tools']);
  }

  save(): void {
    this.formSubmitted = true;
    if (this.formGroup.status === FormControlStatusEnum.Invalid) return;

    this.requestQuote();
  }

  getKeys(enumData: unknown): string[] {
    return Object.keys(enumData).filter(x => !(parseInt(x) >= 0));
  }

  setCheckboxStatus(value: number): void {
    const companyFormArray = this.getFormArray('interests');

    if (companyFormArray?.length) {
      const companyIndex = companyFormArray?.controls.findIndex(item => item.value === value);

      if (companyIndex >= 0) {
        companyFormArray.removeAt(companyIndex);
      } else {
        companyFormArray.push(new FormControl(value));
      }
    } else {
      companyFormArray.push(new FormControl(value));
    }
  }

  private getFormArray(property: string): FormArray {
    return this.formGroup.get(property) as FormArray;
  }

  private requestQuote(): void {
    this.httpService.post<unknown>(ToolsApiEnum.RequestPortfolioReview, this.formGroup.value).subscribe(
      () => this.router.navigate([`/tools/thank-you/${this.toolId}`]),
      () => this.snackbarService.showError('general.defaultErrorLabel')
    );
  }
}
