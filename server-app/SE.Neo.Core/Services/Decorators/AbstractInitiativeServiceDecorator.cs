using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Common.Models;
using SE.Neo.Common.Enums;
using InitiativeScale = SE.Neo.Core.Entities.InitiativeScale;

namespace SE.Neo.Core.Services.Decorators
{
    public abstract class AbstractInitiativeServiceDecorator : IInitiativeService
    {
        protected readonly IInitiativeService _initiativeService;

        public AbstractInitiativeServiceDecorator(IInitiativeService initiativeService)
        {
            _initiativeService = initiativeService;
        }

        public virtual async Task<InitiativeDTO> CreateOrUpdateInitiativeAsync(int id, InitiativeDTO initiativeDTO)
        {
            return await _initiativeService.CreateOrUpdateInitiativeAsync(id, initiativeDTO);
        }

        /// <summary>
        /// Delete Initiative
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteInitiativeAsync(int userId, int initiativeId)
        {
            return await _initiativeService.DeleteInitiativeAsync(userId, initiativeId);
        }

        public async Task<List<int>> GetInitiativeIdsByUserId(int userId)
        {
            return await _initiativeService.GetInitiativeIdsByUserId(userId);
        }

        public async Task<InitiativeContentsWrapperModel<ArticleForInitiativeDTO>> GetRecommendedArticlesForInitiativeAsync(InitiativeRecommendationRequest request, int userId, List<int>? roleIds, int initiativeId)
        {
            return await _initiativeService.GetRecommendedArticlesForInitiativeAsync(request, userId, roleIds, initiativeId);

        }
        public async Task<WrapperModel<ArticleForInitiativeDTO>> GetSavedArticlesForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            return await _initiativeService.GetSavedArticlesForInitiativeAsync(initiativeId, filter, userId, isAdmin);
        }

        public async Task<WrapperModel<InitiativeFileDTO>> GetSavedFilesOfAnInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            return await _initiativeService.GetSavedFilesOfAnInitiativeAsync(initiativeId, filter, userId, isAdmin);
        }

        public async Task<InitiativeAndProgressDetailsDTO?> GetInitiativeAndProgressTrackerDetailsByIdAsync(int initiativeId, int userId, bool isEditMode, bool isAdmin)
        {
            return await _initiativeService.GetInitiativeAndProgressTrackerDetailsByIdAsync(initiativeId, userId, isEditMode, isAdmin);
        }
        public virtual async Task<bool> SaveContentsToAnInitiativeAsync(InitiativeContentDTO model, int userId)
        {
            return await _initiativeService.SaveContentsToAnInitiativeAsync(model, userId);
        }

        public virtual async Task<bool> UploadFileToAnInitiativeAsync(InitiativeFileDTO model, int userId, int initiativeId, bool isAdmin)
        {
            return await _initiativeService.UploadFileToAnInitiativeAsync(model, userId, initiativeId, isAdmin);
        }
        public virtual async Task<WrapperModel<InitiativeAdminDTO>> GetAllInitiativesAsync(BaseSearchFilterModel filter)
        {
            return await _initiativeService.GetAllInitiativesAsync(filter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeSubStepProgressDTO"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateInitiativeSubStepProgressAsync(InitiativeSubStepProgressDTO initiativeSubStepProgressDTO, int userId)
        {
            return await _initiativeService.UpdateInitiativeSubStepProgressAsync(initiativeSubStepProgressDTO, userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="initiativeId"></param>
        /// <param name="contentId"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public virtual async Task<bool> RemoveContentFromInitiativeAsync(int userId, int initiativeId, int contentId, InitiativeModule contentType, bool isAdmin)
        {
            return await _initiativeService.RemoveContentFromInitiativeAsync(userId, initiativeId, contentId, contentType, isAdmin);
        }

        public virtual async Task<WrapperModel<InitiativeAndProgressDetailsDTO>> GetInitiativesAndProgressDetailsByUserIdAsync(int userId, InitiativeViewSource initiativeType, BaseSearchFilterModel filter)
        {
            return await _initiativeService.GetInitiativesAndProgressDetailsByUserIdAsync(userId, initiativeType, filter);
        }
        /// <summary>
        /// Method to get count of new recommendations
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<List<InitiativeRecommendationCount>> GetNewRecommendationsCountAsync(InitiativeRecommendationCountRequest request, int userId, List<int> roleIds,int companyId)
        {
            return await _initiativeService.GetNewRecommendationsCountAsync(request, userId, roleIds,companyId);
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        public virtual async Task<WrapperModel<ConversationForInitiativeDTO>> GetRecommendedConversationsForInitiativeAsync(InitiativeRecommendationRequest request, int userId, int initiativeId)
        {
            return await _initiativeService.GetRecommendedConversationsForInitiativeAsync(request, userId, initiativeId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task<WrapperModel<ConversationForInitiativeDTO>> GetSavedConversationsForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            return await _initiativeService.GetSavedConversationsForInitiativeAsync(initiativeId, filter, userId, isAdmin);
        }

        public virtual async Task<InitiativeContentsWrapperModel<CommunityUserForInitiativeDTO>> GetRecommendedCommunityUsersForInitiativeAsync(InitiativeRecommendationRequest request, int userId, int initiativeId)
        {
            return await _initiativeService.GetRecommendedCommunityUsersForInitiativeAsync(request, userId, initiativeId);
        }

        public virtual async Task<InitiativeContentsWrapperModel<ToolForInitiativeDTO>> GetRecommendedToolsForInitiativeAsync(InitiativeRecommendationRequest request, int userId, int companyId, int initiativeId)
        {
            return await _initiativeService.GetRecommendedToolsForInitiativeAsync(request, userId, companyId, initiativeId);
        }

        public virtual async Task<WrapperModel<ToolForInitiativeDTO>> GetSavedToolsForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            return await _initiativeService.GetSavedToolsForInitiativeAsync(initiativeId, filter, userId, isAdmin);
        }

        public async Task<List<InitiativesAttachedContentDTO>> GetInitiativesByContentIdAsync(int userId, int contentId, InitiativeModule contentType)
        {
            return await _initiativeService.GetInitiativesByContentIdAsync(userId, contentId, contentType);
        }

        public async Task<bool> AttachContentToInitiativeAsync(int userId, AttachContentToInitiativeDTO attachContentToInitiativeDTO)
        {
            return await _initiativeService.AttachContentToInitiativeAsync(userId, attachContentToInitiativeDTO);
        }
        public virtual async Task<InitiativeContentsWrapperModel<ProjectForInitiativeDTO>> GetRecommendedProjectsForInitiativeAsync(InitiativeRecommendationRequest request, int userId, List<int>? roleIds, int intiativeId)
        {
            return await _initiativeService.GetRecommendedProjectsForInitiativeAsync(request, userId, roleIds, intiativeId);

        }
        public async Task<(FileExistResponseDTO?, int)> ValidateFileCountAndIfExistsByInitiativeIdAsync(int initiativeId, string fileName, int userId, bool isAdmin)
        {
            return await _initiativeService.ValidateFileCountAndIfExistsByInitiativeIdAsync(initiativeId, fileName, userId, isAdmin);
        }

        public async Task<string> GetBlobFileName(int fileId)
        {
            return await _initiativeService.GetBlobFileName(fileId);
        }

        public virtual async Task<WrapperModel<ProjectForInitiativeDTO>> GetSavedProjectsForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            return await _initiativeService.GetSavedProjectsForInitiativeAsync(initiativeId, filter, userId, isAdmin);

        }

        public virtual async Task<WrapperModel<CommunityUserForInitiativeDTO>> GetSavedCommunityUsersForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            return await _initiativeService.GetSavedCommunityUsersForInitiativeAsync(initiativeId, filter, userId, isAdmin);
        }
        public async Task<bool> UpdateInitiativeContentLastViewedDate(InitiativeContentRecommendationActivityRequest request)
        {
            return await _initiativeService.UpdateInitiativeContentLastViewedDate(request);
        }

        public async Task<bool> UpdateInitiativeFileModifiedDateAndSize(string fileName, int fileSize, int initiativeId, int currentUserId)
        {
            return await _initiativeService.UpdateInitiativeFileModifiedDateAndSize(fileName, fileSize, initiativeId, currentUserId);
        }
    }
}