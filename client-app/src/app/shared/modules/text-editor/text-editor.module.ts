// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// components
import { TextEditorComponent } from './text-editor.component';

@NgModule({
  declarations: [TextEditorComponent],
  exports: [TextEditorComponent],
  imports: [CommonModule]
})
export class TextEditorModule {}
