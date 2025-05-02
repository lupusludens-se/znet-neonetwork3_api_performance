using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.Common.Models;
using SE.Neo.Common.Enums;

namespace SE.Neo.Core.Services.Interfaces
{
    /// <summary>
    /// Initiative Service Interface
    /// </summary>
    public interface IInitiativeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="initiativeDTO"></param>
        /// <returns></returns>
        Task<InitiativeDTO> CreateOrUpdateInitiativeAsync(int id, InitiativeDTO initiativeDTO);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        Task<bool> DeleteInitiativeAsync(int userId, int initiativeId);        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        Task<InitiativeContentsWrapperModel<ArticleForInitiativeDTO>> GetRecommendedArticlesForInitiativeAsync(InitiativeRecommendationRequest request, int userId, List<int> roleIds, int initiativeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<int>> GetInitiativeIdsByUserId(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WrapperModel<ArticleForInitiativeDTO>> GetSavedArticlesForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WrapperModel<InitiativeFileDTO>> GetSavedFilesOfAnInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<WrapperModel<InitiativeAdminDTO>> GetAllInitiativesAsync(BaseSearchFilterModel filter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> SaveContentsToAnInitiativeAsync(InitiativeContentDTO request, int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        Task<bool> UploadFileToAnInitiativeAsync(InitiativeFileDTO request, int userId, int initiativeId, bool isAdmin);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="userId"></param>
        /// <param name="isEditMode"></param>
        /// <returns></returns>
        Task<InitiativeAndProgressDetailsDTO?> GetInitiativeAndProgressTrackerDetailsByIdAsync(int initiativeId, int userId, bool isEditMode, bool isAdmin);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeSubStepProgress"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> UpdateInitiativeSubStepProgressAsync(InitiativeSubStepProgressDTO initiativeSubStepProgress, int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="initiativeId"></param>
        /// <param name="contentId"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        Task<bool> RemoveContentFromInitiativeAsync(int userId, int initiativeId, int contentId, InitiativeModule contentType, bool isAdmin);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WrapperModel<InitiativeAndProgressDetailsDTO>> GetInitiativesAndProgressDetailsByUserIdAsync(int userId, InitiativeViewSource initiativeType, BaseSearchFilterModel filter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Task<List<InitiativeRecommendationCount>> GetNewRecommendationsCountAsync(InitiativeRecommendationCountRequest request, int userId, List<int> roleIds,int companyId);
         

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WrapperModel<ConversationForInitiativeDTO>> GetSavedConversationsForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        Task<WrapperModel<ConversationForInitiativeDTO>> GetRecommendedConversationsForInitiativeAsync(InitiativeRecommendationRequest request, int userId, int initiativeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        Task<InitiativeContentsWrapperModel<ToolForInitiativeDTO>> GetRecommendedToolsForInitiativeAsync(InitiativeRecommendationRequest request, int userId, int companyId, int initiativeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WrapperModel<ToolForInitiativeDTO>> GetSavedToolsForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin);
        /// <summary>
        /// attach content to initiative
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="attachContentToInitiativeDTO"></param>
        /// <returns></returns>
        Task<bool> AttachContentToInitiativeAsync(int userId, AttachContentToInitiativeDTO attachContentToInitiativeDTO);

        /// <param name="userId"></param>
        /// <param name="contentId"></param>
        /// <param name="contentType"></param>
        Task<List<InitiativesAttachedContentDTO>> GetInitiativesByContentIdAsync(int userId, int contentId, InitiativeModule contentType);
        Task<InitiativeContentsWrapperModel<ProjectForInitiativeDTO>> GetRecommendedProjectsForInitiativeAsync(InitiativeRecommendationRequest request, int userId, List<int>? roleIds, int intiativeId);
        Task<InitiativeContentsWrapperModel<CommunityUserForInitiativeDTO>> GetRecommendedCommunityUsersForInitiativeAsync(InitiativeRecommendationRequest request, int userId, int initiativeId);

        /// <param name="initiativeId"></param>
        /// <param name="fileName"></param>
        /// <param name="userId"></param>
        Task<(FileExistResponseDTO?, int)> ValidateFileCountAndIfExistsByInitiativeIdAsync(int initiativeId, string fileName, int userId, bool isAdmin);

        Task<string> GetBlobFileName(int fileId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WrapperModel<ProjectForInitiativeDTO>> GetSavedProjectsForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin);

        Task<WrapperModel<CommunityUserForInitiativeDTO>> GetSavedCommunityUsersForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin);
        Task<bool> UpdateInitiativeContentLastViewedDate(InitiativeContentRecommendationActivityRequest request);

        Task<bool> UpdateInitiativeFileModifiedDateAndSize(string fileName, int fileSize, int initiativeId, int currentUserId);
    }
}