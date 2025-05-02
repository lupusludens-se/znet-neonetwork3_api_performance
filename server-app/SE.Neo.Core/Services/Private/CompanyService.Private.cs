using Microsoft.EntityFrameworkCore;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Services
{
    public partial class CompanyService
    {
        private IQueryable<Company> ExpandSortCompanies(IQueryable<Company> companiesQueryable, string? expand, string? sort = null)
        {
            companiesQueryable = companiesQueryable.Include(c => c.Industry);
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("projects"))
                {
                    companiesQueryable = companiesQueryable.Include(c => c.Projects);
                }
                if (expand.Contains("users"))
                {
                    companiesQueryable = companiesQueryable.Include(c => c.Users);
                }
                if (expand.Contains("image"))
                {
                    companiesQueryable = companiesQueryable.Include(p => p.Image);
                }
                if (expand.Contains("categories"))
                {
                    companiesQueryable = companiesQueryable
                        .Include(p => p.Categories.Where(s => !s.Category.IsDeleted))
                        .ThenInclude(c => c.Category);
                }
                if (expand.Contains("urllinks"))
                {
                    companiesQueryable = companiesQueryable.Include(p => p.UrlLinks);
                }
                if (expand.Contains("offsiteppas"))
                {
                    companiesQueryable = companiesQueryable.Include(p => p.OffsitePPAs).ThenInclude(c => c.OffsitePPA);
                }
                if (expand.Contains("country"))
                {
                    companiesQueryable = companiesQueryable.Include(p => p.Country);
                }
            }
            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();
                if (sort.Contains("name"))
                {
                    if (sort.Contains("name.asc"))
                    {
                        return companiesQueryable.OrderBy(o => o.Name);
                    }
                    if (sort.Contains("name.desc"))
                    {
                        return companiesQueryable.OrderByDescending(o => o.Name);
                    }
                }
                if (sort.Contains("type"))
                {
                    if (sort.Contains("type.asc"))
                    {
                        return companiesQueryable.OrderBy(o => o.TypeId).ThenBy(p => p.Name);
                    }
                    if (sort.Contains("type.desc"))
                    {
                        return companiesQueryable.OrderByDescending(o => o.TypeId).ThenBy(p => p.Name);
                    }
                }
                if (sort.Contains("status"))
                {
                    if (sort.Contains("status.asc"))
                    {
                        return companiesQueryable.OrderBy(o => o.Status.Name).ThenBy(p => p.Name);
                    }
                    if (sort.Contains("status.desc"))
                    {
                        return companiesQueryable.OrderByDescending(o => o.Status.Name).ThenBy(p => p.Name);
                    }
                }
            }
            return companiesQueryable;
        }

        private IQueryable<Company> FilterSearchCompanies(IQueryable<Company> companiesQueryable, string? search, string? filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                foreach (string property in filter.Split("&").ToList())
                {
                    var ids = ParseFilterByField(property);
                    if (ids != null && ids.Count > 0)
                    {
                        if (property.Contains("typeids"))
                        {
                            companiesQueryable = companiesQueryable.Where(x => ids.Contains((int)x.TypeId));
                        }

                        if (property.Contains("statusids"))
                        {
                            companiesQueryable = companiesQueryable.Where(x => ids.Contains((int)x.StatusId));
                        }

                        if (property.Contains("categoryids"))
                        {
                            companiesQueryable =
                                companiesQueryable.Where(x => x.Categories.Select(r => r.CategoryId).Any(rId => ids.Contains(rId)));
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                companiesQueryable = companiesQueryable.Where(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(search));
            }
            return companiesQueryable;
        }

        private void UpdateDependentEntitiesByCompany(Company company)
        {
            if (company.StatusId != Enums.CompanyStatus.Active)
            {
                bool isDeletedCompanyStatus = company.StatusId == Enums.CompanyStatus.Deleted;
                Enums.UserStatus usersNewStatus =
                   isDeletedCompanyStatus ? Enums.UserStatus.Deleted : Enums.UserStatus.Inactive;
                Enums.ProjectStatus projectsNewStatus =
                   isDeletedCompanyStatus ? Enums.ProjectStatus.Deleted : Enums.ProjectStatus.Inactive;

                if (!string.IsNullOrEmpty(company.MDMKey) && isDeletedCompanyStatus)
                {
                    var newMDMKEyValue = "X" + company.MDMKey[1..];
                    company.MDMKey = newMDMKEyValue;
                }

                _context.Users.Where(s => s.CompanyId.Equals(company.Id) && s.StatusId != usersNewStatus).ToList().
                        ForEach(e => e.StatusId = usersNewStatus);
                _context.Projects.Where(s => s.CompanyId.Equals(company.Id) && s.StatusId != projectsNewStatus).ToList().
                    ForEach(e => e.StatusId = projectsNewStatus);
            }
        }

        private IQueryable<CompanyFile> ExpandSortFiles(IQueryable<CompanyFile> companyFilesQuery, string? expand, string? sort)
        {
            companyFilesQuery = companyFilesQuery.Include(i => i.File);

            var query = from cf in companyFilesQuery
                        join u in _context.Users on cf.File.UpdatedByUserId equals u.Id
                        select new { CompanyFile = cf, User = u };

            if (!string.IsNullOrEmpty(sort))
            {
                sort = sort.ToLower();
                if (sort.Contains("name"))
                {
                    if (sort.Contains("name.asc"))
                    {
                        return query.OrderBy(o => o.CompanyFile.File.ActualFileTitle).Select(o => o.CompanyFile);
                    }
                    else if (sort.Contains("name.desc"))
                    {
                        return query.OrderByDescending(o => o.CompanyFile.File.ActualFileTitle).Select(o => o.CompanyFile);
                    }
                }
                else if (sort.Contains("size"))
                {
                    if (sort.Contains("size.asc"))
                    {
                        return query.OrderBy(o => o.CompanyFile.File.Size).Select(o => o.CompanyFile);
                    }
                    else if (sort.Contains("size.desc"))
                    {
                        return query.OrderByDescending(o => o.CompanyFile.File.Size).Select(o => o.CompanyFile);
                    }
                }
                else if (sort.Contains("type"))
                {
                    if (sort.Contains("type.asc"))
                    {
                        return query.OrderBy(o => o.CompanyFile.File.Type).Select(o => o.CompanyFile);
                    }
                    else if (sort.Contains("type.desc"))
                    {
                        return query.OrderByDescending(o => o.CompanyFile.File.Type).Select(o => o.CompanyFile);
                    }
                }
                else if (sort.Contains("date"))
                {
                    if (sort.Contains("date.asc"))
                    {
                        return query.OrderBy(o => o.CompanyFile.File.ModifiedOn).Select(o => o.CompanyFile);
                    }
                    else if (sort.Contains("date.desc"))
                    {
                        return query.OrderByDescending(o => o.CompanyFile.File.ModifiedOn).Select(o => o.CompanyFile);
                    }
                }
                else if (sort.Contains("modifiedby"))
                {
                    if (sort.Contains("modifiedby.asc"))
                    {
                        return query.OrderBy(o => o.User.FirstName).ThenBy(o => o.User.LastName).Select(o => o.CompanyFile);
                    }
                    else if (sort.Contains("modifiedby.desc"))
                    {
                        return query.OrderByDescending(o => o.User.FirstName).ThenByDescending(o => o.User.LastName).Select(o => o.CompanyFile);
                    }
                }
            }
            else
            {
                return query.OrderByDescending(x => x.CompanyFile.File.ModifiedOn).Select(x => x.CompanyFile);
            }

            return query.Select(x => x.CompanyFile);
        }

    }
}