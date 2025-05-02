using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Event;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Event;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IEventApiService
    {
        Task<EventResponse> CreateUpdateEventAsync(int userId, EventRequest model, int id = 0);

        Task<EventResponse?> GetEventAsync(UserModel user, int id, string? expand = null, bool eventTimeZoneOffset = false);

        Task<WrapperModel<EventResponse>> GetEventsAsync(UserModel user, EventsFilter filter);

        Task AddOrUpdateEventAttendeeAsync(int id, int userId, EventAttendeeRequest model);

        Task<WrapperModel<EventUserResponse>> GetEventAttendeesAsync(int id, UserModel user, EventAttendeesFilter filter);

        Task<WrapperModel<EventMatchingUserResponse>> GetUsersForEventAsync(int id, EventMatchingUserFilter filter);

        Task<byte[]?> ExportEventOccurrenceCalendarAsync(UserModel currentUser, int id, List<int>? occurrenceId = null);

        Task PatchEventAsync(int eventId, JsonPatchDocument patchDoc);

        Task RemoveEventAsync(int eventId);

        Task<int> UpdateEventVisibilityAsync(int id, bool showInPublicSite);

        Task<WrapperModel<EventResponse>> GetPastEventsAsync(EventsFilter filter);

    }
}
