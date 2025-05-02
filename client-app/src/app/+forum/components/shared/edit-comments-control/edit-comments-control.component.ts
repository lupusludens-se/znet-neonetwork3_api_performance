import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';

import { CoreService } from '../../../../core/services/core.service';

import { UserInterface } from '../../../../shared/interfaces/user/user.interface';
import { ForumEditCommentsInterface } from '../../../interfaces/forum-edit.interface';
import { TextEditorComponent } from '../../../../shared/modules/text-editor/text-editor.component';

@Component({
  selector: 'neo-edit-control',
  templateUrl: 'edit-comments-control.component.html',
  styleUrls: ['edit-comments-control.component.scss']
})
export class EditCommentControlComponent {
  @Input() user: UserInterface;
  @Input() placeholder: string;
  @Input() parentMessageId: number;
  @Input() editorValue: string;

  @Output() editResponse: EventEmitter<ForumEditCommentsInterface> = new EventEmitter<ForumEditCommentsInterface>();
  @Output() cancelClick: EventEmitter<void> = new EventEmitter<void>();

  @ViewChild('editorComponent') editorComponent: TextEditorComponent;

  constructor(private readonly coreService: CoreService) {}

  
  

  emitData(): void {
    this.editResponse.emit({
      parentMessageId: this.parentMessageId ? this.parentMessageId : 0,
      text: this.coreService.modifyEditorText(this.editorComponent.editor.nativeElement as HTMLElement)
    });
  }
}
