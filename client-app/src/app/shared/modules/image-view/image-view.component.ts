import { Component, EventEmitter, Input, Output } from '@angular/core';

import { AttachmentInterface } from '../../interfaces/attachment.interface';

@Component({
  selector: 'neo-image-view',
  templateUrl: './image-view.component.html',
  styleUrls: ['./image-view.component.scss']
})
export class ImageViewComponent {
  @Input() currentIndex: number = 0;
  @Input() images: AttachmentInterface[];
  @Output() closed = new EventEmitter<Record<string, string>>();
}
