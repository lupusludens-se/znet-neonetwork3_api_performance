import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { AfterViewInit, ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { BULLET_SYMBOL } from '../../../shared/constants/symbols.const';

@Component({
  selector: 'neo-highlights',
  templateUrl: 'highlights.component.html',
  styleUrls: ['../+create-event/create-event.component.scss', 'highlights.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class HighlightsComponent implements AfterViewInit {
  @Input() highlights: string;

  previousLength: number = 0;
  private bulletIdentation: string = `${BULLET_SYMBOL} `;

  constructor(private controlContainer: ControlContainer) {}

  ngAfterViewInit() {
    if (this.controlContainer.control.get('highlights').value) {
      this.handleInput(this.controlContainer.control.get('highlights').value);
    }
  }

  checkValidity(event): void {
    if (event.target?.value?.trim() === BULLET_SYMBOL) {
      this.previousLength = 0;
      event.target.value = '';
    }
  }

  handleInput(event): void {
    if (event.target) {
      const newLength = event.target.value.length;
      const characterCode = event.target.value.substring(newLength - 1).charCodeAt(0);

      if (newLength > this.previousLength) {
        if (characterCode === 10) {
          // * 'new line'
          event.target.value = `${event.target.value}${this.bulletIdentation}`;
        } else if (newLength === 1) {
          event.target.value = `${this.bulletIdentation}${event.target.value}`;
        }
      }

      this.previousLength = newLength;
      this.controlContainer.control.get('highlights').patchValue(event.target.value);
    }
  }

  onPaste(event): void {
    event.preventDefault();
    let pastedText = (event.clipboardData || (<any>window).clipboardData).getData('text');
    if (!pastedText) {
      return;
    }

    const editor = event.target;
    const textBeforeCursor = editor.value?.substring(0, editor.selectionStart) || '';
    const textAfterCursor = editor.value?.substring(editor.selectionEnd, editor.value.lenght) || '';

    pastedText = this.addBulletsToNewLines(pastedText);
    pastedText = this.addBulletToFirstLine(pastedText, textBeforeCursor);
    pastedText = this.clearBulletsWithSpaceBefore(pastedText);

    const newValue = textBeforeCursor + pastedText + textAfterCursor;
    this.controlContainer.control.get('highlights').patchValue(newValue);
    this.previousLength = newValue.length;
  }

  private addBulletsToNewLines(text: string): string {
    const newLineWithoutStartBullets = new RegExp(`(\r\n|\r|\n)(?!(\\s*${BULLET_SYMBOL}))`, 'g');
    return text.replace(newLineWithoutStartBullets, `$1${this.bulletIdentation}`);
  }

  private addBulletToFirstLine(text: string, textBeforeCursor: string): string {
    const newLineWithSpacesAtEnd = /(\r\n|\r|\n)\s*$/;
    const bulletWithSpacesAtStart = new RegExp(`^\\s*${BULLET_SYMBOL}`);
    return (!textBeforeCursor?.trim() || newLineWithSpacesAtEnd.test(textBeforeCursor)) &&
      !bulletWithSpacesAtStart.test(text)
      ? this.bulletIdentation + text
      : text;
  }

  private clearBulletsWithSpaceBefore(text: string): string {
    return text.replace(new RegExp(`(\r\n|\r|\n)\\s*(${BULLET_SYMBOL})`, 'g'), '$1$2');
  }
}
