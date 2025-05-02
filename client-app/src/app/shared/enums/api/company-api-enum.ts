export enum CompanyApiEnum {
  Companies = 'companies',
  GetCompaniesPrivateFiles = 'get-saved-private-files',
  GetCompaniesPublicFiles = 'get-saved-public-files',
  GetSPCompanyActiveProjects = 'get-sp-company-active-projects',
  ValidateFileCountAndFileExistence = 'validate-file-count-and-file-existence',
  SaveCompanyFile = 'upload-file',
  DeleteFile = 'delete-file',
  UpdateFileModifiedDate = 'update-company-file-modified-date',
  UpdateFileModifiedDateAndSize = 'update-company-file-modified-date-size',
  UpdateFileTitle = 'update-company-file-title',
  CreateUpdateAnnouncement = 'create-or-update',
  GetCompanyAnnouncements = 'get-all-company-announcements',
  DeleteAnnouncement = 'delete-announcement',
  GetCompanyAnnouncementById = 'get-company-announcement-by-id'
}
