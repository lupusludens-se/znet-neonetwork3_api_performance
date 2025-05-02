using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Event;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Models.Event;
using SE.Neo.Core.Services.Interfaces;


namespace SE.Neo.Core.Services
{
    public partial class EventService : BaseService, IEventService, IEventPublicService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<EventService> _logger;
        private readonly IMapper _mapper;
        private readonly ICommonService _commonService;

        public EventService(ApplicationContext context, ILogger<EventService> logger, IMapper mapper, ICommonService commonService) : base(null)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _commonService = commonService;
        }

        public async Task AddOrUpdateEventAttendeeAsync(int id, int userId, bool? isAttending)
        {
            Event @event = _context.Events.AsNoTracking().Where(e => e.Id == id).FirstOrDefault();
            if (@event == null || @event.StatusId == EventStatus.Deleted)
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            EventAttendee? eventAttendee = await _context.EventAttendees.SingleOrDefaultAsync(sp => sp.UserId == userId && sp.EventId == id);
            if (isAttending.HasValue)
            {
                if (eventAttendee == null)
                {
                    await _context.EventAttendees.AddAsync(new EventAttendee
                    {
                        UserId = userId,
                        EventId = id,
                        IsAttending = isAttending.Value
                    });
                }
                else
                {
                    if (eventAttendee.IsAttending == isAttending.Value)
                    {
                        throw new CustomException(eventAttendee.IsAttending ? "User already attending event." : "User already is not attending event.");
                    }

                    eventAttendee.IsAttending = isAttending.Value;
                    _context.EventAttendees.Update(eventAttendee);
                }
            }
            else if (eventAttendee != null)
            {
                _context.EventAttendees.Remove(eventAttendee);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateUpdateEventAsync(EventDTO model)
        {
            int eventId = model.Id;
            bool isEdit = eventId > 0;
            if (string.IsNullOrEmpty(model.Highlights))
                model.Highlights = null;
            var eventInviteType = model.InviteType;
            bool isPastEvent = model.Occurrences.OrderByDescending(oc => oc.ToDate).FirstOrDefault()?.ToDate < DateTime.UtcNow;

            List<int> alreadyInvitedUserIds = _context.EventInvitedUsers.Where(ec => ec.EventId == eventId).Select(e => e.UserId).ToList();
            List<int> invitedUserIds = model.InvitedUsers.Select(u => u.Id).ToList();
            List<int> newInvitedUserIds = invitedUserIds.Where(x => !alreadyInvitedUserIds.Contains(x)).ToList();

            List<int?> existingModeratorUserIds = _context.EventModerators.Where(ec => ec.EventId == eventId).Select(e => e.UserId).ToList();
            List<int?> newModeratorUserIds = model.Moderators.Select(u => u.UserId).ToList();

            bool moderatorsUpdated = newModeratorUserIds.Count() != existingModeratorUserIds.Count() ||
                existingModeratorUserIds.Except(newModeratorUserIds).Any();

            Event @event = new Event();
            if (isEdit)
            {
                @event = _context.Events.SingleOrDefault(b => b.Id == model.Id);
                if (@event == null || @event.StatusId == EventStatus.Deleted)
                    throw new CustomException("Error occurred on saving data. Event does not exist.");
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _mapper.Map(model, @event);
                    @event.StatusId = EventStatus.Active;
                    if (!isEdit)
                    {
                        _context.Events.AddRange(@event);
                        await _context.SaveChangesAsync();
                        eventId = @event.Id;
                    }
                    // remove related rows if updating 
                    else
                    {
                        _context.RemoveRange(_context.EventCategories.Where(ec => ec.EventId == eventId));
                        _context.RemoveRange(_context.EventLinks.Where(el => el.EventId == eventId));
                        _context.RemoveRange(_context.EventOccurrences.Where(eo => eo.EventId == eventId));
                        if (eventInviteType == EventInviteType.InviteAll || moderatorsUpdated)
                            _context.RemoveRange(_context.EventModerators.Where(em => em.EventId == eventId));
                        _context.RemoveRange(_context.EventInvitedCategories.Where(eic => eic.EventId == eventId));
                        _context.RemoveRange(_context.EventInvitedRegions.Where(ec => ec.EventId == eventId));
                        _context.RemoveRange(_context.EventInvitedRoles.Where(ec => ec.EventId == eventId));
                        if (eventInviteType == EventInviteType.InviteAll || (isPastEvent && newInvitedUserIds?.Count > 0))
                            _context.RemoveRange(_context.EventInvitedUsers.Where(ec => ec.EventId == eventId));
                    }
                    // add related rows
                    if (model.Categories != null)
                    {
                        _context.EventCategories.AddRange(model.Categories.Select(item => new EventCategory() { EventId = eventId, CategoryId = item.Id }));
                    }
                    if (model.Links != null)
                    {
                        _context.EventLinks.AddRange(model.Links.Select(item => new EventLink { EventId = eventId, Name = item.Name, Type = item.Type, Url = item.Url }));
                    }
                    if (model.Occurrences != null)
                    {
                        _context.EventOccurrences.AddRange(model.Occurrences.Select(item => new EventOccurrence { EventId = eventId, FromDate = item.FromDate, ToDate = item.ToDate }));
                    }
                    if ((model.Moderators != null && eventInviteType == EventInviteType.InviteAll) || moderatorsUpdated)
                    {
                        var invitedModeratorsFromDB = _context.EventModerators.Where(ec => ec.EventId == eventId);
                        var invitedModeratorsFromModel = model.Moderators.Select(item =>
                        new EventModerator
                        {
                            EventId = eventId,
                            UserId = item.UserId,
                            Name = item.Name,
                            Company = item.Company,
                            IsFirstTimeEmail = invitedModeratorsFromDB?.FirstOrDefault(x => x.EventId == eventId && x.UserId == item.UserId)?.IsFirstTimeEmail ?? true
                        });
                        _context.EventModerators.AddRange(invitedModeratorsFromModel);
                    }
                    if (model.InvitedCategories != null)
                    {
                        _context.EventInvitedCategories.AddRange(model.InvitedCategories.Select(item => new EventInvitedCategory { EventId = eventId, CategoryId = item.Id }));
                    }
                    if (model.InvitedRegions != null)
                    {
                        List<int> regionIds = _commonService.ExpandRegionListForFiltration(model.InvitedRegions.Select(item => item.Id).ToList(), false, true, false);
                        _context.EventInvitedRegions.AddRange(regionIds.Select(item => new EventInvitedRegion { EventId = eventId, RegionId = item }));
                    }
                    if (model.InvitedRoles != null)
                    {
                        _context.EventInvitedRoles.AddRange(model.InvitedRoles.Select(item => new EventInvitedRole { EventId = eventId, RoleId = item.Id }));
                        if (model.InvitedRoles.Any(r => r.Id == (int)RoleType.SolutionProvider) && !model.InvitedRoles.Any(r => r.Id == (int)RoleType.SPAdmin))
                        {
                            _context.EventInvitedRoles.Add(new EventInvitedRole { EventId = eventId, RoleId = (int)RoleType.SPAdmin });
                        }
                    }
                    // Invited Users
                    if (model.InvitedUsers != null)
                    {
                        var invitedUsersFromDB = _context.EventInvitedUsers.Where(ec => ec.EventId == eventId);
                        if (eventInviteType == EventInviteType.InviteAll || (isPastEvent && newInvitedUserIds?.Count > 0))
                        {
                            var invitedUsersFromModel = model.InvitedUsers.Select(item =>
                            new EventInvitedUser
                            {
                                EventId = eventId,
                                UserId = item.Id,
                                IsFirstTimeEmail = invitedUsersFromDB?.FirstOrDefault(x => x.EventId == eventId && x.UserId == item.Id)?.IsFirstTimeEmail ?? true
                            });
                            _context.EventInvitedUsers.AddRange(invitedUsersFromModel);
                        }
                        else if (eventInviteType == EventInviteType.InviteNewlyAdded)
                        {
                            _context.EventInvitedUsers.AddRange(model.InvitedUsers.Where(u => newInvitedUserIds.Any(nu => u.Id == nu))
                            .Select(item => new EventInvitedUser
                            {
                                EventId = eventId,
                                UserId = item.Id,
                                IsFirstTimeEmail = invitedUsersFromDB?.FirstOrDefault(x => x.EventId == eventId && x.UserId == item.Id)?.IsFirstTimeEmail ?? true
                            }));
                        }
                    }


                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            return @event.Id;
        }

        public async Task<WrapperModel<EventUserDTO>> GetEventAttendeesForUserAsync(int id, int userId, EventAttendeesFilter filter, bool allowedPrivate = false)
        {
            // get only users that attending event
            IQueryable<EventAttendee> query = ExpandEventAttendees(_context.EventAttendees.AsNoTracking(), filter.Expand)
                .Where(ea => ea.IsAttending && ea.EventId == id && ea.User.StatusId == Enums.UserStatus.Active);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await query.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<EventUserDTO>
                    {
                        DataList = new List<EventUserDTO>()
                    };
                }
            }

            if (filter.Skip.HasValue)
            {
                query = query.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                query = query.Take(filter.Take.Value);
            }

            List<EventAttendee> eventAttendees = await query
                .ToListAsync();

            var userDTOs = new List<EventUserDTO>();

            foreach (var eventAttendee in eventAttendees)
            {
                userDTOs.Add(MapEventAttendee(eventAttendee, userId));
            }

            return new WrapperModel<EventUserDTO>
            {
                Count = count,
                DataList = userDTOs
            };
        }

        public async Task<EventDTO?> GetEventForUserAsync(int userId, int id, string? expand = null, bool allowedPrivate = false)
        {
            IQueryable<Event> eventsQuery = ExpandEvents(_context.Events.AsNoTracking(), expand);
            eventsQuery = eventsQuery.Where(e => e.StatusId == EventStatus.Active);

            if (userId > 0)
            {
                List<int> userProfileRegions = await _commonService.GetRegionListForUserProfile(userId, true, true, true);
                eventsQuery = CreateDefaultInvitedEventQuery(eventsQuery, userProfileRegions, userId, allowedPrivate);
            }
            Event? @event = await eventsQuery.AsSplitQuery().FirstOrDefaultAsync(c => c.Id == id);

            if (@event == null)
            {
                return null;
            }

            return userId == 0
                ? _mapper.Map<EventDTO>(@event)
                : MapEvent(@event, userId);
        }


        public async Task<EventDTO?> GetEventAsync(int id, string? expand = null)
        {
            IQueryable<Event> eventsQuery = ExpandEvents(_context.Events.AsNoTracking(), expand);
            eventsQuery = eventsQuery.Where(e => e.StatusId == EventStatus.Active && e.ShowInPublicSite == true);

            Event? @event = await eventsQuery.AsSplitQuery().FirstOrDefaultAsync(c => c.Id == id);

            if (@event == null)
            {
                return null;
            }

            return _mapper.Map<EventDTO>(@event);
        }

        public async Task<WrapperModel<EventDTO>> GetEventsForUserAsync(int userId, EventsFilter filter, bool allowedPrivate = false)
        {
            IQueryable<Event> eventsQuery = ExpandEvents(_context.Events.AsNoTracking(), filter.Expand);
            eventsQuery = eventsQuery.Where(e => e.StatusId == EventStatus.Active);
            List<int> userProfileRegions = await _commonService.GetRegionListForUserProfile(userId, true, true, true);
            eventsQuery = CreateDefaultInvitedEventQuery(eventsQuery, userProfileRegions, userId, allowedPrivate);

            if (!string.IsNullOrEmpty(filter.Search))
            {
                eventsQuery = eventsQuery.Where(e => e.Subject.ToLower().Contains(filter.Search.ToLower()) || e.Description.ToLower().Contains(filter.Search.ToLower()));
            }

            DateTime fromDateValue = DateTime.UtcNow;
            if (filter.From.HasValue)
                fromDateValue = filter.From.Value.UtcDateTime;

            if (filter.From.HasValue && filter.To.HasValue)
            {
                DateTime toDateValue = filter.To.Value.UtcDateTime;
                eventsQuery = eventsQuery.Where(e => e.EventOccurrences.Any(eo => eo.FromDate >= fromDateValue && eo.ToDate <= toDateValue));
            }
            else
            {
                eventsQuery = eventsQuery
                    .Include(e => e.EventOccurrences.Where(eo => eo.FromDate >= fromDateValue))
                    .Where(e => e.EventOccurrences.Any(eo => eo.FromDate >= fromDateValue));
            }

            eventsQuery = eventsQuery.OrderBy(e => e.EventOccurrences.Where(eo => eo.FromDate >= fromDateValue).OrderBy(x => x.FromDate).Select(x => x.FromDate).FirstOrDefault());

            if (filter.HighlightedOnly)
                eventsQuery = eventsQuery.Where(e => e.IsHighlighted);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await eventsQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<EventDTO>
                    {
                        DataList = new List<EventDTO>()
                    };
                }
            }

            if (filter.Skip.HasValue)
            {
                eventsQuery = eventsQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                eventsQuery = eventsQuery.Take(filter.Take.Value);
            }

            List<Event> events = await eventsQuery
                .ToListAsync();

            var eventDTOs = new List<EventDTO>();

            foreach (var eventEntity in events)
            {
                eventDTOs.Add(MapEvent(eventEntity, userId));
            }

            return new WrapperModel<EventDTO>
            {
                Count = count,
                DataList = eventDTOs
            };
        }

        public async Task<WrapperModel<EventMatchingUserDTO>> GetUsersForEvent(int eventId, EventMatchingUserFilter filter)
        {
            IQueryable<EventUser> query = CreateEventUserQuery(eventId, filter)
                .OrderBy(eu => eu.Name);
            int count = 0;
            if (filter.IncludeCount)
            {
                count = await query.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<EventMatchingUserDTO>
                    {
                        DataList = new List<EventMatchingUserDTO>()
                    };
                }
            }
            if (filter.Skip.HasValue)
            {
                query = query.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                query = query.Take(filter.Take.Value);
            }
            List<EventUser> users = await query.ToListAsync();

            return new WrapperModel<EventMatchingUserDTO>
            {
                Count = count,
                DataList = users.Select(_mapper.Map<EventMatchingUserDTO>)
            };
        }

        public async Task<List<int>> GetEventInvitedUserIdsAsync(int eventId)
        {
            return await _context.EventInvitedUsers
                .AsNoTracking()
                .Where(eiu => eiu.EventId == eventId)
                .Include(eiu => eiu.User)
                .Select(eiu => eiu.User)
                .Where(u => u.StatusId != Enums.UserStatus.Deleted)
                .Select(u => u.Id)
                .ToListAsync();
        }

        public async Task<EventDTO> PatchEventAsync(int id, JsonPatchDocument patchDoc)
        {
            Event? @event = await _context.Events.SingleOrDefaultAsync(e => e.Id == id && e.StatusId != EventStatus.Deleted);
            if (@event != null)
            {
                try
                {
                    patchDoc.ApplyTo(@event);
                    _context.SaveChanges();
                    return _mapper.Map<EventDTO>(@event);
                }
                catch (Exception ex)
                {
                    throw new CustomException("Could not apply patch to Event.", ex);
                }
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }

        public async Task RemoveEventAsync(int id)
        {
            Event? @event = await _context.Events.SingleOrDefaultAsync(e => e.Id == id);
            if (@event != null)
            {
                @event.StatusId = EventStatus.Deleted;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }

        /// <summary>
        /// Updates only visibility property of Event without updating updated on field
        /// </summary>
        /// <param name="id"></param>
        /// <param name="showInPublicSite"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<int> UpdateEventVisibilityAsync(int id, bool showInPublicSite)
        {
            try
            {
                var param = GetParamsForVisibilityUpdate(id, showInPublicSite);

                int updatedRowsCount = await _context.Database.ExecuteSqlRawAsync("[dbo].[sp_UpdateEventShowInPublicSite] @Event_Id, @Show_In_Public_Site", param);
                await _context.SaveChangesAsync();
                return updatedRowsCount;
            }
            catch (Exception ex)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
        }

        public async Task<WrapperModel<EventDTO>> GetPublicUpcomingEvents(EventsFilter filter)
        {
            IQueryable<Event> eventsQuery = ExpandEvents(_context.Events.AsNoTracking(), filter.Expand);
            eventsQuery = eventsQuery.Where(e => e.StatusId == EventStatus.Active && e.ShowInPublicSite == true);

            DateTime fromDateValue = DateTime.UtcNow;
            if (filter.From.HasValue)
                fromDateValue = filter.From.Value.UtcDateTime;

            if (filter.From.HasValue && filter.To.HasValue)
            {
                DateTime toDateValue = filter.To.Value.UtcDateTime;
                eventsQuery = eventsQuery.Include(e => e.EventOccurrences.Where(eo => eo.FromDate >= fromDateValue && eo.ToDate <= toDateValue)).Where(e => e.EventOccurrences.Any(eo => eo.FromDate >= fromDateValue && eo.ToDate <= toDateValue));
            }
            else
            {
                eventsQuery = eventsQuery
                        .Include(e => e.EventOccurrences.Where(eo => eo.FromDate >= fromDateValue))
                        .Where(e => e.EventOccurrences.Any(eo => eo.FromDate >= fromDateValue));
            }

            eventsQuery = eventsQuery.OrderBy(e => e.EventOccurrences.Where(eo => eo.FromDate >= fromDateValue).OrderBy(x => x.FromDate).FirstOrDefault().FromDate);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await eventsQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<EventDTO>
                    {
                        DataList = new List<EventDTO>()
                    };
                }
            }

            if (filter.Skip.HasValue)
            {
                eventsQuery = eventsQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                eventsQuery = eventsQuery.Take(filter.Take.Value);
            }

            List<Event> events = await eventsQuery
                .ToListAsync();
            List<EventDTO> eventDTOs = events.Select(_mapper.Map<EventDTO>).ToList();

            return new WrapperModel<EventDTO>
            {
                Count = count,
                DataList = eventDTOs
            };
        }

        public async Task<WrapperModel<EventDTO>> GetPastEvents(EventsFilter filter)
        {
            IQueryable<Event> eventsQuery = ExpandEvents(_context.Events.AsNoTracking(), filter.Expand);
            eventsQuery = eventsQuery.Where(e => e.StatusId == EventStatus.Active && e.ShowInPublicSite == true);

            DateTime fromDateValue = DateTime.UtcNow;
            DateTime PastEventsDateRange = DateTime.UtcNow.AddYears(-1);

            eventsQuery = eventsQuery
                    .Include(e => e.EventOccurrences.Where(eo => eo.FromDate < fromDateValue && eo.FromDate >= PastEventsDateRange))
                    .Where(e => e.EventOccurrences.Any(eo => eo.FromDate < fromDateValue && eo.FromDate >= PastEventsDateRange));

            eventsQuery = eventsQuery.OrderByDescending(e => e.EventOccurrences.Where(eo => eo.FromDate < fromDateValue && eo.FromDate >= PastEventsDateRange).OrderBy(x => x.FromDate).Select(x => x.FromDate).FirstOrDefault());


            int count = 0;
            if (filter.IncludeCount)
            {
                count = await eventsQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<EventDTO>
                    {
                        DataList = new List<EventDTO>()
                    };
                }
            }

            if (filter.Skip.HasValue)
            {
                eventsQuery = eventsQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                eventsQuery = eventsQuery.Take(filter.Take.Value);
            }

            List<Event> events = await eventsQuery
                .ToListAsync();

            var eventDTOs = new List<EventDTO>();

            foreach (var eventEntity in events)
            {
                eventDTOs.Add(_mapper.Map<EventDTO>(eventEntity));
            }

            return new WrapperModel<EventDTO>
            {
                Count = count,
                DataList = eventDTOs
            };
        }

        private SqlParameter[] GetParamsForVisibilityUpdate(int eventId, bool showInPublicSite)
        {

            return new SqlParameter[] {
                         new SqlParameter() {
                            ParameterName = "@Event_Id",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = eventId
                        }, new SqlParameter() {
                            ParameterName = "@Show_In_Public_Site",
                            SqlDbType =  System.Data.SqlDbType.Bit,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = showInPublicSite
                        }, };
        }
    }
}
