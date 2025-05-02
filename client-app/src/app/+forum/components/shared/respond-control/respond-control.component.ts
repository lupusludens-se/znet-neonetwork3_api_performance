import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';

import { CoreService } from '../../../../core/services/core.service';

import { UserInterface } from '../../../../shared/interfaces/user/user.interface';
import { RespondInterface } from '../../../interfaces/respond.interface';
import { TextEditorComponent } from '../../../../shared/modules/text-editor/text-editor.component';

@Component({
  selector: 'neo-respond-control',
  templateUrl: 'respond-control.component.html',
  styleUrls: ['respond-control.component.scss']
})
export class RespondControlComponent {
  @Input() user: UserInterface;
  @Input() placeholder: string;
  @Input() parentMessageId: number;
  @Input() editorValue: string;

  @Output() respond: EventEmitter<RespondInterface> = new EventEmitter<RespondInterface>();
  @Output() cancelClick: EventEmitter<void> = new EventEmitter<void>();

  @ViewChild('editorComponent') editorComponent: TextEditorComponent;

  constructor(private readonly coreService: CoreService) {}

  emitData(): void {
    const text = this.coreService.modifyEditorText(this.editorComponent.editor.nativeElement as HTMLElement);
    this.respond.emit({
      parentMessageId: this.parentMessageId ? this.parentMessageId : 0,
      text: text,
      textContent: this.coreService.convertToPlain(text ?? '')

    });
  }
}
