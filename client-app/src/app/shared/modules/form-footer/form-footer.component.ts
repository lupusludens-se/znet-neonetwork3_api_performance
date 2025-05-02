import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SVG_CONFIG } from '@ngneat/svg-icon/lib/types';

@Component({
  selector: 'neo-form-footer',
  templateUrl: 'form-footer.component.html',
  styleUrls: ['./form-footer.component.scss']
})
export class FormFooterComponent {
  @Input() disabled: boolean;

  @Input() cancelButtonName: string = 'general.cancelLabel';
  @Input() submitButtonName: string = 'userManagement.addUserLabel';

  @Input() icon: string;
  @Input() iconSize: keyof SVG_CONFIG['sizes'] = 'md';
  @Input() iconOnLeftSide: boolean;

  @Input() backButton: boolean;
  @Input() backButtonIcon: string;
  @Output() backButtonClick: EventEmitter<void> = new EventEmitter<void>();

  @Input() deleteButton: boolean;
  @Input() deleteButtonText: boolean;
  @Input() deleteButtonIcon: string;
  @Output() deleteButtonClick: EventEmitter<void> = new EventEmitter<void>();

  @Input() draftButton: boolean;
  @Input() draftButtonText: boolean;
  @Input() draftButtonDisabled: boolean;
  @Output() draftButtonClick: EventEmitter<void> = new EventEmitter<void>();

  @Output() cancel: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() save: EventEmitter<boolean> = new EventEmitter<boolean>();
}
