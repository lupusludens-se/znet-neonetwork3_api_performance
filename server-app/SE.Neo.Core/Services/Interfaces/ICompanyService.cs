using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<WrapperModel<CompanyDTO>> GetCompaniesAsync(BaseSearchFilterModel filter, int? userId = null);

        Task<CompanyDTO?> GetCompanyAsync(int id, int? userId = null, string? expand = null);

        Task<CompanyDTO> CreateUpdateCompanyAsync(int id, CompanyDTO modelDTO);

        Task<CompanyDTO> PatchCompanyAsync(int id, JsonPatchDocument patchDoc);

        Task<IEnumerable<BaseIdNameDTO>> GetIndustriesAsync();

        Task<IEnumerable<BaseIdNameDTO>> GetOffsitePPAsAsync();

        Task CreateCompanyFollowerAsync(int followerId, int companyId);

        Task RemoveCompanyFollowerAsync(int followerId, int companyId);

        Task<bool> IsCompanyIdExistAsync(int companyId);

        Task<bool> IsIndustryIdExistAsync(int industryId);

        bool IsCompanyNameUnique(string companyName, bool includeActiveStatusOnly = false, int? companyId = null);

        Task<CompanyDTO?> GetCompanyByName(string companyName);

        Task<WrapperModel<CompanyDTO>> GetFollowedCompaniesAsync(int userId, PaginationModel filter);

        Task<List<int>> GetCompanyFollowersIdsAsync(int companyId);

        Task<IEnumerable<CompanyDomainDTO>> GetCompanyDomains(int companyId);

        bool IsCompanyDomainExist(int companyId, string domainName);

        Task CreateCompanyDomainAsync(int companyId, string domainName);

        Task<WrapperModel<CompanyFileDTO>> GetSavedFilesOfACompanyAsync(int companyIdFromQS, BaseSearchFilterModel filter, int userId, bool isAdmin, bool isPrivate, int currentUserCompanyId);

        Task<bool> DeleteCompanyFileByTypeAsync(int currentUserId, int currentUserCompanyId, int fileId, bool IsPrivate, bool isAdmin);

        Task<string> GetBlobFileName(int fileId);

        Task<FileExistResponseDTO> ValidateFileCountAndIfExistsByCompanyIdAsync(int companyIdFromQS, string fileName, int currentUserId, bool isPrivateFile);

        Task<bool> UploadFileByCompanyIdAsync(CompanyFileDTO request, int currentUserId, int companyIdFromQS);

        Task<bool> UpdateCompanyFileModifiedDateAndSize(string fileName, int fileSize);

        Task<bool> UpdateFileTitleOfSelectedFileByCompany(string fileTitle, int fileId, int currentUserId, bool isAdmin, int currentUserCompanyId);
        Task<bool> DeleteCompanyAnnouncementAsync(int currentUserId, int announcementId, bool isAdmin);
        Task<WrapperModel<CompanyAnnouncementDTO>> GetAllCompanyAnnouncementsAsync(BaseSearchFilterModel filter, int companyId);
        Task<bool> CreateOrUpdateCompanyAnnouncementAsync(int announcementId, CompanyAnnouncementDTO companyAnnouncementsDTO,int currentUserId, bool isAdmin, int currentUserCompanyId);
        Task<CompanyAnnouncementDTO> GetCompanyAnnouncementByIdAsync(int id);
    }
}