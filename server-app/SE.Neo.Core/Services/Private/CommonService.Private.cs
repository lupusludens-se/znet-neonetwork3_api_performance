using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SE.Neo.Common.Extensions;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;
using TimeZone = SE.Neo.Core.Entities.TimeZone;

namespace SE.Neo.Core.Services
{
    public partial class CommonService
    {
        private IQueryable<Region> ExpandSortRegions(IQueryable<Region> queryable, string? expand, string? sort = null)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();

                if (sort.Contains("name"))
                {
                    if (sort.Contains("name.asc"))
                    {
                        return queryable.OrderBy(o => o.Name);
                    }
                    if (sort.Contains("name.desc"))
                    {
                        return queryable.OrderByDescending(o => o.Name);
                    }
                }
            }
            else
            {
                queryable = queryable.OrderBy(o => o.Slug == RegionsSlugs.UsAll ? 0 : 1).ThenBy(o => o.Name);
            }

            return queryable;
        }

        private IQueryable<Region> FilterSearchRegions(IQueryable<Region> regionsQueryable, string? search, string? filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                foreach (string property in filter.Split("&").ToList())
                {
                    var ids = ParseFilterByField(property);
                    if (ids != null && ids.Count > 0)
                    {
                        if (property.Contains("parentids"))
                        {
                            regionsQueryable =
                                regionsQueryable.Where(x => ids.Contains(x.ParentId ?? 0));
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                regionsQueryable = regionsQueryable.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            }
            return regionsQueryable;
        }

        private IQueryable<State> ExpandSortStates(IQueryable<State> statesQueryable, string? expand, string? sort = null)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();

                if (sort.Contains("name"))
                {
                    if (sort.Contains("name.asc"))
                    {
                        statesQueryable = statesQueryable.OrderBy(o => o.Name);
                    }
                    if (sort.Contains("name.desc"))
                    {
                        statesQueryable = statesQueryable.OrderByDescending(o => o.Name);
                    }
                }
            }
            else
            {
                statesQueryable = statesQueryable.OrderBy(o => o.Name);
            }
            return statesQueryable;
        }

        private IQueryable<State> FilterSearchStates(IQueryable<State> statesQueryable, string? search, string? filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                foreach (string property in filter.Split("&").ToList())
                {
                    var ids = ParseFilterByField(property);
                    if (ids != null && ids.Count > 0)
                    {
                        if (property.Contains("countryids"))
                        {
                            statesQueryable =
                                statesQueryable.Where(x => ids.Contains(x.CountryId));
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                statesQueryable = statesQueryable.Where(p => p.Name.ToLower().Contains(search.ToLower())
                || p.Abbr.ToLower().Contains(search.ToLower()));
            }
            return statesQueryable;
        }

        private IQueryable<Country> ExpandSortCountries(IQueryable<Country> countriesQueryable, string? expand, string? sort = null)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();

                if (sort.Contains("name"))
                {
                    if (sort.Contains("name.asc"))
                    {
                        countriesQueryable = countriesQueryable.OrderBy(o => o.Name);
                    }
                    if (sort.Contains("name.desc"))
                    {
                        countriesQueryable = countriesQueryable.OrderByDescending(o => o.Name);
                    }
                }
            }
            else
            {
                countriesQueryable = countriesQueryable.OrderBy(o => o.Name);
            }
            return countriesQueryable;
        }

        private IQueryable<Country> FilterSearchCountries(IQueryable<Country> countriesQueryable, string? search, string? filter)
        {
            if (!string.IsNullOrEmpty(search))
            {
                countriesQueryable = countriesQueryable.Where(p => p.Name.ToLower().Contains(search.ToLower())
                || p.Code.ToLower().Contains(search.ToLower()) || p.Code3.ToLower().Contains(search.ToLower()));
            }
            return countriesQueryable;
        }

        private IQueryable<Role> ExpandRoles(IQueryable<Role> rolesQueryable, string? expand)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("permissions"))
                {
                    rolesQueryable = rolesQueryable.Include(r => r.Permissions).ThenInclude(p => p.Permission);
                }
            }
            return rolesQueryable;
        }

        private IQueryable<Permission> ExpandPermissions(IQueryable<Permission> permissionsQueryable, string? expand)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("roles"))
                {
                    permissionsQueryable = permissionsQueryable.Include(r => r.Roles).ThenInclude(p => p.Role);
                }
            }
            return permissionsQueryable;
        }

        private async Task<List<Entities.TimeZone>> GetCachedTimeZonesAsync()
        {
            var timeZones = await _cache.GetRecordAsync<List<Entities.TimeZone>>(CoreCacheKeys.TimeZoneContext);
            if (timeZones is null)
            {
                timeZones = await CacheTimeZonesAsync();
            }
            else if (string.IsNullOrEmpty(timeZones.First().WindowsName))
            {
                await _cache.RemoveAsync(CoreCacheKeys.TimeZoneContext);
                timeZones = await CacheTimeZonesAsync();
            }
            return timeZones;
        }

        private IQueryable<Category> ExpandSortCategories(IQueryable<Category> categoriesQueryable, string? expand,
                   List<int>? userRoleIds, int? userCompanyId)
        {
            categoriesQueryable = categoriesQueryable.Include(r => r.Technologies.Where(w => !w.Technology.IsDeleted)).ThenInclude(p => p.Technology);
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("resources"))
                {
                    categoriesQueryable = categoriesQueryable
                        .Include(c => c.CategoryResources.Where(t =>
                        userRoleIds.Contains((int)Common.Enums.RoleType.Admin) || (t.Resource.Article == null && t.Resource.Tool == null)
                        || (t.Resource.Article != null && t.Resource.Article.ArticleRoles.Any(a => userRoleIds.Contains(a.RoleId)))
                        || (t.Resource.Tool != null && (t.Resource.Tool.Roles.Any(a => userRoleIds.Contains(a.RoleId))
                            || t.Resource.Tool.Companies.Any(c => c.CompanyId.Equals(userCompanyId))))
                        )).ThenInclude(cr => cr.Resource);
                }
            }
            return categoriesQueryable.OrderBy(o => o.Name);
        }

        private IQueryable<Solution> ExpandSortSolutions(IQueryable<Solution> solutionsQueryable, string? expand,
                    List<int>? userRoleIds, int? userCompanyId)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (userRoleIds?.Count > 0 && userCompanyId > 0)
                {
                    if (expand.Contains("resources"))
                    {
                        solutionsQueryable = solutionsQueryable
                            .Include(r => r.Categories.Where(w => !w.IsDeleted)).ThenInclude(c => c.CategoryResources.Where(t =>
                            userRoleIds.Contains((int)Common.Enums.RoleType.Admin) || (t.Resource.Article == null && t.Resource.Tool == null)
                            || (t.Resource.Article != null && t.Resource.Article.ArticleRoles.Any(a => userRoleIds.Contains(a.RoleId)))
                            || (t.Resource.Tool != null && (t.Resource.Tool.Roles.Any(a => userRoleIds.Contains(a.RoleId))
                                || t.Resource.Tool.Companies.Any(c => c.CompanyId.Equals(userCompanyId))))
                            )).ThenInclude(cr => cr.Resource);
                    }
                    else
                    {
                        solutionsQueryable = solutionsQueryable.Include(r => r.Categories.Where(w => !w.IsDeleted));
                    }
                }
                else
                {
                    if (expand.Contains("resources"))
                    {
                        solutionsQueryable = solutionsQueryable.Include(r => r.Categories.Where(w => !w.IsDeleted)).ThenInclude(cr => cr.CategoryResources).ThenInclude(r => r.Resource);
                    }
                }
            }
            else
            {
                solutionsQueryable = solutionsQueryable.Include(r => r.Categories.Where(w => !w.IsDeleted));
            }
            return solutionsQueryable.OrderBy(o => o.Name);
        }

        private IQueryable<Technology> ExpandSortTechnologies(IQueryable<Technology> technologiesQueryable, string? expand,
                    List<int>? userRoleIds, int? userCompanyId)
        {
            technologiesQueryable = technologiesQueryable.Include(r => r.Categories.Where(w => !w.Category.IsDeleted)).ThenInclude(p => p.Category);
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("resources"))
                {
                    technologiesQueryable = technologiesQueryable
                        .Include(c => c.TechnologyResources.Where(t =>
                        userRoleIds.Contains((int)Common.Enums.RoleType.Admin) || (t.Resource.Article == null && t.Resource.Tool == null)
                        || (t.Resource.Article != null && t.Resource.Article.ArticleRoles.Any(a => userRoleIds.Contains(a.RoleId)))
                        || (t.Resource.Tool != null && (t.Resource.Tool.Roles.Any(a => userRoleIds.Contains(a.RoleId))
                            || t.Resource.Tool.Companies.Any(c => c.CompanyId.Equals(userCompanyId))))
                        )).ThenInclude(cr => cr.Resource);
                }
            }
            return technologiesQueryable.OrderBy(o => o.Name);
        }

        private IQueryable<Category> FilterCategories(IQueryable<Category> categoriesQueryable, string? filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                foreach (string property in filter.Split("&").ToList())
                {
                    var ids = ParseFilterByField(property);
                    if (ids != null && ids.Count > 0)
                    {
                        if (property.Contains("solutionids"))
                        {
                            categoriesQueryable =
                                categoriesQueryable.Where(x => ids.Contains(x.SolutionId ?? 0));
                        }
                    }
                }
            }
            return categoriesQueryable;
        }

        private IQueryable<Solution> FilterSolutions(IQueryable<Solution> solutionsQueryable, string? filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                foreach (string property in filter.Split("&").ToList())
                {
                    var ids = ParseFilterByField(property);
                    if (ids != null && ids.Count > 0)
                    {
                        if (property.Contains("ids"))
                        {
                            solutionsQueryable =
                                solutionsQueryable.Where(x => ids.Contains(x.Id));
                        }
                    }
                }
            }
            return solutionsQueryable;
        }

        private async Task<List<TimeZone>> CacheTimeZonesAsync()
        {
            List<TimeZone> timeZones = await _context.TimeZones.ToListAsync();
            await _cache.SetRecordAsync(CoreCacheKeys.TimeZoneContext, timeZones, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCacheTimeStamp.Long)
            });
            return timeZones;
        }

        private async Task<List<Region>> GetRegionListFromCacheAsync()
        {
            List<Region>? regions = await _cache.GetRecordAsync<List<Region>>(CoreCacheKeys.RegionContext);
            if (regions is null)
            {
                regions = await _context.Regions.ToListAsync();
                await _cache.SetRecordAsync(CoreCacheKeys.RegionContext, regions, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCacheTimeStamp.Short)
                });
            }
            return regions;
        }

        private async Task<List<Country>> GetCachedCountriesAsync()
        {
            List<Country>? countries = await _cache.GetRecordAsync<List<Country>>(CoreCacheKeys.CountryContext);
            if (countries is null)
            {
                countries = await _context.Countries.ToListAsync();
                await _cache.SetRecordAsync(CoreCacheKeys.CountryContext, countries, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCacheTimeStamp.Long)
                });
            }
            return countries;
        }

        private async Task<List<State>> GetCachedStatesAsync()
        {
            List<State>? states = await _cache.GetRecordAsync<List<State>>(CoreCacheKeys.StateContext);
            if (states is null)
            {
                states = await _context.States.AsNoTracking().ToListAsync();
                await _cache.SetRecordAsync(CoreCacheKeys.StateContext, states, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCacheTimeStamp.Long)
                });
            }
            return states;
        }

        private async Task<List<InitiativeScale>> GetCachedRegionScaleTypesAsync()
        {
            var regionScaleTypes = await _cache.GetRecordAsync<List<InitiativeScale>>(CoreCacheKeys.InitiativeScaleContext);
            if (regionScaleTypes is null)
            {
                regionScaleTypes = await CacheRegionScaleTypesAsync();
            }
            return regionScaleTypes;
        }

        private async Task<List<InitiativeScale>> CacheRegionScaleTypesAsync()
        {
            List<InitiativeScale> regionScaleTypes = await _context.InitiativeScale.ToListAsync();
            await _cache.SetRecordAsync(CoreCacheKeys.InitiativeScaleContext, regionScaleTypes, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCacheTimeStamp.Long)
            });
            return regionScaleTypes;
        }
    }
}