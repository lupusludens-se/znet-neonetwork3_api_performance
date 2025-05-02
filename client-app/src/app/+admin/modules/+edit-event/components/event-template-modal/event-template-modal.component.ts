import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'neo-event-template-modal',
  templateUrl: './event-template-modal.component.html',
  styleUrls: ['./event-template-modal.component.scss']
})
export class EventTemplateModalComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    confirmationMessageId: ['1']
  });
  @Input() title: string;
  @Input() showRadioButtons: boolean;
  @Input() showTitle: boolean;
  @Input() confirmationMessages: any;
  @Input() selectedConfirmationId: number;

  @Output() confirmClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() cancelClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() confirmSelection: EventEmitter<number> = new EventEmitter<number>();

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.form.patchValue({
      confirmationMessageId: this.selectedConfirmationId
    });
    this.form.controls['confirmationMessageId'].valueChanges.subscribe(v => {
      this.selectedConfirmationId = v.value;
    });
  }
}
