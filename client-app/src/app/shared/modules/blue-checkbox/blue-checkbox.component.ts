import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { UserInterface } from '../../interfaces/user/user.interface';

@Component({
  selector: 'neo-blue-checkbox',
  templateUrl: 'blue-checkbox.component.html',
  styleUrls: ['blue-checkbox.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BlueCheckboxComponent {
  @Input() textSize: 'text-xxs' | 'text-xs' | 'text-s' | 'text-m' = 'text-s';
  @Input() name: string;
  @Input() parentControlName: string = 'control';
  @Input() value: number | string;
  @Input() checked: boolean;
  @Input() disabled: boolean;
  @Input() hideCompany: boolean = false;
  @Input() hideLabel: boolean = false;
  @Input() user: UserInterface;
  @Input() companyName: string;
  @Input() imageSize: 'size16' | 'size20' | 'size24' | 'size32' | 'size48' | 'size56' | 'size96' = 'size24';
  @Input() nameContainsHTML?: boolean = false;

  @Output() selectCheckbox: EventEmitter<any> = new EventEmitter<any>();
}
