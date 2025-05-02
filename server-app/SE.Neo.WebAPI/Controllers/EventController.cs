using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Models.Event;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Event;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize, Active]
    [ApiController]
    [Route("api/events")]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;
        private readonly IEventApiService _eventApiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="eventApiService">The event API service instance.</param>
        public EventController(ILogger<EventController> logger, IEventApiService eventApiService)
        {
            _logger = logger;
            _eventApiService = eventApiService;
        }

        /// <summary>
        /// Get list of the events by filters. 
        /// </summary>
        /// <param name="filter">Defines filter parameters.</param>
        /// <returns>List of the events.</returns>
        /// <remarks>Expand values: timezone,links,categories,occurrences,moderators,moderators.company,moderators.image,attendees,attendees.image.</remarks>
        [Active(skipAuthorize: true)]
        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery] EventsFilter filter)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<EventResponse> events = await _eventApiService.GetEventsAsync(currentUser, filter);
            return Ok(events);
        }

        /// <summary>
        /// Get list of past events for the public site.
        /// </summary>
        /// <param name="filter">Defines filter parameters.</param>
        /// <returns>List of past events.</returns>
        [Active(skipAuthorize: true)]
        [HttpGet("past-events")]
        public async Task<IActionResult> GetPastEventsForPublicSite([FromQuery] EventsFilter filter)
        {
            WrapperModel<EventResponse> events = await _eventApiService.GetPastEventsAsync(filter);
            return Ok(events);
        }

        /// <summary>
        /// Get event by the given id.
        /// </summary>
        /// <param name="id">Unique identifier of the event.</param>
        /// <param name="expand">Parameter to attach related entities. Possible values: timezone,links,categories,occurrences,
        /// moderators,moderators.followers,moderators.company,moderators.image,
        /// attendees,attendees.followers,attendees.company,attendees.image,invitedcategories,invitedregions,invitedroles,
        /// invitedusers,invitedusers.company,invitedusers.image.</param>
        /// <param name="eventTimeZoneOffset">Indicates if event time zone offset should be applied.</param>
        /// <returns>Event object, or 404 NotFound if nothing found.</returns>
        [HttpGet("{id}")]
        [Active(skipAuthorize: true)]
        public async Task<IActionResult> GetEvent(int id, string? expand = null, bool eventTimeZoneOffset = false)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            EventResponse? eventResponse = await _eventApiService.GetEventAsync(currentUser, id, expand, eventTimeZoneOffset);
            return eventResponse != null ? Ok(eventResponse) : currentUser == null ? StatusCode(401) : NotFound();
        }

        /// <summary>
        /// Create new event.
        /// </summary>
        /// <param name="model">Event definition.</param>
        /// <returns>Newly created event object.</returns>
        [HttpPost]
        [Permissions(PermissionType.EventManagement)]
        public async Task<IActionResult> CreateEvent(EventRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            if (!model.IsEventModeratorsUnique())
            {
                ModelState.AddModelError(nameof(model.Moderators), "Moderators list has to be unique.");
                return UnprocessableEntity(new ValidationResponse(ModelState));
            }
            EventResponse eventResponse = await _eventApiService.CreateUpdateEventAsync(currentUser.Id, model, 0);
            return Ok(eventResponse);
        }

        /// <summary>
        /// Get list of the event attendees.
        /// </summary>
        /// <param name="id">Unique identifier of the event.</param>
        /// <param name="filter">Defines filter parameters.</param>
        /// <returns>List of the event attendees.</returns>
        /// <remarks>Expand values: company,image,followers.</remarks>
        [HttpGet("{id}/attendees")]
        public async Task<IActionResult> GetEventAttendees(int id, [FromQuery] EventAttendeesFilter filter)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<EventUserResponse> eventAttendees = await _eventApiService.GetEventAttendeesAsync(id, currentUser, filter);
            return Ok(eventAttendees);
        }

        /// <summary>
        /// Set attending status for current user.
        /// </summary>
        /// <param name="id">Unique identifier of the event.</param>
        /// <param name="model">Attending status.</param>
        /// <returns>Ok result.</returns>
        [HttpPut("{id}/attendees/current")]
        public async Task<IActionResult> AddOrUpdateEventAttendee(int id, EventAttendeeRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            await _eventApiService.AddOrUpdateEventAttendeeAsync(id, currentUser.Id, model);
            return Ok();
        }

        /// <summary>
        /// Get list of users that match event conditions or have been invited.
        /// </summary>
        /// <param name="id">Unique identifier of the event. 0 if event has not been created.</param>
        /// <param name="filter">Defines filter parameters.</param>
        /// <returns>List of users with booleans indicating if they match conditions or have been invited.</returns>
        /// <remarks>MatchBy values: regionIds,roleIds,categoryIds.</remarks>
        /// <remarks>Search: string of length > 1 </remarks>
        [HttpGet("{id}/users")]
        public async Task<WrapperModel<EventMatchingUserResponse>> GetUsersForEvent(int id, [FromQuery] EventMatchingUserFilter filter)
        {
            return await _eventApiService.GetUsersForEventAsync(id, filter);
        }

        /// <summary>
        /// Get calendar invite for event occurrences.
        /// </summary>
        /// <param name="id">Unique identifier of the event.</param>
        /// <param name="filter">Optional list of event occurrence ids.</param>
        /// <returns>.ics file.</returns>
        [HttpGet("{id}/occurrences/export")]
        public async Task<IActionResult> ExportAllEventOccurrences(int id, [FromQuery] EventCalendarExportRequest filter)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            byte[]? bytes = await _eventApiService.ExportEventOccurrenceCalendarAsync(currentUser, id, filter.EventOccurrenceIds);
            if (bytes == null)
            {
                return NotFound();
            }
            return File(bytes, "text/calendar", "calendar.ics");
        }

        /// <summary>
        /// Update an event.
        /// </summary>
        /// <param name="id">Unique identifier of the event.</param>
        /// <param name="onlyVisibilityPropertyChanged">Indicates if only visibility property has changed.</param>
        /// <param name="model">Updated Event model.</param>
        /// <returns>Updated Event.</returns>
        [HttpPut("{id}")]
        [Permissions(PermissionType.EventManagement)]
        public async Task<IActionResult> UpdateEvent(int id, EventRequest model, bool onlyVisibilityPropertyChanged = false)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            if (!model.IsEventModeratorsUnique())
            {
                ModelState.AddModelError(nameof(model.Moderators), "Moderators list has to be unique.");
                return UnprocessableEntity(new ValidationResponse(ModelState));
            }
            if (onlyVisibilityPropertyChanged)
            {
                int? response = await _eventApiService.UpdateEventVisibilityAsync(id, model.ShowInPublicSite ?? false);
                return response == 1 ? Ok() : StatusCode(500);
            }
            else
            {
                EventResponse? updatedEvent = await _eventApiService.CreateUpdateEventAsync(currentUser.Id, model, id);
                return Ok(updatedEvent);
            }
        }

        /// <summary>
        /// Patch an event.
        /// </summary>  
        /// <param name="id">Unique identifier of the event.</param>
        /// <param name="patchRequest">Partially updated event object.</param>
        /// <returns>Ok result.</returns>
        /// <remarks>
        /// JsonPatchDocument -> [{op: "replace", "path": "/Subject", "value": "New Subject}]
        /// <br />
        /// Only path /Subject, /Description, /Highights, /IsHighlighted, /Location, /UserRegistration currently allowed, do not pass contractResolver, operationType, or from.
        /// </remarks>
        [HttpPatch("{id}")]
        [Permissions(PermissionType.EventManagement)]
        public async Task<IActionResult> PatchEvent(int id, EventJsonPatchRequest patchRequest)
        {
            JsonPatchDocument patchDoc = patchRequest.JsonPatchDocument;
            await _eventApiService.PatchEventAsync(id, patchDoc);
            return Ok();
        }

        /// <summary>
        /// Delete an event.
        /// </summary>
        /// <param name="id">Unique identifier of the event.</param>
        /// <returns>Ok result.</returns>
        [Permissions(PermissionType.EventManagement)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            await _eventApiService.RemoveEventAsync(id);
            return Ok();
        }
    }
}
