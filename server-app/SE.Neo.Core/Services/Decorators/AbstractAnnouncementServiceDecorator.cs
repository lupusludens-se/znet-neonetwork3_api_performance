using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Announcement;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Decorators
{
    public class AbstractAnnouncementServiceDecorator : IAnnouncementService
    {
        protected readonly IAnnouncementService _announcementService;

        public AbstractAnnouncementServiceDecorator(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public virtual async Task<int> CreateUpdateAnnouncementAsync(AnnouncementDTO modelDTO, bool forceActivate = false)
        {
            return await _announcementService.CreateUpdateAnnouncementAsync(modelDTO, forceActivate);
        }

        public virtual async Task<int> PatchAnnouncementAsync(int id, JsonPatchDocument patchDoc, bool forceActivate = false)
        {
            return await _announcementService.PatchAnnouncementAsync(id, patchDoc, forceActivate);
        }

        public virtual async Task<AnnouncementDTO?> GetLatestAnnouncementAsync(int audienceId, string? expand = null)
        {
            return await _announcementService.GetLatestAnnouncementAsync(audienceId, expand);
        }

        public virtual async Task<AnnouncementDTO?> GetAnnouncementAsync(int id, string? expand = null)
        {
            return await _announcementService.GetAnnouncementAsync(id, expand);
        }

        public virtual async Task<WrapperModel<AnnouncementDTO>> GetAnnouncementsAsync(ExpandOrderModel expandOrderModel)
        {
            return await _announcementService.GetAnnouncementsAsync(expandOrderModel);
        }

        public virtual async Task RemoveAnnouncementAsync(int id)
        {
            await _announcementService.RemoveAnnouncementAsync(id);
        }
    }
}
