using AutoMapper.Internal;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Index.HPRtree;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Notifications.Details.Multiple;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Enums;
using SE.Neo.EmailAlertSender.Models;
using SE.Neo.EmailTemplates.Models;
using System.Text.Json;
using TimeZone = SE.Neo.Core.Entities.TimeZone;

namespace SE.Neo.EmailAlertSender
{
    public partial class EmailAlertSendService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastRun"></param>
        /// <param name="now"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task SendEventAlertsAsync(DateTime lastRun, DateTime now, ActionContext context)
        {
            // Event Invitation Alerts
            try
            {
                int totalEmailsToSend = 0;
                int failedEmails = 0;
                List<int> eventIds = new();

                List<EventInvitationItem> newEventInvitations = await _context.EventInvitedUsers
                    .AsNoTracking()
                    .Where(eiu => lastRun <= eiu.CreatedOn)
                    .Where(eiu => eiu.Event.StatusId == EventStatus.Active)
                    .Where(eiu => eiu.Event.EventOccurrences.Count > 0 && eiu.Event.EventOccurrences.OrderByDescending(e => e.ToDate).First().ToDate > now)
                    .Select(eiu => new EventInvitationItem
                    {
                        EventId = eiu.EventId,
                        IsFirstTimeEmail = eiu.IsFirstTimeEmail,
                        UserFirstName = eiu.User.FirstName,
                        UserEmailAddress = eiu.User.Email,
                        LocalTimeZone = eiu.Event.LocationType == EventLocationType.Virtual ? eiu.User.TimeZone : eiu.Event.TimeZone,
                        EventDescription = eiu.Event.Description,
                        EventHighlights = eiu.Event.Highlights,
                        EventType = eiu.Event.LocationType,
                        EventName = eiu.Event.Subject,
                        EventCreatedDate = Convert.ToDateTime(eiu.Event.CreatedOn),
                        EventModifiedDate = Convert.ToDateTime(eiu.Event.ModifiedOn),
                        EventDates = eiu.Event.EventOccurrences.Select(eo => new EventAlertDateInfo
                        {
                            EventDateStart = eo.FromDate,
                            EventDateEnd = eo.ToDate,
                        }).ToList()
                    })
                    .ToListAsync();

                List<EventInvitationItem> eventModerators = await _context.EventModerators
                    .AsNoTracking()
                    .Where(eiu => lastRun <= eiu.CreatedOn)
                    .Where(eiu => eiu.Event.StatusId == EventStatus.Active)
                    .Where(eiu => eiu.Event.EventOccurrences.Count > 0 && eiu.Event.EventOccurrences.OrderByDescending(e => e.ToDate).First().ToDate > now)
                    .Select(eiu => new EventInvitationItem
                    {
                        EventId = eiu.EventId,
                        UserFirstName = eiu.User.FirstName,
                        IsFirstTimeEmail = eiu.IsFirstTimeEmail,
                        UserEmailAddress = eiu.User.Email,
                        LocalTimeZone = eiu.Event.LocationType == EventLocationType.Virtual ? eiu.User.TimeZone : eiu.Event.TimeZone,
                        EventDescription = eiu.Event.Description,
                        EventHighlights = eiu.Event.Highlights,
                        EventType = eiu.Event.LocationType,
                        EventName = eiu.Event.Subject,
                        EventCreatedDate = Convert.ToDateTime(eiu.Event.CreatedOn),
                        EventModifiedDate = Convert.ToDateTime(eiu.Event.ModifiedOn),
                        EventDates = eiu.Event.EventOccurrences.Select(eo => new EventAlertDateInfo
                        {
                            EventDateStart = eo.FromDate,
                            EventDateEnd = eo.ToDate,
                        }).ToList()
                    })
                    .ToListAsync();

                if (eventModerators.Any())
                {
                    newEventInvitations.AddRange(eventModerators);
                }

                newEventInvitations = newEventInvitations.DistinctBy(x => x.UserEmailAddress).ToList();

                totalEmailsToSend += newEventInvitations.Count();
                _logger.LogInformation($"{totalEmailsToSend} event invitation alerts found");

                //List<Task> emailInvitationTasks = new List<Task>();
                _logger.LogInformation($"Email Address: {string.Join(",", newEventInvitations.Select(x => x.UserEmailAddress).ToList())}");
                foreach (EventInvitationItem invitation in newEventInvitations)
                {
                    var subject = invitation.IsFirstTimeEmail == true ? _emailAlertConfig.CreateEventInvitationAlertSubject : _emailAlertConfig.UpdateEventInvitationAlertSubject;
                    try
                    {
                        var template = _mapper.Map<EventInvitationEmailTemplatedModel>(invitation);

                        template.EventDates = invitation.EventDates.Select(ed => ConvertEventDateToLocal(ed, invitation.LocalTimeZone)).ToList();
                        template.Link = string.Format(_baseAppConfig.BaseAppUrlPattern, "events", invitation.EventId, "/email");
                        template.EventInfo = invitation.EventDescription;
                        SetLogoLinks(template);

                        _logger.LogInformation(invitation.UserEmailAddress, string.Format(_emailAlertConfig.CreateEventInvitationAlertSubject, template.EventName));
                        await _emailService.SendTemplatedEmailAsync(invitation.UserEmailAddress, string.Format(subject, template.EventName), template, context);

                        eventIds.Add(invitation.EventId);
                    }
                    catch (Exception ex)
                    {
                        failedEmails++;
                        _logger.LogError(ex, $"Error attempting to send {string.Format(subject, invitation.EventName)} to {invitation.UserEmailAddress}");
                    }
                }

                _context.EventModerators.Where(e => eventIds.Contains(e.EventId) && e.IsFirstTimeEmail == true).ToList().ForEach(c =>
                {
                    c.IsFirstTimeEmail = false;
                });

                _context.EventInvitedUsers.Where(e => eventIds.Contains(e.EventId) && e.IsFirstTimeEmail == true).ToList().ForEach(c =>
                {
                    c.IsFirstTimeEmail = false;
                });
                _context.SaveChanges();


                _logger.LogInformation($"Finished event invitation alerts, {totalEmailsToSend} alerts found, {failedEmails} failed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Email alert job failed during event invitation alerts.");
            }

            // Event Reminder Alerts
            try
            {
                int totalEmailsToSend = 0;
                int failedEmails = 0;

                if (!(now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday))
                {
                    List<EventReminderItem> eventReminders = await _context.Events
                    .AsNoTracking()
                    .Where(e => e.StatusId == EventStatus.Active)
                    .Where(e => e.EventOccurrences.Any(eo => lastRun.AddWorkDays(3) < eo.FromDate && eo.FromDate < now.AddWorkDays(3)))
                    .Select(e => new EventReminderItem
                    {
                        EventId = e.Id,
                        EventDescription = e.Description,
                        EventHighlights = e.Highlights,
                        EventType = e.LocationType,
                        EventName = e.Subject,
                        EventTimeZone = e.TimeZone,
                        EventDates = e.EventOccurrences.Select(eo => new EventAlertDateInfo
                        {
                            EventDateStart = eo.FromDate,
                            EventDateEnd = eo.ToDate
                        }),
                        Users = e.EventAttendees.Select(ea => new EventUserInfo
                        {
                            UserEmailAddress = ea.User.Email,
                            UserFirstName = ea.User.FirstName,
                            UserId = ea.UserId,
                            UserTimeZone = ea.User.TimeZone
                        })
                        .Union(e.EventInvitedUsers.Select(eiu => new EventUserInfo { UserEmailAddress = eiu.User.Email, UserFirstName = eiu.User.FirstName, UserId = eiu.UserId, UserTimeZone = eiu.User.TimeZone }))
                    })
                    .ToListAsync();

                    totalEmailsToSend += eventReminders.SelectMany(eri => eri.Users).Count();
                    _logger.LogInformation($"{totalEmailsToSend} event reminder alerts found");

                    foreach (EventReminderItem eventReminderItem in eventReminders)
                    {
                        _logger.LogInformation($"Email Address {eventReminderItem.EventName}: {string.Join(",", eventReminderItem.Users.DistinctBy(x => x.UserId).Select(x => x.UserEmailAddress).ToList())}");
                        foreach (EventUserInfo user in eventReminderItem.Users.DistinctBy(eui => eui.UserId))
                        {
                            try
                            {
                                TimeZone localTimeZone = eventReminderItem.EventType == EventLocationType.Virtual ? user.UserTimeZone : eventReminderItem.EventTimeZone;
                                //TimeZoneInfo localTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(localTimeZone.SystemName);

                                var template = _mapper.Map<EventInvitationEmailTemplatedModel>(eventReminderItem);
                                template.EventDates = eventReminderItem.EventDates.Select(e => ConvertEventDateToLocal(e, localTimeZone)).ToList();
                                template.FirstName = user.UserFirstName;
                                template.EventInfo = eventReminderItem.EventDescription;
                                template.Link = String.Format(_baseAppConfig.BaseAppUrlPattern, "events", eventReminderItem.EventId, "/email");
                                SetLogoLinks(template);

                                _logger.LogInformation(user.UserEmailAddress, string.Format(_emailAlertConfig.EventReminderAlertSubject, template.EventName));

                                await _emailService.SendTemplatedEmailAsync(user.UserEmailAddress, string.Format(_emailAlertConfig.EventReminderAlertSubject, template.EventName), template, context);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Failed trying to send {_emailAlertConfig.EventReminderAlertSubject} to {user.UserEmailAddress}");
                            }
                        }
                    }
                }
                _logger.LogInformation($"Finished sending event reminder alerts {totalEmailsToSend} alerts found, {failedEmails} failed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Email alert job failed during event reminder alerts.");
            }

            // Event Deleted Alerts
            try
            {
                int totalEmailsToSend = 0;
                int failedEmails = 0;

                List<EventReminderItem> deletedEvents = await _context.Events
                    .AsNoTracking()
                    .Where(e => lastRun <= e.ModifiedOn && e.StatusId == EventStatus.Deleted)
                    .Select(e => new EventReminderItem
                    {
                        EventId = e.Id,
                        EventDescription = e.Description,
                        EventHighlights = e.Highlights, // check if this is right
                        EventType = e.LocationType,
                        EventName = e.Subject,
                        EventTimeZone = e.TimeZone,
                        EventDates = e.EventOccurrences.Select(eo => new EventAlertDateInfo
                        {
                            EventDateStart = eo.FromDate,
                            EventDateEnd = eo.ToDate
                        }),
                        Users = e.EventAttendees.Select(ea => new EventUserInfo
                        {
                            UserEmailAddress = ea.User.Email,
                            UserFirstName = ea.User.FirstName,
                            UserId = ea.UserId,
                            UserTimeZone = ea.User.TimeZone
                        })
                        .Union(e.EventInvitedUsers.Select(eiu => new EventUserInfo { UserEmailAddress = eiu.User.Email, UserFirstName = eiu.User.FirstName, UserId = eiu.UserId, UserTimeZone = eiu.User.TimeZone }))
                    })
                    .ToListAsync();

                totalEmailsToSend += deletedEvents.Sum(eir => eir.Users.Count());
                _logger.LogInformation($"{totalEmailsToSend} event delete alerts found");

                foreach (EventReminderItem deletedEventItem in deletedEvents)
                {
                    _logger.LogInformation($"Email Address {deletedEventItem.EventName}: {string.Join(",", deletedEventItem.Users.DistinctBy(x => x.UserId).Select(x => x.UserEmailAddress).ToList())}");
                    foreach (EventUserInfo user in deletedEventItem.Users.DistinctBy(eui => eui.UserId))
                    {
                        try
                        {
                            var template = _mapper.Map<EventInvitationEmailTemplatedModel>(deletedEventItem);

                            template.EventDates = deletedEventItem.EventDates.Select(e => deletedEventItem.EventType == EventLocationType.Virtual ? ConvertEventDateToLocal(e, user.UserTimeZone) : ConvertEventDateToLocal(e, deletedEventItem.EventTimeZone)).ToList();
                            template.FirstName = user.UserFirstName;
                            template.EventInfo = deletedEventItem.EventDescription;
                            template.Link = string.Format(_baseAppConfig.BaseAppUrlPattern, "events", deletedEventItem.EventId, "/email");
                            template.EventDateLogoUrl = _emailAssetsConfig.EventsDate;
                            template.EventTimeLogoUrl = _emailAssetsConfig.EventsTime;
                            SetLogoLinks(template);

                            _logger.LogInformation(user.UserEmailAddress, string.Format(_emailAlertConfig.EventDeletedAlertSubject, template.EventName));

                            await _emailService.SendTemplatedEmailAsync(user.UserEmailAddress, string.Format(_emailAlertConfig.EventDeletedAlertSubject, template.EventName), template, context);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error attempting to send {_emailAlertConfig.EventDeletedAlertSubject} to {user.UserEmailAddress}");
                        }
                    }
                }
                _logger.LogInformation($"Finished sending cancelled email alerts {totalEmailsToSend} alerts found, {failedEmails} failed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed during sending cancelled email alerts.");
            }
        }

        private async Task SendSummaryAlertsAsync(List<UserEmailAlertItem> summaryAlerts, DateTime lastRun, DateTime now, ActionContext context)
        {
            int totalEmails = 0;
            int failedEmails = 0;

            // List<Task> emailTasks = new List<Task>();
            _logger.LogInformation($"Email Address: {string.Join(",", summaryAlerts.DistinctBy(x => x.UserId).Select(x => x.UserEmailAddress).ToList())}");
            foreach (var group in summaryAlerts.GroupBy(ueai => ueai.UserId))
            {
                var userId = group.First().UserId;
                try
                {
                    List<UserEmailAlertItem> userAlerts = group.ToList();

                    var userTimeZone = group.First().UserTimeZone.WindowsName;
                    var userTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
                    var userCurrentTime = TimeZoneInfo.ConvertTimeFromUtc(now, GetTimeZoneInfoByWindowsName(userTimeZone));
                    var userLastRunCurrentTime = TimeZoneInfo.ConvertTimeFromUtc(lastRun, GetTimeZoneInfoByWindowsName(userTimeZone));

                    bool daily = SendSummaryDaily(userLastRunCurrentTime, userCurrentTime);
                    bool weekly = SendSummaryWeekly(userLastRunCurrentTime, userCurrentTime);
                    bool monthly = SendSummaryMonthly(userLastRunCurrentTime, userCurrentTime);

                    _logger.LogInformation($"Email alert for user id:{group.First().UserFirstName} , {group.First().UserLastName} : user last current time : {userLastRunCurrentTime}, user current time : {userCurrentTime}, : last run time = {lastRun}, daily = {daily}, weekly = {weekly}, monthly = {monthly}");


                    if (!(daily || weekly || monthly))
                        continue;

                    Tuple<DateTime, DateTime> boundaries = GetSummaryTimeBoundaries(group.First().EmailAlertFrequency, userCurrentTime, userLastRunCurrentTime, userTimeZoneInfo);

                    DateTime lastTuestday = now.AddDays(-7);
                    IQueryable<SummaryAlertIdItem>? summaryProjectQuery = null;
                    IQueryable<SummaryAlertIdItem>? summaryLearnQuery = null;
                    IQueryable<SummaryAlertIdItem>? summaryEventQuery = null;
                    IQueryable<SummaryAlertIdItem>? summaryForumQuery = null;
                    IQueryable<SummaryAlertIdItem>? summaryInitiativeQuery = null;
                    UserEmailAlertItem? summaryAlert = userAlerts.Where(ueai => ueai.EmailAlertCategory == EmailAlertCategory.Summary &&
                            ((ueai.EmailAlertFrequency == EmailAlertFrequency.Daily && daily) ||
                            (ueai.EmailAlertFrequency == EmailAlertFrequency.Weekly && weekly) ||
                            (ueai.EmailAlertFrequency == EmailAlertFrequency.Monthly && monthly))).FirstOrDefault();

                    if (summaryAlert != null)
                    {
                        if (summaryAlert.UserRoleIds.Any(id => id == (int)RoleType.Corporation) == true)
                        {
                            summaryInitiativeQuery = _context.Initiative
                                .AsNoTracking()
                                .Where(p => p.StatusId == Core.Enums.InitiativeStatus.Active)
                                .Where(p => p.UserId == userId)
                                .OrderByDescending(m => m.ModifiedOn)
                                .Select(p => new SummaryAlertIdItem
                                {
                                    ItemType = EmailSummaryItemType.Initiative,
                                    Id = p.Id
                                });
                        }

                        if (summaryAlert.UserRoleIds.Any(id => id == (int)RoleType.SolutionProvider) == false && summaryAlert.UserRoleIds.Any(id => id == (int)RoleType.SPAdmin) == false)
                        {
                            summaryProjectQuery = _context.Projects
                                .AsNoTracking()
                                .Where(p => p.StatusId == Core.Enums.ProjectStatus.Active)
                                .Where(p => p.Regions.Select(pr => pr.RegionId).Any(rId => summaryAlert.UserRegionIds.Contains(rId)) ||
                                            summaryAlert.UserCategoryIds.Any(cId => cId == p.CategoryId))
                                .Where(p => boundaries.Item1 < p.ModifiedOn && p.ModifiedOn < boundaries.Item2)
                                .OrderBy(p => p.CreatedOn == p.ModifiedOn ? 1 :
                                 (summaryAlert.EmailAlertFrequency == EmailAlertFrequency.Daily && (p.CreatedOn.Value.Day == now.Day && p.CreatedOn.Value.Month == now.Month
                                 && p.CreatedOn.Value.Year == now.Year)) ||
                                 (summaryAlert.EmailAlertFrequency == EmailAlertFrequency.Weekly && p.CreatedOn > new DateTime(lastTuestday.Year, lastTuestday.Month, lastTuestday.Day, 0, 0, 0)) ||
                                 (summaryAlert.EmailAlertFrequency == EmailAlertFrequency.Monthly && p.CreatedOn > new DateTime(now.Year, now.Month, 1, 0, 0, 0))
                                 ? 2 : 3).ThenByDescending(m => m.ModifiedOn)
                                .Select(p => new SummaryAlertIdItem
                                {
                                    ItemType = EmailSummaryItemType.Project,
                                    Id = p.Id
                                });
                        }

                        summaryLearnQuery = _context.Articles
                                                   .AsNoTracking()
                                                   .Where(a => a.IsDeleted == false)
                                                   .Where(e => e.ArticleRoles.Select(eir => eir.RoleId).Any(rId => summaryAlert.UserRoleIds.Contains(rId) || (summaryAlert.UserRoleIds.Contains((int)RoleType.SPAdmin) == true && rId == (int)RoleType.SolutionProvider)))
                                                   .Where(a => a.ArticleRegions.Select(ar => ar.RegionId).Any(rId => summaryAlert.UserRegionIds.Contains(rId)) ||
                                                               a.ArticleCategories.Select(ac => ac.CategoryId).Any(cId => summaryAlert.UserCategoryIds.Contains(cId)))
                                                   .Where(a => boundaries.Item1 < a.ModifiedOn && a.ModifiedOn < boundaries.Item2)
                                                   .Where(a => !string.IsNullOrEmpty(_summaryEmailSettingsConfig.LearnStartDate) ? a.CreatedOn >= Convert.ToDateTime(_summaryEmailSettingsConfig.LearnStartDate) : a.CreatedOn >= DateTime.MinValue)
                                                   .OrderBy(p => p.CreatedOn == p.ModifiedOn ? 1 :
                                                        (summaryAlert.EmailAlertFrequency == EmailAlertFrequency.Daily && (p.CreatedOn.Value.Day == now.Day && p.CreatedOn.Value.Month == now.Month
                                                        && p.CreatedOn.Value.Year == now.Year)) ||
                                                        (summaryAlert.EmailAlertFrequency == EmailAlertFrequency.Weekly && p.CreatedOn > new DateTime(lastTuestday.Year, lastTuestday.Month, lastTuestday.Day, 0, 0, 0)) ||
                                                        (summaryAlert.EmailAlertFrequency == EmailAlertFrequency.Monthly && p.CreatedOn > new DateTime(now.Year, now.Month, 1, 0, 0, 0))
                                                        ? 2 : 3).ThenByDescending(m => m.ModifiedOn)

                                                   .Take(5)
                                                   .Select(a => new SummaryAlertIdItem
                                                   {
                                                       ItemType = EmailSummaryItemType.Learn,
                                                       Id = a.Id
                                                   });

                        summaryEventQuery = _context.Events
                            .AsNoTracking()
                            .Where(e => e.StatusId == EventStatus.Active)
                            .Where(e => (e.EventInvitedRegions.Any() == false || e.EventInvitedRegions.Select(eir => eir.RegionId).Any(rId => summaryAlert.UserRegionIds.Contains(rId))))
                            .Where(e => (e.EventInvitedCategories.Any() == false || e.EventInvitedCategories.Select(eic => eic.CategoryId).Any(cId => summaryAlert.UserCategoryIds.Contains(cId))))
                            .Where(e => e.EventInvitedRoles.Select(eir => eir.RoleId).Any(rId => summaryAlert.UserRoleIds.Contains(rId) || (summaryAlert.UserRoleIds.Contains((int)RoleType.SPAdmin) == true && rId == (int)RoleType.SolutionProvider)))
                            .Where(e => e.EventCategories.Select(ec => ec.CategoryId).Any(cId => summaryAlert.UserCategoryIds.Contains(cId)))
                            .Where(e => boundaries.Item1 < e.ModifiedOn && e.ModifiedOn < boundaries.Item2)
                            .Where(e => e.IsHighlighted == true)
                            .OrderByDescending(e => e.EventOccurrences.OrderByDescending(eo => eo.FromDate).First())
                            .Take(5)
                            .Select(e => new SummaryAlertIdItem
                            {
                                ItemType = EmailSummaryItemType.Event,
                                Id = e.Id
                            });


                        summaryForumQuery = _context.Discussions
                            .AsNoTracking()
                            .Where(d => d.CreatedByUserId != summaryAlert.UserId)
                            .Where(d => d.Type == DiscussionType.PublicForum && d.IsDeleted == false)
                            .Where(d => d.DiscussionCategories.Select(dc => dc.CategoryId).Any(cId => summaryAlert.UserCategoryIds.Contains(cId)) ||
                                d.DiscussionRegions.Select(dr => dr.RegionId).Any(cId => summaryAlert.UserRegionIds.Contains(cId)))
                            .Where(d => boundaries.Item1 < d.ModifiedOn && d.ModifiedOn < boundaries.Item2)
                            .OrderBy(p => p.CreatedOn == p.ModifiedOn ? 1 :
                                 (summaryAlert.EmailAlertFrequency == EmailAlertFrequency.Daily && (p.CreatedOn.Value.Day == now.Day && p.CreatedOn.Value.Month == now.Month
                                 && p.CreatedOn.Value.Year == now.Year)) ||
                                 (summaryAlert.EmailAlertFrequency == EmailAlertFrequency.Weekly && p.CreatedOn > new DateTime(lastTuestday.Year, lastTuestday.Month, lastTuestday.Day, 0, 0, 0)) ||
                                 (summaryAlert.EmailAlertFrequency == EmailAlertFrequency.Monthly && p.CreatedOn > new DateTime(now.Year, now.Month, 1, 0, 0, 0))
                                 ? 2 : 3).ThenByDescending(m => m.ModifiedOn)
                            .Take(5)
                            .Select(d => new SummaryAlertIdItem
                            {
                                ItemType = EmailSummaryItemType.Forum,
                                Id = d.Id
                            });
                    }
                    IQueryable<SummaryAlertIdItem>? summaryItemIdQuery = null;
                    if (summaryProjectQuery != null)
                    {
                        summaryItemIdQuery = summaryProjectQuery;
                    }
                    if (summaryLearnQuery != null)
                    {
                        summaryItemIdQuery = summaryItemIdQuery == null ? summaryLearnQuery : summaryItemIdQuery.Union(summaryLearnQuery);
                    }
                    if (summaryEventQuery != null)
                    {
                        summaryItemIdQuery = summaryItemIdQuery == null ? summaryEventQuery : summaryItemIdQuery.Union(summaryEventQuery);
                    }
                    if (summaryForumQuery != null)
                    {
                        summaryItemIdQuery = summaryItemIdQuery == null ? summaryForumQuery : summaryItemIdQuery.Union(summaryForumQuery);
                    }
                    if (summaryInitiativeQuery != null)
                    {
                        summaryItemIdQuery = summaryItemIdQuery == null ? summaryInitiativeQuery : summaryItemIdQuery.Union(summaryInitiativeQuery);
                    }

                    if (summaryItemIdQuery != null)
                    {
                        List<SummaryAlertIdItem> summaryIdItems = await summaryItemIdQuery.ToListAsync();

                        if (summaryIdItems.Any())
                        {
                            totalEmails++;
                            List<SummaryEmailItem>? userSummaryItems = null;

                            if (summaryIdItems.Where(si => si.ItemType == EmailSummaryItemType.Initiative).Any())
                            {
                                //Instead of fetching all records (articles, community, tools and projects to get the new item counter, we are using this date value as the DIG initiative in prod has been created during this month).
                                //All items created after this date only needs to be considered.
                                DateTime initiativePIImplementationDate = new DateTime(2024, 8, 01);
                                List<int> initiativeIds = summaryIdItems.Where(si => si.ItemType == EmailSummaryItemType.Initiative)
                                    .Select(si => si.Id).ToList();

                                List<SummaryEmailItem> initiativeSummaryItems = await _context.Initiative
                                    .AsNoTracking()
                                    .Include(c => c.ProgressDetails)
                                    .Where(p => initiativeIds.Any(pId => p.Id == pId))
                                    .Select(p => new SummaryEmailItem
                                    {
                                        ItemTypeName = "Your Initiative",
                                        ItemLink = "decarbonization-initiatives",
                                        MainTitle = p.Title,
                                        ItemId = p.Id,
                                        InitiativeStepName = p.InitiativeStep.Description,
                                        InitiativeModifiedDate = p.ModifiedOn,
                                        InitiativeCategoryId = p.ProjectTypeId
                                    })
                                    .ToListAsync();

                                { 
                                    var categoryIds = initiativeSummaryItems.Select(x => x.InitiativeCategoryId).ToList();

                                    var articles = await _context.Articles.AsNoTracking().Include(y => y.ArticleCategories).Where(p => !p.IsDeleted && p.ArticleRoles.Any(r => summaryAlert.UserRoleIds.Contains(r.RoleId))
                                                   && p.ArticleCategories.Any(c => categoryIds.Contains(c.CategoryId)) && p.CreatedOn > initiativePIImplementationDate).Select(x => new { x.CreatedOn, x.ArticleCategories }).ToListAsync();

                                    var projects = await _context.Projects.AsNoTracking().Where(p => categoryIds.Contains(p.CategoryId) && p.StatusId == Core.Enums.ProjectStatus.Active && p.CreatedOn > initiativePIImplementationDate).Select(x => new { x.CreatedOn, x.CategoryId }).ToListAsync();

                                    var tools = await _context.Tools.AsNoTracking().Where(t => (t.IsActive && t.Companies.Select(c => c.CompanyId).Contains(group.First().CompanyId) || t.Roles.Any(role => (role.RoleId == Convert.ToInt32(RoleType.Corporation)) || (role.RoleId == Convert.ToInt32(RoleType.All)))) && t.CreatedOn > new DateTime(2024, 8, 01)).Select(x => new { x.CreatedOn }).ToListAsync();

                                    var communityUsers = await _context.Users.AsNoTracking().Include(x => x.UserProfile).ThenInclude(y => y.Categories).Where(x => x.CreatedOn > initiativePIImplementationDate && x.StatusId == Core.Enums.UserStatus.Active).Where(x => x.Roles.Any(y => y.Role.Id == (int)RoleType.Corporation
                                       || y.Role.Id == (int)RoleType.SolutionProvider || y.Role.Id == (int)RoleType.SPAdmin)).Where(x => x.UserProfile.Categories.Any(c => categoryIds.Contains(c.CategoryId))).
                                       Where(x => x.Id != group.First().UserId).Select(x => new { x.CreatedOn, x.UserProfile }).ToListAsync();

                                    List<InitiativeRecommendationActivity> recommendationLastViewedDateByInitiativeIds = await _context.InitiativeRecommendationActivity.Include(x => x.Initiative)
                                                                                             .Where(x => initiativeIds.Contains(x.InitiativeId))
                                                                                             .Select(g => g)
                                                                                             .ToListAsync();

                                    foreach (var item in initiativeSummaryItems)
                                    {
                                        _logger.LogError($"initiative for the user: ${group.First().UserEmailAddress} and initiative id : ${item.ItemId}");

                                        var recommendationLastViewedDateByInitiativeId = recommendationLastViewedDateByInitiativeIds?.FirstOrDefault(x => item.ItemId == x.InitiativeId);
                                       
                                        var articlesNewCount = articles?.Count(x => x.CreatedOn > recommendationLastViewedDateByInitiativeId.ArticleLastViewedDate && x.ArticleCategories?.Any(c => c.CategoryId == item.InitiativeCategoryId) == true);
                                        
                                        var projectsNewCount = projects?.Count(x => x.CreatedOn > recommendationLastViewedDateByInitiativeId.ProjectsLastViewedDate && x.CategoryId == item.InitiativeCategoryId);
                                         
                                        var toolsNewCount = tools?.Count(x => x.CreatedOn > recommendationLastViewedDateByInitiativeId.ToolsLastViewedDate);
                                        
                                        var communityUsersNewCount = communityUsers?.Count(x => x.CreatedOn > recommendationLastViewedDateByInitiativeId.CommunityLastViewedDate && x.UserProfile?.Categories?.Any(c => c.CategoryId == item.InitiativeCategoryId) == true);
                                         
                                        var messagesNewCount = await _context.InitiativeConversation.Include(x => x.Discussion).ThenInclude(x => x.DiscussionUsers)
                                                                     .Where(x => item.ItemId == x.InitiativeId && x.Discussion.DiscussionUsers.Any(y => y.UserId == userId && y.UnreadCount > 0 && y.DiscussionId == x.DiscussionId))
                                                                     .SelectMany(x => x.Discussion.DiscussionUsers.Where(y => y.UserId == userId && y.UnreadCount > 0)
                                                                     .Select(y => new { initiativeId = x.InitiativeId, unreadCount = y.UnreadCount }))
                                                                     .GroupBy(g => g.initiativeId)
                                                                     .Select(g => new
                                                                     {
                                                                         InitiativeId = g.Key,
                                                                         MessagesUnreadCount = g.Sum(x => x.unreadCount)

                                                                     }).ToListAsync(); 


                                        var newCount = articlesNewCount + projectsNewCount + toolsNewCount + communityUsersNewCount;
                                        var newMessagesCount = messagesNewCount;
                                        item.ItemTypeLogoUrl = _emailAssetsConfig.InitiativeLogo;
                                        item.Tags = new List<string>() { };
                                        var newMessageCount = newMessagesCount.FirstOrDefault(x => x.InitiativeId == item.ItemId)?.MessagesUnreadCount; 

                                        if (newCount > 0)
                                        {
                                            item.Tags.Add(newCount + " New Suggestion" + (newCount == 1 ? "" : "s"));
                                        }
                                        if (newMessageCount > 0)
                                        {
                                            item.Tags.Add(newMessageCount + " New Message" + (newCount == 1 ? "" : "s"));
                                        }
                                    }
                                    initiativeSummaryItems = initiativeSummaryItems.Where(x => x.Tags.Any()).ToList();

                                    userSummaryItems = userSummaryItems != null ? userSummaryItems.Union(initiativeSummaryItems).ToList() : initiativeSummaryItems;
                                }

                            }

                            if (summaryIdItems.Where(si => si.ItemType == EmailSummaryItemType.Project).Any())
                            {
                                List<int> projectIds = summaryProjectQuery.Where(si => si.ItemType == EmailSummaryItemType.Project)
                                    .Select(si => si.Id).ToList();

                                List<SummaryEmailItem> projectSummaryItems = await _context.Projects
                                    .AsNoTracking()
                                    .Include(c => c.Company)
                                    .Where(p => projectIds.Any(pId => p.Id == pId))
                                    .Select(p => new SummaryEmailItem
                                    {
                                        ItemTypeName = "Project",
                                        ItemLink = "projects",
                                        MainTitle = p.Title,
                                        MainText = p.SubTitle,
                                        Tags = new List<string> { p.Category.Name },
                                        Regions = p.Regions.Select(pr => pr.Region.Name).ToList(),
                                        ItemId = p.Id,
                                        Company = p.Company.Name,
                                        CompanyId = p.CompanyId
                                    })
                                    .ToListAsync();
                                projectSummaryItems = SortProjectsForSummaryEmail(projectSummaryItems, projectIds);

                                projectSummaryItems.ForEach(psi => psi.ItemTypeLogoUrl = _emailAssetsConfig.ProjectsLogo);

                                userSummaryItems = userSummaryItems != null ? userSummaryItems.Union(projectSummaryItems).ToList() : projectSummaryItems;
                            }

                            if (summaryIdItems.Where(si => si.ItemType == EmailSummaryItemType.Event).Any())
                            {
                                List<int> eventIds = summaryIdItems.Where(si => si.ItemType == EmailSummaryItemType.Event)
                                    .Select(si => si.Id).ToList();

                                List<SummaryAlertItem> eventSummaryItems = await _context.Events
                                    .AsNoTracking()
                                    .Where(e => eventIds.Any(eId => e.Id == eId))
                                    .Select(e => new SummaryAlertItem
                                    {
                                        ItemTypeName = "Event",
                                        MainTitle = e.Subject,
                                        Tags = e.EventCategories.Select(ec => ec.Category.Name),
                                        Regions = e.EventInvitedRegions.Select(eir => eir.Region.Name),
                                        EventAlertDateInfo = new EventAlertDateInfo { EventDateStart = e.EventOccurrences.First().FromDate, EventDateEnd = e.EventOccurrences.First().ToDate },
                                        EventLocationType = e.LocationType,
                                        ItemId = e.Id,
                                        MainText = e.Description,
                                        EventTimeZone = e.TimeZone,
                                        IsDisplayedInPublicSite = e.ShowInPublicSite ?? false
                                    })
                                    .ToListAsync();

                                List<SummaryEmailItem> summaryEventEmailItems = new List<SummaryEmailItem>();
                                foreach (SummaryAlertItem eventAlertItem in eventSummaryItems)
                                {
                                    var eventEmailItem = new SummaryEmailItem
                                    {
                                        ItemId = eventAlertItem.ItemId,
                                        MainText = eventAlertItem.MainText,
                                        EventDateInfo = eventAlertItem.EventLocationType == EventLocationType.Virtual ? ConvertEventDateToLocal(eventAlertItem!.EventAlertDateInfo, summaryAlert!.UserTimeZone) : ConvertEventDateToLocal(eventAlertItem.EventAlertDateInfo, eventAlertItem.EventTimeZone),
                                        Tags = eventAlertItem.Tags.ToList(),
                                        Regions = eventAlertItem.Regions.ToList(),
                                        ItemTypeName = "Event",
                                        ItemLink = "events",
                                        MainTitle = eventAlertItem.MainTitle,
                                        IsDisplayedInPublicSite = eventAlertItem.IsDisplayedInPublicSite
                                    };
                                    summaryEventEmailItems.Add(eventEmailItem);
                                }

                                summaryEventEmailItems.ForEach(esi => esi.EventDateLogoUrl = _emailAssetsConfig.EventsDate);
                                summaryEventEmailItems.ForEach(est => est.EventTimeLogoUrl = _emailAssetsConfig.EventsTime);
                                summaryEventEmailItems.ForEach(esi => esi.ItemTypeLogoUrl = _emailAssetsConfig.EventsLogo);

                                userSummaryItems = userSummaryItems != null ? userSummaryItems.Union(summaryEventEmailItems).ToList() : summaryEventEmailItems;
                            }

                            if (summaryIdItems.Where(si => si.ItemType == EmailSummaryItemType.Forum).Any())
                            {
                                List<int> forumIds = summaryIdItems.Where(si => si.ItemType == EmailSummaryItemType.Forum)
                                    .Select(si => si.Id).ToList();
                                List<SummaryEmailItem> forumSummaryItems = await _context.Discussions
                                    .AsNoTracking()
                                    .Where(d => forumIds.Any(eId => d.Id == eId))
                                    .Select(d => new SummaryEmailItem
                                    {
                                        ItemTypeName = "Forum",
                                        ItemLink = "forum/topic",
                                        MainTitle = d.Subject,
                                        MainText = d.Messages.OrderBy(m => m.CreatedOn).Select(m => m.Text).FirstOrDefault()!,
                                        Tags = d.DiscussionCategories.Select(dc => dc.Category.Name).ToList(),
                                        Regions = d.DiscussionRegions.Select(dr => dr.Region.Name).ToList(),
                                        ItemId = d.Id
                                    })
                                    .ToListAsync();

                                forumSummaryItems.ForEach(fsi => fsi.ItemTypeLogoUrl = _emailAssetsConfig.ForumLogo);

                                userSummaryItems = userSummaryItems != null ? userSummaryItems.Union(forumSummaryItems).ToList() : forumSummaryItems;
                            }

                            if (summaryIdItems.Where(si => si.ItemType == EmailSummaryItemType.Learn).Any())
                            {
                                List<int> learnIds = summaryIdItems.Where(si => si.ItemType == EmailSummaryItemType.Learn)
                                    .Select(si => si.Id).ToList();
                                List<SummaryEmailItem> learnSummaryItems = await _context.Articles
                                    .AsNoTracking()
                                    .Where(d => learnIds.Any(eId => d.Id == eId))
                                    .Select(a => new SummaryEmailItem
                                    {
                                        ItemTypeName = "Learn",
                                        ItemLink = "learn",
                                        MainTitle = a.Title,
                                        Tags = MergeList(a.ArticleCategories.Select(ac => ac.Category.Name), (a.ArticleContentTags.Where(c => !c.ContentTag.IsDeleted).Select(ac => ac.ContentTag.Name))),
                                        Regions = a.ArticleRegions.Select(ar => ar.Region.Name).ToList(),
                                        ItemId = a.Id,
                                        IsDisplayedInPublicSite = a.IsPublicArticle
                                    })
                                    .ToListAsync();

                                learnSummaryItems.ForEach(lsi => lsi.ItemTypeLogoUrl = _emailAssetsConfig.LearnLogo);

                                userSummaryItems = userSummaryItems != null ? userSummaryItems.Union(learnSummaryItems).ToList() : learnSummaryItems;
                            }

                            List<SummaryEmailItem> summaryEmailItems = userSummaryItems.Select(_mapper.Map<SummaryEmailItem>).ToList();
                            summaryEmailItems.ForEach(summaryEmailItem =>
                                summaryEmailItem.ItemLink = string.Format(_baseAppConfig.BaseAppUrlPattern, summaryEmailItem.ItemLink.ToLower(), summaryEmailItem.ItemId,
                                summaryEmailItem.IsDisplayedInPublicSite && (summaryEmailItem.ItemTypeName == "Learn" || summaryEmailItem.ItemTypeName == "Event") ? "/email" : ""));
                            SummaryEmailTemplatedModel template = new SummaryEmailTemplatedModel
                            {
                                FirstName = userAlerts.First().UserFirstName,
                                Items = userSummaryItems.Select(_mapper.Map<SummaryEmailItem>).ToList()
                            };
                            template.Link = string.Format(_baseAppConfig.BaseAppUrlPattern, "login", "");
                            SetLogoLinks(template);
                            await _emailService.SendTemplatedEmailAsync(userAlerts.First().UserEmailAddress, _emailAlertConfig.SummaryAlertSubject, template, context, false, UnsubscribeEmailType.SummaryEmail, userAlerts.First().UserId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed trying to send summary emails. {ex.InnerException?.Message ?? ex.Message}");
                }
            }
            _logger.LogInformation($"Completed sending summary emails {totalEmails} alerts found, {failedEmails} failed.");
        }

        private static List<string> MergeList(IEnumerable<string> enumerable1, IEnumerable<string> enumerable2)
        {
            return enumerable1.Concat(enumerable2).ToList();
        }

        private async Task SendNewMessageAlertsAsync(List<UserEmailAlertItem> newMessageAlerts, DateTime lastRun, DateTime now, ActionContext context)
        {
            int totalAlerts = 0;
            int failedEmails = 0;

            _logger.LogInformation($"New Message Email Address: {string.Join(",", newMessageAlerts.Select(x => x.UserEmailAddress).ToList())}");

            foreach (UserEmailAlertItem alert in newMessageAlerts)
            {
                try
                {
                    var userTimeZone = alert.UserTimeZone.WindowsName;
                    var userTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
                    var userCurrentTime = TimeZoneInfo.ConvertTimeFromUtc(now, GetTimeZoneInfoByWindowsName(userTimeZone));
                    var userLastRunCurrentTime = TimeZoneInfo.ConvertTimeFromUtc(lastRun, GetTimeZoneInfoByWindowsName(userTimeZone));


                    var IsDaily = alert.EmailAlertFrequency == EmailAlertFrequency.Daily ? SendSummaryDaily(userLastRunCurrentTime, userCurrentTime) : false;
                    var IsWeekly = alert.EmailAlertFrequency == EmailAlertFrequency.Weekly ? SendSummaryWeekly(userLastRunCurrentTime, userCurrentTime) : false;
                    var IsMonthly = alert.EmailAlertFrequency == EmailAlertFrequency.Monthly ? SendSummaryMonthly(userLastRunCurrentTime, userCurrentTime) : false;
                    _logger.LogInformation($"New Message Alert TimeZone for email address {alert.UserEmailAddress} - IsDaily : {IsDaily} IsWeekly : {IsWeekly} IsMonthly : {IsMonthly} Frequency : {alert.EmailAlertFrequency}");

                    if (!(IsDaily || IsWeekly || IsMonthly || alert.EmailAlertFrequency == EmailAlertFrequency.Immediately))
                        continue;
                    Tuple<DateTime, DateTime>? boundaries = GetSummaryTimeBoundaries(alert.EmailAlertFrequency, userCurrentTime, userLastRunCurrentTime, userTimeZoneInfo);

                    _logger.LogInformation($"New Message Alert TimeZone : {alert.UserEmailAddress} boundries :  {boundaries} ");
                    if (boundaries == null)
                        continue; // error case keep going

                    List<string> unreadMessageNotificationDetails = await _context.UserNotifications
                        .AsNoTracking()
                        .Where(un => un.UserId == alert.UserId)
                        .Where(un => boundaries.Item1 < un.ModifiedOn && boundaries.Item2 > un.ModifiedOn)
                        .Where(un => un.IsRead == false)
                        .Where(un => un.Type == NotificationType.MessagesMe || un.Type == NotificationType.ContactZeigoNetwork)
                        .Select(un => un.Details)
                        .ToListAsync();
                    NewMessageEmailTemplatedModel messagesTemplate = new NewMessageEmailTemplatedModel() { };

                    messagesTemplate.messages = new List<MessageEmailTemplateModel>() { };
                    messagesTemplate.FirstName = alert.UserFirstName;
                    messagesTemplate.LastName = alert.UserLastName;
                    messagesTemplate.Link = string.Format(_baseAppConfig.BaseAppUrlPattern, "messages", string.Empty);
                    SetLogoLinks(messagesTemplate);
                    messagesTemplate.attachmentLogoUrl = _emailAssetsConfig.AttachmentLink;
                    int notificationIndex = 0;

                    if (unreadMessageNotificationDetails.Any())
                    {
                        foreach (string notificationDetails in unreadMessageNotificationDetails)
                        {
                            try
                            {
                                totalAlerts++;
                                var notificationDetailJson = JsonSerializer.Deserialize<GroupMessagesNotificationDetails>(notificationDetails);
                                messagesTemplate.NumberOfMessages = notificationDetailJson.Count == 0 ? 1 : notificationDetailJson.Count;
                                messagesTemplate.NumberOfConversations = notificationIndex + 1;
                                // if single notification, count defaults to 0
                                if (notificationDetailJson.Count == 0)
                                {
                                    int messageId = notificationDetailJson.MessageId;
                                    int conversationId = notificationDetailJson.ConversationId;
                                    var messages = await _context.Messages
                                        .AsNoTracking()
                                        .Where(m => m.Id == notificationDetailJson.MessageId)
                                        .OrderByDescending(s => s.ModifiedOn)
                                        .Include(m => m.User).ThenInclude(u => u.Image)
                                        .Include(m => m.User).ThenInclude(u => u.Company).Include(m => m.Discussion).ThenInclude(du => du.DiscussionUsers).ToListAsync();

                                    Message? activeMessage = messages?.FirstOrDefault(message => message.StatusId == MessageStatus.Active);
                                    if (activeMessage == null)
                                    {
                                        activeMessage = messages?.LastOrDefault();
                                    }
                                    _logger.LogInformation($"New Message Email Address: {alert.UserEmailAddress}. Message Single Subject : {activeMessage?.Discussion?.Subject ?? "Nodata"}. Status : {activeMessage?.StatusId} MID: {messageId} - CID: {conversationId} Msg ModifiedOn : {activeMessage?.ModifiedOn}");

                                    if (activeMessage != null)
                                    {
                                        if (activeMessage.StatusId == MessageStatus.Active && (activeMessage.ModifiedOn > (alert.EmailAlertFrequency == EmailAlertFrequency.Immediately ? lastRun : boundaries.Item1)))
                                        {
                                            bool isAttachmentWithoutText = false;
                                            var attachment = _context.Attachments.FirstOrDefault(a => a.MessageId == activeMessage.Id);
                                            if (attachment != null && string.IsNullOrEmpty(activeMessage.Text))
                                            {
                                                activeMessage.Text = "Attachment ";
                                                isAttachmentWithoutText = true;
                                            }

                                            messagesTemplate.messages.Add(new MessageEmailTemplateModel
                                            {
                                                MessageId = activeMessage.Id,
                                                AuthorProfileText = new string(activeMessage.User.FirstName[0].ToString().ToUpper() + activeMessage.User.LastName[0].ToString().ToUpper()),
                                                MessageAuthor = activeMessage.User.FirstName + " " + activeMessage.User.LastName,
                                                MessageText = TruncateMessageText(activeMessage.Text, 120),
                                                MessageTime = TimeAgo((DateTime)activeMessage.ModifiedOn),
                                                ModifiedOn = activeMessage.ModifiedOn,
                                                AuthorProfileUrl = activeMessage.User.Image != null ? (await _azureStorageBlobService.GetBlobAsync(_mapper.Map<BlobBaseDTO>(activeMessage.User.Image), BlobSasPermissions.Read)).Uri.ToString() : "",
                                                AuthorCompany = activeMessage.User.Company.Name,
                                                NumberOfDiscussionUsers = activeMessage.Discussion.DiscussionUsers.Count,
                                                ConversationSubject = activeMessage.Discussion.Subject,
                                                replyLink = string.Format(_baseAppConfig.BaseAppUrlPattern, "messages", notificationDetailJson.ConversationId),
                                                Attachment = isAttachmentWithoutText
                                            });

                                            if (alert.EmailAlertFrequency == EmailAlertFrequency.Immediately && messagesTemplate.messages.Any())
                                            {
                                                await _emailService.SendTemplatedEmailAsync(alert.UserEmailAddress, string.Format(_emailAlertConfig.NewMessageAlertSingleSubject, messagesTemplate.messages[0].MessageAuthor), messagesTemplate, context);
                                                notificationIndex = 0;
                                                messagesTemplate.messages.Clear();
                                            }
                                            else
                                            {
                                                notificationIndex++;
                                            }
                                        }
                                        else
                                        {
                                            notificationIndex++;
                                        }

                                    }
                                    else
                                    {
                                        throw new Exception($"Single Message {notificationDetailJson.MessageId} not found in db.");
                                    }
                                }
                                // case when group notifications (Count != 0)
                                else
                                {
                                    var messages = await _context.Messages
                                        .AsNoTracking()
                                        .Where(m => m.DiscussionId == notificationDetailJson.ConversationId)
                                        .OrderByDescending(s => s.ModifiedOn).Include(m => m.User).ThenInclude(u => u.Image)
                                        .Include(m => m.User).ThenInclude(u => u.Company).Include(m => m.Discussion).ThenInclude(du => du.DiscussionUsers).ToListAsync();

                                    Message? activeMessage = messages?.FirstOrDefault(message => message.StatusId == MessageStatus.Active);
                                    if (activeMessage == null)
                                    {
                                        activeMessage = messages?.LastOrDefault();
                                    }

                                    _logger.LogInformation($"New Message Email Address: {alert.UserEmailAddress}. Message Multiple Subject : {activeMessage?.Discussion?.Subject ?? "Nodata"}. Status : {activeMessage?.StatusId} MID: {activeMessage?.Id} - CID: {notificationDetailJson.ConversationId} Datetime : {DateTime.Now}");

                                    if (activeMessage != null)
                                    {
                                        if (activeMessage.StatusId == MessageStatus.Active && (activeMessage.ModifiedOn > (alert.EmailAlertFrequency == EmailAlertFrequency.Immediately ? lastRun : boundaries.Item1)))

                                        {
                                            bool isAttachmentWithoutText = false;
                                            var attachment = _context.Attachments.FirstOrDefault(a => a.MessageId == activeMessage.Id);
                                            if (attachment != null && string.IsNullOrEmpty(activeMessage.Text))
                                            {
                                                activeMessage.Text = "Attachment ";
                                                isAttachmentWithoutText = true;
                                            }
                                            messagesTemplate.messages.Add(new MessageEmailTemplateModel
                                            {
                                                MessageId = activeMessage.Id,
                                                AuthorProfileText = new string(activeMessage.User.FirstName[0].ToString().ToUpper() + activeMessage.User.LastName[0].ToString().ToUpper()),
                                                MessageAuthor = activeMessage.User.FirstName + " " + activeMessage.User.LastName,
                                                MessageText = TruncateMessageText(activeMessage.Text, 120),
                                                MessageTime = TimeAgo((DateTime)activeMessage.ModifiedOn),
                                                ModifiedOn = activeMessage.ModifiedOn,
                                                AuthorProfileUrl = activeMessage.User.Image != null ? (await _azureStorageBlobService.GetBlobAsync(_mapper.Map<BlobBaseDTO>(activeMessage.User.Image), BlobSasPermissions.Read)).Uri.ToString() : "",
                                                AuthorCompany = activeMessage.User.Company.Name,
                                                NumberOfDiscussionUsers = activeMessage.Discussion.DiscussionUsers.Count,
                                                ConversationSubject = activeMessage.Discussion.Subject,
                                                replyLink = string.Format(_baseAppConfig.BaseAppUrlPattern, "messages", notificationDetailJson.ConversationId),
                                                Attachment = isAttachmentWithoutText
                                            });
                                            if (alert.EmailAlertFrequency == EmailAlertFrequency.Immediately && messagesTemplate.messages.Any())
                                            {
                                                string subject = messagesTemplate.messages.DistinctBy(x => x.MessageAuthor).Count() > 1 ? string.Format(_emailAlertConfig.NewMessageAlertMultipleSubject, messagesTemplate.messages[0].MessageAuthor, messagesTemplate.messages.DistinctBy(x => x.MessageAuthor).Count() - 1) : string.Format(_emailAlertConfig.NewMessageAlertSingleSubject, messagesTemplate.messages[0].MessageAuthor);
                                                await _emailService.SendTemplatedEmailAsync(alert.UserEmailAddress, subject, messagesTemplate, context);
                                                notificationIndex = 0;
                                                messagesTemplate.messages.Clear();
                                            }
                                            else
                                            {
                                                notificationIndex++;
                                            }
                                        }
                                        else
                                        {
                                            notificationIndex++;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception($"Group Message {notificationDetailJson.MessageId} not found in db.");
                                    }
                                }

                                _logger.LogInformation($"Attempting to send {alert.UserEmailAddress}. and notificationIndex {notificationIndex} and unread count {unreadMessageNotificationDetails.Count} " +
                                    $". message Ids: {JsonSerializer.Serialize(messagesTemplate.messages?.Select(x => x.MessageId).ToList())}");

                                if (alert.EmailAlertFrequency != EmailAlertFrequency.Immediately && unreadMessageNotificationDetails.Count == notificationIndex && messagesTemplate.messages.Any())
                                {
                                    messagesTemplate.messages = messagesTemplate.messages.OrderByDescending(x => x.ModifiedOn).Take(5).ToList();

                                    messagesTemplate.NumberOfConversations = messagesTemplate.messages.DistinctBy(x => x.MessageAuthor).Count();
                                    messagesTemplate.NumberOfMessages = messagesTemplate.messages.DistinctBy(x => x.MessageAuthor).Count();
                                    string subject = messagesTemplate.messages.DistinctBy(x => x.MessageAuthor).Count() > 1 ? string.Format(_emailAlertConfig.NewMessageAlertMultipleSubject, messagesTemplate.messages[0].MessageAuthor, messagesTemplate.messages.DistinctBy(x => x.MessageAuthor).Count() - 1) : string.Format(_emailAlertConfig.NewMessageAlertSingleSubject, messagesTemplate.messages[0].MessageAuthor);
                                    await _emailService.SendTemplatedEmailAsync(alert.UserEmailAddress, subject, messagesTemplate, context);
                                    messagesTemplate.messages.Clear();
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Error attempting to send {_emailAlertConfig.NewMessageAlertSingleSubject} to {alert.UserEmailAddress}. Error: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed trying to get new message alerts for {alert.UserEmailAddress}.");
                }
            }

            _logger.LogInformation($"Completed sending new message alerts {totalAlerts} alerts found, {failedEmails} failed.");
        }




        private async Task SendForumResponseAlertsAsync(List<UserEmailAlertItem> forumResponseAlerts, DateTime lastRun, DateTime now, ActionContext context)
        {
            int totalEmails = 0;
            int failedEmails = 0;
            _logger.LogInformation($"Email Address: {string.Join(",", forumResponseAlerts.Select(x => x.UserEmailAddress).ToList())}");

            foreach (UserEmailAlertItem alert in forumResponseAlerts)
            {
                try
                {
                    Tuple<DateTime, DateTime>? boundaries = GetTimeBoundaries(alert.EmailAlertFrequency, now, lastRun);
                    if (boundaries == null)
                        continue; // error case keep going

                    List<ForumResponseItem> forumResponses = await _context.Discussions
                        .AsNoTracking()
                        .Where(d => d.IsDeleted == false)
                        .Where(d => d.CreatedByUserId == alert.UserId)
                        .Where(d => d.Type != DiscussionType.PrivateChat)
                        .Where(d => d.Messages.Any(m => boundaries.Item1 < m.CreatedOn && m.CreatedOn < boundaries.Item2 && m.UserId != alert.UserId))
                        .Select(d => new ForumResponseItem
                        {
                            Messages = d.Messages.Where(m => boundaries.Item1 < m.CreatedOn && m.CreatedOn < boundaries.Item2 && m.UserId != alert.UserId)
                            .Select(m => new ForumResponseMessageItem
                            {
                                MessageAuthor = m.User.FirstName + " " + m.User.LastName,
                                MessageId = m.Id,
                                MessageText = m.Text
                            }),
                            ForumId = d.Id,
                            ForumTopic = d.Subject,
                        })
                        .ToListAsync();

                    if (forumResponses.Any())
                    {
                        foreach (ForumResponseItem response in forumResponses)
                        {
                            totalEmails++;
                            try
                            {
                                var template = _mapper.Map<ForumResponseEmailTemplatedModel>(response);
                                template.FirstName = alert.UserFirstName;
                                SetLogoLinks(template);
                                if (response.Messages.Count() == 1)
                                {
                                    template.Link = string.Format(_baseAppConfig.BaseAppUrlPattern, "forum/topic", response.ForumId);
                                    await _emailService.SendTemplatedEmailAsync(alert.UserEmailAddress, string.Format(_emailAlertConfig.ForumResponseAlertSingleSubject, template.ForumTopic), template, context);
                                }
                                else
                                {
                                    template.Link = string.Format(_baseAppConfig.BaseAppUrlPattern, "forum", string.Empty);
                                    await _emailService.SendTemplatedEmailAsync(alert.UserEmailAddress, string.Format(_emailAlertConfig.ForumResponseAlertMultipleSubject, template.ResponseCount.ToString(), template.ForumTopic), template, context);
                                }
                            }
                            catch (Exception ex)
                            {
                                failedEmails++;
                                _logger.LogError(ex, $"Failed attempting to send forum response alert to {alert.UserEmailAddress}.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed attempting to send forum response alerts to {alert.UserEmailAddress}.");
                }
            }
            _logger.LogInformation($"Finished sending forum response emails {totalEmails} found, {failedEmails} failed.");
        }

        private async Task SendOnboardingReminders(DateTime lastRun, DateTime now, ActionContext context)
        {

            IEnumerable<User> notCompletedProfileUsers = await _context.Users
                .AsNoTracking()
                .Where(u => u.StatusId == Core.Enums.UserStatus.Onboard)
                .Where(u => u.CreatedOn != null && u.CreatedOn.HasValue)
                .Where(u => u.UserProfile == null)
                .ToListAsync();

            notCompletedProfileUsers = notCompletedProfileUsers
                .Where(u => (lastRun <= u.CreatedOn.Value.AddWorkDays(3) && u.CreatedOn.Value.AddWorkDays(3) <= now) ||
                            (lastRun <= u.CreatedOn.Value.AddWorkDays(6) && u.CreatedOn.Value.AddWorkDays(6) <= now) ||
                            (lastRun <= u.CreatedOn.Value.AddWorkDays(9) && u.CreatedOn.Value.AddWorkDays(9) <= now))
                .ToList();

            int totalEmails = 0;
            int failedEmails = 0;

            _logger.LogInformation($"{notCompletedProfileUsers.Count()} profile completion alerts found");

            foreach (var user in notCompletedProfileUsers)
            {
                try
                {
                    totalEmails++;
                    var template = _mapper.Map<CompleteProfileEmailTemplatedModel>(user);
                    template.Link = String.Format(_baseAppConfig.BaseAppUrlPattern, "", "");
                    SetLogoLinks(template);
                    await _emailService.SendTemplatedEmailAsync(user.Email, _emailAlertConfig.CompleteProfileReminderSubject, template, context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error sending profile completion reminder to {user.Email} for user {user.Username}.");
                    failedEmails++;
                }
            }
        }

        private Tuple<DateTime, DateTime> GetTimeBoundaries(EmailAlertFrequency frequency, DateTime now, DateTime lastRun)
        {
            DateTime fromTime;
            DateTime toTime;

            switch (frequency)
            {
                case EmailAlertFrequency.Immediately:
                    fromTime = lastRun;
                    toTime = now;
                    break;

                case EmailAlertFrequency.Daily:
                    fromTime = new DateTime(lastRun.Year, lastRun.Month, lastRun.Day, 0, 0, 0);
                    toTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                    break;

                case EmailAlertFrequency.Weekly:
                    DateTime lastSunday = now.AddDays(-7);
                    fromTime = new DateTime(lastSunday.Year, lastSunday.Month, lastSunday.Day, 0, 0, 0);
                    toTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                    break;

                case EmailAlertFrequency.Monthly:
                    fromTime = new DateTime(lastRun.Year, lastRun.Month, 1, 0, 0, 0);
                    toTime = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
                    break;

                default:
                    return null;
            }
            return new Tuple<DateTime, DateTime>(fromTime, toTime);
        }

        private Tuple<DateTime, DateTime> GetSummaryTimeBoundaries(EmailAlertFrequency frequency, DateTime now, DateTime lastRun, TimeZoneInfo userTimeZoneInfo = null, bool logInfo = false)
        {
            DateTime fromTime;
            DateTime toTime;

            switch (frequency)
            {
                case EmailAlertFrequency.Immediately:
                    fromTime = lastRun;
                    toTime = now;
                    break;

                case EmailAlertFrequency.Daily:
                    DateTime previousDay = now.DayOfWeek == DayOfWeek.Monday ? now.AddDays(-3) : now.AddDays(-1);
                    fromTime = new DateTime(previousDay.Year, previousDay.Month, previousDay.Day, 15, 0, 0);
                    toTime = new DateTime(now.Year, now.Month, now.Day, 15, 0, 0);
                    break;

                case EmailAlertFrequency.Weekly:
                    DateTime lastTuesday = now.AddDays(-7);
                    fromTime = new DateTime(lastTuesday.Year, lastTuesday.Month, lastTuesday.Day, 0, 0, 0);
                    toTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                    break;

                case EmailAlertFrequency.Monthly:
                    DateTime previousMonthInfo = now.AddMonths(-1);
                    fromTime = new DateTime(previousMonthInfo.Year, previousMonthInfo.Month, 1, 0, 0, 0);
                    toTime = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
                    break;

                default:
                    return null;
            }

            _logger.LogInformation($"User TimeZone Info :  {userTimeZoneInfo}");
            _logger.LogInformation($"Boundaries From :  {fromTime}  To :  {toTime}");
            _logger.LogInformation($"Boundaries UTC From :  {TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(fromTime, DateTimeKind.Unspecified), userTimeZoneInfo)} " +
                $" To :  {TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(toTime, DateTimeKind.Unspecified), userTimeZoneInfo)}");

            return new Tuple<DateTime, DateTime>(TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(fromTime, DateTimeKind.Unspecified), userTimeZoneInfo), TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(toTime, DateTimeKind.Unspecified), userTimeZoneInfo));
        }

        private EventDateInfo ConvertEventDateToLocal(EventAlertDateInfo eventDate, TimeZone localTimeZone)
        {
            TimeZoneInfo localTimeZoneInfo = GetTimeZoneInfoByWindowsName(localTimeZone.WindowsName);

            DateTime localTimeStart = TimeZoneInfo.ConvertTimeFromUtc(eventDate.EventDateStart, localTimeZoneInfo);
            DateTime localTimeEnd = TimeZoneInfo.ConvertTimeFromUtc(eventDate.EventDateEnd, localTimeZoneInfo);

            string timeZoneAbbreviation = localTimeZoneInfo.IsDaylightSavingTime(localTimeStart)
                    ? localTimeZone.DaylightAbbreviation ?? localTimeZone.Abbreviation
                    : localTimeZone.Abbreviation;


            return new EventDateInfo
            {
                EventDate = localTimeStart,
                EventStart = localTimeStart.ToString("h:mmtt"),
                EventEnd = localTimeEnd.ToString("h:mmtt"),
                TimeZoneAbbreviation = timeZoneAbbreviation
            };
        }

        private void SetLogoLinks(NotificationTemplateEmailModel template)
        {
            template.LogoUrl = _emailAssetsConfig.ZeigoLogo;
            if (template is EventInvitationEmailTemplatedModel eventTemplate)
            {
                eventTemplate.EventDateLogoUrl = _emailAssetsConfig.EventsDate;
                eventTemplate.EventTimeLogoUrl = _emailAssetsConfig.EventsTime;
            }
        }

        private bool SendSummaryDaily(DateTime lastRun, DateTime now)
        {
            DateTime todayMidnight = new DateTime(now.Year, now.Month, now.Day, 15, 0, 0);
            if (now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
                return false;
            return (int)((now - todayMidnight).TotalMinutes) == 0;
        }

        private bool SendSummaryWeekly(DateTime lastRun, DateTime now)
        {
            // DayOfWeek is enum with int values [0,6]
            DateTime thisWeekTuesday = now.AddDays(-(int)now.DayOfWeek).AddDays(2);

            // Get boundaries for current week
            DateTime weeklySummaryTime = new DateTime(thisWeekTuesday.Year, thisWeekTuesday.Month, thisWeekTuesday.Day, 15, 0, 0);

            _logger.LogInformation(thisWeekTuesday.ToLongDateString() + " * " + weeklySummaryTime);

            // If lastRun is in current week don't send weekly emails
            return (int)((now - weeklySummaryTime).TotalMinutes) == 0;
        }

        private bool SendSummaryMonthly(DateTime lastRun, DateTime now)
        {
            DateTime firstOfCurrentMonthMidnight = new DateTime(now.Year, now.Month, 1, 15, 0, 0);
            if (firstOfCurrentMonthMidnight.DayOfWeek == DayOfWeek.Saturday)
            {
                firstOfCurrentMonthMidnight = new DateTime(now.Year, now.Month, 3, 15, 0, 0);
                lastRun = lastRun.AddDays(2);
            }
            else if (firstOfCurrentMonthMidnight.DayOfWeek == DayOfWeek.Sunday)
            {
                firstOfCurrentMonthMidnight = new DateTime(now.Year, now.Month, 2, 15, 0, 0);
                lastRun = lastRun.AddDays(1);
            }

            return (int)((now - firstOfCurrentMonthMidnight).TotalMinutes) == 0;
        }

        private bool SendWeekly(DateTime lastRun, DateTime now)
        {
            // DayOfWeek is enum with int values [0,6]
            DateTime thisWeekSunday = now.AddDays(-(int)now.DayOfWeek);

            // Get boundaries for current week
            DateTime thisWeekSundayMidnight = new DateTime(thisWeekSunday.Year, thisWeekSunday.Month, thisWeekSunday.Day, 0, 0, 0);

            // If lastRun is in current week don't send weekly emails
            return lastRun < thisWeekSundayMidnight;
        }

        private bool SendMonthly(DateTime lastRun, DateTime now)
        {
            DateTime firstOfCurrentMonthMidnight = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
            return lastRun < firstOfCurrentMonthMidnight;
        }

        private bool SendDaily(DateTime lastRun, DateTime now)
        {
            DateTime todayMidnight = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            return lastRun < todayMidnight;
        }

        private string TimeAgo(DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.UtcNow.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("Just now");
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = String.Format("{0}m", timeSpan.Minutes);
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = String.Format("{0}h", timeSpan.Hours);
            }
            else
            {
                result = String.Format("{0}", dateTime);
            }

            return result;
        }

        private string TruncateMessageText(string value, int maxChars = 70)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        private async Task SendAlertForUndeliveredMailsAsync(ActionContext context)
        {
            UndeliveredMailTemplatedModel undeliveredMails = new()
            {
                messages = await _sendgridService.GetDataFromSendgrid(_emailAlertConfig.UndeliveredMailSubject.Replace(": date", ""))
            };
            if ((undeliveredMails.messages?.Any()) == true)
            {
                var template = _mapper.Map<UndeliveredMailTemplatedModel>(undeliveredMails);

                SetLogoLinks(template);

                await _emailService.SendTemplatedEmailAsync(_config.ToAddressForUnDeliveredEmail, _emailAlertConfig.UndeliveredMailSubject.Replace("date", DateTime.UtcNow.AddDays(-1).ToLongDateString()), template, context, true);
            }
        }

        private List<SummaryEmailItem> SortProjectsForSummaryEmail(List<SummaryEmailItem> projectsList, List<int> projectOrder)
        {
            List<SummaryEmailItem> summaryItems = new();
            if (projectsList?.Any() == true)
            {
                projectsList = projectsList.OrderBy(b => projectOrder.FindIndex(a => a == b.ItemId)).ToList();
                List<int?> companyIds = projectsList.Select(p => p.CompanyId).Distinct().ToList();
                while (summaryItems.Count < 10 && projectsList.Count > 0)
                {
                    foreach (var companyId in companyIds)
                    {
                        if (summaryItems.Count < 10)
                        {
                            var item = projectsList.FirstOrDefault(p => p.CompanyId == companyId);
                            if (item != null)
                            {
                                summaryItems.Add(item);
                                projectsList.Remove(item);
                            }
                        }

                    }
                }
            }

            return summaryItems;
        }

    }
}