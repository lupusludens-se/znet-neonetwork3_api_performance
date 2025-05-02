using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Announcement;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public partial class AnnouncementService : BaseService, IAnnouncementService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AnnouncementService> _logger;

        public AnnouncementService(
            ApplicationContext context,
            ILogger<AnnouncementService> logger,
            IMapper mapper,
            IDistributedCache cache) : base(cache)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> CreateUpdateAnnouncementAsync(AnnouncementDTO modelDTO, bool forceActivate = false)
        {
            _logger.LogInformation($"Attempt to add announcement {modelDTO.Name} with active status {modelDTO.IsActive} and {modelDTO.Audience} audience. Force activate flag set to {forceActivate}");

            Announcement model;
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (modelDTO.Id == 0)
                    {
                        model = _mapper.Map<Announcement>(modelDTO);
                        await _context.Announcements.AddAsync(model);
                    }
                    else
                    {
                        model = await EnsureAnnouncementExistsAsync(modelDTO.Id);
                        _mapper.Map(modelDTO, model);
                    }

                    bool isAnnouncementStatusApplicable = await ApplyAnnouncementStatusAsync(model, forceActivate);
                    if (!isAnnouncementStatusApplicable)
                        return default(int);

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            return model.Id;
        }

        public async Task<int> PatchAnnouncementAsync(int id, JsonPatchDocument patchDoc, bool forceActivate = false)
        {
            Announcement model = await EnsureAnnouncementExistsAsync(id);

            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    patchDoc.ApplyTo(model);

                    bool isAnnouncementStatusApplicable = await ApplyAnnouncementStatusAsync(model, forceActivate);
                    if (!isAnnouncementStatusApplicable)
                        return default(int);

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            return model.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="audienceId"></param>
        /// <param name="expand"></param>
        /// <returns></returns>
        public async Task<AnnouncementDTO?> GetLatestAnnouncementAsync(int audienceId, string? expand = null)
        {
            IQueryable<Announcement> announcementsQueryable =
                ExpandSortAnnouncements(_context.Announcements.Where(a => a.IsActive &&
                    (audienceId > 0
                        ? (a.AudienceId == (audienceId == (int)RoleType.SPAdmin ? (int)RoleType.SolutionProvider : audienceId) || a.AudienceId == (int)RoleType.All)
                        : (a.AudienceId == (int)RoleType.Corporation || a.AudienceId == (int)RoleType.All))).AsNoTracking(), expand);

            announcementsQueryable = audienceId > 0 ? announcementsQueryable.OrderBy(a => a.AudienceId == (int)RoleType.All) :
                     announcementsQueryable.OrderBy(a => a.AudienceId == (int)RoleType.Corporation ? 1 : 2).ThenByDescending(a => a.ModifiedOn);

            Announcement? announcement = await announcementsQueryable.FirstOrDefaultAsync();

            return _mapper.Map<AnnouncementDTO>(announcement);
        }

        public async Task<AnnouncementDTO?> GetAnnouncementAsync(int id, string? expand = null)
        {
            IQueryable<Announcement> announcementsQueryable = ExpandSortAnnouncements(_context.Announcements.AsNoTracking(), expand);

            Announcement? announcement = await announcementsQueryable.FirstOrDefaultAsync(a => a.Id == id);
            return _mapper.Map<AnnouncementDTO>(announcement);
        }

        public async Task<WrapperModel<AnnouncementDTO>> GetAnnouncementsAsync(ExpandOrderModel expandOrderModel)
        {
            IQueryable<Announcement> announcementsQueryable = ExpandSortAnnouncements(_context.Announcements.AsNoTracking(), expandOrderModel.Expand, expandOrderModel.OrderBy);

            int count = 0;
            if (expandOrderModel.IncludeCount)
            {
                count = await announcementsQueryable.CountAsync();
                if (count == 0)
                    return new WrapperModel<AnnouncementDTO> { Count = count, DataList = new List<AnnouncementDTO>() };
            }

            if (expandOrderModel.Skip.HasValue)
                announcementsQueryable = announcementsQueryable.Skip(expandOrderModel.Skip.Value);

            if (expandOrderModel.Take.HasValue)
                announcementsQueryable = announcementsQueryable.Take(expandOrderModel.Take.Value);

            IEnumerable<Announcement> announcements = await announcementsQueryable.ToListAsync();
            IEnumerable<AnnouncementDTO> announcementDTOs = announcements.Select(_mapper.Map<AnnouncementDTO>);
            return new WrapperModel<AnnouncementDTO> { Count = count, DataList = announcementDTOs };
        }

        public async Task RemoveAnnouncementAsync(int id)
        {
            try
            {
                Announcement model = await EnsureAnnouncementExistsAsync(id);
                _context.Announcements.Remove(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnRemoving, ex);
            }
        }
    }
}
