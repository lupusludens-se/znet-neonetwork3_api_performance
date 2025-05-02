using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models;
using SE.Neo.WebAPI.Models.Company;
using SE.Neo.WebAPI.Models.Initiative;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface ICompanyApiService
    {
        Task<WrapperModel<CompanyResponse>> GetCompaniesAsync(BaseSearchFilterModel filter, int userId);

        Task<CompanyResponse?> GetCompanyAsync(int id, int userId, string? expand);

        Task PatchCompanyAsync(int id, JsonPatchDocument patchDoc);

        Task<int> CreateUpdateCompanyAsync(int id, CompanyRequest viewModel);

        Task<IEnumerable<BaseIdNameResponse>> GetIndustriesAsync();

        Task<IEnumerable<BaseIdNameResponse>> GetOffsitePPAsAsync();

        Task CreateCompanyFollowerAsync(int followerId, int companyId);

        Task RemoveCompanyFollowerAsync(int followerId, int companyId);

        Task<WrapperModel<CompanyResponse>> GetFollowedCompaniesAsync(int userId, PaginationModel filter);

        Task<WrapperModel<CompanyFileResponse>> GetSavedFilesOfACompanyAsync(int companyIdFromQS, UserModel currentUser, BaseSearchFilterModel filter, bool isPrivate);

        Task<bool> DeleteCompanyFileByTypeAsync(int currentUserId, int currentUserCompanyId, int fileId, bool IsPrivate, bool isAdmin);

        Task<FileExistResponse> ValidateFileCountAndIfExistsByCompanyIdAsync(int companyIdFromQS, string fileName, UserModel currentUser, bool isPrivateFile);


        Task<bool> UploadFileByCompanyIdAsync(CompanyFileRequest request, int currentUserId, int companyIdFromQS);

        Task<bool> UpdateCompanyFileModifiedDateAndSize(string fileName, int fileSize);

        Task<bool> UpdateFileTitleOfSelectedFileByCompany(string fileTitle, int fileId, int currentUserId, bool isAdmin, int currentUserCompanyId);
        Task<bool> DeleteCompanyAnnouncementAsync(int currentUserId, int announcementId, bool isAdmin);
        Task<WrapperModel<CompanyAnnouncementResponse>> GetAllCompanyAnnouncementsAsync(BaseSearchFilterModel filter, int companyId);
        Task<bool> CreateOrUpdateCompanyAnnouncementAsync(int announcementId, CompanyAnnouncementCreateOrUpdateRequest request, UserModel currentUser);

        Task<CompanyAnnouncementResponse> GetCompanyAnnouncementByIdAsync(int id);
    }
}