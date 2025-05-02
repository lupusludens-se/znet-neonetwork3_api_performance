import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { NG_VALIDATORS, NG_VALUE_ACCESSOR } from '@angular/forms';

import { BaseControlDirective } from '../../../controls/base-control.component';
import { RadioControlInterface } from '../../../radio-control/radio-control.component';

@Component({
  selector: 'neo-square-radio-control',
  templateUrl: './square-radio-control.component.html',
  styleUrls: ['./square-radio-control.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SquareRadioControlComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => SquareRadioControlComponent),
      multi: true
    }
  ]
})
export class SquareRadioControlComponent extends BaseControlDirective {
  @Input() disabled: boolean;
  @Input() height: string = '48';
  @Output() valueChanged: EventEmitter<RadioControlInterface> = new EventEmitter<RadioControlInterface>();
  @Input() list: RadioControlInterface[];
}
