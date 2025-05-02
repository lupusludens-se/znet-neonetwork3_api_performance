using Microsoft.EntityFrameworkCore;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Services
{
    public partial class ToolService
    {
        private IQueryable<Tool> ExpandSearchOrderTools(IQueryable<Tool> toolsQueryable, BaseSearchFilterModel filter, int? userId = null)
        {
            if (!string.IsNullOrEmpty(filter.FilterBy))
            {
                var filterBy = filter.FilterBy.ToLower();
                if (filterBy.Contains("isactive"))
                    toolsQueryable = toolsQueryable.Where(p => p.IsActive);
            }
            if (!string.IsNullOrEmpty(filter.Expand))
            {
                var expand = filter.Expand.ToLower();
                if (expand.Contains("roles"))
                    toolsQueryable = toolsQueryable.Include(p => p.Roles).ThenInclude(r => r.Role);
                if (expand.Contains("companies"))
                    toolsQueryable = toolsQueryable.Include(p => p.Companies).ThenInclude(c => c.Company);
                if (expand.Contains("pinned"))
                    toolsQueryable = toolsQueryable.Include(p => p.Pinned);
                if (expand.Contains("icon"))
                    toolsQueryable = toolsQueryable.Include(p => p.Icon);
            }
            if (!string.IsNullOrEmpty(filter.Search))
            {
                toolsQueryable = toolsQueryable.Where(t => (!string.IsNullOrEmpty(t.Title)) && t.Title.ToLower().Contains(filter.Search.ToLower()));
            }
            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                string orderBy = filter.OrderBy.ToLower();
                if (orderBy.Contains("title.asc"))
                    toolsQueryable = toolsQueryable.OrderBy(t => t.Title);
                if (orderBy.Contains("title.desc"))
                    toolsQueryable = toolsQueryable.OrderByDescending(t => t.Title);
                if (orderBy.Contains("status.asc"))
                    toolsQueryable = toolsQueryable.OrderBy(t => t.IsActive).ThenBy(t => t.Title);
                if (orderBy.Contains("status.desc"))
                    toolsQueryable = toolsQueryable.OrderByDescending(t => t.IsActive).ThenBy(t => t.Title);
                if (userId.HasValue)
                {
                    if (orderBy.Contains("pinned.desc"))
                        toolsQueryable = toolsQueryable.OrderByDescending(o => o.Pinned.Any(df => df.UserId == userId.Value)).ThenBy(t => t.Title);
                    if (orderBy.Contains("pinned.asc"))
                        toolsQueryable = toolsQueryable.OrderBy(o => o.Pinned.Any(df => df.UserId == userId.Value)).ThenBy(t => t.Title);
                }
            }
            return toolsQueryable;
        }

        private IQueryable<Tool> FilterUserAllowedTools(int userId, IQueryable<Tool> query)
        {

            if (userId > 0)
            {
                IQueryable<User> user = _context.Users
                .AsNoTracking()
                .Include(r => r.Roles)
                .Where(p => p.Id == userId);
                query = query.Where(t => !t.Companies.Any() || t.Companies.Select(c => c.CompanyId).Contains(user.FirstOrDefault().CompanyId));
                query = query.Where(t => !t.Roles.Any() || t.Roles.Select(tr => tr.RoleId).Any(roleId => user.FirstOrDefault().Roles.Select(ur => ur.RoleId).Contains(roleId)));
            }
            else
            {
                query = query.Where(t => t.Roles.Any(role => (role.RoleId == Convert.ToInt32(RoleType.Corporation)) || (role.RoleId == Convert.ToInt32(RoleType.All))));
            }
            return query;
        }
    }
}