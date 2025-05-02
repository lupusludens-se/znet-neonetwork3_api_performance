import { Component, EventEmitter, Input, Output } from '@angular/core';

import { TagInterface } from '../../../../core/interfaces/tag.interface';

@Component({
  selector: 'neo-select-item',
  templateUrl: './select-item.component.html',
  styleUrls: ['./select-item.component.scss']
})
export class SelectItemComponent {
  @Input() maxSelectedItems: number;
  @Input() selectedItems: number[];
  @Input() item: TagInterface;

  @Output() add: EventEmitter<number> = new EventEmitter<number>();
}
