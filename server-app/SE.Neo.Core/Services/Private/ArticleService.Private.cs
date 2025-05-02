using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;
using System.Net;

namespace SE.Neo.Core.Services
{
    public partial class ArticleService
    {
        private const string FilterForYou = "foryou";
        private const string FilterSaved = "saved";

        private async Task HandleTaxonomiesAsync<T>(IEnumerable<BaseTaxonomyDTO> modelDTO, DbSet<T> _contextModel, string tableName) where T : BaseCMSEntity, new()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    IEnumerable<T> taxonomyToInsert = modelDTO
                        .Where(s => !_contextModel.Where(w => !w.IsDeleted).Select(es => es.Id).Contains(s.Id))
                        .Select(c => new T() { Name = WebUtility.HtmlDecode(c.Name), Id = c.Id, Slug = c.Slug, Description = WebUtility.HtmlDecode(c.Description) });

                    if (taxonomyToInsert.Any())
                    {
                        _contextModel.AddRange(taxonomyToInsert);
                    }

                    // Update list of technologies and categories
                    IEnumerable<T> entities = await _contextModel
                        .Where(s => modelDTO.Select(es => es.Id).Contains(s.Id)).ToListAsync();

                    modelDTO.ToList().ForEach(taxonomyDTO =>
                    {
                        T? item = entities.Where(i =>
                            i.Id.Equals(taxonomyDTO.Id) && (!i.Name.Equals(WebUtility.HtmlDecode(taxonomyDTO.Name))
                            || !i.Slug.Equals(taxonomyDTO.Slug)
                            || !i.Description.Equals(WebUtility.HtmlDecode(taxonomyDTO.Description)))
                                && !i.IsDeleted).FirstOrDefault();
                        if (item != null)
                        {
                            item.Name = WebUtility.HtmlDecode(taxonomyDTO.Name);
                            item.Slug = taxonomyDTO.Slug;
                            item.Description = WebUtility.HtmlDecode(taxonomyDTO.Description);
                            _contextModel.Update(item);
                        }
                    });

                    (await _contextModel.Where(s => !modelDTO.Select(es => es.Id).Contains(s.Id))
                        .ToListAsync()).ForEach(e => e.IsDeleted = true);

                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT " + tableName + " ON;");
                    await _context.SaveChangesAsync();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT " + tableName + " OFF;");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
        }

        private async Task HandleRegionTaxonomiesAsync(IEnumerable<RegionDTO> modelDTO, DbSet<Region> _contextModel, string tableName)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    IEnumerable<Region> regionsToInsert = modelDTO
                        .Where(s => !_contextModel.Where(w => !w.IsDeleted).Select(es => es.Id).Contains(s.Id))
                        .Select(c => new Region() { Name = WebUtility.HtmlDecode(c.Name), Id = c.Id, Slug = c.Slug, ParentId = ((RegionDTO)c).ParentId, Description = WebUtility.HtmlDecode(c.Description) });

                    if (regionsToInsert.Any())
                    {
                        _contextModel.AddRange(regionsToInsert);
                    }

                    // Update list of regions
                    IEnumerable<Region> entities = await _contextModel
                        .Where(s => modelDTO.Select(es => es.Id).Contains(s.Id)).ToListAsync();

                    modelDTO.ToList().ForEach(taxonomyDTO =>
                    {
                        Region? item = entities.Where(i =>
                            i.Id.Equals(taxonomyDTO.Id) && (!i.Name.Equals(WebUtility.HtmlDecode(taxonomyDTO.Name))
                            || !i.ParentId.Equals(taxonomyDTO.ParentId) || !i.Slug.Equals(taxonomyDTO.Slug)
                            || !i.Description.Equals(WebUtility.HtmlDecode(taxonomyDTO.Description)))
                                && !i.IsDeleted).FirstOrDefault();

                        if (item != null)
                        {
                            item.Name = WebUtility.HtmlDecode(taxonomyDTO.Name);
                            item.Slug = taxonomyDTO.Slug;
                            item.ParentId = taxonomyDTO.ParentId;
                            item.Description = WebUtility.HtmlDecode(taxonomyDTO.Description);
                            _contextModel.Update(item);
                        }
                    });

                    (await _contextModel.Where(s => !modelDTO.Select(es => es.Id).Contains(s.Id))
                        .ToListAsync()).ForEach(e => e.IsDeleted = true);

                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT " + tableName + " ON;");
                    await _context.SaveChangesAsync();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT " + tableName + " OFF;");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
        }

        private async Task HandleTaxonomiesForArticleAsync(ArticleDTO modelDTO, List<Role> roleList)
        {
            _context.RemoveRange(_context.ArticleCategories.Where(a => a.ArticleId == modelDTO.Id));
            _context.RemoveRange(_context.ArticleRegions.Where(a => a.ArticleId == modelDTO.Id));
            _context.RemoveRange(_context.ArticleSolutions.Where(a => a.ArticleId == modelDTO.Id));
            _context.RemoveRange(_context.ArticleTechnologies.Where(a => a.ArticleId == modelDTO.Id));
            _context.RemoveRange(_context.ArticleRoles.Where(a => a.ArticleId == modelDTO.Id));
            _context.RemoveRange(_context.ArticleContentTags.Where(a => a.ArticleId == modelDTO.Id));

            // Add new CMS dependencies
            _context.ArticleCategories.AddRange(modelDTO.Categories.Select(item => new ArticleCategory() { ArticleId = modelDTO.Id, CategoryId = item.Id }));
            _context.ArticleRegions.AddRange(modelDTO.Regions.Select(item => new ArticleRegion() { ArticleId = modelDTO.Id, RegionId = item.Id }));
            _context.ArticleContentTags.AddRange(modelDTO.ContentTags.Select(item => new ArticleContentTag() { ArticleId = modelDTO.Id, ContentTagId = item.Id }));
            _context.ArticleSolutions.AddRange(modelDTO.Solutions.Select(item => new ArticleSolution() { ArticleId = modelDTO.Id, SolutionId = item.Id }));
            _context.ArticleTechnologies.AddRange(modelDTO.Technologies.Select(item => new ArticleTechnology() { ArticleId = modelDTO.Id, TechnologyId = item.Id }));
            _context.ArticleRoles.AddRange(modelDTO.Roles.Select(item => new ArticleRole()
            {
                ArticleId = modelDTO.Id,
                RoleId = roleList.FirstOrDefault(role => role.CMSRoleId == item.CMSRoleId)?.Id ?? (int)RoleType.All
            }));
            var solutionProviderCMSRoleId = roleList.FirstOrDefault(r => r.Id == (int)RoleType.SolutionProvider)?.CMSRoleId;
            var adminCMSRoleId = roleList.FirstOrDefault(r => r.Id == (int)RoleType.Admin)?.CMSRoleId;
            if (modelDTO.Roles.Any(r => r.CMSRoleId == solutionProviderCMSRoleId))
            {
                _context.ArticleRoles.Add(new ArticleRole { ArticleId = modelDTO.Id, RoleId = (int)RoleType.SPAdmin });
            }
            if (modelDTO.Roles.Any(r => r.CMSRoleId == adminCMSRoleId))
            {
                _context.ArticleRoles.Add(new ArticleRole { ArticleId = modelDTO.Id, RoleId = (int)RoleType.SystemOwner });
            }
        }

        private IQueryable<Article> ExpandSortArticles(IQueryable<Article> articlesQueryable, string? expand, string? sort = null)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("categories"))
                {
                    articlesQueryable = articlesQueryable
                        .Include(p => p.ArticleCategories.Where(s => !s.Category.IsDeleted))
                        .ThenInclude(c => c.Category);
                }
                if (expand.Contains("regions"))
                {
                    articlesQueryable = articlesQueryable.Include(p => p.ArticleRegions.Where(s => !s.Region.IsDeleted))
                         .ThenInclude(c => c.Region);
                }
                if (expand.Contains("solutions"))
                {
                    articlesQueryable = articlesQueryable.Include(p => p.ArticleSolutions.Where(s => !s.Solution.IsDeleted))
                         .ThenInclude(c => c.Solution);
                }
                if (expand.Contains("technologies"))
                {
                    articlesQueryable = articlesQueryable.Include(p => p.ArticleTechnologies.Where(s => !s.Technology.IsDeleted))
                         .ThenInclude(c => c.Technology);

                }
                if (expand.Contains("contenttags"))
                {
                    //dont remove the condition, this is needed to load tags in some sections - .Where(x => x.ArticleContentTags.Count > -1)
                    articlesQueryable = articlesQueryable.Where(x => x.ArticleContentTags.Count > -1)
                        .Include(p => p.ArticleContentTags.Where(s => !s.ContentTag.IsDeleted))
                         .ThenInclude(c => c.ContentTag);
                }
            }
            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();
                if (sort.Contains("id"))
                {
                    if (sort.Contains("id.asc"))
                    {
                        return articlesQueryable.OrderBy(o => o.Id);
                    }
                    if (sort.Contains("id.desc"))
                    {
                        return articlesQueryable.OrderByDescending(o => o.Id);
                    }
                }
                if (sort.Contains("date"))
                {
                    if (sort.Contains("date.asc"))
                    {
                        return articlesQueryable.OrderBy(o => o.Date);
                    }
                    if (sort.Contains("date.desc"))
                    {
                        return articlesQueryable.OrderByDescending(o => o.Date);
                    }
                }
                if (sort.Contains("modified"))
                {
                    if (sort.Contains("modified.asc"))
                    {
                        return articlesQueryable.OrderBy(o => o.Modified);
                    }
                    if (sort.Contains("modified.desc"))
                    {
                        return articlesQueryable.OrderByDescending(o => o.Modified);
                    }
                }
            }
            return articlesQueryable;
        }

        private IQueryable<Article> FilterSearchArticles(IQueryable<Article> articlesQueryable, string? search, string? filter, int userId)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();

                if (userId > 0)
                {
                    if (filter.Equals(FilterSaved, StringComparison.InvariantCultureIgnoreCase))
                    {
                        articlesQueryable = articlesQueryable.Where(f => f.ArticleSaved.Any(ds => ds.UserId == userId));
                    }
                    else if (filter.Equals(FilterForYou, StringComparison.InvariantCultureIgnoreCase))
                    {
                        List<int> userProfileRegions =
                            Task.Run(async () => await _commonService.GetRegionListForUserProfile(userId, true)).Result;

                        IQueryable<int> userProfileCategories = _context.UserProfileCategories
                            .Where(upc => upc.UserProfile.UserId == userId)
                            .Select(x => x.CategoryId);

                        articlesQueryable = articlesQueryable
                            .Where(p => !p.IsDeleted
                                && (p.ArticleRegions.Any(dr => userProfileRegions.Contains(dr.RegionId))
                                || p.ArticleCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId))))
                            .OrderByDescending(f => f.ArticleRegions.Any(dr => userProfileRegions.Contains(dr.RegionId))
                                && f.ArticleCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId)))
                            .ThenByDescending(f => f.ArticleCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId)))
                            .ThenByDescending(f => f.ArticleRegions.Any(dr => userProfileRegions.Contains(dr.RegionId)))
                            .ThenByDescending(f => f.Modified);
                    }
                }
                else
                {
                    if (filter.Equals(FilterForYou, StringComparison.InvariantCultureIgnoreCase))
                    {
                        articlesQueryable = articlesQueryable.Where(a => !a.IsDeleted && a.IsPublicArticle);
                    }
                }

                foreach (string property in filter.Split("&").ToList())
                {
                    var ids = ParseFilterByField(property);
                    if (ids != null && ids.Count > 0)
                    {
                        if (property.Contains("regionids") || property.Contains("regions"))
                        {
                            List<int> regionIds = _commonService.ExpandRegionListForFiltration(ids, true, true);
                            articlesQueryable =
                                articlesQueryable.Where(x => x.ArticleRegions.Select(r => r.RegionId).Any(rId => regionIds.Contains(rId)));
                        }

                        if (property.Contains("categoryids") || property.Contains("categories"))
                        {
                            articlesQueryable =
                                articlesQueryable.Where(x => x.ArticleCategories.Select(r => r.CategoryId).Any(rId => ids.Contains(rId)));
                        }
                        if (property.Contains("solutionids") || property.Contains("solutions"))
                        {
                            articlesQueryable = articlesQueryable.Where(x => x.ArticleSolutions.Select(r => r.SolutionId).Any(rId => ids.Contains(rId)));
                        }

                        if (property.Contains("technologyids") || property.Contains("technologies"))
                        {
                            articlesQueryable = articlesQueryable.Where(x => x.ArticleTechnologies.Select(r => r.TechnologyId).Any(rId => ids.Contains(rId)));
                        }

                    }
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                if (userId > 0)
                {
                    articlesQueryable = articlesQueryable.Where(p => (!string.IsNullOrEmpty(p.Title) && p.Title.ToLower().Contains(search.ToLower())) ||
                    (!string.IsNullOrEmpty(p.Content) && p.Content.ToLower().Contains(search.ToLower())));
                }
                else
                {
                    articlesQueryable = articlesQueryable.Where(p => (!string.IsNullOrEmpty(p.Title) && p.Title.ToLower().Contains(search.ToLower())) ||
                    (!string.IsNullOrEmpty(p.Content) && p.Content.ToLower().Contains(search.ToLower())) ||
                    p.ArticleTechnologies.Any(s => s.Technology.Name.ToLower().Contains(search.ToLower())) ||
                    p.ArticleSolutions.Any(s => s.Solution.Name.ToLower().Contains(search.ToLower())) ||
                    p.ArticleCategories.Any(s => s.Category.Name.ToLower().Contains(search.ToLower())) ||
                    p.ArticleContentTags.Any(s => s.ContentTag.Name.ToLower().Contains(search.ToLower())));
                }
            }
            return articlesQueryable;
        }

        private async Task<bool> IsArticleSavedAsync(int articleId, int userId)
        {
            var userSavedArticlesContext = await _cache.GetRecordAsync<List<ArticleSaved>>(CoreCacheKeys.ArticleSavedContext + userId);
            if (userSavedArticlesContext is null)
            {
                userSavedArticlesContext = await _context.SavedArticles.Where(sa => sa.UserId == userId).ToListAsync();
                await _cache.SetRecordAsync(CoreCacheKeys.ArticleSavedContext + userId, userSavedArticlesContext, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCacheTimeStamp.Long)
                });
            }

            return userSavedArticlesContext.Any(m => m.ArticleId.Equals(articleId));
        }
    }
}