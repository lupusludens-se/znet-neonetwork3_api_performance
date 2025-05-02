using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Services
{
    public partial class AnnouncementService
    {
        private async Task<Announcement> EnsureAnnouncementExistsAsync(int id)
        {
            Announcement? announcement = await _context.Announcements.SingleOrDefaultAsync(a => a.Id == id);
            if (announcement == null)
                throw new CustomException($"{CoreErrorMessages.EntityNotFound} Announcement does not exist.");
            return announcement;
        }

        private async Task<bool> ApplyAnnouncementStatusAsync(Announcement announcement, bool forceActivate = false)
        {
            if (announcement.IsActive)
            {
                IQueryable<Announcement> activeAnnouncements = _context.Announcements
                    .Where(a => a.Id != announcement.Id && a.AudienceId == announcement.AudienceId && a.IsActive);

                if (activeAnnouncements.Any() && !forceActivate)
                {
                    _logger.LogWarning($"There are already active announcements for audience ID {announcement.AudienceId}. Force activate flag turned off.");
                    return false;
                }

                foreach (Announcement activeAnnouncement in activeAnnouncements)
                    activeAnnouncement.IsActive = false;

                await _context.SaveChangesAsync();
            }

            return true;
        }

        private IQueryable<Announcement> ExpandSortAnnouncements(IQueryable<Announcement> announcementsQueryable, string? expand, string? sort = null)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("backgroundimage"))
                {
                    announcementsQueryable = announcementsQueryable.Include(a => a.BackgroundImage);
                }
                if (expand.Contains("audience"))
                {
                    announcementsQueryable = announcementsQueryable.Include(a => a.Audience);
                }
            }

            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();
                if (sort.Contains("name"))
                {
                    if (sort.Contains("name.asc"))
                    {
                        announcementsQueryable = announcementsQueryable.OrderBy(a => a.Name);
                    }
                    if (sort.Contains("name.desc"))
                    {
                        announcementsQueryable = announcementsQueryable.OrderByDescending(a => a.Name);
                    }
                }
                if (sort.Contains("isactive"))
                {
                    if (sort.Contains("isactive.asc"))
                    {
                        announcementsQueryable = announcementsQueryable.OrderBy(a => a.IsActive).ThenBy(a => a.Name);
                    }
                    if (sort.Contains("isactive.desc"))
                    {
                        announcementsQueryable = announcementsQueryable.OrderByDescending(a => a.IsActive).ThenBy(a => a.Name);
                    }
                }
                if (sort.Contains("audience"))
                {
                    if (sort.Contains("audience.asc"))
                    {
                        announcementsQueryable = announcementsQueryable.OrderBy(a => a.Audience.Name).ThenBy(a => a.Name);
                    }
                    if (sort.Contains("audience.desc"))
                    {
                        announcementsQueryable = announcementsQueryable.OrderByDescending(a => a.Audience.Name).ThenBy(a => a.Name);
                    }
                }
            }

            return announcementsQueryable;
        }
    }
}
