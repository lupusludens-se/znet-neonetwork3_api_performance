using AutoMapper;
using CsvHelper.Configuration;
using CsvHelper;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.Initiative;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;
using System.Globalization;
using System.Text;
using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services;

namespace SE.Neo.WebAPI.Services
{
    public class InitiativeApiService : IInitiativeApiService
    {
        private readonly ILogger<InitiativeApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IInitiativeService _initiativeService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly ICompanyService _companyService;

        public InitiativeApiService(ILogger<InitiativeApiService> logger,
            IMapper mapper,
            IInitiativeService initiativeService,
            IUserService userService, INotificationService notificationService, ICompanyService companyService)
        {
            _logger = logger;
            _initiativeService = initiativeService;
            _mapper = mapper;
            _userService = userService;
            _notificationService = notificationService;
            _companyService = companyService;
        }

        public async Task<InitiativeResponse> CreateOrUpdateInitiativeAsync(int id, InitiativeCreateOrUpdateRequest initiativeRequest, UserModel user)
        {
            var initiativeDTO = _mapper.Map<InitiativeDTO>(initiativeRequest);
            initiativeDTO.Id = id > 0 ? id : 0;
            initiativeDTO.User.Id = user.Id;
            initiativeDTO.User.CompanyId = user.CompanyId;
            initiativeDTO.StatusId = (int)Core.Enums.InitiativeStatus.Active;
            initiativeDTO.CollaboratorIds = initiativeRequest.CollaboratorIds;
            InitiativeDTO initiativeResponseDTO = await _initiativeService.CreateOrUpdateInitiativeAsync(id, initiativeDTO);
            if(id == 0)
            {                    
                List<int> adminUsersIds = await _userService.GetAdminUsersIdsAsync();
                CompanyDTO? company = await _companyService.GetCompanyAsync(user.CompanyId);
                await _notificationService.AddNotificationsAsync(adminUsersIds, NotificationType.NewInitiativeCreated,                
                    new InitiativeNotificationDetails
                    {
                            UserId = user.Id,
                            UserName = $"{user.FirstName} {user.LastName}",
                            CompanyName = company?.Name,
                            CompanyId = user.CompanyId,
                            InitiativeId = initiativeResponseDTO.Id,
                            InitiativeTitle = initiativeResponseDTO.Title
                    });
            }
            return _mapper.Map<InitiativeDTO, InitiativeResponse>(initiativeResponseDTO);
        }

        public async Task<bool> DeleteInitiativeAsync(int userId, int initiativeId)
        {
            return await _initiativeService.DeleteInitiativeAsync(userId, initiativeId);

        }

        public async Task<List<int>> GetInitiativeIdsByUserId(int userId)
        {
            return await _initiativeService.GetInitiativeIdsByUserId(userId);
        }


        public async Task<WrapperModel<InitiativeAdminResponse>> GetAllInitiativesAsync(BaseSearchFilterModel filter)
        {
            WrapperModel<InitiativeAdminDTO> initiatives = await _initiativeService.GetAllInitiativesAsync(filter);
            var wrapper = new WrapperModel<InitiativeAdminResponse>
            {
                Count = initiatives.Count,
                DataList = initiatives.DataList.Select(_mapper.Map<InitiativeAdminResponse>)
            };
            return wrapper;

        }

        public async Task<InitiativeContentsWrapperModel<InitiativeArticleResponse>> GetRecommendedArticlesForInitiativeAsync(InitiativeRecommendationRequest request, UserModel currentUser, int initiativeId)
        {
            InitiativeContentsWrapperModel<ArticleForInitiativeDTO> articles = await _initiativeService.GetRecommendedArticlesForInitiativeAsync(request, currentUser.Id, currentUser.RoleIds, initiativeId);
            var wrapper = new InitiativeContentsWrapperModel<InitiativeArticleResponse>
            {
                Count = articles.Count,
                DataList = articles.DataList.Select(_mapper.Map<InitiativeArticleResponse>),
                NewRecommendationsCount=articles.NewRecommendationsCount
            };

            return wrapper;
        }


        public async Task<WrapperModel<InitiativeArticleResponse>> GetSavedArticlesForInitiativeAsync(int initiativeId, UserModel currentUser, BaseSearchFilterModel filter, bool isAdmin)
        {
            WrapperModel<ArticleForInitiativeDTO> initiativeSavedArticleDTOs = await _initiativeService.GetSavedArticlesForInitiativeAsync(initiativeId, filter, currentUser.Id, isAdmin);
            var wrapper = new WrapperModel<InitiativeArticleResponse>
            {
                Count = initiativeSavedArticleDTOs.Count,
                DataList = initiativeSavedArticleDTOs.DataList.Select(_mapper.Map<InitiativeArticleResponse>)
            };
            return wrapper;

        }

        public async Task<WrapperModel<InitiativeFileResponse>> GetSavedFilesOfAnInitiativeAsync(int initiativeId, UserModel currentUser, BaseSearchFilterModel filter, bool isAdmin)
        {
            WrapperModel<InitiativeFileDTO> initiativeFileDTOs = await _initiativeService.GetSavedFilesOfAnInitiativeAsync(initiativeId, filter, currentUser.Id, isAdmin);
            var wrapper = new WrapperModel<InitiativeFileResponse>
            {
                Count = initiativeFileDTOs.Count,
                DataList = initiativeFileDTOs.DataList.Select(_mapper.Map<InitiativeFileResponse>)
            };
            return wrapper;
        }

        public async Task<InitiativeAndProgressDetailsResponse> GetInitiativeAndProgressTrackerDetailsByIdAsync(int initiativeId, int userId, bool isEditMode, bool isAdmin)
        {
            InitiativeAndProgressDetailsDTO? initiativeAndProgressDetailsDTO = await _initiativeService.GetInitiativeAndProgressTrackerDetailsByIdAsync(initiativeId, userId, isEditMode, isAdmin);

            var initiativeProgressDetailsResponse = _mapper.Map<InitiativeAndProgressDetailsResponse>(initiativeAndProgressDetailsDTO);
            if (initiativeAndProgressDetailsDTO != null)
            {
                foreach (var subStep in initiativeProgressDetailsResponse.Steps.SelectMany(step => step.SubSteps))
                {
                    subStep.IsChecked = initiativeAndProgressDetailsDTO.SubStepsProgress.Any(m => m.SubStepId == subStep.SubStepId);
                }
            }

            return initiativeProgressDetailsResponse;

        }
        public async Task<bool> SaveContentsToAnInitiativeAsync(InitiativeContentRequest request, int userId)
        {
            var initiativeContentDTO = _mapper.Map<InitiativeContentDTO>(request);
            return await _initiativeService.SaveContentsToAnInitiativeAsync(initiativeContentDTO, userId);
        }
        public async Task<bool> UploadFileToAnInitiativeAsync(InitiativeFileRequest request, int userId, int id, bool isAdmin)
        {
            var initiativeFileDTO = _mapper.Map<InitiativeFileDTO>(request);
            return await _initiativeService.UploadFileToAnInitiativeAsync(initiativeFileDTO, userId, id, isAdmin);
        }

        public async Task<bool> UpdateInitiativeSubStepProgressAsync(InitiativeSubStepRequest initiativeSubStepProgressRequest, int userId)
        {
            var initiativeSubStepDTO = _mapper.Map<InitiativeSubStepProgressDTO>(initiativeSubStepProgressRequest);
            return await _initiativeService.UpdateInitiativeSubStepProgressAsync(initiativeSubStepDTO, userId);
        }

        public async Task<bool> RemoveContentFromInitiativeAsync(int userId, int inititativeId, int contentId, InitiativeModule contentType, bool isAdmin)
        {
            return await _initiativeService.RemoveContentFromInitiativeAsync(userId, inititativeId, contentId, contentType, isAdmin);
        }

        public async Task<WrapperModel<InitiativeAndProgressDetailsResponse>> GetInitiativesAndProgressTrackerDetailsByUserIdAsync(int userId, List<int> roleIds, int companyId, InitiativeViewSource initiativeType, BaseSearchFilterModel filter)
        {
            int index = 0;
            WrapperModel<InitiativeAndProgressDetailsDTO> initiativeAndProgressDetailsDTO = await _initiativeService.GetInitiativesAndProgressDetailsByUserIdAsync(userId, initiativeType, filter);
            int count = initiativeAndProgressDetailsDTO.Count;
            if (initiativeAndProgressDetailsDTO.DataList.Any())
            {
                var initiativeIds = initiativeAndProgressDetailsDTO.DataList.Select(x => x.Id).ToList();

                List<InitiativeAndProgressDetailsResponse> initiatives = initiativeAndProgressDetailsDTO.DataList.Select(_mapper.Map<InitiativeAndProgressDetailsResponse>).ToList();

                List<InitiativeRecommendationCount> newRecommendations = await _initiativeService.GetNewRecommendationsCountAsync(new InitiativeRecommendationCountRequest() { InitiativeIds = initiativeIds, InitiativeContentType = Common.Enums.InitiativeModules.All }, userId, roleIds,companyId);

                if (initiativeAndProgressDetailsDTO.Count > 0)
                {
                    foreach (InitiativeAndProgressDetailsResponse initiativeAndProgressDetails in initiatives)
                    {
                        initiativeAndProgressDetails.RecommendationsCount = newRecommendations.FirstOrDefault(x => x.InitiativeId == initiativeAndProgressDetails.InitiativeId);

                        foreach (var subStep in initiativeAndProgressDetails.Steps.SelectMany(step => step.SubSteps))
                        {
                            subStep.IsChecked = initiativeAndProgressDetailsDTO.DataList.ElementAt(index).SubStepsProgress.Any(m => m.SubStepId == subStep.SubStepId);
                        }
                        index++;
                    }

                }
                return new WrapperModel<InitiativeAndProgressDetailsResponse>
                {
                    Count = count,
                    DataList = initiatives
                };
            }

            return new WrapperModel<InitiativeAndProgressDetailsResponse>
            {
                Count = 0,
                DataList = null
            };
        }
        public async Task<List<InitiativeRecommendationCount>> GetNewRecommendationsCountAsync(UserModel user, InitiativeRecommendationCountRequest request)
        {
            List<InitiativeRecommendationCount> newRecommendations = await _initiativeService.GetNewRecommendationsCountAsync(request, user.Id, user.RoleIds,user.CompanyId);
            return newRecommendations;
        }
         


        public async Task<WrapperModel<InitiativeConversationResponse>> GetRecommendedConversationsForInitiativeAsync(InitiativeRecommendationRequest request, UserModel currentUser, int initiativeId)
        {
            WrapperModel<ConversationForInitiativeDTO> messagesDTOs = await _initiativeService.GetRecommendedConversationsForInitiativeAsync(request, currentUser.Id, initiativeId);
            var wrapper = new WrapperModel<InitiativeConversationResponse>
            {
                Count = messagesDTOs.Count,
                DataList = messagesDTOs.DataList.Select(_mapper.Map<InitiativeConversationResponse>)
            };

            return wrapper;
        }


        public async Task<WrapperModel<InitiativeConversationResponse>> GetSavedConversationsForInitiativeAsync(int initiativeId, UserModel currentUser, BaseSearchFilterModel filter, bool isAdmin)
        {
            WrapperModel<ConversationForInitiativeDTO> savedMessagesDTOs = await _initiativeService.GetSavedConversationsForInitiativeAsync(initiativeId, filter, currentUser.Id, isAdmin);
            var wrapper = new WrapperModel<InitiativeConversationResponse>
            {
                Count = savedMessagesDTOs.Count,
                DataList = savedMessagesDTOs.DataList.Select(_mapper.Map<InitiativeConversationResponse>)
            };
            return wrapper;

        }
        public async Task<InitiativeContentsWrapperModel<InitiativeToolResponse>> GetRecommendedToolsForInitiativeAsync(InitiativeRecommendationRequest request, UserModel currentUser, int initiativeId)
        {
            InitiativeContentsWrapperModel<ToolForInitiativeDTO> tools = await _initiativeService.GetRecommendedToolsForInitiativeAsync(request, currentUser.Id, currentUser.CompanyId, initiativeId);
            var wrapper = new InitiativeContentsWrapperModel<InitiativeToolResponse>
            {
                Count = tools.Count,
                DataList = tools.DataList.Select(_mapper.Map<InitiativeToolResponse>),
                NewRecommendationsCount=tools.NewRecommendationsCount
            };

            return wrapper;
        }

        public async Task<bool> AttachContentToInitiativeAsync(int userId, AttachContentToInitiativeRequest attachContentToInitiativeRequest)
        {
            var attachContentToInitiativeDTO = _mapper.Map<AttachContentToInitiativeDTO>(attachContentToInitiativeRequest);
            bool isAdded = await _initiativeService.AttachContentToInitiativeAsync(userId, attachContentToInitiativeDTO);
            return isAdded;
        }

        public async Task<IEnumerable<InitiativesAttachedContentResponse>> GetInitiativesByContentIdAsync(int userId, int contentId, InitiativeModule contentType)
        {
            var initiativeAttachedContentDTO = await _initiativeService.GetInitiativesByContentIdAsync(userId, contentId, contentType);
            return initiativeAttachedContentDTO.Select(_mapper.Map<InitiativesAttachedContentResponse>);
        }

        public async Task<WrapperModel<InitiativeToolResponse>> GetSavedToolsForInitiativeAsync(int initiativeId, UserModel currentUser, BaseSearchFilterModel filter, bool isAdmin)
        {
            WrapperModel<ToolForInitiativeDTO> savedToolsDTOs = await _initiativeService.GetSavedToolsForInitiativeAsync(initiativeId, filter, currentUser.Id, isAdmin);
            var wrapper = new WrapperModel<InitiativeToolResponse>
            {
                Count = savedToolsDTOs.Count,
                DataList = savedToolsDTOs.DataList.Select(_mapper.Map<InitiativeToolResponse>)
            };
            return wrapper;
        }


        public async Task<InitiativeContentsWrapperModel<InitiativeCommunityUserResponse>> GetRecommendedCommunityUsersForInitiativeAsync(InitiativeRecommendationRequest request, UserModel currentUser, int initiativeId)
        {
            InitiativeContentsWrapperModel<CommunityUserForInitiativeDTO> users = await _initiativeService.GetRecommendedCommunityUsersForInitiativeAsync(request, currentUser.Id, initiativeId);
            var wrapper = new InitiativeContentsWrapperModel<InitiativeCommunityUserResponse>
            {
                Count = users.Count,
                DataList = users.DataList.Select(_mapper.Map<InitiativeCommunityUserResponse>),
                NewRecommendationsCount = users.NewRecommendationsCount
            };

            return wrapper;
        }
        public async Task<InitiativeContentsWrapperModel<InitiativeProjectResponse>> GetRecommendedProjectsForInitiativeAsync(InitiativeRecommendationRequest request, UserModel currentUser, int initiativeId)
        {
            InitiativeContentsWrapperModel<ProjectForInitiativeDTO> projects = await _initiativeService.GetRecommendedProjectsForInitiativeAsync(request, currentUser.Id, currentUser.RoleIds, initiativeId);
            var wrapper = new InitiativeContentsWrapperModel<InitiativeProjectResponse>
            {
                Count = projects.Count,
                DataList = projects.DataList.Select(_mapper.Map<InitiativeProjectResponse>),
                NewRecommendationsCount=projects.NewRecommendationsCount
            };

            return wrapper;
        }
        public async Task<(FileExistResponse, int)> ValidateFileCountAndIfExistsByInitiativeIdAsync(int initiativeId, string fileName, UserModel currentUser, bool isAdmin)
        {
            var (fileExistResponse, initiativeFilesCount) = await _initiativeService.ValidateFileCountAndIfExistsByInitiativeIdAsync(initiativeId, fileName, currentUser.Id, isAdmin);
            var wrapper = _mapper.Map<FileExistResponse>(fileExistResponse);
            return (wrapper, initiativeFilesCount);
        }

        public async Task<WrapperModel<InitiativeProjectResponse>> GetSavedProjectsForInitiativeAsync(int initiativeId, UserModel currentUser, BaseSearchFilterModel filter, bool isAdmin)
        {
            WrapperModel<ProjectForInitiativeDTO> savedProjectsDTOs = await _initiativeService.GetSavedProjectsForInitiativeAsync(initiativeId, filter, currentUser.Id, isAdmin);
            var wrapper = new WrapperModel<InitiativeProjectResponse>
            {
                Count = savedProjectsDTOs.Count,
                DataList = savedProjectsDTOs.DataList.Select(_mapper.Map<InitiativeProjectResponse>)
            };
            return wrapper;
        }


        public async Task<WrapperModel<InitiativeCommunityUserResponse>> GetSavedCommunityUsersForInitiativeAsync(int initiativeId, UserModel currentUser, BaseSearchFilterModel filter, bool isAdmin)
        {
            WrapperModel<CommunityUserForInitiativeDTO> savedProjectsDTOs = await _initiativeService.GetSavedCommunityUsersForInitiativeAsync(initiativeId, filter, currentUser.Id, isAdmin);
            var wrapper = new WrapperModel<InitiativeCommunityUserResponse>
            {
                Count = savedProjectsDTOs.Count,
                DataList = savedProjectsDTOs.DataList.Select(_mapper.Map<InitiativeCommunityUserResponse>)
            };
            return wrapper;
        }
        public async Task<bool> UpdateInitiativeContentLastViewedDate(InitiativeContentRecommendationActivityRequest request)
        {
            var result = await _initiativeService.UpdateInitiativeContentLastViewedDate(request);
            return result;
        }
        public async Task<int> ExportInitiativesAsync(BaseSearchFilterModel filter, UserModel? currentuser, MemoryStream stream)
        {
            filter.FilterBy = $"{(filter.FilterBy ?? string.Empty)}";
            WrapperModel<InitiativeAdminDTO> initiativesResult = await _initiativeService.GetAllInitiativesAsync(filter);
            List<InitiativeExportResponse> initiativeResponses = initiativesResult.DataList.Select(_mapper.Map<InitiativeExportResponse>).ToList();
            using (var writeFile = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                using (var csv = new CsvWriter(writeFile, new CsvConfiguration(CultureInfo.InvariantCulture) { Encoding = Encoding.UTF8, LeaveOpen = true }))
                {
                    var classMap = new DefaultClassMap<InitiativeExportResponse>();
                    writeFile.Write($"Exported from Zeigo Network on {DateTime.Now.ToString("MM/dd/yyyy")}\n");
                    writeFile.Write($"Exported By User: {currentuser.FirstName} {currentuser.LastName} \n");
                    if (!string.IsNullOrEmpty(filter.Search))
                    {
                        writeFile.Write($"Filtered Records By: Search Text - {filter.Search}\n");
                    }
                    csv.NextRecord();
                    classMap.Map(o => o.Title).Name("Title").Index(0);
                    classMap.Map(o => o.Category.Name).Name("Project Type").Index(1);
                    classMap.Map(o => o.CompanyName).Name("Company").Index(2);
                    classMap.Map(o => o.UserName).Name("User").Index(3);
                    classMap.Map(o => o.RegionsString).Name("Geography").Index(4);
                    classMap.Map(o => o.Phase).Name("Phase").Index(5);
                    classMap.Map(o => o.ChangedOn).Name("Last Updated").Index(6);
                    classMap.Map(o => o.StatusName).Name("Status").Index(7);
                    csv.Context.RegisterClassMap(classMap);
                    csv.WriteRecords(initiativeResponses);
                }
                stream.Position = 0;
            }
            return initiativeResponses.Count;
        }

        public async Task<bool> UpdateInitiativeFileModifiedDateAndSize(string fileName, int fileSize, int initiativeId, int currentUserId)
        {
            var result = await _initiativeService.UpdateInitiativeFileModifiedDateAndSize(fileName, fileSize, initiativeId, currentUserId);
            return result;
        }
    }
}