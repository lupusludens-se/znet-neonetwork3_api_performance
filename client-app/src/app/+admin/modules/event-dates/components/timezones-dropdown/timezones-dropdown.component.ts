import { Component, ElementRef, EventEmitter, HostListener, Input, Output, ViewChild } from '@angular/core';
import { TimezoneInterface } from '../../../../../shared/interfaces/common/timezone.interface';

@Component({
  selector: 'neo-timezones-dropdown',
  templateUrl: './timezones-dropdown.component.html',
  styleUrls: ['./timezones-dropdown.component.scss']
})
export class TimezonesDropdownComponent {
  @ViewChild('inputEl') inputEl: ElementRef;
  @Input() dropdownOptions: TimezoneInterface[];
  @Input() selectedOption: TimezoneInterface;
  @Output() chosenOption: EventEmitter<TimezoneInterface> = new EventEmitter();
  showDropdown: boolean;
  clickOutside: boolean;

  @HostListener('document:click', ['$event'])
  documentClick(event: Event) {
    this.clickOutside = !this.inputEl.nativeElement.contains(event.target);

    if (this.clickOutside && this.showDropdown) {
      this.showDropdown = false;
    }
  }

  inputClick(): void {
    this.showDropdown = !this.showDropdown;
  }

  chooseOption(option: TimezoneInterface): void {
    this.selectedOption = option;
    this.chosenOption.emit(option);
  }
}
