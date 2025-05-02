import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'neo-save-cancel-controls',
  templateUrl: 'save-cancel-controls.component.html',
  styleUrls: ['./save-cancel-controls.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SaveCancelControlsComponent {
  @Input() cancelBtnText: string;
  @Input() saveBtnText: string;
  @Input() disable: boolean;
  @Output() cancel: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() save: EventEmitter<boolean> = new EventEmitter<boolean>();
}
