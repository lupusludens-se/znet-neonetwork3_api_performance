import { ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, Validator } from '@angular/forms';
import { Component, ElementRef, forwardRef, Input, ViewChild } from '@angular/core';
import { BaseControlDirective } from '../base-control.component';

@Component({
  selector: 'neo-number-input',
  templateUrl: './number-input.component.html',
  styleUrls: ['./number-input.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => NumberInputComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => NumberInputComponent),
      multi: true
    }
  ]
})
export class NumberInputComponent extends BaseControlDirective implements ControlValueAccessor, Validator {
  @Input() subLabel: string;
  @Input() width: string = '100%';
  @Input() showLabel: boolean = true;
  @Input() allowFloat: boolean = false;
  @Input() thousandsSeparator: boolean;
  @Input() allowDecimal: boolean;
  @ViewChild('numberInput') numberInput: ElementRef;

  onKeyDown(evt: KeyboardEvent): void {
    const regex: RegExp = new RegExp(/^\d*\.?\d{0,4}$/g);
    const allow: boolean = !!evt.code.match(/Arrow|Delete|Backspace|Tab|NumpadDecimal/),
      isNumberKey: boolean = !!evt.key.match(this.allowFloat || this.allowDecimal ? /[0-9.]/ : /[0-9]/),
      allowCtrlv: boolean = evt.key === 'v' && evt.ctrlKey === true;
    if (allowCtrlv) return;
    if (!this.allowDecimal) {
      if (isNumberKey || allow) return;
      evt.preventDefault();
      evt.stopPropagation();
    } else {
      if (allow) return;
      let current: string = this.value;
      const position = this.numberInput.nativeElement.selectionStart;
      const next: string = [
        current?.toString().slice(0, position),
        evt.key == 'Decimal' ? '.' : evt.key,
        current?.toString().slice(position)
      ].join('');
      if (next && !String(next).match(regex)) {
        evt.preventDefault();
      }
    }
  }

  roundDecimalValue() {
    if (this.allowDecimal) return;
    let value = parseFloat(this.value);
    if (Number.isNaN(value)) {
      this.value = null;
    } else {
      this.value = Number(Math.round(value));
    }
  }
}
