import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'neo-filter-button',
  templateUrl: './filter-button.component.html',
  styleUrls: ['filter-button.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FilterButtonComponent {
  @Input() name: string;
  @Input() icon: string;
  @Input() isActive: string;

  @Output() clicked: EventEmitter<string> = new EventEmitter<string>();

  changeActiveState() {
    this.clicked.emit(this.name);
  }
}
