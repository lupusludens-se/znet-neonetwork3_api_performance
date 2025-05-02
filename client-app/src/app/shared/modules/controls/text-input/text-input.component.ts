import { NG_VALIDATORS, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Component, forwardRef, Input } from '@angular/core';

import { BaseControlDirective } from '../base-control.component';

@Component({
  selector: 'neo-text-input',
  templateUrl: 'text-input.component.html',
  styleUrls: ['text-input.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TextInputComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => TextInputComponent),
      multi: true
    }
  ]
})
export class TextInputComponent extends BaseControlDirective {
  @Input() width: string = '100%';
  @Input() labelForError: string;
  @Input() counterMax: number;
  @Input() showLabel: boolean = true;
  @Input() applyAsterisk?: boolean = false;

  constructor() {
    super();
  }
}
