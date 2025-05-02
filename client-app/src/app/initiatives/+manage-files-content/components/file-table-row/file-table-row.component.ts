import { Component, EventEmitter, Input, OnInit, Output, OnChanges, SimpleChanges } from '@angular/core';
import { FileTypeEnum } from 'src/app/shared/enums/file-type.enum';
import { FileInterface } from 'src/app/shared/interfaces/file.interface';
import { DatePipe } from '@angular/common';
import { MENU_OPTIONS, MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { FileExtensionEnum } from 'src/app/shared/enums/file-extension.enum';

@Component({
  selector: 'neo-file-table-row',
  templateUrl: './file-table-row.component.html',
  styleUrls: ['./file-table-row.component.scss'],
  providers: [DatePipe]
})
export class FileTableRowComponent implements OnInit, OnChanges {
  @Input() fileDetails: FileInterface;
  @Input() isAdmin: boolean = false;
  @Input() hasAccessToDeleteForInitiativeFiles: boolean = false;
  @Output() readonly deleteFileClick: EventEmitter<number> = new EventEmitter<number>();
  @Output() readonly downloadFileClick: EventEmitter<number> = new EventEmitter<number>();

  fileName: string;
  fileType: string;
  userStatuses = UserStatusEnum;

  readonly menuOptions: MenuOptionInterface[] = MENU_OPTIONS;
  options: MenuOptionInterface[] = [];

  constructor() {}

  ngOnInit(): void {
    this.updateFileDetails();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.fileDetails || changes.isAdmin) {
      this.updateFileDetails();
    }
  }

  private updateFileDetails(): void {
    this.fileName = `${
      this.fileDetails.actualFileTitle + (this.fileDetails.version > 0 ? '(' + this.fileDetails.version + ')' : '')
    }.${FileExtensionEnum[this.fileDetails.extension]}`;
    this.fileType = FileTypeEnum[this.fileDetails.type];
    this.options = this.hasAccessToDeleteForInitiativeFiles ? this.getOptions() : this.getAdminOptions();
  }
  getOptions(): MenuOptionInterface[] {
    return this.menuOptions.map(opt => ({
      ...opt,
      hidden: ['actions.editLabel', 'actions.activateLabel', 'actions.previewLabel'].includes(opt.name)
        ? true
        : opt.name === 'actions.downloadLabel'
        ? false
        : opt.hidden
    }));
  }

  getAdminOptions(): MenuOptionInterface[] {
    return [
      {
        icon: 'download',
        name: 'actions.downloadLabel',
        operation: TableCrudEnum.Download,
        hidden: false
      }
    ];
  }

  getFileKey(type: number): string {
    return type === FileTypeEnum.Image ? 'image' : 'file';
  }

  optionClick(option: MenuOptionInterface): void {
    switch (option.operation) {
      case TableCrudEnum.Delete:
        this.deleteFile();
        break;
      case TableCrudEnum.Download:
        this.downloadFile();
        break;
    }
  }

  private deleteFile(): void {
    this.deleteFileClick.emit(this.fileDetails.id);
  }

  private downloadFile(): void {
    this.downloadFileClick.emit(this.fileDetails.id);
  }
}
