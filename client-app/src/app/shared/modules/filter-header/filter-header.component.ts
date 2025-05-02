import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'neo-filter-header',
  templateUrl: 'filter-header.component.html',
  styleUrls: ['filter-header.component.scss']
})
export class FilterHeaderComponent {
  @Output() clearFilters: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() filterName: string = 'Filters';
  @Input() showClearButton: boolean;
}
