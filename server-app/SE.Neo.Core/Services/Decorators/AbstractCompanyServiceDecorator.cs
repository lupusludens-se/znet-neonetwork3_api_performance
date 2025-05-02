using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services.Decorators
{
    public abstract class AbstractCompanyServiceDecorator : ICompanyService
    {
        protected readonly ICompanyService _companyService;

        public AbstractCompanyServiceDecorator(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public virtual async Task<WrapperModel<CompanyDTO>> GetCompaniesAsync(BaseSearchFilterModel filter, int? userId = null)
        {
            return await _companyService.GetCompaniesAsync(filter, userId);
        }

        public virtual async Task<CompanyDTO?> GetCompanyAsync(int id, int? userId = null, string? expand = null)
        {
            return await _companyService.GetCompanyAsync(id, userId, expand);
        }

        public virtual async Task<CompanyDTO> CreateUpdateCompanyAsync(int id, CompanyDTO modelDTO)
        {
            return await _companyService.CreateUpdateCompanyAsync(id, modelDTO);
        }

        public virtual async Task<CompanyDTO> PatchCompanyAsync(int id, JsonPatchDocument patchDoc)
        {
            return await _companyService.PatchCompanyAsync(id, patchDoc);
        }

        public virtual async Task CreateCompanyFollowerAsync(int followerId, int companyId)
        {
            await _companyService.CreateCompanyFollowerAsync(followerId, companyId);
        }

        public virtual async Task RemoveCompanyFollowerAsync(int followerId, int companyId)
        {
            await _companyService.RemoveCompanyFollowerAsync(followerId, companyId);
        }

        public virtual async Task<IEnumerable<BaseIdNameDTO>> GetIndustriesAsync()
        {
            return await _companyService.GetIndustriesAsync();
        }

        public virtual async Task<IEnumerable<BaseIdNameDTO>> GetOffsitePPAsAsync()
        {
            return await _companyService.GetOffsitePPAsAsync();
        }

        public virtual async Task<bool> IsCompanyIdExistAsync(int companyId)
        {
            return await _companyService.IsCompanyIdExistAsync(companyId);
        }

        public virtual bool IsCompanyNameUnique(string companyName, bool includeActiveStatusOnly = false, int? companyId = null)
        {
            return _companyService.IsCompanyNameUnique(companyName, includeActiveStatusOnly, companyId);
        }

        public virtual async Task<CompanyDTO?> GetCompanyByName(string companyName)
        {
            return await _companyService.GetCompanyByName(companyName);
        }

        public virtual async Task<bool> IsIndustryIdExistAsync(int industryId)
        {
            return await _companyService.IsIndustryIdExistAsync(industryId);
        }

        public virtual async Task<WrapperModel<CompanyDTO>> GetFollowedCompaniesAsync(int userId, PaginationModel filter)
        {
            return await _companyService.GetFollowedCompaniesAsync(userId, filter);
        }

        public virtual async Task<List<int>> GetCompanyFollowersIdsAsync(int companyId)
        {
            return await _companyService.GetCompanyFollowersIdsAsync(companyId);
        }

        public async Task<IEnumerable<CompanyDomainDTO>> GetCompanyDomains(int companyId)
        {
            return await _companyService.GetCompanyDomains(companyId);
        }

        public virtual bool IsCompanyDomainExist(int companyId, string domainName)
        {
            return _companyService.IsCompanyDomainExist(companyId, domainName);
        }

        public Task CreateCompanyDomainAsync(int companyId, string domainName)
        {
            return _companyService.CreateCompanyDomainAsync(companyId, domainName);
        }

        public async Task<WrapperModel<CompanyFileDTO>> GetSavedFilesOfACompanyAsync(int companyIdFromQS, BaseSearchFilterModel filter, int userId, bool isAdmin, bool isPrivate, int currentUserCompanyId)
        {
            return await _companyService.GetSavedFilesOfACompanyAsync(companyIdFromQS, filter, userId, isAdmin, isPrivate, currentUserCompanyId);
        } 

        public virtual async Task<bool> DeleteCompanyFileByTypeAsync(int currentUserId, int currentUserCompanyId, int fileId, bool IsPrivate, bool isAdmin)
        {
            return await _companyService.DeleteCompanyFileByTypeAsync(currentUserId, currentUserCompanyId, fileId, IsPrivate, isAdmin);
        }

        public async Task<string> GetBlobFileName(int fileId)
        {
            return await _companyService.GetBlobFileName(fileId);
        }

        public async Task<FileExistResponseDTO> ValidateFileCountAndIfExistsByCompanyIdAsync(int companyIdFromQS, string fileName, int currentUserId, bool isPrivateFile)
        {
            return await _companyService.ValidateFileCountAndIfExistsByCompanyIdAsync(companyIdFromQS, fileName, currentUserId,isPrivateFile);
        }

        public virtual async Task<bool> UploadFileByCompanyIdAsync(CompanyFileDTO model, int currentUserId, int companyIdFromQS)
        {
            return await _companyService.UploadFileByCompanyIdAsync(model, currentUserId, companyIdFromQS);
        }

        public virtual async Task<bool> UpdateCompanyFileModifiedDateAndSize(string fileName, int fileSize)
        {
            return await _companyService.UpdateCompanyFileModifiedDateAndSize(fileName, fileSize);
        }

        public virtual async Task<bool> UpdateFileTitleOfSelectedFileByCompany(string fileTtile, int fileId, int currentUserId, bool isAdmin, int currentUserCompanyId)
        {
            return await _companyService.UpdateFileTitleOfSelectedFileByCompany(fileTtile, fileId, currentUserId, isAdmin, currentUserCompanyId);
        }

        public virtual async Task<bool> DeleteCompanyAnnouncementAsync(int currentUserId, int announcementId, bool isAdmin)
        {
            return await _companyService.DeleteCompanyAnnouncementAsync(currentUserId, announcementId, isAdmin);
        }

        public virtual async Task<WrapperModel<CompanyAnnouncementDTO>> GetAllCompanyAnnouncementsAsync(BaseSearchFilterModel filter, int companyId)
        {
            return await _companyService.GetAllCompanyAnnouncementsAsync(filter, companyId);
        }

        public virtual async Task<bool> CreateOrUpdateCompanyAnnouncementAsync(int announcementId, CompanyAnnouncementDTO companyAnnouncementsDTO,int currentUserId, bool isAdmin, int currentUserCompanyId)
        {
            return await _companyService.CreateOrUpdateCompanyAnnouncementAsync(announcementId, companyAnnouncementsDTO, currentUserId, isAdmin, currentUserCompanyId);
        }

        public async Task<CompanyAnnouncementDTO> GetCompanyAnnouncementByIdAsync(int id)
        {
            return await _companyService.GetCompanyAnnouncementByIdAsync(id);
        }
    }
}