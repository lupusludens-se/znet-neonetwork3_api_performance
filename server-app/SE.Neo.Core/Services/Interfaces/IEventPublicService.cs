
using SE.Neo.Common.Models.Event;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IEventPublicService
    {

        Task<WrapperModel<EventDTO>> GetPublicUpcomingEvents(EventsFilter filter);

        Task<WrapperModel<EventDTO>> GetPastEvents(EventsFilter filter);
    }
}
