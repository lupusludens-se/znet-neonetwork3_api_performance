import { NG_VALIDATORS, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Component, forwardRef } from '@angular/core';

import { BaseControlDirective } from '../base-control.component';

@Component({
  selector: 'neo-date-input',
  templateUrl: 'date-input.component.html',
  styles: [
    `
      :host {
        display: flex;
        flex-direction: column;

        input {
          height: 48px;
        }
      }
      :host.text-zeigo-near-black {
        .input-label {
          color: #1A1523;
          font-size: 16px;
          line-height: 20px;
          .optional {
            color: #868686;
            font-family: 'Arial Rounded MT Bold', serif;
          }
        }
      }
    `
  ],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DateInputComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => DateInputComponent),
      multi: true
    }
  ]
})
export class DateInputComponent extends BaseControlDirective {
  constructor() {
    super();
  }
}
