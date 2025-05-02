import { Directive, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormControl, ValidationErrors } from '@angular/forms';

@Directive()
export class BaseControlDirective implements OnChanges {
  @Input() formControlName: string;
  @Input() labelName: string;
  @Input() optionalText: string;
  @Input() type: string = 'text';
  @Input() placeholder: string = '';
  @Input() autofocus: boolean;
  @Input() maxLength: number;
  @Input() minLength: number;
  @Input() disabled: boolean;
  @Input() tabInd: number;
  @Input() submitted: boolean;

  errors: ValidationErrors | null;
  control: FormControl;
  innerValue: any = null;
  default: any;
  isFocused: boolean;

  ngOnChanges(changes: SimpleChanges): void {
    if (this.control && changes['submitted']?.currentValue && !changes['submitted']?.previousValue) {
      this.updateState();
    }
  }

  onChangeCallback: (v: any) => void;
  onTouchCallback: (v: any) => void;

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    this.innerValue = v;
    this.updateValue();
  }

  writeValue(v: any): void {
    this.innerValue = v;
  }

  registerOnChange(fn: (v: any) => void): void {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: (v: any) => void): void {
    this.onTouchCallback = fn;
  }

  setDisabledState(disabled: boolean): void {
    this.disabled = disabled;
  }

  validate(c: FormControl): any {
    this.control = c;
    this.control.statusChanges.subscribe(() => this.updateState());
  }

  updateValue(): void {
    this.onChangeCallback(this.innerValue);
    this.updateState();
  }

  updateState(): void {
    if (!this.control) return;
    this.errors = this.control.errors;
  }

  onBlur(): void {
    this.isFocused = false;
    this.control.markAsTouched();
  }

  onFocus(): void {
    this.isFocused = true;
  }
}
