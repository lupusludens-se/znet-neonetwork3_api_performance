import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MAX_FILE_SIZE } from 'src/app/shared/constants/image-size.const';
import { FileService } from 'src/app/shared/services/file.service';
import { CustomValidator } from 'src/app/shared/validators/custom.validator';

@Component({
  selector: 'neo-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent implements OnInit, AfterViewInit {
  @Output() readonly closed = new EventEmitter<void>();
  @Input() infoTitle: string;
  @Input() fileDescription: string;
  @Output() fileSelected = new EventEmitter<{ event: Event; title: string; isEditMode: boolean }>();
  @Input() existingFileName?: string;
  @Input() existingFileTile?: string;
  @ViewChild('fileInput') fileInput: ElementRef;
  titleMaxLength: number = 100;
  readonly fileTitle = new FormControl(null, [
    CustomValidator.required,
    CustomValidator.preventWhitespacesOnly,
    Validators.maxLength(this.titleMaxLength),
    Validators.pattern(/^[^\\\/:*?"<>|]*$/) // Regular expression to disallow specific special characters
  ]);
  selectedFileDetails: Event;
  isFileTitleEmpty: boolean = false;
  isFileSelected: boolean = true;
  selectedFileName: string;
  fileRequiredError: string = 'uploadFile.fileRequiredLabel';
  fileExtension: string;
  maxFileSize = MAX_FILE_SIZE;
  isSaveButtonClicked: boolean = false;

  file: File;

  constructor(private fileService: FileService) {}

  ngOnInit(): void {
    if (this.existingFileName) {
      this.fileTitle.setValue(this.existingFileTile);
      this.selectedFileName = this.existingFileName;
    }
  }

  ngAfterViewInit() {
    this.fileInput.nativeElement.addEventListener('cancel', () => {
      this.clearFileSelection();
    });
  }

  onFileSelect(event: Event) {
    this.selectedFileDetails = event;
    this.isFileSelected = true;
    const input = event.target as HTMLInputElement;
    const file: File = input.files[0];
    this.file = file;
    if (file) {
      this.selectedFileName = file.name;
      this.fileExtension = file.name.substring(file.name.lastIndexOf('.') + 1);

      if (file.size > this.maxFileSize) {
        this.clearFileSelection();
        this.fileRequiredError = 'general.wrongLargeFileSizeLabel';
        return;
      }

      const supported = this.fileService.isfileExtensionValid(this.fileExtension);

      if (!supported) {
        this.clearFileSelection();
        this.fileRequiredError = 'uploadFile.wrongFileTypeLabel';
        return;
      }
    }
  }

  clearFileSelection() {
    if (!this.existingFileName) {
      this.isFileSelected = false;
      this.selectedFileDetails = null;
      this.selectedFileName = null;
    }
  }

  saveFile() {
    this.isSaveButtonClicked = true;
    if (!this.existingFileName) {
      if (this.fileTitle.invalid || !this.selectedFileDetails) {
        this.isFileTitleEmpty = this.fileTitle.invalid;
        if (this.selectedFileDetails) this.isFileSelected = true;
        else this.isFileSelected = false;
        return;
      }
      this.fileSelected.emit({ event: this.selectedFileDetails, title: this.fileTitle.value, isEditMode: false });
    } else {
      if (this.fileTitle.invalid) {
        this.isFileTitleEmpty = this.fileTitle.invalid;
        return;
      }
      this.fileSelected.emit({ event: this.selectedFileDetails, title: this.fileTitle.value, isEditMode: true });
    }
  }
}
