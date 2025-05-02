import { TableCrudEnum } from '../../table/enums/table-crud.enum';

export interface MenuOptionInterface {
  icon: string;
  name: string;
  link?: string;
  hidden?: boolean;
  operation?: TableCrudEnum;
  appSeperatorForLastElement?: boolean;
  iconSize?: string;
  customClass?: string;
  disabled?: boolean;
}

export const MENU_OPTIONS: MenuOptionInterface[] = [
  {
    icon: 'doc-magnify',
    name: 'actions.previewLabel',
    hidden: true,
    operation: TableCrudEnum.Preview,
    iconSize: '18px'
  },
  {
    icon: 'pencil',
    name: 'actions.editLabel',
    operation: TableCrudEnum.Edit
  },
  {
    icon: 'eye',
    name: 'actions.activateLabel',
    hidden: false,
    operation: TableCrudEnum.Status
  },
  {
    icon: 'crossed-eye',
    name: 'actions.deactivateLabel',
    hidden: true,
    operation: TableCrudEnum.Status
  },
  {
    icon: 'download',
    name: 'actions.downloadLabel',
    operation: TableCrudEnum.Download,
    hidden: true
  },
  {
    icon: 'trash-can-red',
    name: 'actions.deleteLabel',
    operation: TableCrudEnum.Delete,
    customClass: 'error-red-imp' //delete icon in project library
  }
];
