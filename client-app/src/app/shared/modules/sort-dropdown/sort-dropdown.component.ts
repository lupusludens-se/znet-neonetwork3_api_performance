import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges
} from '@angular/core';

export enum SortingOptionsEnum {
  Trending = 'Trending',
  Popular = 'Popular',
  Newest = 'Newest'
}

export interface SortingOptionsKeyValuePair {
  key: string;
  value: string;
}

@Component({
  selector: 'neo-sort-dropdown',
  templateUrl: './sort-dropdown.component.html',
  styleUrls: ['./sort-dropdown.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SortDropdownComponent implements OnChanges {
  @Output() optionSelected: EventEmitter<string> = new EventEmitter<string>();

  openList: boolean;
  @Input() sortingOptions: SortingOptionsKeyValuePair[] = [];
  @Input() selectedOption: string;
  option: string;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedOption']?.currentValue !== changes['selectedOption']?.previousValue) {
      let sortOption = this.sortingOptions.find(x => x.key == this.selectedOption);
      this.option = sortOption?.value ?? '';
    }
  }

  selectOption(option: SortingOptionsKeyValuePair): void {
    this.optionSelected.emit(option.key);
    this.selectedOption = option.value;
    this.openList = false;
  }
}
