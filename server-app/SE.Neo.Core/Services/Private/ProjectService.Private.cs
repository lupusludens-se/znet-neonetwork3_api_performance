using Microsoft.EntityFrameworkCore;
using SE.Neo.Common.Attributes;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Models.Project;

namespace SE.Neo.Core.Services
{
    public partial class ProjectService
    {
        private async Task<BaseProjectDetails> AddProjectDetailsAsync(int projectId, BaseProjectDetails projectDetails)
        {
            projectDetails.Id = 0;
            projectDetails.ProjectId = projectId;
            await _context.AddAsync(projectDetails);

            await _context.SaveChangesAsync();

            return projectDetails;
        }

        private async Task<Project> AddProjectAsync(Project project, BaseProjectDetails projectDetails)
        {
            if (project.CompanyId == default(int))
            {
                int userCompanyId;
                if (project.Owner == null)
                {
                    User user = await _context.Users.FirstAsync(u => u.Id == project.OwnerId);
                    userCompanyId = user.CompanyId;
                }
                else
                {
                    userCompanyId = project.Owner.CompanyId;
                }

                project.CompanyId = userCompanyId;
            }

            project.ChangedOn = DateTime.UtcNow;

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            await AddProjectDetailsAsync(project.Id, projectDetails);
            await _context.SaveChangesAsync();

            return project;
        }

        private async Task<Project> EnsureAccessibleProjectExistsAsync(int id, int currentUserId, int currentUserCompanyId, bool accessAll = false, bool relationsIntermediatesInclude = true)
        {
            IQueryable<Project> projects = _context.Projects;

            if (relationsIntermediatesInclude)
                projects = projects
                    .Include(p => p.Regions)
                        .ThenInclude(r => r.Region)
                    .Include(p => p.Technologies)
                        .ThenInclude(r => r.Technology)
                    .Include(p => p.ValuesProvided)
                        .ThenInclude(v => v.ValueProvided)
                    .Include(p => p.ContractStructures)
                        .ThenInclude(v => v.ContractStructure);

            Project? project = await projects.SingleOrDefaultAsync(p => (accessAll || p.OwnerId == currentUserId || p.CompanyId == currentUserCompanyId) && p.Id == id && p.StatusId != Enums.ProjectStatus.Deleted);

            if (project == null)
                throw new CustomException($"{CoreErrorMessages.EntityNotFound} Project does not exist or deleted.");

            return project;
        }

        private async Task<bool> ApplyProjectStatusAsync(Project model)
        {
            if (model.StatusId == Enums.ProjectStatus.Draft)
            {
                model.PublishedOn = null;
                model.FirstTimePublishedOn = null;
            }


            if (model.PublishedOn == null && model.StatusId == Enums.ProjectStatus.Active)
            {
                model.PublishedOn = DateTime.UtcNow;
            }

            if (model.FirstTimePublishedOn == null && model.StatusId == Enums.ProjectStatus.Active)
            {
                model.FirstTimePublishedOn = DateTime.UtcNow;
            }


            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectsQueryable"></param>
        /// <param name="currentUserId"></param>
        /// <param name="userRoleIds"></param>
        /// <param name="userCompanyId"></param>
        /// <param name="expand"></param>
        /// <param name="sort">sort parameter </param>
        /// <param name="considerSort"> To check if sort has to be performed of or not as the recommonded projects is sortedby logic from ALS system</param>
        /// <returns></returns>
        private IQueryable<Project> ExpandSortProjects(IQueryable<Project> projectsQueryable, int currentUserId, List<int> userRoleIds, int userCompanyId,
                string? expand, string? sort = null, bool considerSort = true)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("category"))
                {
                    projectsQueryable = projectsQueryable.Include(p => p.Category);
                    if (expand.Contains("category.resources"))
                    {
                        projectsQueryable = projectsQueryable.Include(p => p.Category)
                            .ThenInclude(c => c.CategoryResources.Where(t =>
                            userRoleIds.Contains((int)Common.Enums.RoleType.Admin) || (t.Resource.Article == null && t.Resource.Tool == null)
                            || (t.Resource.Article != null && t.Resource.Article.ArticleRoles.Any(a => userRoleIds.Contains(a.RoleId)))
                            || (t.Resource.Tool != null && (t.Resource.Tool.Roles.Any(a => userRoleIds.Contains(a.RoleId))
                                || t.Resource.Tool.Companies.Any(c => c.CompanyId.Equals(userCompanyId))))
                            ))
                            .ThenInclude(cr => cr.Resource);
                    }
                    if (expand.Contains("category.technologies"))
                    {
                        projectsQueryable = projectsQueryable.Include(p => p.Category)
                            .ThenInclude(c => c.Technologies.Where(t => !t.Technology.IsDeleted))
                            .ThenInclude(t => t.Technology);
                    }
                }
                if (expand.Contains("company"))
                {
                    projectsQueryable = projectsQueryable.Include(p => p.Company);
                }
                if (expand.Contains("company.image"))
                {
                    projectsQueryable = projectsQueryable.Include(p => p.Company).ThenInclude(c => c.Image);
                }
                if (expand.Contains("owner"))
                {
                    projectsQueryable = projectsQueryable.Include(p => p.Owner);
                }
                if (expand.Contains("technologies"))
                {
                    projectsQueryable = projectsQueryable.Include(p => p.Technologies).ThenInclude(pl => pl.Technology);
                    if (expand.Contains("technologies.resources"))
                    {
                        projectsQueryable = projectsQueryable.Include(p => p.Technologies)
                            .ThenInclude(pl => pl.Technology)
                            .ThenInclude(t => t.TechnologyResources.Where(t =>
                             userRoleIds.Contains((int)Common.Enums.RoleType.Admin) || (t.Resource.Article == null && t.Resource.Tool == null)
                            || (t.Resource.Article != null && t.Resource.Article.ArticleRoles.Any(a => userRoleIds.Contains(a.RoleId)))
                            || (t.Resource.Tool != null && (t.Resource.Tool.Roles.Any(a => userRoleIds.Contains(a.RoleId))
                                || t.Resource.Tool.Companies.Any(c => c.Id.Equals(userCompanyId))))
                            ))
                            .ThenInclude(tr => tr.Resource);
                    }
                }
                if (expand.Contains("regions"))
                {
                    projectsQueryable = projectsQueryable.Include(p => p.Regions).ThenInclude(pl => pl.Region);
                }
                if (expand.Contains("saved"))
                {
                    projectsQueryable = projectsQueryable.Include(c => c.ProjectSaved);
                }
                if (expand.Contains("projectdetails"))
                {
                    projectsQueryable = projectsQueryable
                        .Include(c => c.ValuesProvided)
                        .Include(c => c.ContractStructures)
                        .Include(c => c.ProjectBatteryStorageDetails)
                        .Include(c => c.ProjectCarbonOffsetsDetails)
                            .ThenInclude(co => co.StripLengths)
                        .Include(c => c.ProjectCommunitySolarDetails)
                        .Include(c => c.ProjectEACDetails)
                            .ThenInclude(eac => eac.StripLengths)
                        .Include(c => c.ProjectEfficiencyAuditsAndConsultingDetails)
                        .Include(c => c.ProjectEfficiencyEquipmentMeasuresDetails)
                        .Include(c => c.ProjectEmergingTechnologyDetails)
                        .Include(c => c.ProjectEVChargingDetails)
                        .Include(c => c.ProjectFuelCellsDetails)
                        .Include(c => c.ProjectGreenTariffsDetails)
                        .Include(c => c.ProjectOffsitePowerPurchaseAgreementDetails)
                            .ThenInclude(oppa => oppa.ValuesToOfftakers)
                        .Include(c => c.ProjectOnsiteSolarDetails)
                        .Include(c => c.ProjectRenewableRetailDetails)
                            .ThenInclude(oppa => oppa.PurchaseOptions);
                }
            }
            if (considerSort)
            {
                if (!string.IsNullOrEmpty(sort))
                {
                    // This .OrderBy(p => 0) is done to acquire IOrderedQueryable type and then use it
                    // to build order query in this scope.
                    IOrderedQueryable<Project> projectsOrderedQueryable = projectsQueryable.OrderBy(p => 0);

                    sort = sort.ToLower();
                    if (sort.Contains("ispinned"))
                    {
                        if (sort.Contains("ispinned.asc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenBy(p => p.IsPinned);

                        if (sort.Contains("ispinned.desc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenByDescending(p => p.IsPinned);
                    }
                    if (sort.Contains("title"))
                    {
                        if (sort.Contains("title.asc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenBy(p => p.Title);

                        if (sort.Contains("title.desc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenByDescending(p => p.Title);
                    }
                    if (sort.Contains("category"))
                    {
                        if (sort.Contains("category.asc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenBy(p => p.Category.Name);

                        if (sort.Contains("category.desc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenByDescending(p => p.Category.Name);
                    }
                    if (sort.Contains("publishedon"))
                    {
                        if (sort.Contains("publishedon.asc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenBy(p => p.PublishedOn ?? DateTime.MaxValue);

                        if (sort.Contains("publishedon.desc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenByDescending(p => p.PublishedOn ?? DateTime.MinValue);
                    }
                    if (sort.Contains("changedon"))
                    {
                        if (sort.Contains("changedon.asc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenBy(p => p.ChangedOn);

                        if (sort.Contains("changedon.desc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenByDescending(p => p.ChangedOn);
                    }
                    if (sort.Contains("company"))
                    {
                        if (sort.Contains("company.asc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenBy(p => p.Company.Name);

                        if (sort.Contains("company.desc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenByDescending(p => p.Company.Name);
                    }
                    if (sort.Contains("publishedby"))
                    {
                        if (sort.Contains("publishedby.asc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenBy(p => p.Owner.FirstName);

                        if (sort.Contains("publishedby.desc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenByDescending(p => p.Owner.FirstName);
                    }
                    if (sort.Contains("status"))
                    {
                        if (sort.Contains("status.asc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenBy(p => p.Status.Name);

                        if (sort.Contains("status.desc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenByDescending(p => p.Status.Name);
                    }
                    if (sort.Contains("regions"))
                    {
                        //TODO: Advance ordering of regions.
                        if (sort.Contains("regions.asc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenBy(p => p.Regions.OrderBy(pr => pr.Region.Name).Select(pr => pr.Region.Name).FirstOrDefault());

                        if (sort.Contains("regions.desc"))
                            projectsOrderedQueryable = projectsOrderedQueryable.ThenByDescending(p => p.Regions.OrderBy(pr => pr.Region.Name).Select(pr => pr.Region.Name).FirstOrDefault());
                    }

                    return projectsOrderedQueryable.ThenBy(p => p.Title);
                }
                else
                {
                    projectsQueryable = projectsQueryable.OrderByDescending(p => p.StatusId == Enums.ProjectStatus.Draft).ThenByDescending(p => p.ChangedOn);
                }
            }

            return projectsQueryable;
        }

        private IQueryable<Project> SearchProjects(IQueryable<Project> projectsQueryable, int currentUserId, string? search, string? filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                foreach (string property in filter.Split("&").ToList())
                {
                    if (property.Contains("foryou"))
                    {
                        IQueryable<int> userProfileRegions = _context.UserProfileRegions
                            .Where(upc => upc.UserProfile.UserId == currentUserId)
                            .Select(x => x.RegionId);
                        IQueryable<int> userProfileCategories = _context.UserProfileCategories
                            .Where(upc => upc.UserProfile.UserId == currentUserId)
                            .Select(x => x.CategoryId);

                        projectsQueryable = projectsQueryable
                            .Where(p => p.StatusId != Enums.ProjectStatus.Deleted
                                && (p.Regions.Any(dr => userProfileRegions.Contains(dr.RegionId))
                                    || userProfileCategories.Contains(p.CategoryId)))
                            .OrderByDescending(p => p.IsPinned)
                            .ThenByDescending(p => p.Regions.Any(dr => userProfileRegions.Contains(dr.RegionId))
                                && userProfileCategories.Contains(p.CategoryId))
                            .ThenByDescending(p => userProfileCategories.Contains(p.CategoryId))
                            .ThenByDescending(f => f.Regions.Any(dr => userProfileRegions.Contains(dr.RegionId)))
                            .ThenByDescending(f => f.ModifiedOn);
                    }

                    if (property.Contains("saved"))
                    {
                        projectsQueryable = projectsQueryable.Where(p => p.StatusId == Enums.ProjectStatus.Active && p.ProjectSaved.Any(ds => ds.UserId == currentUserId));
                    }

                    var ids = ParseFilterByField(property);
                    if (ids != null && ids.Count > 0)
                    {
                        if (property.Contains("solutionids"))
                        {
                            projectsQueryable = projectsQueryable.Where(x => ids.Contains(x.Category.SolutionId.GetValueOrDefault()));
                        }

                        if (property.Contains("categoryids"))
                        {
                            projectsQueryable = projectsQueryable.Where(x => ids.Contains(x.CategoryId));
                        }

                        if (property.Contains("technologyids"))
                        {
                            projectsQueryable = projectsQueryable.Where(x => x.Technologies.Any(t => ids.Contains(t.TechnologyId)));
                        }

                        if (property.Contains("regionids"))
                        {
                            List<int> regionIds = _commonService.ExpandRegionListForFiltration(ids, true, true);
                            projectsQueryable = projectsQueryable.Where(x => x.Regions.Any(t => regionIds.Contains(t.RegionId)));
                        }

                        if (property.Contains("statusids"))
                        {
                            projectsQueryable = projectsQueryable.Where(x => ids.Contains((int)x.StatusId));
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                if (search.Contains(" in "))
                {
                    string tag = search.Split(" in ")[0].Trim();
                    string region = search.Replace(tag + " in ", string.Empty).Trim();

                    var specialRegions = new List<string>();
                    _context.Regions.Where(r => r.Name.Contains("&")).ForEachAsync(r =>
                    {
                        specialRegions.AddRange(r.Name.Split("&").Select(item => item.Trim().ToLower()).ToList());
                    }).Wait();

                    var regionId = region.Equals("USA", StringComparison.CurrentCultureIgnoreCase) ?
                            _context.Regions.Where(r => r.Name.Contains(region)).FirstOrDefault()?.Id ?? 0
                        : (_context.Regions.Where(r => r.Name.Equals(region) || r.Name.Equals("US - " + region)).FirstOrDefault()?.Id ??
                    _context.Regions.Where(r => r.Name.Contains("&") && specialRegions.Contains(region.ToLower())).FirstOrDefault()?.Id ?? 0);

                    if (regionId > 0)
                    {
                        List<int> regionIds = new()
                        {
                            regionId
                        };

                        var regionParentId = _context.Regions.Where(r => r.Id == regionId).First().ParentId ?? 0;

                        regionIds.Add(regionParentId);

                        if (regionParentId == 0) // This logic is for if user searches with region instead of country
                        {
                            var regIds = _context.Regions.Where(r => r.ParentId == regionId)?.Select(x => x.Id);
                            if (regIds?.Count() > 0)
                            {
                                regionIds.AddRange(regIds);
                            }
                        }

                        if (region.Equals("USA", StringComparison.CurrentCultureIgnoreCase) || region.Equals("ALL", StringComparison.CurrentCultureIgnoreCase))
                        {
                            var regionIdsUnderUSA = _context.Regions.Where(r => r.ParentId == regionId && r.Slug.StartsWith("us-"))?.Select(x => x.Id);
                            if (regionIdsUnderUSA?.Count() > 0)
                            {
                                regionIds.AddRange(regionIdsUnderUSA);
                            }
                        }

                        projectsQueryable = projectsQueryable.Where(p => p.Title.ToLower().Contains(search)
                            || p.Owner.FirstName.StartsWith(search)
                            || p.Owner.LastName.StartsWith(search)
                            || (p.Owner.FirstName + " " + p.Owner.LastName).StartsWith(search)
                            || p.Regions.Any(r => r.Region.Name.ToLower().Contains(search))
                            || p.Category.Name.ToLower().Contains(search)
                            || p.Opportunity.ToLower().Contains(search)
                            || p.Company.Name.StartsWith(search)
                            || (p.Tags != null && p.Tags.ToLower().Contains(tag) && p.Regions.Any(r => regionIds.Contains(r.RegionId)))).
                            OrderByDescending(x => x.Regions.Select(r => r.RegionId).Contains(regionId) ? 1 : 0).OrderByDescending(x => x.ModifiedOn);
                    }
                    else
                    {
                        projectsQueryable = projectsQueryable.Where(p => p.Title.ToLower().Contains(search)
                        || p.Owner.FirstName.StartsWith(search)
                        || p.Owner.LastName.StartsWith(search)
                        || (p.Owner.FirstName + " " + p.Owner.LastName).StartsWith(search)
                        || p.Regions.Any(r => r.Region.Name.ToLower().Contains(search))
                        || p.Category.Name.ToLower().Contains(search)
                        || p.Opportunity.ToLower().Contains(search)
                        || p.Company.Name.StartsWith(search)
                        || (p.Tags != null && p.Tags.ToLower().Contains(search))).OrderByDescending(x => x.ModifiedOn); ;
                    }
                }
                else
                {
                    projectsQueryable = projectsQueryable.Where(p => p.Title.ToLower().Contains(search)
                        || p.Owner.FirstName.StartsWith(search)
                        || p.Owner.LastName.StartsWith(search)
                        || (p.Owner.FirstName + " " + p.Owner.LastName).StartsWith(search)
                        || p.Regions.Any(r => r.Region.Name.ToLower().Contains(search))
                        || p.Category.Name.ToLower().Contains(search)
                        || p.Opportunity.ToLower().Contains(search)
                        || p.Company.Name.StartsWith(search)
                        || (p.Tags != null && p.Tags.ToLower().Contains(search)));
                }
            }

            return projectsQueryable;
        }

    }
}