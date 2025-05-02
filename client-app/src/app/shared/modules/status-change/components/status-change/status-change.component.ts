import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'neo-status-change',
  templateUrl: './status-change.component.html',
  styleUrls: ['./status-change.component.scss']
})
export class StatusChangeComponent {
  @Input() activeStatusValue: number | boolean;
  @Input() inactiveStatusValue: number | boolean;
  @Input() currentStatus: number | boolean;
  @Input() disableClass: boolean;

  @Output() statusChanged: EventEmitter<number | boolean> = new EventEmitter<number | boolean>();
}
