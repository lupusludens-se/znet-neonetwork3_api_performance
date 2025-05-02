using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Models.Feedback;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<FeedbackService> _logger;
        private readonly IMapper _mapper;

        public FeedbackService(ApplicationContext context, ILogger<FeedbackService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<WrapperModel<FeedbackDTO>> GetFeedbacksAsync(BaseSearchFilterModel filter)
        {
            IQueryable<Feedback> feedbacksQuery = ExpandSortFeedbacks(_context.Feedbacks.AsNoTracking(), filter.Expand, filter.OrderBy);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await feedbacksQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<FeedbackDTO>
                    {
                        DataList = new List<FeedbackDTO>()
                    };
                }
            }

            if (filter.Skip.HasValue)
            {
                feedbacksQuery = feedbacksQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                feedbacksQuery = feedbacksQuery.Take(filter.Take.Value);
            }

            List<Feedback> feedbacks = await feedbacksQuery
                .ToListAsync();
            List<FeedbackDTO> feedbackDTOs = feedbacks.Select(_mapper.Map<FeedbackDTO>).ToList();

            return new WrapperModel<FeedbackDTO>
            {
                Count = count,
                DataList = feedbackDTOs
            };

        }


        private static IQueryable<Feedback> ExpandSortFeedbacks(IQueryable<Feedback> feedbacksQueryable, string? expand, string? sort)
        {
            feedbacksQueryable = feedbacksQueryable.Include(m => m.User).ThenInclude(u => u.Company).Include(m => m.User).ThenInclude(u => u.Image).Include(m => m.User).ThenInclude(u => u.Roles).ThenInclude(u => u.Role);
            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();
                if (sort.Contains("company"))
                {
                    if (sort.Contains("company.asc"))
                    {
                        return feedbacksQueryable.OrderBy(o => o.User.Company.Name).ThenBy(p => p.User.LastName);
                    }
                    if (sort.Contains("company.desc"))
                    {
                        return feedbacksQueryable.OrderByDescending(o => o.User.Company.Name).ThenBy(p => p.User.LastName);
                    }
                }
                if (sort.Contains("rating"))
                {
                    if (sort.Contains("rating.asc"))
                    {
                        return feedbacksQueryable.OrderBy(o => o.Rating).ThenBy(p => p.User.LastName);
                    }
                    if (sort.Contains("rating.desc"))
                    {
                        return feedbacksQueryable.OrderByDescending(o => o.Rating).ThenBy(p => p.User.LastName);
                    }
                }
                if (sort.Contains("lastname"))
                {
                    if (sort.Contains("lastname.asc"))
                    {
                        return feedbacksQueryable.OrderBy(o => o.User.LastName);
                    }
                    if (sort.Contains("lastname.desc"))
                    {
                        return feedbacksQueryable.OrderByDescending(o => o.User.LastName);
                    }
                }
                if (sort.Contains("createdon"))
                {
                    if (sort.Contains("createdon.asc"))
                    {
                        return feedbacksQueryable.OrderBy(o => o.CreatedOn).ThenBy(p => p.User.LastName);
                    }
                    if (sort.Contains("createdon.desc"))
                    {
                        return feedbacksQueryable.OrderByDescending(o => o.CreatedOn).ThenBy(p => p.User.LastName);
                    }
                }
            }
            else
            {
                return feedbacksQueryable = feedbacksQueryable.OrderByDescending(x => x.CreatedOn);
            }

            return feedbacksQueryable;
        }

        public async Task<int> CreateFeedbackAsync(CreateFeedbackDTO modelDTO)
        {
            var model = new Feedback();
            int feedbackId = 0;
            try
            {
                _mapper.Map(modelDTO, model);
                await _context.Feedbacks.AddAsync(model);
                await _context.SaveChangesAsync();
                feedbackId = model.Id;
            }
            catch (Exception ex)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
            return feedbackId;

        }


        public async Task<FeedbackDTO> GetFeedbackAsync(int feedbackId)
        {
            IQueryable<Feedback> feedbacksQuery = ExpandSortFeedbacks(_context.Feedbacks.AsNoTracking(), null, null);
            Feedback? feedback = await feedbacksQuery.FirstOrDefaultAsync(x => x.Id == feedbackId);
            return _mapper.Map<FeedbackDTO>(feedback);

        }


    }
}
