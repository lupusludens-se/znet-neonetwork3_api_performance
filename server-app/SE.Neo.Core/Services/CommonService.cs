using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Configs;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;
using SE.Neo.Core.Services.Interfaces;
using TimeZoneConverter;
using TimeZone = SE.Neo.Core.Entities.TimeZone;

namespace SE.Neo.Core.Services
{
    public partial class CommonService : BaseService, ICommonService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<CommonService> _logger;
        private readonly IMapper _mapper;
        private readonly MemoryCacheTimeStamp _memoryCacheTimeStamp;

        public CommonService(ApplicationContext context, ILogger<CommonService> logger, IMapper mapper,
                IOptions<MemoryCacheTimeStamp> memoryCacheTimeStamp, IDistributedCache cache) : base(cache)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _memoryCacheTimeStamp = memoryCacheTimeStamp.Value;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string? expand = null, string? filterBy = null, List<int>? userRoleIds = null, int? userCompanyId = null)
        {
            IQueryable<Category> categoriesQueryable = ExpandSortCategories(
                    _context.Categories.Where(w => !w.IsDeleted && !w.Slug.Equals(CategoriesSlugs.MarketBrief)).AsNoTracking(), expand, userRoleIds, userCompanyId);
            categoriesQueryable = FilterCategories(categoriesQueryable, filterBy);

            IEnumerable<Category> categories = await categoriesQueryable.ToListAsync();
            return categories.Select(_mapper.Map<CategoryDTO>);
        }

        public async Task<Category> GetCategory(int categoryId)
        {
            return await _context.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TechnologyDTO>> GetTechnologiesAsync(string? expand = null, List<int>? userRoleIds = null, int? userCompanyId = null)
        {
            IQueryable<Technology> technologiesQueryable = ExpandSortTechnologies(
                    _context.Technologies.Where(w => !w.IsDeleted).AsNoTracking(), expand, userRoleIds, userCompanyId);
            IEnumerable<Technology> technologies = await technologiesQueryable.ToListAsync();
            return technologies.Select(_mapper.Map<TechnologyDTO>);
        }

        public async Task<IEnumerable<SolutionDTO>> GetSolutionsAsync(string? expand = null, string? filterBy = null, List<int>? userRoleIds = null, int? userCompanyId = null)
        {
            IQueryable<Solution> solutionsQueryable = ExpandSortSolutions(
                    _context.Solutions.Where(w => !w.IsDeleted).AsNoTracking(), expand, userRoleIds, userCompanyId);
            solutionsQueryable = FilterSolutions(solutionsQueryable, filterBy);

            IEnumerable<Solution> solutions = await solutionsQueryable.ToListAsync();
            foreach (var solution in solutions)
            {
                foreach (var category in solution.Categories)
                {
                    if (category.CategoryResources?.Count > 1)
                        category.CategoryResources = category.CategoryResources.OrderByDescending(cr => cr.Resource.ContentTitle.Length).ToList();
                }
            }
            return solutions.Select(_mapper.Map<SolutionDTO>);
        }

        public async Task<IEnumerable<RegionDTO>> GetRegionsAsync(BaseSearchFilterModel filter)
        {
            IQueryable<Region> regionsQueryable = ExpandSortRegions(_context.Regions.Where(w => !w.IsDeleted).AsNoTracking(), filter.Expand, filter.OrderBy);

            regionsQueryable = FilterSearchRegions(regionsQueryable, filter.Search, filter.FilterBy);

            IEnumerable<Entities.CMS.Region> regions = await regionsQueryable.ToListAsync();

            return regions.Select(_mapper.Map<RegionDTO>);
        }

        public async Task<IEnumerable<StateDTO>> GetStatesAsync(BaseSearchFilterModel filter)
        {
            List<State> stateContext = await GetCachedStatesAsync();
            List<Country> countryContext = await GetCachedCountriesAsync();

            var countriesStates = countryContext.GroupJoin(
                stateContext, country => country.Id,
                state => state.CountryId,
                (country, states) => new { Country = country, States = states.ToList() }).ToList();

            countriesStates.ForEach(group => group.States.ForEach(s => s.Country = group.Country));
            stateContext = countriesStates.SelectMany(group => group.States).ToList();

            IQueryable<State> statesQueryable = ExpandSortStates(stateContext.AsQueryable(), filter.Expand, filter.OrderBy);
            statesQueryable = FilterSearchStates(statesQueryable, filter.Search, filter.FilterBy);
            return statesQueryable.Select(_mapper.Map<StateDTO>);
        }

        public async Task<IEnumerable<CountryDTO>> GetCountriesAsync(BaseSearchFilterModel filter)
        {
            List<State> stateContext = await GetCachedStatesAsync();
            List<Country> countryContext = await GetCachedCountriesAsync();

            var countriesStates = countryContext.GroupJoin(
                stateContext, country => country.Id,
                state => state.CountryId,
                (country, states) => new { Country = country, States = states.ToList() }).ToList();

            countriesStates.ForEach(group => group.Country.States = group.States);
            countryContext = countriesStates.Select(group => group.Country).ToList();

            IQueryable<Country> countriesQueryable = ExpandSortCountries(countryContext.AsQueryable(), filter.Expand, filter.OrderBy);
            countriesQueryable = FilterSearchCountries(countriesQueryable, filter.Search, filter.FilterBy);
            return countriesQueryable.Select(_mapper.Map<CountryDTO>);
        }

        public async Task<IEnumerable<RoleDTO>> GetRolesAsync(string? expand)
        {
            List<Role> rolesQueryable = await ExpandRoles(_context.Roles.AsNoTracking().OrderBy(o => o.Name), expand).ToListAsync();
            return rolesQueryable.Select(_mapper.Map<RoleDTO>);
        }

        public async Task<IEnumerable<PermissionDTO>> GetPermissionsAsync(string? expand)
        {
            List<Permission> permissions = await ExpandPermissions(_context.Permissions.AsNoTracking().OrderBy(o => o.Name), expand).OrderBy(o => o.Name).ToListAsync();
            return permissions.Select(_mapper.Map<PermissionDTO>);
        }

        public async Task<IEnumerable<BaseIdNameDTO>> GetHeardViaAsync()
        {
            List<HeardVia> heardVia = await _context.HeardVia.AsNoTracking().ToListAsync();
            return heardVia.Select(_mapper.Map<BaseIdNameDTO>);
        }

        public async Task<IEnumerable<TimeZoneDTO>> GetTimeZonesAsync()
        {
            List<TimeZone> timeZones = await GetCachedTimeZonesAsync();
            return timeZones.Select(_mapper.Map<TimeZoneDTO>);
        }

        public async Task<TimeZone> GetTimeZoneByClientIdOrDefault(string timeZoneClientId = "", string defaultWindowsName = "Eastern Standard Time")
        {
            List<TimeZone> timeZones = await GetCachedTimeZonesAsync();

            if (!string.IsNullOrEmpty(timeZoneClientId))
            {
                string? windowsTimeZoneName = null;
                if (TZConvert.TryIanaToWindows(timeZoneClientId, out windowsTimeZoneName))
                {
                    return timeZones.FirstOrDefault(tz => tz.WindowsName == windowsTimeZoneName!)
                        ?? timeZones.Single(tz => tz.WindowsName == defaultWindowsName);
                }
                else
                {
                    return timeZones.FirstOrDefault(tz => tz.WindowsName == timeZoneClientId!)
                        ?? timeZones.Single(tz => tz.WindowsName == defaultWindowsName);
                }
            }
            else
            {
                return timeZones.First(t => t.WindowsName == defaultWindowsName);
            }
        }

        public async Task<TimeZoneDTO> GetTimeZone(int timeZoneId)
        {
            List<TimeZone> timeZones = await GetCachedTimeZonesAsync();
            TimeZone timeZone = timeZones.FirstOrDefault(tz => tz.Id == timeZoneId);
            return _mapper.Map<TimeZoneDTO>(timeZone);
        }

        public async Task<List<int>> GetRegionListForUserProfile(int userId, bool parentInclude = false, bool childInclude = false, bool usaAllInclude = false)
        {
            List<Region>? regions = await GetRegionListFromCacheAsync();

            var userProfileRegions =
                await _context.UserProfileRegions.Include(r => r.Region).Where(upc => upc.UserProfile.UserId == userId).ToListAsync();

            List<int> userRegionList = userProfileRegions.Select(t => t.RegionId).ToList();

            if (parentInclude)
                userRegionList.AddRange(userProfileRegions.SelectMany(t => new[] { t.RegionId, t.Region.ParentId }).Where(x => x != null).Cast<int>());

            if (childInclude)
            {
                List<int> userRegionParentList = userProfileRegions.Where(t => !t.Region.ParentId.HasValue || t.Region.ParentId.Equals(0)).Select(t => t.RegionId).ToList();
                if (userRegionParentList.Any())
                    userRegionList.AddRange(regions.Where(r => userRegionParentList.Contains(r.ParentId.Value)).Select(t => t.Id));
            }

            if (usaAllInclude)
            {
                List<int> userRegionUsaList = userProfileRegions.Where(t => t.Region.Slug.ToLower().Equals(RegionsSlugs.UsAll)).Select(t => t.RegionId).ToList();
                if (userRegionUsaList.Any())
                    userRegionList.AddRange(regions.Where(r => r.Slug.ToLower().Contains(RegionsSlugs.UsAllFiltration)).Select(t => t.Id));
            }
            return userRegionList.Distinct().ToList();
        }

        public List<int> ExpandRegionListForFiltration(List<int> regionIds, bool childInclude = false, bool usaStatesInclude = false, bool usaAllInclude = true)
        {
            var regions = Task.Run(async () => await GetRegionListFromCacheAsync()).Result;

            List<int> expandedRegionList = regionIds;

            if (childInclude)
                expandedRegionList.AddRange(regions.Where(r => regionIds.Contains(r.ParentId.Value)).Select(t => t.Id));

            if (usaStatesInclude)
            {
                List<Region> userRegionUsaList = regions.Where(r => regionIds.Contains(r.Id) && r.Slug.ToLower().Equals(RegionsSlugs.UsAll)).ToList();
                if (userRegionUsaList.Any())
                    expandedRegionList.AddRange(regions.Where(r => r.Slug.ToLower().Contains(RegionsSlugs.UsAllFiltration)).Select(t => t.Id));
            }

            if (usaAllInclude)
            {
                List<Region> userRegionUSList = regions.Where(r => regionIds.Contains(r.Id) && r.Slug.ToLower().Contains(RegionsSlugs.UsAllFiltration)).ToList();
                if (userRegionUSList.Any())
                    expandedRegionList.AddRange(regions.Where(r => r.Slug.ToLower().Equals(RegionsSlugs.UsAll)).Select(t => t.Id));
            }

            return expandedRegionList.Distinct().ToList();
        }

        public async Task<IEnumerable<InitiativeScale>> GetRegionScaleTypesAsync()
        {
            return await GetCachedRegionScaleTypesAsync();
        }

        public async Task<List<Region>> GetAllRegions()
        {
            return await GetRegionListFromCacheAsync();
        }

        #region Validation

        public async Task<bool> IsRoleIdExistAsync(int id)
        {
            return await IsIdExistAsync(id, _context.Roles, CoreCacheKeys.RoleContext, _memoryCacheTimeStamp.Medium);
        }

        public async Task<bool> IsCountryIdExistAsync(int id)
        {
            return await IsIdExistAsync(id, _context.Countries, CoreCacheKeys.CountryContext, _memoryCacheTimeStamp.Long);
        }

        public async Task<bool> IsStateIdExistAsync(int id)
        {
            return await IsIdExistAsync(id, _context.States, CoreCacheKeys.StateContext, _memoryCacheTimeStamp.Long);
        }

        public async Task<bool> IsCategoryIdExistAsync(int id)
        {
            return await IsIdExistAsync(id, _context.Categories, CoreCacheKeys.CategoryContext, _memoryCacheTimeStamp.Short);
        }

        public async Task<bool> IsSolutionIdExistAsync(int id)
        {
            return await IsIdExistAsync(id, _context.Solutions, CoreCacheKeys.SolutionContext, _memoryCacheTimeStamp.Short);
        }

        public async Task<bool> IsTechnologyIdExistAsync(int id)
        {
            return await IsIdExistAsync(id, _context.Technologies, CoreCacheKeys.TechnologyContext, _memoryCacheTimeStamp.Short);
        }

        public async Task<bool> IsRegionIdExistAsync(int id)
        {
            return await IsIdExistAsync(id, _context.Regions, CoreCacheKeys.RegionContext, _memoryCacheTimeStamp.Short);
        }

        public async Task<bool> IsTimeZoneIdExistAsync(int id)
        {
            return await IsIdExistAsync(id, _context.TimeZones, CoreCacheKeys.TimeZoneContext, _memoryCacheTimeStamp.Long);
        }

        #endregion Validation
    }
}