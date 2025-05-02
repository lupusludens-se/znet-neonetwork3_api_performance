// modules
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared.module';
import { TextEditorModule } from '../text-editor/text-editor.module';
import { ImageViewModule } from '../image-view/image-view.module';

// components
import { MessageControlComponent } from './components/message-comtrol/message-control.component';
import { ControlErrorModule } from '../controls/control-error/control-error.module';

@NgModule({
  declarations: [MessageControlComponent],
  exports: [MessageControlComponent],
  imports: [CommonModule, SharedModule, FormsModule, TextEditorModule, ControlErrorModule, ImageViewModule]
})
export class MessageControlModule {}
