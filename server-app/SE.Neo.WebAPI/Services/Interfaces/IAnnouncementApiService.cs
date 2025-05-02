using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Announcement;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IAnnouncementApiService
    {
        Task<int> CreateUpdateAnnouncementAsync(AnnouncementRequest model, int id = 0, bool forceActivate = false);

        Task<int> PatchAnnouncementAsync(int id, JsonPatchDocument patchDoc, bool forceActivate = false);

        Task<AnnouncementResponse?> GetLatestAnnouncementAsync(int audienceId, string? expand);

        Task<AnnouncementResponse?> GetAnnouncementAsync(int id, string? expand);

        Task<WrapperModel<AnnouncementResponse>> GetAnnouncementsAsync(ExpandOrderModel expandOrderModel);

        Task RemoveAnnouncementAsync(int id);
    }
}
