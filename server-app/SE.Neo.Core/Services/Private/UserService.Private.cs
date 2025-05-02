using Microsoft.EntityFrameworkCore;
using SE.Neo.Common.Enums;
using SE.Neo.Core.Entities;
using System.Globalization;
using System.Text;
namespace SE.Neo.Core.Services
{
    public partial class UserService
    {
        private IQueryable<User> ExpandSortUsers(IQueryable<User> usersQueryable, string? expand, string? sort = null, bool accessPrivateInfo = false)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("company"))
                {
                    usersQueryable = usersQueryable.Include(p => p.Company);
                }
                if (expand.Contains("country"))
                {
                    usersQueryable = usersQueryable.Include(p => p.Country);
                }
                if (expand.Contains("image"))
                {
                    usersQueryable = usersQueryable.Include(p => p.Image);
                }
                if (expand.Contains("roles"))
                {
                    usersQueryable = usersQueryable.Include(p => p.Roles).ThenInclude(c => c.Role);
                }
                if (expand.Contains("permissions") && accessPrivateInfo)
                {
                    usersQueryable = usersQueryable.Include(p => p.Permissions).ThenInclude(c => c.Permission);
                    usersQueryable = usersQueryable.Include(p => p.Roles)
                        .ThenInclude(c => c.Role).ThenInclude(r => r.Permissions)
                        .ThenInclude(p => p.Permission);
                }
                if (expand.Contains("userprofile"))
                {
                    usersQueryable = usersQueryable.Include(p => p.UserProfile);
                }
                if (expand.Contains("userprofile.state"))
                {
                    usersQueryable = usersQueryable.Include(p => p.UserProfile).ThenInclude(p => p.State);
                }
                if (expand.Contains("userprofile.urllinks"))
                {
                    usersQueryable = usersQueryable.Include(p => p.UserProfile).ThenInclude(p => p.UrlLinks);
                }
                if (expand.Contains("userprofile.categories"))
                {
                    usersQueryable = usersQueryable.Include(p => p.UserProfile)
                        .ThenInclude(c => c.Categories.Where(s => !s.Category.IsDeleted))
                        .ThenInclude(c => c.Category);
                }
                if (expand.Contains("userprofile.regions"))
                {
                    usersQueryable = usersQueryable.Include(p => p.UserProfile)
                         .ThenInclude(c => c.Regions.Where(s => !s.Region.IsDeleted))
                         .ThenInclude(c => c.Region);
                }
                if (expand.Contains("timezone"))
                {
                    usersQueryable = usersQueryable.Include(u => u.TimeZone);
                }
                if (expand.Contains("userprofile.skills"))
                {
                    usersQueryable = usersQueryable.Include(p => p.UserProfile).ThenInclude(s => s.SkillsByCategory).ThenInclude(z => z.Skills).Include(p => p.UserProfile).ThenInclude(z => z.SkillsByCategory).ThenInclude(y => y.Categories);
                }
            }

            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();
                if (sort.Contains("company"))
                {
                    if (sort.Contains("company.asc"))
                    {
                        return usersQueryable.OrderBy(o => o.Company.Name).ThenBy(p => p.LastName);
                    }
                    if (sort.Contains("company.desc"))
                    {
                        return usersQueryable.OrderByDescending(o => o.Company.Name).ThenBy(p => p.LastName);
                    }
                }
                if (sort.Contains("statusname"))
                {
                    if (sort.Contains("statusname.asc"))
                    {
                        return usersQueryable.OrderBy(o => o.Status.Name).ThenBy(p => p.LastName);
                    }
                    if (sort.Contains("statusname.desc"))
                    {
                        return usersQueryable.OrderByDescending(o => o.Status.Name).ThenBy(p => p.LastName);
                    }
                }
                if (sort.Contains("lastname"))
                {
                    if (sort.Contains("lastname.asc"))
                    {
                        return usersQueryable.OrderBy(o => o.LastName);
                    }
                    if (sort.Contains("lastname.desc"))
                    {
                        return usersQueryable.OrderByDescending(o => o.LastName);
                    }
                }
                if (sort.Contains("email"))
                {
                    if (sort.Contains("email.asc"))
                    {
                        return usersQueryable.OrderBy(o => o.Email);
                    }
                    if (sort.Contains("email.desc"))
                    {
                        return usersQueryable.OrderByDescending(o => o.Email);
                    }
                }
                if (sort.Contains("title"))
                {
                    if (sort.Contains("title.asc"))
                    {
                        return usersQueryable.OrderBy(o => o.UserProfile.JobTitle);
                    }
                    if (sort.Contains("title.desc"))
                    {
                        return usersQueryable.OrderByDescending(o => o.UserProfile.JobTitle);
                    }
                }
            }
            return usersQueryable;
        }

        private IQueryable<User> FilterSearchUsers(IQueryable<User> usersQueryable, string? search, string? filter, bool accessPrivateInfo = false)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                foreach (string property in filter.Split("&").ToList())
                {
                    var ids = ParseFilterByField(property);
                    if (ids != null && ids.Count > 0)
                    {
                        if (property.Contains("roleids"))
                        {
                            if (ids.Contains((int)RoleType.SolutionProvider) && !ids.Contains((int)RoleType.SPAdmin))
                            {
                                ids.Add((int)RoleType.SPAdmin);
                            }
                            usersQueryable =
                                usersQueryable.Where(x => x.Roles.Select(r => r.RoleId).Any(roleId => ids.Contains(roleId)));
                        }
                        if (property.Contains("statusids"))
                        {
                            usersQueryable = usersQueryable.Where(x => ids.Contains((int)x.StatusId));
                        }
                        if (property.Contains("regionids"))
                        {
                            usersQueryable =
                                usersQueryable.Where(x => x.UserProfile.Regions.Select(r => r.RegionId).Any(rId => ids.Contains(rId)));
                        }

                        if (property.Contains("categoryids"))
                        {
                            if (property.StartsWith("companycategoryids"))
                            {
                                usersQueryable =
                                    usersQueryable.Where(x => x.Company.Categories.Select(r => r.CategoryId).Any(rId => ids.Contains(rId)));
                            }
                            else
                            {
                                usersQueryable =
                                    usersQueryable.Where(x => x.UserProfile.Categories.Select(r => r.CategoryId).Any(rId => ids.Contains(rId)));
                            }
                        }

                        if (property.Contains("permissionids") && accessPrivateInfo)
                        {
                            usersQueryable =
                                usersQueryable.Where(x => x.Permissions.Select(p => (int)p.PermissionId).Any(pId => ids.Contains(pId))
                                    || x.Roles.SelectMany(r => r.Role.Permissions).Select(p => (int)p.PermissionId).Any(pId => ids.Contains(pId)));
                        }

                        if (property.Contains("companyids"))
                        {
                            usersQueryable =
                                usersQueryable.Where(x => ids.Contains(x.CompanyId));
                        }

                        if (property.Contains("companytypesids"))
                        {
                            usersQueryable =
                                usersQueryable.Where(x => ids.Contains((int)x.Company.TypeId));
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                var NormalizedText = RemoveDiacritics(search.ToLower());
                usersQueryable = SearchUser(usersQueryable, NormalizedText);
            }
            return usersQueryable;
        }


        #region Usersearch
        public IQueryable<User> SearchUser(IQueryable<User> users, string searchWord)
        {
            List<User> matchingUsers = new List<User>();
            foreach (var item in users)
            {

                int SortOrder = !string.IsNullOrEmpty(item.FirstName) && RemoveDiacritics(item.FirstName.ToLower()).StartsWith(searchWord) ? 1 :
                                 !string.IsNullOrEmpty(item.LastName) && RemoveDiacritics(item.LastName.ToLower()).StartsWith(searchWord) ? 2 : RemoveDiacritics(item.FirstName.ToLower() + " " + item.LastName.ToLower()).Contains(searchWord) ? 3 :
                                 (item.Company != null) && !string.IsNullOrEmpty(item.Company.Name) && RemoveDiacritics(item.Company.Name.ToLower()).Contains(searchWord) ? 4 :
                                  !string.IsNullOrEmpty(item.Email) && RemoveDiacritics(item.Email.ToLower()).Contains(searchWord) ? 5 : 0;
                item.SearchedBy = SortOrder;
                if (SortOrder != 0)
                {
                    matchingUsers.Add(item);
                }

            }
            matchingUsers = matchingUsers.OrderBy(o => o.SearchedBy).ToList();
            return matchingUsers.AsQueryable();
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string RemoveDiacritics(string? text)
        {
            if (text != null)
            {
                var normalizedString = text.Normalize(NormalizationForm.FormD);
                var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

                for (int i = 0; i < normalizedString.Length; i++)
                {
                    char c = normalizedString[i];
                    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(c);
                    }
                }

                return stringBuilder
                    .ToString()
                    .Normalize(NormalizationForm.FormC);
            }
            else return null;
        }
    }
}