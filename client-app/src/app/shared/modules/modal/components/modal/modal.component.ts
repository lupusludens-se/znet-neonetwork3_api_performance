import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'neo-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent {
  @Input() titleSeparator: boolean;
  @Input() showBadge: boolean;
  @Input() badgeNumber: number;
  @Input() title: string;  
  @Input() logoPath?: string;
  @Input() size: 'small' | 'medium' | 'extra-medium' | 'large' | 'fit-content' = 'small';
  @Input() padding: 'p-48' | 'p-32' | 'p-32-48' = 'p-32';
  @Output() closed = new EventEmitter<Record<string, string>>();
}
