using AutoMapper;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Event;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Common.Service.Interfaces;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Constants;
using SE.Neo.WebAPI.Models.Event;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;
using System.Text;
using Calendar = Ical.Net.Calendar;

namespace SE.Neo.WebAPI.Services
{
    public class EventApiService : TimeZoneHelper, IEventApiService
    {
        private readonly ILogger<EventApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;
        private readonly IEventPublicService _eventPublicService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly ICommonService _commonService;

        public EventApiService(ILogger<EventApiService> logger,
            IMapper mapper,
            IEventService eventService,
            IEventPublicService eventPublicService,
            INotificationService notificationService,
            IUserService userService,
            ICommonService commonService)
        {
            _logger = logger;
            _mapper = mapper;
            _eventService = eventService;
            _notificationService = notificationService;
            _userService = userService;
            _commonService = commonService;
            _eventPublicService = eventPublicService;
        }

        public async Task AddOrUpdateEventAttendeeAsync(int id, int userId, EventAttendeeRequest model)
        {
            await _eventService.AddOrUpdateEventAttendeeAsync(id, userId, model.IsAttending);
        }

        public async Task<EventResponse> CreateUpdateEventAsync(int userId, EventRequest model, int id = 0)
        {
            var eventDTO = _mapper.Map<EventDTO>(model);
            var links = _mapper.Map<List<EventLinkDTO>>(model.Links);
            var recordings = _mapper.Map<List<EventLinkDTO>>(model.Recordings);
            eventDTO.Links = links.Union(recordings);
            DateTime now = DateTime.UtcNow;

            // convert from Event Timezone to UTC
            TimeZoneDTO timeZoneDTO = await _commonService.GetTimeZone(model.TimeZoneId);
            try
            {
                TimeZoneInfo eventTimeZoneInfo = GetTimeZoneInfoByWindowsName(timeZoneDTO.WindowsName);
                foreach (var occurrence in eventDTO.Occurrences)
                {
                    occurrence.FromDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(occurrence.FromDate, DateTimeKind.Unspecified), eventTimeZoneInfo);
                    occurrence.ToDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(occurrence.ToDate, DateTimeKind.Unspecified), eventTimeZoneInfo);
                }
            }
            catch (TimeZoneNotFoundException ex)
            {
                _logger.LogError(ex, $"Failed trying to get Iana Time Zone from {timeZoneDTO.WindowsName}");
                throw new CustomException(CoreErrorMessages.TimeZoneNotFound, ex);
            }

            bool isEdit = id > 0;
            List<int>? alreadyInvitedUserIds = isEdit ? await _eventService.GetEventInvitedUserIdsAsync(id) : null;
            EventDTO? oldEventDTO = isEdit ? await _eventService.GetEventForUserAsync(userId, id, "categories,moderators,occurrences,links,invitedcategories,invitedroles,invitedregions,timezone", true) : null;
            eventDTO.Id = id;
            int eventId = await _eventService.CreateUpdateEventAsync(eventDTO);
            EventDTO? newEventDTO = await _eventService.GetEventForUserAsync(userId, eventId, "categories,moderators,occurrences,links,invitedcategories,invitedroles,invitedregions,timezone", true);
            //notification
            try
            {
                if (isEdit)
                {
                    List<int> invitedUserIds = await _eventService.GetEventInvitedUserIdsAsync(eventId);
                    if (invitedUserIds.Count > 0)
                    {
                        List<int> newInvitedUsers = invitedUserIds.Where(x => !alreadyInvitedUserIds.Contains(x)).ToList();
                        List<string>? changedProperties = oldEventDTO!.FindChangedProperties(newEventDTO!, (oldCollection, newCollection, type) =>
                        {
                            return CompareEnumerableEventProperty(oldCollection, newCollection, type);
                        });
                        if (changedProperties != null && changedProperties.Count != 0)
                        {
                            if (changedProperties.Count() > 1)
                            {
                                await _notificationService.AddNotificationsAsync(alreadyInvitedUserIds, NotificationType.ChangesToEventIInvited,
                                new EventNotificationDetails
                                {
                                    EventTitle = eventDTO.Subject,
                                    EventId = eventId
                                });
                            }
                            else if (changedProperties.Count() == 1)
                            {
                                await _notificationService.AddNotificationsAsync(alreadyInvitedUserIds, NotificationType.ChangesToEventIInvited,
                                    new ChangeEventNotificationDetails
                                    {
                                        EventTitle = eventDTO.Subject,
                                        EventId = eventId,
                                        FieldName = changedProperties.First()
                                    });
                            }
                        }
                        await _notificationService.AddNotificationsAsync(newInvitedUsers, NotificationType.InvitesMeToEvent,
                           new EventNotificationDetails
                           {
                               EventTitle = eventDTO.Subject,
                               EventId = eventId
                           });
                    }
                }
                else
                {
                    if (model.InvitedUsers != null)
                        await _notificationService.AddNotificationsAsync(model.InvitedUsers.Select(u => u.Id).ToList(), NotificationType.InvitesMeToEvent,
                            new EventNotificationDetails
                            {
                                EventTitle = eventDTO.Subject,
                                EventId = eventId
                            });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
            }

            if (newEventDTO.LocationType == EventLocationType.Virtual)
            {
                UserDTO? currentUser = await _userService.GetUserAsync(userId, "timezone")!;
                return MapEvent(newEventDTO, currentUser: currentUser!)!;
            }
            else
            {
                return MapEvent(newEventDTO);
            }
        }

        public async Task<byte[]?> ExportEventOccurrenceCalendarAsync(UserModel user, int id, List<int>? occurrenceIds = null)
        {
            var hasPermissionToViewAllEvents = user.PermissionIds.Contains((int)PermissionType.EventManagement);
            EventDTO? @event = await _eventService.GetEventForUserAsync(user.Id, id, "occurrences,timezone", hasPermissionToViewAllEvents);

            if (@event == null)
            {
                return null;
            }
            string localTimeZoneStandardName = "";

            TimeZoneInfo localTimeZoneInfo;
            string localTimeZoneWindowsName;

            if (@event.LocationType == EventLocationType.Virtual)
            {
                UserDTO? currentUser = await _userService.GetUserAsync(user.Id, "timezone");
                try
                {
                    localTimeZoneWindowsName = currentUser!.TimeZone.WindowsName;
                    localTimeZoneInfo = GetTimeZoneInfoByWindowsName(currentUser!.TimeZone.WindowsName);
                }
                catch (TimeZoneNotFoundException ex)
                {
                    _logger.LogError(ex, $"Failed trying to get IANA timezone for {currentUser.TimeZone.WindowsName}, user: {currentUser.Id}");
                    throw new CustomException(CoreErrorMessages.TimeZoneNotFound, ex);
                }
            }
            else
            {
                try
                {
                    localTimeZoneWindowsName = @event.TimeZone.WindowsName;
                    localTimeZoneInfo = GetTimeZoneInfoByWindowsName(@event.TimeZone.WindowsName);
                }
                catch (TimeZoneNotFoundException ex)
                {
                    _logger.LogError(ex, $"Failed trying to get IANA timezone for {@event.TimeZone.WindowsName}, event: {@event.Id}");
                    throw new CustomException(CoreErrorMessages.TimeZoneNotFound, ex);
                }
            }

            List<EventOccurrenceDTO> occurrences = (occurrenceIds is null ? @event.Occurrences : @event.Occurrences.Where(eo => occurrenceIds.Contains(eo.Id))).ToList();

            @event.Occurrences.ToList().ForEach(eo => eo.FromDate = TimeZoneInfo.ConvertTimeFromUtc(eo.FromDate, localTimeZoneInfo));
            @event.Occurrences.ToList().ForEach(eo => eo.ToDate = TimeZoneInfo.ConvertTimeFromUtc(eo.ToDate, localTimeZoneInfo));

            Calendar calendar = CreateCalendar(@event, localTimeZoneWindowsName, occurrences);
            var serializer = new CalendarSerializer();
            byte[] bytes = Encoding.UTF8.GetBytes(serializer.SerializeToString(calendar));

            return bytes;
        }

        public async Task<EventResponse?> GetEventAsync(UserModel user, int id, string? expand = null, bool eventTimeZoneOffset = false)
        {
            EventDTO? @event;
            if (user != null)
            {
                var hasPermissionToViewAllEvents = user.PermissionIds.Contains((int)PermissionType.EventManagement);
                expand += "timezone";
                @event = await _eventService.GetEventForUserAsync(user.Id, id, expand, hasPermissionToViewAllEvents);

                if (@event != null && (@event.LocationType == EventLocationType.Virtual || !eventTimeZoneOffset))
                {
                    UserDTO? currentUser = await _userService.GetUserAsync(user.Id, "timezone")!;
                    return MapEvent(@event, currentUser: currentUser, eventTimeZoneOffset: eventTimeZoneOffset);
                }
            }
            else
            {
                expand += "timezone";
                @event = await _eventService.GetEventAsync(id, expand);
                return MapEvent(@event, null, eventTimeZoneOffset: eventTimeZoneOffset, true);
            }
            return MapEvent(@event, eventTimeZoneOffset: eventTimeZoneOffset);
        }

        public async Task<WrapperModel<EventUserResponse>> GetEventAttendeesAsync(int id, UserModel user, EventAttendeesFilter filter)
        {
            bool hasPermissionToViewAllEvents = user.PermissionIds.Contains((int)PermissionType.EventManagement);
            WrapperModel<EventUserDTO> wrapperModel = await _eventService.GetEventAttendeesForUserAsync(id, user.Id, filter, hasPermissionToViewAllEvents);

            return new WrapperModel<EventUserResponse>
            {
                Count = wrapperModel.Count,
                DataList = _mapper.Map<List<EventUserResponse>>(wrapperModel.DataList)
            };
        }

        public async Task<WrapperModel<EventResponse>> GetEventsAsync(UserModel user, EventsFilter filter)
        {
            WrapperModel<EventDTO> wrapperModel;
            UserDTO? userDTO = null;
            filter.Expand += "timezone";
            if (user != null)
            {

                bool hasPermissionToViewAllEvents = user.PermissionIds.Contains((int)PermissionType.EventManagement);
                wrapperModel = await _eventService.GetEventsForUserAsync(user.Id, filter, hasPermissionToViewAllEvents);
                userDTO = await _userService.GetUserAsync(user.Id, "timezone");
            }
            else
            {
                wrapperModel = await _eventPublicService.GetPublicUpcomingEvents(filter);

            }

            return new WrapperModel<EventResponse>
            {
                Count = wrapperModel.Count,
                DataList = wrapperModel.DataList.Select(e => MapEvent(e, currentUser: userDTO, false, isPublicUser: user == null))
            };
        }

        public async Task<WrapperModel<EventResponse>> GetPastEventsAsync(EventsFilter filter)
        {
            WrapperModel<EventDTO> wrapperModel;
            filter.Expand += "timezone";
            wrapperModel = await _eventPublicService.GetPastEvents(filter);

            return new WrapperModel<EventResponse>
            {
                Count = wrapperModel.Count,
                DataList = wrapperModel.DataList.Select(e => MapEvent(e, currentUser: null, false, true))
            };
        }

        public async Task<WrapperModel<EventMatchingUserResponse>> GetUsersForEventAsync(int eventId, EventMatchingUserFilter filter)
        {
            WrapperModel<EventMatchingUserDTO> users = await _eventService.GetUsersForEvent(eventId, filter);
            return new WrapperModel<EventMatchingUserResponse>
            {
                Count = users.Count,
                DataList = users.DataList.Select(_mapper.Map<EventMatchingUserResponse>)
            };
        }

        public async Task PatchEventAsync(int eventId, JsonPatchDocument patchDoc)
        {
            EventDTO eventDTO = await _eventService.PatchEventAsync(eventId, patchDoc);
            try
            {
                List<int>? invitedUserIds = await _eventService.GetEventInvitedUserIdsAsync(eventId);
                string? changedField = patchDoc.Operations.FirstOrDefault()?.path;
                if (changedField != null)
                {
                    await _notificationService.AddNotificationsAsync(invitedUserIds, NotificationType.ChangesToEventIInvited,
                        new ChangeEventNotificationDetails
                        {
                            EventTitle = eventDTO.Subject,
                            EventId = eventId,
                            FieldName = changedField.Remove(0, 1)
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
            }
        }

        public async Task RemoveEventAsync(int eventId)
        {
            EventDTO eventDTO = await _eventService.GetEventForUserAsync(0, eventId, "attendees", true);
            await _eventService.RemoveEventAsync(eventId);
            try
            {
                List<int> attendingUserIds = eventDTO.Attendees.Select(eu => eu.Id).ToList();
                await _notificationService.AddNotificationsAsync(attendingUserIds, NotificationType.ChangesToEventIInvited,
                    new ChangeEventNotificationDetails
                    {
                        EventTitle = eventDTO.Subject,
                        FieldName = "Status",
                        EventId = eventId
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
            }
        }


        public async Task<int> UpdateEventVisibilityAsync(int id, bool showInPublicSite)
        {
            int response = await _eventService.UpdateEventVisibilityAsync(id, showInPublicSite);
            return response;
        }

        private Calendar CreateCalendar(EventDTO @event, string localTimeZoneWindowsName, List<EventOccurrenceDTO> occurrences)
        {
            var calendar = new Calendar();

            try
            {
                TimeZoneInfo localWindowsTimeZoneInfo;
                try
                {
                    // try to find timezone info using default .NET (>= 6.0) mechanism
                    localWindowsTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneWindowsName);
                }
                catch (TimeZoneNotFoundException ex)
                {
                    localWindowsTimeZoneInfo = GetTimeZoneInfoByWindowsName(localTimeZoneWindowsName);
                }

                foreach (EventOccurrenceDTO eo in occurrences)
                {
                    calendar.Events.Add(new CalendarEvent
                    {
                        Start = new CalDateTime(eo.FromDate, localWindowsTimeZoneInfo.Id),
                        End = new CalDateTime(eo.ToDate, localWindowsTimeZoneInfo.Id),

                        Description = @event.Description,
                        Summary = @event.Subject,
                        Location = @event.Location
                    });
                }
                calendar.AddTimeZone(VTimeZone.FromSystemTimeZone(localWindowsTimeZoneInfo));
                return calendar;
            }
            catch (TimeZoneNotFoundException ex)
            {
                _logger.LogError(ex, $"Failed trying to get windows timezone info for {localTimeZoneWindowsName}");
                throw new CustomException(CoreErrorMessages.TimeZoneNotFound, ex);
            }
        }

        private bool CompareEnumerableEventProperty(IEnumerable<dynamic> oldCollection, IEnumerable<dynamic> newCollection, Type type)
        {
            if (type == typeof(EventLinkDTO))
                return oldCollection.Select(el => el.Url).SequenceEqual(newCollection.Select(el => el.Url));
            else if (type == typeof(EventOccurrenceDTO))
                return oldCollection.Select(eo => eo.FromDate).SequenceEqual(newCollection.Select(eo => eo.FromDate)) &&
                        oldCollection.Select(eo => eo.ToDate).SequenceEqual(newCollection.Select(eo => eo.ToDate));
            else if (type == typeof(EventModeratorDTO))
                return oldCollection.Select(em => em.UserId).SequenceEqual(newCollection.Select(em => em.UserId));
            //compare Id in all other cases
            else return oldCollection.Select(o => o.Id).SequenceEqual(newCollection.Select(o => o.Id));
        }

        private EventResponse? MapEvent(EventDTO? eventDTO, UserDTO? currentUser = null, bool eventTimeZoneOffset = false, bool isPublicUser = false)
        {
            EventResponse? response = _mapper.Map<EventResponse>(eventDTO);

            // Showing only the required properties for public user
            if (isPublicUser && eventDTO?.ShowInPublicSite == true)
            {
                response = new EventResponse()
                {
                    Id = response.Id,
                    TimeZone = response.TimeZone,
                    TimeZoneId = response.TimeZoneId,
                    Description = response.Description,
                    EventType = response.EventType,
                    Occurrences = response.Occurrences,
                    Subject = response.Subject,
                    Highlights = response.Highlights,
                    Categories = response.Categories,
                    LocationType = response.LocationType,
                    Location = response.Location
                };
            }

            if (response == null)
            {
                return null;
            }

            // For Edit Event page / For In-Person View Events / Virtual Events for public users
            if (eventTimeZoneOffset || response.LocationType == EventLocationType.InPerson || (response.LocationType == EventLocationType.Virtual && isPublicUser))
            {
                response.Occurrences = MapOccurrences(eventDTO!.TimeZone, response.Occurrences);
            }
            // for Virtual View Events
            else if (currentUser != null)
            {
                response.Occurrences = MapOccurrences(currentUser!.TimeZone, response.Occurrences);
            }

            return response;
        }

        private List<EventOccurrenceResponse> MapOccurrences(TimeZoneDTO timeZoneDto, List<EventOccurrenceResponse> occurrences)
        {
            try
            {
                TimeZoneInfo userTimeZoneInfo = GetTimeZoneInfoByWindowsName(timeZoneDto.WindowsName);

                foreach (var occurrence in occurrences)
                {
                    occurrence.FromDate = TimeZoneInfo.ConvertTimeFromUtc(occurrence.FromDate, userTimeZoneInfo);
                    occurrence.ToDate = TimeZoneInfo.ConvertTimeFromUtc(occurrence.ToDate, userTimeZoneInfo);

                    if (userTimeZoneInfo.IsDaylightSavingTime(occurrence.FromDate))
                    {
                        occurrence.TimeZoneName = timeZoneDto.DaylightName;
                        occurrence.TimeZoneAbbr = timeZoneDto.DaylightAbbreviation;
                        occurrence.TimeZoneUtcOffset = timeZoneDto.UTCOffset + 1;
                    }
                    else
                    {
                        occurrence.TimeZoneName = timeZoneDto.StandardName;
                        occurrence.TimeZoneAbbr = timeZoneDto.Abbreviation;
                        occurrence.TimeZoneUtcOffset = timeZoneDto.UTCOffset;
                    }
                }
            }
            catch (TimeZoneNotFoundException ex)
            {
                _logger.LogError(ex, $"Failed trying to get IANA time zone from {timeZoneDto.WindowsName}, Time_Zone_Id {timeZoneDto.Id}");
                throw new CustomException(CoreErrorMessages.TimeZoneNotFound, ex);
            }

            return occurrences;
        }
    }
}