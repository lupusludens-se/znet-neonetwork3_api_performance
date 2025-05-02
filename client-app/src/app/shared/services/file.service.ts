import { BehaviorSubject, Observable } from 'rxjs';
import { Injectable } from '@angular/core';

import { HttpService } from '../../core/services/http.service';
import { FileInterface } from '../interfaces/file.interface';
import { InitiativeApiEnum } from 'src/app/initiatives/enums/initiative-api.enum';
import { ImageInterface } from '../interfaces/image.interface';
import { BlobTypeEnum, GeneralApiEnum } from '../enums/api/general-api.enum';
import { PaginateResponseInterface } from '../interfaces/common/pagination-response.interface';
import { FileExtensionEnum } from '../enums/file-extension.enum';
import { FileExistInterface } from 'src/app/initiatives/+initatives/+view-initiative/interfaces/file-exist.interface';
import { FileBlobInterface } from '../interfaces/file-blob.interface';
import { PaginationInterface } from '../modules/pagination/pagination.component';
import { FileResponseInterface } from 'src/app/user-management/services/user.data.service';
import { CompanyApiEnum } from '../enums/api/company-api-enum';
import { RolesEnum } from '../enums/roles.enum';
import { UserInterface } from '../interfaces/user/user.interface';

@Injectable()
export class FileService {
  constructor(private httpService: HttpService) {}
  paging$: BehaviorSubject<PaginationInterface> = new BehaviorSubject<PaginationInterface>({
    take: 10,
    skip: 0,
    total: null
  });
  fileObj: FileResponseInterface;

  orderBy$: BehaviorSubject<string> = new BehaviorSubject<string>(null);

  getPaging() {
    return this.paging$.asObservable();
  }

  getOrderBy() {
    return this.orderBy$.asObservable();
  }

  setPaging(pageData: PaginationInterface) {
    this.paging$.next(pageData);
  }

  setOrderBy(orderBy: string) {
    this.orderBy$.next(orderBy);
  }

  saveInitiativeFile(fileData: FileInterface, initiativeId: number): Observable<boolean> {
    return this.httpService.post<boolean>(`${InitiativeApiEnum.SaveInitiativeFile}` + `/${initiativeId}`, fileData);
  }

  saveCompanyFile(fileData: FileInterface, companyId: number): Observable<boolean> {
    return this.httpService.post<boolean>(`${CompanyApiEnum.SaveCompanyFile}` + `/${companyId}`, fileData);
  }

  uploadFile(formData: FormData, fileBlob: FileBlobInterface): Observable<ImageInterface> {
    return this.httpService.post<ImageInterface>(
      GeneralApiEnum.Media,
      formData,
      {
        BlobType: fileBlob.blobType,
        BlobName: fileBlob.blobName,
        Overwrite: fileBlob.overwrite,
        NewFileName: fileBlob.newFileName
      },
      false
    );
  }

  getSavedFilesForInitiative(initiativeId: number, paging): Observable<PaginateResponseInterface<FileInterface>> {
    return this.httpService.get<PaginateResponseInterface<FileInterface>>(
      `${InitiativeApiEnum.GetInitiativeFiles}` + `/${initiativeId}`,
      paging
    );
  }

  isfileExtensionValid(fileExtension: string): boolean {
    return Object.values(FileExtensionEnum).includes(fileExtension);
  }

  isFileImage(fileExtension: string): boolean {
    return (
      fileExtension === FileExtensionEnum[FileExtensionEnum.jpg] ||
      fileExtension === FileExtensionEnum[FileExtensionEnum.jpeg] ||
      fileExtension === FileExtensionEnum[FileExtensionEnum.png]
    );
  }

  validateFileCountAndIsFileExist(initiativeId: number, fileName: string): Observable<[FileExistInterface, number]> {
    const encodedFileName = encodeURIComponent(fileName);
    return this.httpService.get<[FileExistInterface, number]>(
      `${InitiativeApiEnum.ValidateFileCountAndFileExistence}` + `/${initiativeId}/${encodedFileName}`
    );
  }

  validateFileCountAndIsFileExistByCompanyId(
    companyId: number,
    fileName: string,
    isPrivate: string
  ): Observable<FileExistInterface> {
    const formData = {
      companyId: +companyId,
      fileName: fileName,
      isPrivate: isPrivate
    };
    return this.httpService.post<FileExistInterface>(`${CompanyApiEnum.ValidateFileCountAndFileExistence}`, formData);
  }

  exportFile(blobName: string): Observable<any> {
    return this.httpService.downloadFile(`${GeneralApiEnum.MediaDownload}/${BlobTypeEnum.Initiative}/${blobName}`, {});
  }

  downloadFileByCompanyId(blobName: string): Observable<any> {
    return this.httpService.downloadFile(`${GeneralApiEnum.MediaDownload}/${BlobTypeEnum.Companies}/${blobName}`, {});
  }

  updateFileModifiedDateAndSize(fileName: string, fileSize: number, initiativeId: number): Observable<boolean> {
    return this.httpService.put<boolean>(
      `${InitiativeApiEnum.UpdateFileModifiedDateAndSize}` + `/${initiativeId}`,
      null,
      {
        fileName: fileName,
        fileSize: fileSize
      }
    );
  }

  updateFileModifiedDateAndSizeOfAFileByCompany(blobName: string, fileSize: number): Observable<boolean> {
    return this.httpService.put<boolean>(
      `${CompanyApiEnum.UpdateFileModifiedDateAndSize}` + `/${blobName}/${fileSize}`,
      null
    );
  }

  updateFileTitleOfSelectedFileByCompany(fileId: number, title: string): Observable<boolean> {
    let body = { fileTitle: title };
    return this.httpService.put<boolean>(`${CompanyApiEnum.UpdateFileTitle}` + `/${fileId}`, null, body);
  }

  clearAll() {
    this.paging$.next({
      take: 25,
      skip: 0,
      total: null
    });
    this.orderBy$.next(null);
  }

  deleteCompanyFile(fileId: number, isPrivate: boolean): Observable<boolean> {
    return this.httpService.delete<boolean>(`${CompanyApiEnum.DeleteFile}/${fileId}/`, { isPrivate });
  }

  hasAccessToDeleteForInitiativeFiles(
    currentUser: UserInterface,
    file: FileInterface,
    isInitiativeOwner: boolean,
    isInitiativeMember: boolean
  ): boolean {
    if (!currentUser) {
      return false;
    }
    const isAdminuser = currentUser.roles.some(role => role.id === RolesEnum.Admin && role.isSpecial);
    const isFileCreator = file.createdByUserId === currentUser.id;

    return isInitiativeOwner || (isInitiativeMember && isFileCreator) || (isAdminuser && isFileCreator);
  }
}
