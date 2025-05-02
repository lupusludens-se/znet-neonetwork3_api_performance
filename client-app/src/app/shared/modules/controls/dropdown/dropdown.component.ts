import { Component, ElementRef, EventEmitter, forwardRef, HostListener, Input, Output } from '@angular/core';
import { NG_VALIDATORS, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CountryInterface } from '../../../interfaces/user/country.interface';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { BaseControlDirective } from '../base-control.component';
import { TimezoneInterface } from '../../../interfaces/common/timezone.interface';

@Component({
  selector: 'neo-dropdown',
  templateUrl: './dropdown.component.html',
  styleUrls: ['./dropdown.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DropdownComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => DropdownComponent),
      multi: true
    }
  ]
})
export class DropdownComponent extends BaseControlDirective {
  @Input() dropdownOptions: TagInterface[] | CountryInterface[]|TimezoneInterface[] = [];
  @Input() dropdownWidth: string = '100%';
  @Input() dropdownSize: string = '181px';
  @Input() topPosition: string = '74px';
  @Input() rightPosition: string = 'initial';
  @Input() readonly: boolean = true;
  @Input() disabled: boolean;
  @Input() error: boolean;
  @Input() showLabel: boolean = true;
  @Input() spacing: string = 'mb-8';
  @Input() applyAsterisk?: boolean = false;
  @Input() placeholderText: string;
  @Input() labelForError: string;

  @Output() chosenOption: EventEmitter<TagInterface | CountryInterface|TimezoneInterface> = new EventEmitter();

  showDropdown = false;
  clickOutside = false;

  constructor(private readonly elRef: ElementRef) {
    super();
  }

  @HostListener('document:click', ['$event'])
  documentClick(event: Event) {
    this.clickOutside = !this.elRef.nativeElement.contains(event.target);

    if (this.clickOutside && this.showDropdown) {
      this.showDropdown = false;
    }
  }

  inputClick(parent: HTMLElement): void {
    if (!this.disabled) {
      this.setDropdownTopPosition(parent);
      this.showDropdown = !this.showDropdown;
    }
  }

  chooseOption(option: TagInterface | CountryInterface|TimezoneInterface): void {
    if (this.value?.id !== option?.id) {
      this.value = option;
      this.chosenOption.emit(option);
    }

    this.showDropdown = false;
  }

  trackOptionsById(index: number, element: TagInterface | CountryInterface|TimezoneInterface): number {
    return element.id;
  }

  private setDropdownTopPosition(parent: HTMLElement): void {
    const dropDownSize = parseInt(this.dropdownSize);
    const bottom = window.innerHeight - parent.getBoundingClientRect().bottom - 80;

    if (bottom < dropDownSize) {
      this.topPosition = `-${dropDownSize + 5}px`;
    }
  }
}
