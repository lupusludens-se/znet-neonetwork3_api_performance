import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'neo-wizard-nav-controls',
  templateUrl: './wizard-nav-controls.component.html',
  styleUrls: ['./wizard-nav-controls.component.scss']
})
export class WizardNavControlsComponent {
  @Input() nextDisabled: boolean;
  @Input() showBackBtn: boolean = true;
  @Input() nextText: string = 'Next';
  @Output() goNext: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() goBack: EventEmitter<boolean> = new EventEmitter<boolean>();
}
