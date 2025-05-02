using SE.Neo.Common.Enums;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.Initiative;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IInitiativeApiService
    {
        Task<InitiativeResponse> CreateOrUpdateInitiativeAsync(int id, InitiativeCreateOrUpdateRequest initiativeRequest, UserModel user);

        Task<bool> DeleteInitiativeAsync(int userId, int initiativeId);

        Task<InitiativeContentsWrapperModel<InitiativeArticleResponse>> GetRecommendedArticlesForInitiativeAsync(InitiativeRecommendationRequest recommendationRequest, UserModel currentUser, int initiativeId);

        Task<WrapperModel<InitiativeArticleResponse>> GetSavedArticlesForInitiativeAsync(int initiativeId, UserModel currentUser, BaseSearchFilterModel filter, bool isAdmin);

        Task<InitiativeAndProgressDetailsResponse> GetInitiativeAndProgressTrackerDetailsByIdAsync(int initiativeId, int userId, bool isEditMode, bool isAdmin);

        Task<bool> SaveContentsToAnInitiativeAsync(InitiativeContentRequest request, int id);

        Task<bool> UploadFileToAnInitiativeAsync(InitiativeFileRequest request, int userId, int id, bool isAdmin);

        Task<WrapperModel<InitiativeAdminResponse>> GetAllInitiativesAsync(BaseSearchFilterModel filter);

        Task<bool> UpdateInitiativeSubStepProgressAsync(InitiativeSubStepRequest initiativeSubstepProgressRequest, int userId);

        Task<bool> RemoveContentFromInitiativeAsync(int userId, int initiativeId, int contentId, InitiativeModule contentType, bool isAdmin);

        Task<WrapperModel<InitiativeAndProgressDetailsResponse>> GetInitiativesAndProgressTrackerDetailsByUserIdAsync(int userId, List<int> roleIds,int companyId, InitiativeViewSource initiativeType, BaseSearchFilterModel filter);

        Task<List<InitiativeRecommendationCount>> GetNewRecommendationsCountAsync(UserModel user, InitiativeRecommendationCountRequest request);


        Task<List<int>> GetInitiativeIdsByUserId(int id);

        Task<WrapperModel<InitiativeConversationResponse>> GetSavedConversationsForInitiativeAsync(int initiativeId, UserModel currentUser, BaseSearchFilterModel filter, bool isAdmin);

        Task<WrapperModel<InitiativeConversationResponse>> GetRecommendedConversationsForInitiativeAsync(InitiativeRecommendationRequest request, UserModel currentUser, int initiativeId);

        Task<InitiativeContentsWrapperModel<InitiativeToolResponse>> GetRecommendedToolsForInitiativeAsync(InitiativeRecommendationRequest request, UserModel currentUser, int initiativeId);

        Task<WrapperModel<InitiativeToolResponse>> GetSavedToolsForInitiativeAsync(int initiativeId, UserModel currentUser, BaseSearchFilterModel filter, bool isAdmin);

        Task<bool> AttachContentToInitiativeAsync(int userId, AttachContentToInitiativeRequest attachContentToInitiativeRequest);

        Task<IEnumerable<InitiativesAttachedContentResponse>> GetInitiativesByContentIdAsync(int userId, int contentId, InitiativeModule contentType);

        Task<InitiativeContentsWrapperModel<InitiativeProjectResponse>> GetRecommendedProjectsForInitiativeAsync(InitiativeRecommendationRequest request, UserModel currentUser, int initiativeId);

        Task<WrapperModel<InitiativeFileResponse>> GetSavedFilesOfAnInitiativeAsync(int initiativeId, UserModel currentUser, BaseSearchFilterModel filter, bool isAdmin);

        Task<InitiativeContentsWrapperModel<InitiativeCommunityUserResponse>> GetRecommendedCommunityUsersForInitiativeAsync(InitiativeRecommendationRequest request, UserModel currentUser, int initiativeId);

        Task<(FileExistResponse, int)> ValidateFileCountAndIfExistsByInitiativeIdAsync(int initiativeId, string fileName, UserModel currentUser, bool isAdmin);

        Task<WrapperModel<InitiativeProjectResponse>> GetSavedProjectsForInitiativeAsync(int initiativeId, UserModel currentUser, BaseSearchFilterModel filter, bool isAdmin);

        Task<WrapperModel<InitiativeCommunityUserResponse>> GetSavedCommunityUsersForInitiativeAsync(int initiativeId, UserModel currentUser, BaseSearchFilterModel filter, bool isAdmin);
        Task<bool> UpdateInitiativeContentLastViewedDate(InitiativeContentRecommendationActivityRequest request);
        Task<int> ExportInitiativesAsync(BaseSearchFilterModel filter, UserModel? currentuser, MemoryStream stream);

        Task<bool> UpdateInitiativeFileModifiedDateAndSize(string fileName, int fileSize, int initiativeId, int currentUserId);
    }
}