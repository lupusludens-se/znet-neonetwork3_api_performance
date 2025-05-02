using Microsoft.EntityFrameworkCore;
using SE.Neo.Common.Attributes;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Services
{
    public partial class UserPendingService
    {
        private async Task<UserPending> EnsureUserPendingExistsAsync(int id)
        {
            UserPending? userPending = await _context.UserPendings.SingleOrDefaultAsync(b => b.Id == id);
            if (userPending == null)
            {
                throw new CustomException($"{CoreErrorMessages.UserPendingNotFound}");
            }
            return userPending;
        }

        private IQueryable<UserPending> ExpandSortUserPendings(IQueryable<UserPending> userPendingsQueryable, string? expand = null, string? sort = null)
        {
            sort ??= "createddate.desc";
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("company"))
                {
                    userPendingsQueryable = userPendingsQueryable.Include(p => p.Company);
                }
                if (expand.Contains("country"))
                {
                    userPendingsQueryable = userPendingsQueryable.Include(p => p.Country);
                }
                if (expand.Contains("role"))
                {
                    userPendingsQueryable = userPendingsQueryable.Include(p => p.Role);
                }
            }

            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();
                if (sort.Contains("name"))
                {
                    if (sort.Contains("name.asc"))
                    {
                        return userPendingsQueryable.OrderBy(o => o.LastName);
                    }
                    if (sort.Contains("name.desc"))
                    {
                        return userPendingsQueryable.OrderByDescending(o => o.LastName);
                    }
                }
                if (sort.Contains("company"))
                {
                    if (sort.Contains("company.asc"))
                    {
                        return userPendingsQueryable.OrderBy(o => o.Company.Name ?? o.CompanyName).ThenBy(p => p.LastName);
                    }
                    if (sort.Contains("company.desc"))
                    {
                        return userPendingsQueryable.OrderByDescending(o => o.Company.Name ?? o.CompanyName).ThenBy(p => p.LastName);
                    }
                }
                if (sort.Contains("role"))
                {
                    if (sort.Contains("role.asc"))
                    {
                        return userPendingsQueryable.OrderBy(o => o.Role.Name).ThenBy(p => p.LastName);
                    }
                    if (sort.Contains("role.desc"))
                    {
                        return userPendingsQueryable.OrderByDescending(o => o.Role.Name).ThenBy(p => p.LastName);
                    }
                }
                if (sort.Contains("createddate"))
                {
                    if (sort.Contains("createddate.asc"))
                    {
                        return userPendingsQueryable.OrderBy(o => o.CreatedOn);
                    }
                    if (sort.Contains("createddate.desc"))
                    {
                        return userPendingsQueryable.OrderByDescending(o => o.CreatedOn);
                    }
                }
            }
            return userPendingsQueryable;
        }
    }
}