import { AfterViewInit, Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';

import { CoreService } from '../../../core/services/core.service';
import pasteAsPlainText from 'paste-as-plain-text';

@Component({
  selector: 'neo-text-editor',
  templateUrl: 'text-editor.component.html',
  styleUrls: ['text-editor.component.scss']
})
export class TextEditorComponent implements AfterViewInit {
  @Input() classes: string = '';
  @Input() minHeight: string = '142px';
  @Input() maxHeight: string = '400px';
  @Input() maxLength: number;
  @Input() placeholder: string;
  @Input() emitDataOnEnter: boolean = true;
  @Input() clearOnEmit: boolean = true;
  @Input() editable: boolean = true;
  @Input() editorValue: string;

  @Output() emitValue: EventEmitter<string> = new EventEmitter<string>();
  @Output() focused: EventEmitter<void> = new EventEmitter<void>();
  @Output() blurred: EventEmitter<void> = new EventEmitter<void>();
  @Output() emitSymbolsCount: EventEmitter<number> = new EventEmitter<number>();
  @Output() emitLength: EventEmitter<number> = new EventEmitter<number>();

  @ViewChild('editor') editor: ElementRef;

  hidePlaceholder: boolean;

  constructor(private readonly coreService: CoreService) {}

  ngAfterViewInit(): void {
    if (this.editor?.nativeElement) {
      pasteAsPlainText(this.editor.nativeElement);
    }
    if (this.editor.nativeElement.innerHTML === '<br>') {
      this.emitSymbolsCount.emit(0);
    } else {
      this.emitSymbolsCount.emit(this.coreService.convertToPlain(this.editor.nativeElement?.innerHTML).length);
    }
    if (!this.maxLength) {
      this.maxLength = 4000;
    }
  }

  checkLength(event: KeyboardEvent): boolean {
    const length = (event.target as HTMLElement).innerHTML.trim().length;
    const specialButtons = [
      'Backspace',
      'Shift',
      'Ctrl',
      'Alt',
      'Delete',
      'ArrowLeft',
      'ArrowUp',
      'ArrowRight',
      'ArrowDown',
      'Enter',
      'Command',
      'Meta'
    ];

    if (length >= this.maxLength && !specialButtons.includes(event.key)) {
      event.preventDefault();
      return false;
    }

    this.emitLength.emit(length);
  }

  checkSymbols(event: KeyboardEvent): void {
    if ((event.target as HTMLElement).innerHTML === '<br>') {
      this.emitSymbolsCount.emit(0);
    } else {
      this.emitSymbolsCount.emit(this.coreService.convertToPlain((event.target as HTMLElement).innerHTML).length);
    }
  }

  async paste(event: KeyboardEvent): Promise<void> {
    event.preventDefault();

    const insertText = await window.navigator.clipboard.readText();
    const innerHTML = (event.target as HTMLElement).innerHTML;
    const freeSpace = this.maxLength - innerHTML.length;

    if (innerHTML.length >= 0 && freeSpace > 0 && insertText.length > freeSpace) {
      const pasteString = insertText.slice(0, freeSpace - 1);
      window.document.execCommand('insertText', false, pasteString);
    } else if (innerHTML.length >= 0 && freeSpace > 0 && insertText.length <= freeSpace) {
      window.document.execCommand('insertText', false, insertText);
    }

    let targetHTML = (event.target as HTMLElement).innerHTML;

    if (this.maxLength && targetHTML?.length > this.maxLength) {
      targetHTML = targetHTML.slice(0, this.maxLength);
      this.emitValue.emit(this.coreService.modifyEditorText(this.editor.nativeElement as HTMLElement));
    }

    this.emitSymbolsCount.emit(this.coreService.convertToPlain((event.target as HTMLElement).innerHTML).length);
  }

  undo(): void {
    window.document.execCommand('undo');
  }

  cut(): void {
    window.document.execCommand('cut');
  }

  copy(): void {
    window.document.execCommand('copy');
  }

  emitMessage(editor: HTMLElement): void {
    if (!this.emitDataOnEnter) {
      this.emitValue.emit(this.coreService.modifyEditorText(editor));
      this.cleanEditor(editor);
    }

    this.hidePlaceholder = false;
  }

  enterPress(event: KeyboardEvent): void {
    if (this.emitDataOnEnter) {
      event.preventDefault();
      this.emitValue.emit(this.coreService.modifyEditorText(event.target as HTMLElement));
      this.cleanEditor(event.target as HTMLElement);
    }
  }

  private cleanEditor(editor: HTMLElement): void {
    if (this.clearOnEmit) {
      editor.innerHTML = '';
    }
  }
}
