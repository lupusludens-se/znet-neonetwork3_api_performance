using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SE.Neo.Common;
using SE.Neo.Common.Extensions;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;

namespace SE.Neo.Core.Services
{
    public partial class InitiativeService
    {
        private async Task<List<InitiativeScale>> GetCachedInitiativeScaleAsync()
        {
            var initiativeScales = await _cache.GetRecordAsync<List<InitiativeScale>>(CoreCacheKeys.InitiativeScaleContext);
            if (initiativeScales is null)
            {
                initiativeScales = await CacheInitiativeScalesAsync();
            }
            return initiativeScales;
        }

        private async Task<List<InitiativeScale>> CacheInitiativeScalesAsync()
        {
            List<InitiativeScale> initiativeScales = await _context.InitiativeScale.ToListAsync();
            await _cache.SetRecordAsync(CoreCacheKeys.InitiativeScaleContext, initiativeScales, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCacheTimeStamp.Long)
            });
            return initiativeScales;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="articlesQueryable"></param>
        /// <param name="initiative"></param>
        /// <param name="newlyCreatedArticleIds"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        private IQueryable<Article> SortArticles(IQueryable<Article> articlesQueryable, Initiative initiative, List<int> newlyCreatedArticleIds, int? contentId = 0)
        {
            IEnumerable<int> regionIdsFromFilter = initiative.Regions.Select(x => x.RegionId);
            switch (initiative.ScaleId)
            {
                case (int)Common.Enums.InitiativeScale.State:
                    {
                        var usAllAndCanadaRegions = _context.Regions.Where(x => x.Name == ZnConstants.UsAllRegionName || x.Name == ZnConstants.UsAndCanadaRegionName);
                        var usAllRegionId = usAllAndCanadaRegions.FirstOrDefault(x => x.Name == ZnConstants.UsAllRegionName)?.Id;
                        var usaAndCanadaRegionId = usAllAndCanadaRegions.FirstOrDefault(x => x.Name == ZnConstants.UsAndCanadaRegionName)?.Id;
                        var parentIdList = new List<int?>() { usaAndCanadaRegionId };
                        articlesQueryable = articlesQueryable
                            .OrderByDescending(f => f.Id == contentId)
                            .ThenByDescending(f => newlyCreatedArticleIds.Any(na => na == f.Id))
                           .ThenByDescending(f => f.ArticleRegions.Any())
                           .ThenByDescending(f => f.ArticleRegions.All(dr => regionIdsFromFilter.Contains(dr.RegionId)) && f.ArticleRegions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId)) == regionIdsFromFilter.Count() ? 1 : 0) // replace 1 with f.ArticleRegions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId))
                           .ThenByDescending(f => f.ArticleRegions.Any(dr => regionIdsFromFilter.Contains(dr.RegionId)) ? 1 : 0)
                           .ThenByDescending(f => f.ArticleRegions.Any(dr => dr.RegionId == usAllRegionId) ? 1 : 0)
                           .ThenByDescending(f => f.ArticleRegions.Any(dr => parentIdList.Contains(dr.RegionId)) ? 1 : 0);
                        break;
                    }

                case (int)Common.Enums.InitiativeScale.National:
                    {
                        var parentIdOfSelectedRegionList = _context.Regions.Where(x => regionIdsFromFilter.Contains(x.Id)).Select(x => x.ParentId).ToList();
                        var usAllAndStatesIdsList = new List<int>() { };
                        articlesQueryable = articlesQueryable
                            .OrderByDescending(f => f.Id == contentId)
                            .ThenByDescending(f => newlyCreatedArticleIds.Any(na => na == f.Id))
                            .ThenByDescending(f => f.ArticleRegions.Any())
                            .ThenByDescending(f => f.ArticleRegions.All(dr => regionIdsFromFilter.Contains(dr.RegionId)) && f.ArticleRegions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId)) == regionIdsFromFilter.Count() ? 1 : 0) // replace 1 with f.ArticleRegions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId))
                            .ThenByDescending(f => f.ArticleRegions.Any(dr => regionIdsFromFilter.Contains(dr.RegionId)) ? 1 : 0)
                            .ThenByDescending(f => usAllAndStatesIdsList.Count > 0 && f.ArticleRegions.Any(dr => usAllAndStatesIdsList.Contains(dr.RegionId)) ? 1 : 0)
                            .ThenByDescending(f => parentIdOfSelectedRegionList.Count > 0 && f.ArticleRegions.All(dr => parentIdOfSelectedRegionList.Contains(dr.RegionId)) && f.ArticleRegions.Count(dr => parentIdOfSelectedRegionList.Contains(dr.RegionId)) == parentIdOfSelectedRegionList.Count ? 1 : 0)
                            .ThenByDescending(f => parentIdOfSelectedRegionList.Count > 0 && f.ArticleRegions.Any(dr => parentIdOfSelectedRegionList.Contains(dr.RegionId)) ? 1 : 0);
                        break;
                    }

                case (int)Common.Enums.InitiativeScale.Regional:
                    {
                        var childRegionIdList = _context.Regions.Where(x => regionIdsFromFilter.Contains(x.ParentId ?? 0)).Select(x => x.Id).ToList();

                        articlesQueryable = articlesQueryable
                           .OrderByDescending(f => f.Id == contentId)
                           .ThenByDescending(f => newlyCreatedArticleIds.Any(na => na == f.Id))
                           .ThenByDescending(f => f.ArticleRegions.Any())
                           .ThenByDescending(f => f.ArticleRegions.All(dr => regionIdsFromFilter.Contains(dr.RegionId)) && f.ArticleRegions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId)) == regionIdsFromFilter.Count() ? 1 : 0) // replace 1 with f.ArticleRegions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId))
                           .ThenByDescending(f => f.ArticleRegions.Any(dr => regionIdsFromFilter.Contains(dr.RegionId)) ? 1 : 0)
                           .ThenByDescending(f => f.ArticleRegions.Any(dr => childRegionIdList.Contains(dr.RegionId)) ? 1 : 0);
                        break;
                    }
            }

            return articlesQueryable;
        }

        private IQueryable<InitiativeFile> ExpandSortFiles(IQueryable<InitiativeFile> initiativeFilesQuery, string? expand, string? sort)
        {
            initiativeFilesQuery = initiativeFilesQuery.Include(i => i.File);

            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();
                if (sort.Contains("name"))
                {
                    if (sort.Contains("name.asc"))
                    {
                        return initiativeFilesQuery.OrderBy(o => o.File.ActualFileTitle);
                    }
                    else if (sort.Contains("name.desc"))
                    {
                        return initiativeFilesQuery.OrderByDescending(o => o.File.ActualFileTitle);
                    }
                }
                else if (sort.Contains("size"))
                {
                    if (sort.Contains("size.asc"))
                    {
                        return initiativeFilesQuery.OrderBy(o => o.File.Size);
                    }
                    else if (sort.Contains("size.desc"))
                    {
                        return initiativeFilesQuery.OrderByDescending(o => o.File.Size);
                    }
                }
                else if (sort.Contains("type"))
                {
                    if (sort.Contains("type.asc"))
                    {
                        return initiativeFilesQuery.OrderBy(o => o.File.Type);
                    }
                    else if (sort.Contains("type.desc"))
                    {
                        return initiativeFilesQuery.OrderByDescending(o => o.File.Type);
                    }
                }
                else if (sort.Contains("date"))
                {
                    if (sort.Contains("date.asc"))
                    {
                        return initiativeFilesQuery.OrderBy(o => o.File.ModifiedOn);
                    }
                    else if (sort.Contains("date.desc"))
                    {
                        return initiativeFilesQuery.OrderByDescending(o => o.File.ModifiedOn);
                    }
                }
            }
            else
            {
                return initiativeFilesQuery = initiativeFilesQuery.OrderByDescending(x => x.File.ModifiedOn);
            }

            return initiativeFilesQuery;
        }


        private IQueryable<Initiative> SortInitiatives(IQueryable<Initiative> initiativeQuery, string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();
                if (sort.Contains("company"))
                {
                    if (sort.Contains("company.asc"))
                    {
                        return initiativeQuery.OrderBy(o => o.User.Company.Name).ThenBy(p => p.User.LastName);
                    }
                    if (sort.Contains("company.desc"))
                    {
                        return initiativeQuery.OrderByDescending(o => o.User.Company.Name).ThenBy(p => p.User.LastName);
                    }
                }
                if (sort.Contains("phase"))
                {
                    if (sort.Contains("phase.asc"))
                    {
                        return initiativeQuery.OrderBy(o => o.InitiativeStep.Name).ThenBy(p => p.User.LastName);
                    }
                    if (sort.Contains("phase.desc"))
                    {
                        return initiativeQuery.OrderByDescending(o => o.InitiativeStep.Name).ThenBy(p => p.User.LastName);
                    }
                }
                if (sort.Contains("category"))
                {
                    if (sort.Contains("category.asc"))
                    {
                        return initiativeQuery.OrderBy(o => o.ProjectType.Name);
                    }
                    if (sort.Contains("category.desc"))
                    {
                        return initiativeQuery.OrderByDescending(o => o.ProjectType.Name);
                    }
                }
                if (sort.Contains("changedon"))
                {
                    if (sort.Contains("changedon.asc"))
                    {
                        return initiativeQuery.OrderBy(o => o.ModifiedOn).ThenBy(p => p.User.LastName);
                    }
                    if (sort.Contains("changedon.desc"))
                    {
                        return initiativeQuery.OrderByDescending(o => o.ModifiedOn).ThenBy(p => p.User.LastName);
                    }
                }
                if (sort.Contains("statusname"))
                {
                    if (sort.Contains("statusname.asc"))
                    {
                        return initiativeQuery.OrderBy(o => o.Status.Name).ThenBy(p => p.User.LastName);
                    }
                    if (sort.Contains("statusname.desc"))
                    {
                        return initiativeQuery.OrderByDescending(o => o.Status.Name).ThenBy(p => p.User.LastName);
                    }
                }
            }
           
            return initiativeQuery;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeQuery"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static IQueryable<Initiative> SearchAndPriorizeInitiatives(IQueryable<Initiative> initiativeQuery, string search)
        {
            var matchingInitiatives = initiativeQuery.Select(p => new
            {
                Initiative = p,
                Priority = p.Title.ToLower().Contains(search) ? 1 :
                            (p.User.FirstName.ToLower().StartsWith(search) || p.User.LastName.ToLower().StartsWith(search) ||
                            (p.User.FirstName.ToLower() + " " + p.User.LastName.ToLower()).StartsWith(search)) ? 2 :
                            (p.User.FirstName.ToLower().Contains(search) || p.User.LastName.ToLower().Contains(search) ||
                            (p.User.FirstName.ToLower() + " " + p.User.LastName.ToLower()).Contains(search)) ? 3 :
                             p.Company.Name.ToLower().Contains(search) ? 4 :
                             p.InitiativeStep.Name.ToLower().StartsWith(search) ? 5 :
                             p.ProjectType.Name.ToLower().Contains(search) ? 6 :
                             p.Status.Name.ToLower().Contains(search) ? 7 :
                             p.Regions.Any(r => r.Region.Name.ToLower().Contains(search)) ? 8 : 9


            })
              .Where(p => p.Priority < 9)
              .OrderBy(p => p.Priority)
              .Select(p => p.Initiative);

            return matchingInitiatives.AsQueryable();
        }
        private IQueryable<Project> SortProjects(IQueryable<Project> projectsQueryable, Initiative initiative, List<int> newlyCreatedProjectIds, int? projectid = 0)
        {
            IEnumerable<int> regionIdsFromFilter = initiative.Regions.Select(x => x.RegionId);
            switch (initiative.ScaleId)
            {
                case (int)Common.Enums.InitiativeScale.State:
                    {
                        var usAllAndCanadaRegions = _context.Regions.Where(x => x.Name == ZnConstants.UsAllRegionName || x.Name == ZnConstants.UsAndCanadaRegionName);
                        var usAllRegionId = usAllAndCanadaRegions.FirstOrDefault(x => x.Name == ZnConstants.UsAllRegionName)?.Id;
                        var usaAndCanadaRegionId = usAllAndCanadaRegions.FirstOrDefault(x => x.Name == ZnConstants.UsAndCanadaRegionName)?.Id;
                        var parentIdList = new List<int?>() { usaAndCanadaRegionId };
                        projectsQueryable = projectsQueryable
                           .OrderByDescending(f => f.Id == projectid)
                           .ThenByDescending(f => newlyCreatedProjectIds.Any(na => na == f.Id))
                           .ThenByDescending(f => f.Regions.Any())
                           .ThenByDescending(f => f.Regions.All(dr => regionIdsFromFilter.Contains(dr.RegionId)) && f.Regions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId)) == regionIdsFromFilter.Count() ? 1 : 0)
                           .ThenByDescending(f => f.Regions.Any(dr => regionIdsFromFilter.Contains(dr.RegionId)) ? 1 : 0)
                           .ThenByDescending(f => f.Regions.Any(dr => dr.RegionId == usAllRegionId) ? 1 : 0)
                           .ThenByDescending(f => f.Regions.Any(dr => parentIdList.Contains(dr.RegionId)) ? 1 : 0);
                        break;
                    }

                case (int)Common.Enums.InitiativeScale.National:
                    {
                        var parentIdOfSelectedRegionList = _context.Regions.Where(x => regionIdsFromFilter.Contains(x.Id)).Select(x => x.ParentId).ToList();
                        var usAllAndStatesIdsList = new List<int>() { };
                        projectsQueryable = projectsQueryable
                            .OrderByDescending(f => f.Id == projectid)
                            .ThenByDescending(f => newlyCreatedProjectIds.Any(na => na == f.Id))
                            .ThenByDescending(f => f.Regions.Any())
                            .ThenByDescending(f => f.Regions.All(dr => regionIdsFromFilter.Contains(dr.RegionId)) && f.Regions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId)) == regionIdsFromFilter.Count() ? 1 : 0) // replace 1 with f.ArticleRegions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId))
                            .ThenByDescending(f => f.Regions.Any(dr => regionIdsFromFilter.Contains(dr.RegionId)) ? 1 : 0)
                            .ThenByDescending(f => usAllAndStatesIdsList.Count > 0 && f.Regions.Any(dr => usAllAndStatesIdsList.Contains(dr.RegionId)) ? 1 : 0)
                            .ThenByDescending(f => parentIdOfSelectedRegionList.Count > 0 && f.Regions.All(dr => parentIdOfSelectedRegionList.Contains(dr.RegionId)) && f.Regions.Count(dr => parentIdOfSelectedRegionList.Contains(dr.RegionId)) == parentIdOfSelectedRegionList.Count ? 1 : 0)
                            .ThenByDescending(f => parentIdOfSelectedRegionList.Count > 0 && f.Regions.Any(dr => parentIdOfSelectedRegionList.Contains(dr.RegionId)) ? 1 : 0);
                        break;
                    }

                case (int)Common.Enums.InitiativeScale.Regional:
                    {
                        var childRegionIdList = _context.Regions.Where(x => regionIdsFromFilter.Contains(x.ParentId ?? 0)).Select(x => x.Id).ToList();

                        projectsQueryable = projectsQueryable
                           .OrderByDescending(f => f.Id == projectid)
                           .ThenByDescending(f => newlyCreatedProjectIds.Any(na => na == f.Id))
                           .ThenByDescending(f => f.Regions.Any())
                           .ThenByDescending(f => f.Regions.All(dr => regionIdsFromFilter.Contains(dr.RegionId)) && f.Regions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId)) == regionIdsFromFilter.Count() ? 1 : 0) // replace 1 with f.ArticleRegions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId))
                           .ThenByDescending(f => f.Regions.Any(dr => regionIdsFromFilter.Contains(dr.RegionId)) ? 1 : 0)
                           .ThenByDescending(f => f.Regions.Any(dr => childRegionIdList.Contains(dr.RegionId)) ? 1 : 0);
                        break;
                    }
            }

            return projectsQueryable;
        }




        private IQueryable<User> SortCommunityUsers(IQueryable<User> communityUsersQueryable, Initiative initiative, List<int> newlyCreatedInitiativeUserIds, int? userId = 0)
        {
            IEnumerable<int> regionIdsFromFilter = initiative.Regions.Select(x => x.RegionId);
            switch (initiative.ScaleId)
            {
                case (int)Common.Enums.InitiativeScale.State:
                    {
                        var usAllRegionId = _context.Regions.FirstOrDefault(x => x.Name == ZnConstants.UsAllRegionName)?.Id;
                        var usaAndCanadaRegionId = _context.Regions.FirstOrDefault(x => x.Name == ZnConstants.UsAndCanadaRegionName)?.Id;
                        var parentIdList = new List<int?>() { usaAndCanadaRegionId };
                        communityUsersQueryable = communityUsersQueryable
                           .OrderByDescending(f => f.Id == userId)
                           .ThenByDescending(f => newlyCreatedInitiativeUserIds.Any(na => na == f.Id))
                           .ThenByDescending(f => f.UserProfile.Regions.Any())
                           .ThenByDescending(f => f.UserProfile.Regions.All(dr => regionIdsFromFilter.Contains(dr.RegionId)) && f.UserProfile.Regions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId)) == regionIdsFromFilter.Count() ? 1 : 0).ThenByDescending(f => f.UserProfile.UserLoginCount) // replace 1 with f.UserProfile.Regions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId))
                           .ThenByDescending(f => f.UserProfile.Regions.Any(dr => regionIdsFromFilter.Contains(dr.RegionId)) ? 1 : 0).ThenByDescending(f => f.UserProfile.UserLoginCount)
                           .ThenByDescending(f => f.UserProfile.Regions.Any(dr => dr.RegionId == usAllRegionId) ? 1 : 0).ThenByDescending(f => f.UserProfile.UserLoginCount)
                           .ThenByDescending(f => f.UserProfile.Regions.Any(dr => parentIdList.Contains(dr.RegionId)) ? 1 : 0).ThenByDescending(f => f.UserProfile.UserLoginCount);
                        break;
                    }

                case (int)Common.Enums.InitiativeScale.National:
                    {
                        var parentIdOfSelectedRegionList = _context.Regions.Where(x => regionIdsFromFilter.Contains(x.Id)).Select(x => x.ParentId).ToList();
                        var usAllAndStatesIdsList = new List<int>() { };
                        communityUsersQueryable = communityUsersQueryable
                            .OrderByDescending(f => f.Id == userId)
                            .ThenByDescending(f => newlyCreatedInitiativeUserIds.Any(na => na == f.Id))
                            .ThenByDescending(f => f.UserProfile.Regions.Any())
                            .ThenByDescending(f => f.UserProfile.Regions.All(dr => regionIdsFromFilter.Contains(dr.RegionId)) && f.UserProfile.Regions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId)) == regionIdsFromFilter.Count() ? 1 : 0).ThenByDescending(f => f.UserProfile.UserLoginCount) // replace 1 with f.ArticleRegions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId))
                            .ThenByDescending(f => f.UserProfile.Regions.Any(dr => regionIdsFromFilter.Contains(dr.RegionId)) ? 1 : 0).ThenByDescending(f => f.UserProfile.UserLoginCount)
                            .ThenByDescending(f => usAllAndStatesIdsList.Count > 0 && f.UserProfile.Regions.Any(dr => usAllAndStatesIdsList.Contains(dr.RegionId)) ? 1 : 0).ThenByDescending(f => f.UserProfile.UserLoginCount)
                            .ThenByDescending(f => parentIdOfSelectedRegionList.Count > 0 && f.UserProfile.Regions.All(dr => parentIdOfSelectedRegionList.Contains(dr.RegionId)) && f.UserProfile.Regions.Count(dr => parentIdOfSelectedRegionList.Contains(dr.RegionId)) == parentIdOfSelectedRegionList.Count ? 1 : 0).ThenByDescending(f => f.UserProfile.UserLoginCount)
                            .ThenByDescending(f => parentIdOfSelectedRegionList.Count > 0 && f.UserProfile.Regions.Any(dr => parentIdOfSelectedRegionList.Contains(dr.RegionId)) ? 1 : 0).ThenByDescending(f => f.UserProfile.UserLoginCount);
                        break;
                    }

                case (int)Common.Enums.InitiativeScale.Regional:
                    {
                        var childRegionIdList = _context.Regions.Where(x => regionIdsFromFilter.Contains(x.ParentId ?? 0)).Select(x => x.Id).ToList();

                        communityUsersQueryable = communityUsersQueryable
                           .OrderByDescending(f => f.Id == userId)
                           .ThenByDescending(f => newlyCreatedInitiativeUserIds.Any(na => na == f.Id))
                           .ThenByDescending(f => f.UserProfile.Regions.Any())
                           .ThenByDescending(f => f.UserProfile.Regions.All(dr => regionIdsFromFilter.Contains(dr.RegionId)) && f.UserProfile.Regions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId)) == regionIdsFromFilter.Count() ? 1 : 0).ThenByDescending(f => f.UserProfile.UserLoginCount) // replace 1 with f.ArticleRegions.Count(dr => regionIdsFromFilter.Contains(dr.RegionId))
                           .ThenByDescending(f => f.UserProfile.Regions.Any(dr => regionIdsFromFilter.Contains(dr.RegionId)) ? 1 : 0).ThenByDescending(f => f.UserProfile.UserLoginCount)
                           .ThenByDescending(f => f.UserProfile.Regions.Any(dr => childRegionIdList.Contains(dr.RegionId)) ? 1 : 0).ThenByDescending(f => f.UserProfile.UserLoginCount);
                        break;
                    }
            }

            return communityUsersQueryable;
        }

    }
}