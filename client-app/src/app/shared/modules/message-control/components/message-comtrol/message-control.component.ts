import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { NG_VALIDATORS, NG_VALUE_ACCESSOR } from '@angular/forms';

import { BaseControlDirective } from '../../../controls/base-control.component';

import { SnackbarService } from '../../../../../core/services/snackbar.service';
import { ImageInterface } from '../../../../interfaces/image.interface';
import { MAX_IMAGE_SIZE } from '../../../../constants/image-size.const';
import { AttachmentInterface } from '../../../../interfaces/attachment.interface';

export interface MessageControlInterface {
  value: string;
  attachments: {
    text: string;
    link: string;
    type: number;
    imageName: string;
  }[];
}

@Component({
  selector: 'neo-message-control',
  templateUrl: './message-control.component.html',
  styleUrls: ['./message-control.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => MessageControlComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => MessageControlComponent),
      multi: true
    }
  ]
})
export class MessageControlComponent extends BaseControlDirective {
  @Input() height: string = '142px';
  @Input() maxHeight: string = '400px';
  @Input() attachments: ImageInterface[];
  @Input() hasError: boolean;
  @Input() isLoading: boolean;
  @Input() editable: boolean = true;
  @Input() displayAttachments: boolean = true;
  @Input() classes: string = '';
  @Input() placeholder: string = '';

  @Output() valueChanged: EventEmitter<MessageControlInterface> = new EventEmitter<MessageControlInterface>();
  @Output() symbolsCount: EventEmitter<number> = new EventEmitter<number>();
  @Output() length: EventEmitter<number> = new EventEmitter<number>();
  @Output() fileSelected: EventEmitter<FormData> = new EventEmitter<FormData>();

  maxImageSize: number = MAX_IMAGE_SIZE;
  imageViewModal: boolean;
  currentImageIndex: number;

  constructor(private readonly snackbarService: SnackbarService) {
    super();
  }

  onFileSelect(event): void {
    if (event.target.files.length > 0) {
      const file: File = event.target.files[0];
      const isLarge = file.size > this.maxImageSize;

      if (isLarge) {
        this.snackbarService.showError('general.largeFileLabel');
        return;
      }

      const formData: FormData = new FormData();
      formData.append('file', file);

      this.fileSelected.emit(formData);
    }
  }

  removeAttachment(index: number): void {
    this.attachments.splice(index, 1);
  }

  getAttachments(): AttachmentInterface[] {
    return this.attachments.map(
      attachment =>
        ({
          image: attachment
        } as AttachmentInterface)
    );
  }
}
