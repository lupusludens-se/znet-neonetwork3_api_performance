import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { NG_VALIDATORS, NG_VALUE_ACCESSOR } from '@angular/forms';

import { BaseControlDirective } from '../controls/base-control.component';
import { TagInterface } from '../../../core/interfaces/tag.interface';

export interface RadioControlInterface {
  id: number | string;
  name: string;
}

@Component({
  selector: 'neo-radio-control',
  templateUrl: 'radio-control.component.html',
  styleUrls: ['radio-control.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RadioControlComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => RadioControlComponent),
      multi: true
    }
  ]
})
export class RadioControlComponent extends BaseControlDirective {
  @Input() cleared: boolean;
  @Input() disabled: boolean;
  @Input() labelForError: string;
  @Input() flexDirection: string = 'column';
  @Output() valueChanged: EventEmitter<RadioControlInterface> = new EventEmitter<RadioControlInterface>();
  @Input() list: RadioControlInterface[] | TagInterface[];
}
