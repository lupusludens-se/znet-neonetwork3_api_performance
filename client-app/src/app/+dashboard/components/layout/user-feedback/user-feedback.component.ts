import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FeedbackApiEnum } from 'src/app/+admin/modules/+feedback-management/enums/feedback.enum';
import { FeedbackInterface } from 'src/app/core/interfaces/feedback.interface';
import { CoreService } from 'src/app/core/services/core.service';
import { HttpService } from 'src/app/core/services/http.service';
import { CustomValidator } from 'src/app/shared/validators/custom.validator';

@Component({
  selector: 'neo-user-feedback',
  templateUrl: './user-feedback.component.html',
  styleUrls: ['./user-feedback.component.scss']
})
export class UserFeedbackComponent implements OnInit {
  stars: number[] = [1, 2, 3, 4, 5];
  formSubmitted: boolean = false;
  feedbackCommentsMaxLength: number = 1000;
  form: FormGroup = this.formbuilder.group({
    rating: ['', [Validators.required, Validators.min(1), Validators.max(5)]],
    comments: [null, [Validators.required, Validators.maxLength(this.feedbackCommentsMaxLength), CustomValidator.noWhitespaceValidator]]
  });

  constructor(
    private formbuilder: FormBuilder,
    private readonly httpService: HttpService,
    public coreService: CoreService
  ) {}

  ngOnInit() {
    this.coreService.feedbackPopupSubmitted$.next(false);
  }

  countStar(star) {
    this.form.controls.rating.setValue(star);
  }

  hasError(controlName: string): boolean {
    const control = this.form?.get(controlName);
    return control?.invalid && control?.touched && control?.dirty;
  }

  closeFeedback() {
    this.coreService.showFeedbackPopup$.next(false);
  }

  onSubmit() {
    if (
      this.form.controls['rating'].value > 0 &&
      this.form.controls['rating'].value <= 5 &&
      this.form.controls['comments'].value.length <= 1000 &&
      this.form.valid
    ) {
      const formData: FeedbackInterface = {
        rating: this.form.controls['rating'].value,
        comments: this.coreService.convertToPlain(this.form.controls['comments'].value)
      };
      this.httpService.post(FeedbackApiEnum.SubmitFeedback, formData).subscribe((result: any) => {
        if (result > 0) {
          this.formSubmitted = true;
          this.coreService.feedbackPopupSubmitted$.next(true);
        }
      });
    } else {
      Object.keys(this.form.value).map(key => {
        this.form.get(key).markAsDirty();
        this.form.get(key).markAsTouched();
      });
    }
  }
}
