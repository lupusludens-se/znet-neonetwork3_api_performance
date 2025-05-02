using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Community;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Data;
using SE.Neo.Core.Models.Community;
using SE.Neo.Core.Services.Interfaces;
using ArticleType = SE.Neo.Core.Enums.ArticleType;
using ProjectStatus = SE.Neo.Core.Enums.ProjectStatus;
using UserStatus = SE.Neo.Core.Enums.UserStatus;

namespace SE.Neo.Core.Services
{
    public partial class CommunityService : BaseFilterService, ICommunityService
    {
        private const string FilterForYou = "foryou";
        private const string FilterOnlyFollowed = "onlyfollowed";

        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ICommonService _commonService;
        private readonly ILogger<CommunityService> _logger;

        public CommunityService(ApplicationContext context, ILogger<CommunityService> logger, IMapper mapper, ICommonService commonService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _commonService = commonService;
        }

        public async Task<WrapperModel<CommunityItemDTO>> GetCommunityAsync(int userId, CommunityFilter filter)
        {
            CommunityItemType? communityItemType = null;
            if (!string.IsNullOrEmpty(filter.FilterBy))
            {
                filter.FilterBy = filter.FilterBy!.ToLower();
                foreach (string property in filter.FilterBy.Split("&").ToList())
                {
                    var ids = ParseFilterByField(property);
                    if (ids != null && ids.Count > 0)
                    {
                        if (property.Contains("communityitemtype"))
                        {
                            communityItemType = (CommunityItemType)ids.FirstOrDefault();
                        }
                    }
                }
            }

            var createQueryModel = new CommunityCreateQueryModel
            {
                UserId = userId,
                Search = filter.Search,
                Filter = filter.FilterBy,
                Type = communityItemType,
                OnlyFollowed = filter.FilterBy?.Contains(FilterOnlyFollowed) ?? false
            };

            IQueryable<CommunitySearchResult> query = filter.FilterBy != FilterForYou
                    ? CreateCommunityQuery(createQueryModel)
                    : CreateForYouCommunityQuery(createQueryModel);

            if (!string.IsNullOrEmpty(filter.OrderBy) && (string.IsNullOrEmpty(filter.Search)))
            {
                if (filter.OrderBy.ToLower() == "asc")
                    query = query.OrderBy(x => x.Sort);
                else if (filter.OrderBy.ToLower() == "desc")
                    query = query.OrderByDescending(x => x.Sort);
            }
            else if (!string.IsNullOrEmpty(filter.Search))
            {

                query = query.OrderBy(x => x.Order).ThenBy(x => x.StartsOrContains);
            }
            else
            {
                query = query.OrderBy(x => x.Sort);
            }
            int count = 0;
            if (filter.IncludeCount)
            {
                count = string.IsNullOrEmpty(filter.Search) ? await query.CountAsync() : query.Count();
                if (count == 0)
                {
                    return new WrapperModel<CommunityItemDTO> { Count = count, DataList = new List<CommunityItemDTO>() };
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

            List<CommunitySearchResult> communityItems = new List<CommunitySearchResult>();
            communityItems = string.IsNullOrEmpty(filter.Search) ? await query.ToListAsync() : query.ToList();
            if (!communityItems.Any())
                return new WrapperModel<CommunityItemDTO> { Count = count, DataList = new List<CommunityItemDTO>() };

            var communityDTOs = new List<CommunityItemDTO>();

            if (communityItems.Any(ci => ci.Type == CommunityItemType.User))
            {
                List<int> followedUserIds = communityItems
                    .Where(c => c.Type == CommunityItemType.User && c.IsFollowed == true)
                    .Select(c => c.Id).ToList();
                List<int> notFollowedUserIds = communityItems
                    .Where(c => c.Type == CommunityItemType.User && c.IsFollowed == false)
                    .Select(c => c.Id).ToList();
                List<CommunityItem> userItems = await _context.Users
                                .AsNoTracking().Include(u => u.Roles).ThenInclude(u => u.Role)
                                .Where(u => followedUserIds.Union(notFollowedUserIds).Contains(u.Id) && u.IsPrivateUser != true)
                                .Select(u => new CommunityItem
                                {
                                    Id = u.Id,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    Image = u.Image,
                                    JobTitle = u.UserProfile.JobTitle,
                                    Categories = u.UserProfile.Categories.Select(upc => upc.Category),
                                    Type = CommunityItemType.User,
                                    CompanyName = u.Company.Name,
                                    Roles = u.Roles
                                })
                                .ToListAsync();
                foreach (var userItem in userItems)
                {
                    userItem.IsFollowed = followedUserIds.Contains(userItem.Id);
                }
                communityDTOs.AddRange(userItems.Select(_mapper.Map<CommunityItemDTO>));
            }

            if (communityItems.Any(ci => ci.Type == CommunityItemType.Company))
            {
                List<int> followedCompanyIds = communityItems
                    .Where(c => c.Type == CommunityItemType.Company && c.IsFollowed == true)
                    .Select(c => c.Id).ToList();
                List<int> notFollowedCompanyIds = communityItems
                    .Where(c => c.Type == CommunityItemType.Company && c.IsFollowed == false)
                    .Select(c => c.Id).ToList();
                List<CommunityItem> companyItems = await _context.Companies
                                .AsNoTracking()
                                .Where(c => followedCompanyIds.Union(notFollowedCompanyIds).Contains(c.Id))
                                .Select(c => new CommunityItem
                                {
                                    Id = c.Id,
                                    CompanyName = c.Name,
                                    Image = c.Image,
                                    MemberCount = c.Users.Where(u => u.StatusId == UserStatus.Active && u.IsPrivateUser != true).Count(),
                                    Type = CommunityItemType.Company
                                })
                                .ToListAsync();
                foreach (var companyItem in companyItems)
                {
                    companyItem.IsFollowed = followedCompanyIds.Contains(companyItem.Id);
                }
                communityDTOs.AddRange(companyItems.Select(_mapper.Map<CommunityItemDTO>));
            }
            communityDTOs = communityItems.Join(
                communityDTOs,  
                item => item.Id,
                dto => dto.Id,
                (item, dto) => dto).ToList();

            if (createQueryModel.OnlyFollowed)
            {
                if (filter.OrderBy.ToLower() == "companies")
                {
                    communityDTOs = communityDTOs.OrderBy(x => (int) x.Type).ToList();
                }
                else if (filter.OrderBy.ToLower() == "people")
                {
                    communityDTOs = communityDTOs.OrderByDescending(x => (int) x.Type).ToList();
                }

            }
            return new WrapperModel<CommunityItemDTO> { Count = count, DataList = communityDTOs };
        }

        public async Task<NetworkStatsDTO> GetNetworkStats()
        {
            NetworkStatsDTO networkStats = new NetworkStatsDTO();
            try
            {
                networkStats.CorporateCompanyCount = await _context.Companies.CountAsync(c => c.TypeId == CompanyType.Corporation && c.StatusId == Enums.CompanyStatus.Active);
                networkStats.CorporateCompanyCount = networkStats.CorporateCompanyCount < 10 ? networkStats.CorporateCompanyCount : ((networkStats.CorporateCompanyCount / 10) * 10);

                networkStats.SolutionProviderCompanyCount = await _context.Companies.CountAsync(c => c.TypeId == CompanyType.SolutionProvider && c.StatusId == Enums.CompanyStatus.Active);
                networkStats.SolutionProviderCompanyCount = networkStats.SolutionProviderCompanyCount < 10 ? networkStats.SolutionProviderCompanyCount : ((networkStats.SolutionProviderCompanyCount / 10) * 10);

                networkStats.ProjectCount = await _context.Projects.CountAsync(p => p.Status.Id == ProjectStatus.Active);
                networkStats.ProjectCount = networkStats.ProjectCount < 10 ? networkStats.ProjectCount : ((networkStats.ProjectCount / 10) * 10);

                networkStats.ArticleMarketBriefCount = await _context.Articles.CountAsync(a => a.Type.Id == ArticleType.MarketBrief && !a.IsDeleted);
                networkStats.ArticleMarketBriefCount = networkStats.ArticleMarketBriefCount < 10 ? networkStats.ArticleMarketBriefCount : ((networkStats.ArticleMarketBriefCount / 10) * 10);

                networkStats.TotalArticleCount = await _context.Articles.CountAsync(a => !a.IsDeleted);
                networkStats.TotalArticleCount = networkStats.TotalArticleCount < 50 ? networkStats.TotalArticleCount : ((networkStats.TotalArticleCount / 50) * 50);

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error while getting NetworkStats : exception message {ex.Message} : inner exception : {ex.InnerException?.Message}");
            }
            return networkStats;
        }
    }
}