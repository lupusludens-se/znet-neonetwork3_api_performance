using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Event;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services.Decorators
{
    public class AbstractEventServiceDecorator : IEventService
    {
        private readonly IEventService _eventService;

        public AbstractEventServiceDecorator(IEventService eventService)
        {
            _eventService = eventService;
        }

        public virtual async Task AddOrUpdateEventAttendeeAsync(int id, int userId, bool? isAttending)
        {
            await _eventService.AddOrUpdateEventAttendeeAsync(id, userId, isAttending);
        }

        public virtual async Task<int> CreateUpdateEventAsync(EventDTO model)
        {
            return await _eventService.CreateUpdateEventAsync(model);
        }

        public async Task<EventDTO?> GetEventAsync(int id, string? expand = null)
        {
            return await _eventService.GetEventAsync(id, expand);
        }

        public virtual async Task<WrapperModel<EventUserDTO>> GetEventAttendeesForUserAsync(int id, int userId, EventAttendeesFilter filter, bool allowedPrivate = false)
        {
            return await _eventService.GetEventAttendeesForUserAsync(id, userId, filter, allowedPrivate);
        }

        public virtual async Task<EventDTO?> GetEventForUserAsync(int userId, int id, string? expand = null, bool allowedPrivate = false)
        {
            return await _eventService.GetEventForUserAsync(userId, id, expand, allowedPrivate);
        }

        public virtual async Task<List<int>> GetEventInvitedUserIdsAsync(int eventId)
        {
            return await _eventService.GetEventInvitedUserIdsAsync(eventId);
        }

        public virtual async Task<WrapperModel<EventDTO>> GetEventsForUserAsync(int userId, EventsFilter filter, bool allowedPrivate = false)
        {
            return await _eventService.GetEventsForUserAsync(userId, filter, allowedPrivate);
        }

        public virtual async Task<WrapperModel<EventMatchingUserDTO>> GetUsersForEvent(int id, EventMatchingUserFilter filter)
        {
            return await _eventService.GetUsersForEvent(id, filter);
        }

        public virtual async Task<EventDTO> PatchEventAsync(int id, JsonPatchDocument patchDoc)
        {
            return await _eventService.PatchEventAsync(id, patchDoc);
        }

        public virtual async Task RemoveEventAsync(int id)
        {
            await _eventService.RemoveEventAsync(id);
        }

        public virtual async Task<int> UpdateEventVisibilityAsync(int id, bool showInPublicSite)
        {
            return await _eventService.UpdateEventVisibilityAsync(id, showInPublicSite);
        }
    }
}
