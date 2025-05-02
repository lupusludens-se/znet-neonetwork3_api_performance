import { UserInterface } from './user/user.interface';

export interface FileInterface {
  id?: number;
  type: number;
  extension: string;
  link: string;
  blobName: string;
  actualFileName: string;
  actualFileTitle: string;
  createdOn?: Date;
  modifiedOn?: Date;
  size: number;
  version: number;
  createdByUserId?: number;
  createdByUser?: UserInterface;
  updatedByUserId?: number;
  updatedByUser?: UserInterface;
}

export interface CompanyFileInterface extends FileInterface {
  isPrivate: boolean;
  modifiedBy?: number;
}
