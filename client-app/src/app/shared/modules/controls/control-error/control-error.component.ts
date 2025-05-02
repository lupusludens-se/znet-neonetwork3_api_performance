import { ChangeDetectionStrategy, Component, Input, OnChanges } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import { ValidationErrorsMsg } from '../../../validators/validation-error-msg.const';

@Component({
  selector: 'neo-control-error',
  templateUrl: './control-error.component.html',
  styles: [
    `
      .error-msg {
        font-family: 'Arial', sans-serif;
        font-size: 12px;
        margin-top: 2px;
        line-height: 140%;
        color: #e5484d;
        margin-left: 20px;
      }
    `
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ControlErrorComponent implements OnChanges {
  @Input() errors: ValidationErrors;
  @Input() submitted: boolean;
  @Input() fieldName: string;
  errorText: string;

  ngOnChanges(): void {
    if (!this.errors) return (this.errorText = null);
    const errKeys: string[] = Object.keys(this.errors);

    if (errKeys.length && this.submitted) this.generateErrorMsg(errKeys[0]);
    else this.errorText = null;
  }

  generateErrorMsg(key: string): void {
    const errGen: any = ValidationErrorsMsg.get(key);
    if (!errGen) this.errorText = this.errors[key];
    else this.errorText = errGen(this.errors, this.fieldName);
  }
}
