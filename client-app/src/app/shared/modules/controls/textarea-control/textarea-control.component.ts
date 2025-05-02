import { NG_VALIDATORS, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Component, forwardRef, Input } from '@angular/core';

import { BaseControlDirective } from '../base-control.component';

@Component({
  selector: 'neo-textarea-control',
  templateUrl: 'textarea-control.component.html',
  styles: [
    `
      textarea {
        width: 100%;
      }
      :host.text-zeigo-near-black {
        .input-label {
          color: #1a1523;
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
      useExisting: forwardRef(() => TextareaControlComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => TextareaControlComponent),
      multi: true
    }
  ]
})
export class TextareaControlComponent extends BaseControlDirective {
  @Input() optionalText: string;
  @Input() rows: number;
  @Input() height: string = '100px';
  @Input() showLabel: boolean = true;
  @Input() disabled: boolean = false;

  constructor() {
    super();
  }
}
