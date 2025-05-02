import { Component, EventEmitter, OnDestroy, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { HttpService } from 'src/app/core/services/http.service';
import { FormControlStatusEnum } from 'src/app/shared/enums/form-control-status.enum';
import { CustomValidator } from 'src/app/shared/validators/custom.validator';

@Component({
  selector: 'neo-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss']
})
export class ContactComponent implements OnDestroy {
  @Output() closed = new EventEmitter<Record<string, string>>();

  isMessageSent: boolean = false;

  formGroup: FormGroup = new FormGroup({
    firstName: new FormControl(null, Validators.required),
    lastName: new FormControl(null, Validators.required),
    company: new FormControl(null, Validators.required),
    email: new FormControl(null, [Validators.required, CustomValidator.email]),
    message: new FormControl(null, Validators.required),
    recaptchaToken: new FormControl(null, Validators.required)
  });

  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(private readonly httpService: HttpService) {}

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  hasError(controlName: string): boolean {
    const control = this.formGroup?.get(controlName);

    return control?.invalid && control?.touched && control?.dirty;
  }

  sendMessage(): void {
    if (this.formGroup.status === FormControlStatusEnum.Invalid) {
      Object.keys(this.formGroup.value).map(key => {
        this.formGroup.get(key).markAsDirty();
        this.formGroup.get(key).markAsTouched();
      });

      return;
    }

    this.httpService
      .post(CommonApiEnum.ContactUs, { ...this.formGroup.value })
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(() => {
        this.isMessageSent = true;
      });
  }
}
