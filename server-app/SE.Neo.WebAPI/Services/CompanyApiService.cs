using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models;
using SE.Neo.WebAPI.Models.Company;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class CompanyApiService : ICompanyApiService
    {
        private readonly ILogger<CompanyApiService> _logger;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;

        public CompanyApiService(ILogger<CompanyApiService> logger, IMapper mapper, ICompanyService companyService)
        {
            _logger = logger;
            _mapper = mapper;
            _companyService = companyService;
        }

        public async Task<WrapperModel<CompanyResponse>> GetCompaniesAsync(BaseSearchFilterModel filter, int userId)
        {
            WrapperModel<CompanyDTO> companiesResult = await _companyService.GetCompaniesAsync(filter, userId);

            var wrapper = new WrapperModel<CompanyResponse>
            {
                Count = companiesResult.Count,
                DataList = companiesResult.DataList.Select(_mapper.Map<CompanyResponse>)
            };
            return wrapper;
        }

        public async Task<int> CreateUpdateCompanyAsync(int id, CompanyRequest model)
        {
            var modelDTO = _mapper.Map<CompanyDTO>(model);
            var companyId = (await _companyService.CreateUpdateCompanyAsync(id, modelDTO)).Id;
            return companyId;
        }

        public async Task PatchCompanyAsync(int id, JsonPatchDocument patchDoc)
        {
            await _companyService.PatchCompanyAsync(id, patchDoc);
        }

        public async Task<CompanyResponse?> GetCompanyAsync(int id, int userId, string? expand)
        {
            var modelDTO = await _companyService.GetCompanyAsync(id, userId, expand);
            return _mapper.Map<CompanyResponse>(modelDTO);
        }

        public async Task<IEnumerable<BaseIdNameResponse>> GetIndustriesAsync()
        {
            IEnumerable<BaseIdNameDTO> industries = await _companyService.GetIndustriesAsync();
            return industries.Select(_mapper.Map<BaseIdNameResponse>);
        }

        public async Task<IEnumerable<BaseIdNameResponse>> GetOffsitePPAsAsync()
        {
            IEnumerable<BaseIdNameDTO> industries = await _companyService.GetOffsitePPAsAsync();
            return industries.Select(_mapper.Map<BaseIdNameResponse>);
        }

        public async Task CreateCompanyFollowerAsync(int followerId, int companyId)
        {
            await _companyService.CreateCompanyFollowerAsync(followerId, companyId);
        }

        public async Task RemoveCompanyFollowerAsync(int followerId, int companyId)
        {
            await _companyService.RemoveCompanyFollowerAsync(followerId, companyId);
        }

        public async Task<WrapperModel<CompanyResponse>> GetFollowedCompaniesAsync(int userId, PaginationModel filter)

        {
            WrapperModel<CompanyDTO> companiesResult = await _companyService.GetFollowedCompaniesAsync(userId, filter);
            var wrapper = new WrapperModel<CompanyResponse>
            {
                Count = companiesResult.Count,
                DataList = companiesResult.DataList.Select(_mapper.Map<CompanyResponse>)
            };
            return wrapper;
        }

        public async Task<WrapperModel<CompanyFileResponse>> GetSavedFilesOfACompanyAsync(int companyIdFromQS, UserModel currentUser, BaseSearchFilterModel filter, bool isPrivate)
        {
            WrapperModel<CompanyFileDTO> privateFiles = await _companyService.GetSavedFilesOfACompanyAsync(companyIdFromQS, filter, currentUser.Id, currentUser.RoleIds.Contains((int)RoleType.Admin), isPrivate, currentUser.CompanyId);
            var wrapper = new WrapperModel<CompanyFileResponse>
            {
                Count = privateFiles.Count,
                DataList = privateFiles.DataList.Select(_mapper.Map<CompanyFileResponse>)
            };
            return wrapper;
        }

        public async Task<bool> DeleteCompanyFileByTypeAsync(int currentUserId, int currentUserCompanyId, int fileId, bool IsPrivate, bool isAdmin)
        {
            return await _companyService.DeleteCompanyFileByTypeAsync(currentUserId, currentUserCompanyId, fileId, IsPrivate, isAdmin);
        }

        public async Task<FileExistResponse> ValidateFileCountAndIfExistsByCompanyIdAsync(int companyIdFromQS, string fileName, UserModel currentUser, bool isPrivateFile)
        {
            var fileExistResponse = await _companyService.ValidateFileCountAndIfExistsByCompanyIdAsync(companyIdFromQS, fileName, currentUser.Id, isPrivateFile);
            return _mapper.Map<FileExistResponse>(fileExistResponse);
        }

        public async Task<bool> UploadFileByCompanyIdAsync(CompanyFileRequest request, int currentUserId, int companyIdFromQS)
        {
            var companyFileDTO = _mapper.Map<CompanyFileDTO>(request);
            return await _companyService.UploadFileByCompanyIdAsync(companyFileDTO, currentUserId, companyIdFromQS);
        }

        public async Task<bool> UpdateCompanyFileModifiedDateAndSize(string fileName, int fileSize)
        {
            return await _companyService.UpdateCompanyFileModifiedDateAndSize(fileName, fileSize);
        }


        public async Task<bool> UpdateFileTitleOfSelectedFileByCompany(string fileTitle, int fileId, int currentUserId, bool isAdmin, int currentUserCompanyId)
        {
            return await _companyService.UpdateFileTitleOfSelectedFileByCompany(fileTitle, fileId, currentUserId, isAdmin, currentUserCompanyId);
        }
        public async Task<bool> DeleteCompanyAnnouncementAsync(int currentUserId, int announcementId, bool isAdmin)
        {
            return await _companyService.DeleteCompanyAnnouncementAsync(currentUserId, announcementId, isAdmin);
        }

        public async Task<WrapperModel<CompanyAnnouncementResponse>> GetAllCompanyAnnouncementsAsync(BaseSearchFilterModel filter, int companyId)
        {
            WrapperModel<CompanyAnnouncementDTO> companyAnnouncements = await _companyService.GetAllCompanyAnnouncementsAsync(filter, companyId);
            var wrapper = new WrapperModel<CompanyAnnouncementResponse>
            {
                Count = companyAnnouncements.Count,
                DataList = companyAnnouncements.DataList.Select(_mapper.Map<CompanyAnnouncementResponse>)
            };
            return wrapper;
        }

        public async Task<CompanyAnnouncementResponse> GetCompanyAnnouncementByIdAsync(int id)
        {
            var companyAnnouncementDto = await _companyService.GetCompanyAnnouncementByIdAsync(id);
            return _mapper.Map<CompanyAnnouncementResponse>(companyAnnouncementDto);
        }

        public async Task<bool> CreateOrUpdateCompanyAnnouncementAsync(int announcementId, CompanyAnnouncementCreateOrUpdateRequest request, UserModel currentUser)
        {
            var companyAnnouncementsDTO = _mapper.Map<CompanyAnnouncementDTO>(request);
            companyAnnouncementsDTO.Id = announcementId > 0 ? announcementId : 0;
            // user id should not be updated for edit case
            if (announcementId == 0)
                companyAnnouncementsDTO.UserId = currentUser.Id;
            companyAnnouncementsDTO.StatusId = (int)CompanyAnnouncementStatus.Active;
            bool isAdmin = currentUser.RoleIds.Contains((int)RoleType.Admin);
            bool result = await _companyService.CreateOrUpdateCompanyAnnouncementAsync(announcementId, companyAnnouncementsDTO, currentUser.Id, isAdmin, currentUser.CompanyId);
            return result;
        }
    }
}