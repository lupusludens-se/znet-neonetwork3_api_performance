import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'neo-leave-confirmation',
  templateUrl: './leave-confirmation.component.html',
  styleUrls: ['./leave-confirmation.component.scss']
})
export class LeaveConfirmationComponent {
  @Output() closed = new EventEmitter();
  @Output() left = new EventEmitter();
}
