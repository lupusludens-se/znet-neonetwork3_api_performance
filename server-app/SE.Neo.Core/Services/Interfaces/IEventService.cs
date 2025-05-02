using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Event;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IEventService
    {
        Task<int> CreateUpdateEventAsync(EventDTO model);

        Task<int> UpdateEventVisibilityAsync(int id, bool showInPublicSite);

        Task<EventDTO?> GetEventForUserAsync(int userId, int id, string? expand = null, bool allowedPrivate = false);

        Task<EventDTO?> GetEventAsync(int id, string? expand = null);

        Task<WrapperModel<EventDTO>> GetEventsForUserAsync(int userId, EventsFilter filter, bool allowedPrivate = false);

        Task AddOrUpdateEventAttendeeAsync(int id, int userId, bool? isAttending);

        Task<WrapperModel<EventUserDTO>> GetEventAttendeesForUserAsync(int id, int userId, EventAttendeesFilter filter, bool allowedPrivate = false);

        Task<WrapperModel<EventMatchingUserDTO>> GetUsersForEvent(int id, EventMatchingUserFilter filter);

        Task<List<int>> GetEventInvitedUserIdsAsync(int eventId);

        Task<EventDTO> PatchEventAsync(int id, JsonPatchDocument patchDoc);

        Task RemoveEventAsync(int id);

        //Task<WrapperModel<EventDTO>> GetPublicUpcomingEvents(EventsFilter filter);

        //Task<WrapperModel<EventDTO>> GetPastEvents(EventsFilter filter);
    }
}
