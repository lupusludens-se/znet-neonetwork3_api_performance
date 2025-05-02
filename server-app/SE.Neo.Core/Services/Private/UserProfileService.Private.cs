using Microsoft.EntityFrameworkCore;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Services
{
    public partial class UserProfileService
    {
        private IQueryable<UserProfile> ExpandUserProfiles(IQueryable<UserProfile> usersQueryable, string? expand, string? sort = null)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();

                if (expand.Contains("state"))
                {
                    usersQueryable = usersQueryable.Include(p => p.State);
                }
                if (expand.Contains("user"))
                {
                    usersQueryable = usersQueryable.Include(p => p.User);
                }
                if (expand.Contains("user.company"))
                {
                    usersQueryable = usersQueryable.Include(p => p.User).ThenInclude(c => c.Company);
                }
                if (expand.Contains("user.role"))
                {
                    usersQueryable = usersQueryable.Include(p => p.User).ThenInclude(p => p.Roles).ThenInclude(c => c.Role);
                }
                if (expand.Contains("user.country"))
                {
                    usersQueryable = usersQueryable.Include(p => p.User).ThenInclude(p => p.Country);
                }
                if (expand.Contains("user.image"))
                {
                    usersQueryable = usersQueryable.Include(p => p.User).ThenInclude(c => c.Image);
                }
                if (expand.Contains("categories"))
                {
                    usersQueryable = usersQueryable
                        .Include(p => p.Categories.Where(s => !s.Category.IsDeleted))
                        .ThenInclude(c => c.Category);
                }
                if (expand.Contains("regions"))
                {
                    usersQueryable = usersQueryable
                        .Include(p => p.Regions.Where(s => !s.Region.IsDeleted))
                        .ThenInclude(c => c.Region);
                }
                if (expand.Contains("urllinks"))
                {
                    usersQueryable = usersQueryable.Include(p => p.UrlLinks);
                }
                if (expand.Contains("skills"))
                {
                    usersQueryable = usersQueryable.Include(p => p.SkillsByCategory).ThenInclude(y=>y.Skills).Include(p=>p.SkillsByCategory).ThenInclude(z=>z.Categories);
                }
            }

            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();
                if (sort.Contains("user.lastname"))
                {
                    if (sort.Contains("user.lastname.asc"))
                    {
                        return usersQueryable.OrderBy(o => o.User.LastName);
                    }
                    if (sort.Contains("user.lastname.desc"))
                    {
                        return usersQueryable.OrderByDescending(o => o.User.LastName);
                    }
                }
            }
            return usersQueryable;
        }

        private IQueryable<UserProfile> FilterSearchUserProfiles(IQueryable<UserProfile> userProfilesQueryable, string? search, string? filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                foreach (string property in filter.Split("&").ToList())
                {
                    var ids = ParseFilterByField(property);
                    if (ids != null)
                    {
                        if (property.Contains("regionids"))
                        {
                            userProfilesQueryable =
                                userProfilesQueryable.Where(x => x.Regions.Select(r => r.RegionId).Any(rId => ids.Contains(rId)));
                        }

                        if (property.Contains("categoryids"))
                        {
                            userProfilesQueryable =
                                userProfilesQueryable.Where(x => x.Categories.Select(r => r.CategoryId).Any(rId => ids.Contains(rId)));
                        }

                        if (property.Contains("companyids"))
                        {
                            userProfilesQueryable =
                                userProfilesQueryable.Where(x => ids.Contains((int)x.User.CompanyId));
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                userProfilesQueryable = userProfilesQueryable.Where(p => (!string.IsNullOrEmpty(p.User.FirstName) && p.User.FirstName.StartsWith(search)) ||
                (!string.IsNullOrEmpty(p.User.LastName) && p.User.LastName.StartsWith(search)) ||
                (p.User.FirstName + " " + p.User.LastName).StartsWith(search));
            }
            return userProfilesQueryable;
        }
    }
}
