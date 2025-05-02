using SE.Neo.Common.Models.Event;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class EventServiceBlobDecorator : AbstractEventServiceDecorator
    {
        private readonly IBlobServicesFacade _blobServicesFacade;

        public EventServiceBlobDecorator(
            IEventService eventService,
            IBlobServicesFacade blobServicesFacade)
            : base(eventService)
        {
            _blobServicesFacade = blobServicesFacade;
        }

        public override async Task<EventDTO?> GetEventForUserAsync(int userId, int id, string? expand = null, bool allowedPrivate = false)
        {
            EventDTO? eventDTO = await base.GetEventForUserAsync(userId, id, expand, allowedPrivate);

            if (eventDTO != null)
            {
                await AssignImagesToEventsAsync(new List<EventDTO> { eventDTO });
            }

            return eventDTO;
        }

        public override async Task<WrapperModel<EventDTO>> GetEventsForUserAsync(int userId, EventsFilter filter, bool allowedPrivate = false)
        {
            var wrapperModel = await base.GetEventsForUserAsync(userId, filter, allowedPrivate);

            await AssignImagesToEventsAsync(wrapperModel.DataList.ToList());

            return wrapperModel;
        }

        public override async Task<WrapperModel<EventUserDTO>> GetEventAttendeesForUserAsync(int id, int userId, EventAttendeesFilter filter, bool allowedPrivate = false)
        {
            var wrapperModel = await base.GetEventAttendeesForUserAsync(id, userId, filter, allowedPrivate);

            await AssignImagesToUsersAsync(wrapperModel.DataList.ToList());

            return wrapperModel;
        }

        public override async Task<WrapperModel<EventMatchingUserDTO>> GetUsersForEvent(int id, EventMatchingUserFilter filter)
        {
            WrapperModel<EventMatchingUserDTO> wrapperModel = await base.GetUsersForEvent(id, filter);
            wrapperModel.DataList = await AssignImagesToUsersAsync(wrapperModel.DataList.ToList());
            return wrapperModel;
        }

        private async Task AssignImagesToUsersAsync(List<EventUserDTO> users)
        {
            if (!users.Any())
            {
                return;
            }

            await _blobServicesFacade.PopulateWithBlobAsync(users, dto => dto.Image, (dto, b) => dto.Image = b);
        }

        private async Task<List<EventMatchingUserDTO>> AssignImagesToUsersAsync(List<EventMatchingUserDTO> users)
        {
            if (!users.Any())
            {
                return users;
            }

            await _blobServicesFacade.PopulateWithBlobAsync(users, dto => dto.Image, (dto, b) => dto.Image = b);

            return users;
        }

        private async Task AssignImagesToEventsAsync(List<EventDTO> events)
        {
            if (!events.Any())
            {
                return;
            }

            foreach (EventDTO eventDTO in events)
            {
                List<EventUserDTO> attendees = eventDTO.Attendees.ToList();
                await _blobServicesFacade.PopulateWithBlobAsync(attendees, dto => dto.Image, (dto, b) => dto.Image = b);
                eventDTO.Attendees = attendees;

                List<EventUserDTO> invitedUsers = eventDTO.InvitedUsers.ToList();
                await _blobServicesFacade.PopulateWithBlobAsync(invitedUsers, dto => dto.Image, (dto, b) => dto.Image = b);
                eventDTO.InvitedUsers = invitedUsers;

                List<EventModeratorDTO> moderators = eventDTO.Moderators.ToList();
                await _blobServicesFacade.PopulateWithBlobAsync(
                    moderators,
                    dto => dto.User?.Image,
                    (dto, b) =>
                    {
                        if (dto.User?.Image != null)
                        {
                            dto.User.Image = b;
                        }
                    });
                eventDTO.Moderators = moderators;
            }
        }
    }
}
