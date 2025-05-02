import { Component, EventEmitter, Input, OnInit, Output, OnChanges, SimpleChanges } from '@angular/core';
import { FileTabEnum, FileTypeEnum } from 'src/app/shared/enums/file-type.enum';
import { CompanyFileInterface } from 'src/app/shared/interfaces/file.interface';
import { DatePipe } from '@angular/common';
import { MENU_OPTIONS, MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { ActivatedRoute } from '@angular/router';
import { FileExtensionEnum } from 'src/app/shared/enums/file-extension.enum';

@Component({
  selector: 'neo-company-file-table-row',
  templateUrl: './company-file-table-row.component.html',
  styleUrls: ['./company-file-table-row.component.scss'],
  providers: [DatePipe]
})
export class CompanyFileTableRowComponent implements OnInit, OnChanges {
  @Input() fileDetails: CompanyFileInterface;
  @Input() tabSelected: FileTabEnum;
  @Input() currentUser: UserInterface;
  @Input() isAdmin: boolean = false;
  @Output() readonly deleteFileClick = new EventEmitter<number>();
  @Output() readonly downloadFileClick = new EventEmitter<number>();
  @Output() readonly editFileClick = new EventEmitter<number>();

  fileName: string;
  fileType: string;
  readonly menuOptions: MenuOptionInterface[] = MENU_OPTIONS;
  options: MenuOptionInterface[] = [
    {
      icon: 'pencil',
      name: 'actions.editLabel',
      operation: TableCrudEnum.Edit,
      hidden: false
    },
    {
      icon: 'download',
      name: 'actions.downloadLabel',
      operation: TableCrudEnum.Download,
      hidden: false
    },
    {
      icon: 'trash-can-red',
      name: 'actions.deleteLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp',
      hidden: false
    }
  ];
  companyId: number;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.companyId = params['id'];
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.fileDetails || changes.isAdmin) {

      this.route.params.subscribe(params => {
        this.companyId = params['id'];
        this.updateFileDetails();
        this.updateOptionsVisibility();
      });
    }
  }

  private updateFileDetails(): void {
    this.fileName = `${this.fileDetails.actualFileTitle}${this.fileDetails.version > 0 ? `(${this.fileDetails.version})` : ''
      }.${FileExtensionEnum[this.fileDetails.extension]}`;
    this.fileType = FileTypeEnum[this.fileDetails.type];
  }

  private updateOptionsVisibility(): void {
    const canEditAndDelete = this.isUserIsAdminOrSpAdminUserFromSameCompany();

    this.options = this.options.map(option => ({
      ...option,
      hidden: (option.operation === TableCrudEnum.Delete || option.operation === TableCrudEnum.Edit) ? !canEditAndDelete : option.hidden
    }));
  }

  isUserIsAdminOrSpAdminUserFromSameCompany(): boolean {
    return this.isInRole(this.currentUser, RolesEnum.Admin) || (this.currentUser?.companyId.toString() === this.companyId.toString() && this.isInRole(this.currentUser, RolesEnum.SPAdmin));
  }

  getFileKey(type: number): string {
    return type === FileTypeEnum.Image ? 'image' : 'file';
  }

  optionClick(option: MenuOptionInterface): void {
    switch (option.operation) {
      case TableCrudEnum.Delete:
        this.deleteFileClick.emit(this.fileDetails.id);
        break;
      case TableCrudEnum.Download:
        this.downloadFileClick.emit(this.fileDetails.id);
        break;
      case TableCrudEnum.Edit:
        this.editFileClick.emit(this.fileDetails.id);
        break;
    }
  }

  private isInRole(currentUser: UserInterface, roleId: RolesEnum): boolean {
    return currentUser.roles.some(role => role.id === roleId && role.isSpecial);
  }
}
