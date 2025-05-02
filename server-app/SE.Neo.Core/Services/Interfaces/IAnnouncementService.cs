using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Announcement;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IAnnouncementService
    {
        Task<int> CreateUpdateAnnouncementAsync(AnnouncementDTO modelDTO, bool forceActivate = false);

        Task<int> PatchAnnouncementAsync(int id, JsonPatchDocument patchDoc, bool forceActivate = false);

        Task<AnnouncementDTO?> GetLatestAnnouncementAsync(int audienceId, string? expand = null);

        Task<WrapperModel<AnnouncementDTO>> GetAnnouncementsAsync(ExpandOrderModel expandOrderModel);

        Task<AnnouncementDTO?> GetAnnouncementAsync(int id, string? expand = null);

        Task RemoveAnnouncementAsync(int id);
    }
}
