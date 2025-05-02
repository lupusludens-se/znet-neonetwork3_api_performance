using Microsoft.EntityFrameworkCore;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Event;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Models.Event;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public partial class EventService : IEventService
    {
        private IQueryable<EventAttendee> ExpandEventAttendees(IQueryable<EventAttendee> query, string? expand = null)
        {
            query = query.Include(ea => ea.User);
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("company"))
                {
                    query = query
                        .Include(ea => ea.User)
                        .ThenInclude(u => u.Company);
                }
                if (expand.Contains("image"))
                {
                    query = query
                        .Include(ea => ea.User)
                        .ThenInclude(u => u.Image);
                }
                if (expand.Contains("followers"))
                {
                    query = query
                        .Include(ea => ea.User)
                        .ThenInclude(u => u.FollowerUsers);
                }
            }

            return query;
        }
        private IQueryable<Event> ExpandEvents(IQueryable<Event> query, string? expand = null)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("categories"))
                {
                    query = query.Include(e => e.EventCategories).ThenInclude(ec => ec.Category);
                }
                if (expand.Contains("moderators"))
                {
                    query = query.Include(e => e.EventModerators).ThenInclude(em => em.User);
                    if (expand.Contains("moderators.followers"))
                    {
                        query = query.Include(e => e.EventModerators)
                            .ThenInclude(em => em.User)
                            .ThenInclude(emf => emf.FollowerUsers);
                    }
                    if (expand.Contains("moderators.company"))
                    {
                        query = query.Include(e => e.EventModerators)
                            .ThenInclude(em => em.User)
                            .ThenInclude(u => u.Company);
                    }
                    if (expand.Contains("moderators.image"))
                    {
                        query = query.Include(e => e.EventModerators)
                            .ThenInclude(em => em.User)
                            .ThenInclude(u => u.Image);
                    }
                }
                if (expand.Contains("occurrences"))
                {
                    query = query.Include(e => e.EventOccurrences);
                }
                if (expand.Contains("links"))
                {
                    query = query.Include(e => e.EventLinks);
                }
                if (expand.Contains("attendees"))
                {
                    query = query.Include(e => e.EventAttendees
                        .Where(ea => ea.User.StatusId == Enums.UserStatus.Active))
                        .ThenInclude(ea => ea.User);
                    if (expand.Contains("attendees.followers"))
                    {
                        query = query.Include(e => e.EventAttendees
                            .Where(ea => ea.User.StatusId == Enums.UserStatus.Active))
                            .ThenInclude(ea => ea.User)
                            .ThenInclude(eau => eau.FollowerUsers);
                    }
                    if (expand.Contains("attendees.company"))
                    {
                        query = query.Include(e => e.EventAttendees
                            .Where(ea => ea.User.StatusId == Enums.UserStatus.Active))
                            .ThenInclude(ea => ea.User)
                            .ThenInclude(u => u.Company);
                    }
                    if (expand.Contains("attendees.image"))
                    {
                        query = query.Include(e => e.EventAttendees
                            .Where(ea => ea.User.StatusId == Enums.UserStatus.Active))
                            .ThenInclude(ea => ea.User)
                            .ThenInclude(u => u.Image);
                    }
                }
                if (expand.Contains("invitedcategories"))
                {
                    query = query.Include(e => e.EventInvitedCategories)
                        .ThenInclude(eic => eic.Category);
                }
                if (expand.Contains("invitedregions"))
                {
                    query = query.Include(e => e.EventInvitedRegions)
                        .ThenInclude(eir => eir.Region);
                }
                if (expand.Contains("invitedroles"))
                {
                    query = query.Include(e => e.EventInvitedRoles)
                        .ThenInclude(eir => eir.Role);
                }
                if (expand.Contains("invitedusers"))
                {
                    query = query.Include(e => e.EventInvitedUsers)
                        .ThenInclude(eiu => eiu.User);
                    if (expand.Contains("invitedusers.company"))
                    {
                        query = query.Include(e => e.EventInvitedUsers)
                            .ThenInclude(eiu => eiu.User)
                            .ThenInclude(u => u.Company);
                    }
                    if (expand.Contains("invitedusers.image"))
                    {
                        query = query.Include(e => e.EventInvitedUsers)
                            .ThenInclude(ea => ea.User)
                            .ThenInclude(u => u.Image);
                    }
                }
                if (expand.Contains("timezone"))
                {
                    query = query.Include(e => e.TimeZone);
                }
            }

            return query;
        }

        private EventDTO MapEvent(Event @event, int userId)
        {
            var eventDTO = _mapper.Map<EventDTO>(@event);

            eventDTO.IsAttending = @event.EventAttendees?.FirstOrDefault(x => x.UserId == userId)?.IsAttending;

            if (@event.EventModerators != null)
            {
                IEnumerable<User> eventModerators = @event.EventModerators
                    .Where(em => em.UserId.HasValue && em.User != null)
                    .Select(em => em.User)!;
                if (eventModerators.All(em => em.FollowerUsers != null))
                {
                    foreach (var moderatorDTO in eventDTO.Moderators.Where(m => m.UserId.HasValue))
                    {
                        moderatorDTO.User!.IsFollowed = eventModerators
                            .SingleOrDefault(u => u.Id == moderatorDTO.UserId!.Value)!.FollowerUsers
                            .Any(fu => fu.FollowerId == userId);
                    }
                }
            }

            if (@event.EventAttendees != null && @event.EventAttendees.Count != 0)
            {
                IEnumerable<User> eventAttendees = @event.EventAttendees!
                    .Where(ea => ea.User != null)
                    .Select(ea => ea.User)!;
                if (eventAttendees.All(em => em.FollowerUsers != null))
                {
                    foreach (var attendeeDTO in eventDTO.Attendees)
                    {
                        attendeeDTO.IsFollowed = eventAttendees
                            .SingleOrDefault(u => u.Id == attendeeDTO.Id)!.FollowerUsers
                            .Any(fu => fu.FollowerId == userId);
                    }
                }
            }

            return eventDTO;
        }

        private EventUserDTO MapEventAttendee(EventAttendee eventAttendee, int userId)
        {
            var userDTO = _mapper.Map<EventUserDTO>(eventAttendee.User);

            userDTO.IsFollowed = eventAttendee.User?.FollowerUsers?.Any(x => x.FollowerId == userId) ?? false;

            return userDTO;
        }

        private IQueryable<Event> CreateDefaultInvitedEventQuery(IQueryable<Event> query, List<int> userProfileRegions, int userId, bool allowedPrivate)
        {
            IQueryable<int> userProfileCategories = _context.UserProfileCategories
                .Where(upc => upc.UserProfile.UserId == userId)
                .Select(x => x.CategoryId);

            query = query.Where(e => allowedPrivate
                || e.EventInvitedUsers.Any(eiu => eiu.UserId == userId)
                || e.EventModerators.Any(em => em.UserId == userId)
                || e.EventType == EventType.Public);

            return query;
        }

        private IQueryable<EventUser> CreateEventUserQuery(int eventId, EventMatchingUserFilter filter)
        {
            IQueryable<User> matchingUsersQuery = CreateMatchingUserQuery(filter);

            if (eventId == 0) // case where event is not yet created
            {
                IQueryable<EventUser> matchingEventUsersQuery = matchingUsersQuery
                    .Select(u => new EventUser
                    {
                        Id = u.Id,
                        Image = u.Image,
                        Name = u.FirstName + " " + u.LastName,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Company = u.Company.Name,
                        IsInvited = false,
                        IsMatching = true
                    });

                IQueryable<EventUser> notMatchingUsersQuery = CreateNotMatchingUsersQuery(matchingUsersQuery, filter.Search)
                    .Select(u => new EventUser
                    {
                        Id = u.Id,
                        Image = u.Image,
                        Name = u.FirstName + " " + u.LastName,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Company = u.Company.Name,
                        IsInvited = false,
                        IsMatching = false
                    });
                // not matching users only returned if Search is valid
                return !string.IsNullOrEmpty(filter.Search) && filter.Search.Length > 1
                    ? matchingEventUsersQuery
                        .Union(notMatchingUsersQuery)
                    : matchingEventUsersQuery;
            }

            IQueryable<User> invitedUsersQuery = CreateInvitedUserQuery(eventId, filter);
            IQueryable<EventUser> matchAndInvitedUsersQuery = matchingUsersQuery
                .Where(u => invitedUsersQuery.Select(u => u.Id).Any(id => id == u.Id))
                .Select(u => new EventUser
                {
                    Id = u.Id,
                    Image = u.Image,
                    Name = u.FirstName + " " + u.LastName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Company = u.Company.Name,
                    IsInvited = true,
                    IsMatching = true
                });
            IQueryable<EventUser> matchNotInvitedUsersQuery = matchingUsersQuery
                .Where(u => !invitedUsersQuery.Select(u => u.Id).Contains(u.Id))
                .Select(u => new EventUser
                {
                    Id = u.Id,
                    Image = u.Image,
                    Name = u.FirstName + " " + u.LastName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Company = u.Company.Name,
                    IsInvited = false,
                    IsMatching = true
                });
            IQueryable<EventUser> invitedNotMatchingUsersQuery = invitedUsersQuery
                .Where(u => !matchingUsersQuery.Select(u => u.Id).Contains(u.Id))
                .Select(u => new EventUser
                {
                    Id = u.Id,
                    Image = u.Image,
                    Name = u.FirstName + " " + u.LastName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Company = u.Company.Name,
                    IsInvited = true,
                    IsMatching = false
                });
            IQueryable<EventUser> notMatchOrInvitedUser = CreateNotMatchingUsersQuery(matchingUsersQuery, filter.Search)
                .Where(u => !invitedUsersQuery.Select(u => u.Id).Contains(u.Id))
                .Select(u => new EventUser
                {
                    Id = u.Id,
                    Image = u.Image,
                    Name = u.FirstName + " " + u.LastName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Company = u.Company.Name,
                    IsInvited = false,
                    IsMatching = false
                });
            // not match not invited users only returned if Search is valid 
            return !string.IsNullOrEmpty(filter.Search) && filter.Search.Length > 1
                ? matchAndInvitedUsersQuery
                    .Union(matchNotInvitedUsersQuery)
                    .Union(invitedNotMatchingUsersQuery)
                    .Union(notMatchOrInvitedUser)
                : matchAndInvitedUsersQuery
                    .Union(matchNotInvitedUsersQuery)
                    .Union(invitedNotMatchingUsersQuery);
        }

        private IQueryable<User> CreateInvitedUserQuery(int eventId, EventMatchingUserFilter filter)
        {
            IQueryable<User> invitedUser = _context.EventInvitedUsers
                .AsNoTracking()
                .Include(eiu => eiu.User)
                    .ThenInclude(u => u.Company)
                .Include(eiu => eiu.User)
                    .ThenInclude(u => u.Image)
                .Where(eiu => eiu.EventId == eventId)
                .Select(eiu => eiu.User);
            return SearchUsers(invitedUser, filter.Search);
        }

        private IQueryable<User> CreateMatchingUserQuery(EventMatchingUserFilter filter)
        {
            IQueryable<User> query = _context.Users
                .AsNoTracking()
                .Include(u => u.Company)
                .Include(u => u.Image)
                .Where(u => u.StatusId == Enums.UserStatus.Active);

            string matchBy = filter.MatchBy.ToLower();
            foreach (string property in matchBy.Split("&").ToList())
            {
                var ids = ParseFilterByField(property);
                if (ids != null && ids.Count > 0)
                {
                    if (property.Contains("regionids"))
                    {
                        List<int> regionIds = _commonService.ExpandRegionListForFiltration(ids, true, true);
                        query = query
                            .Include(u => u.UserProfile)
                            .ThenInclude(up => up.Regions)
                            .ThenInclude(upr => upr.Region)
                            .Where(u => u.UserProfile.Regions.Where(upr => !upr.Region.IsDeleted).Select(upr => upr.RegionId).Any(rId => regionIds.Contains(rId)));
                    }

                    if (property.Contains("categoryids"))
                    {
                        query = query
                            .Include(u => u.UserProfile)
                            .ThenInclude(up => up.Categories)
                            .ThenInclude(upc => upc.Category)
                            .Where(u => u.UserProfile.Categories.Where(upc => !upc.Category.IsDeleted).Select(upc => upc.CategoryId).Any(cId => ids.Contains(cId)));
                    }

                    if (property.Contains("roleids"))
                    {
                        query = query
                            .Include(u => u.Roles)
                            .Where(u => u.Roles.Select(ur => ur.RoleId).Any(rId => ids.Contains(rId)));
                    }
                }
            }

            return SearchUsers(query, filter.Search);
        }

        private IQueryable<User>? CreateNotMatchingUsersQuery(IQueryable<User> matchingQueryable, string? search)
        {
            IQueryable<User> notMatchingUsersQuery = _context.Users
                    .AsNoTracking()
                    .Include(u => u.Image)
                    .Include(u => u.Company)
                    .Where(u => u.StatusId == Enums.UserStatus.Active)
                    .Where(u => !matchingQueryable.Select(u => u.Id).Contains(u.Id));
            return SearchUsers(notMatchingUsersQuery, search);
        }

        private IQueryable<User> SearchUsers(IQueryable<User> query, string? search)
        {
            if (!string.IsNullOrEmpty(search) && search.Length > 1)
            {
                query = query.Where(u => u.FirstName.StartsWith(search) ||
                        u.LastName.StartsWith(search) ||
                        (u.FirstName + " " + u.LastName).StartsWith(search) ||
                        (u.Company.Status.Id == Enums.CompanyStatus.Active && u.Company.Name.StartsWith(search)));
            }
            return query;
        }
    }
}
