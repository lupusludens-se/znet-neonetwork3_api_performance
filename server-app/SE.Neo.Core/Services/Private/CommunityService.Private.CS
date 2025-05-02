using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SE.Neo.Common.Enums;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Models.Community;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using UserStatus = SE.Neo.Core.Enums.UserStatus;

namespace SE.Neo.Core.Services;


public partial class CommunityService
{
    private IQueryable<CommunitySearchResult> CreateCommunityQuery(CommunityCreateQueryModel model)
    {
        if (model.Filter != null)
        {
            model.Type = MatchFilterToType(model.Filter, model.Type);
        }

        IQueryable<int> followedUserIdQuery = _context.UserFollowers
            .AsNoTracking()
            .Where(uf => uf.FollowerId == model.UserId)
            .Select(uf => uf.FollowedId);
        IQueryable<int> followedCompanyIdQuery = _context.CompanyFollowers
            .AsNoTracking()
            .Where(cf => cf.FollowerId == model.UserId)
            .Select(cf => cf.CompanyId);
        IQueryable<User> followedUserQuery = _context.Users
            .AsNoTracking()
            .Where(u => u.StatusId == UserStatus.Active)
            .Where(u => followedUserIdQuery.Any(id => id == u.Id));
        IQueryable<Company> followedCompanyQuery = _context.Companies
            .AsNoTracking()
            .Include(c => c.Users).Where(c => c.StatusId == Enums.CompanyStatus.Active)
            .Where(c => followedCompanyIdQuery.Any(id => id == c.Id));
        followedUserQuery = FilterSearchCommunityUsers(followedUserQuery, model.Search, model.Filter);
        followedCompanyQuery = FilterSearchCommunityCompanies(followedCompanyQuery, model.Search, model.Filter);
        IQueryable<CommunitySearchResult> followedUserItemQuery = followedUserQuery.Select(u => new CommunitySearchResult
        {
            Id = u.Id,
            Sort = Convert.ToString(u.LastName + " " + u.FirstName),
            Type = CommunityItemType.User,
            IsFollowed = true,
            Order = string.IsNullOrEmpty(model.Search) ? 0 : u.SearchedBy,
            StartsOrContains = string.IsNullOrEmpty(model.Search) ? 0 : u.StartsOrContains
        });

        IQueryable<CommunitySearchResult> followedCompanyItemQuery = followedCompanyQuery.Select(c => new CommunitySearchResult
        {
            Id = c.Id,
            Sort = Convert.ToString(c.Name),
            Type = CommunityItemType.Company,
            IsFollowed = true,
            Order = string.IsNullOrEmpty(model.Search) ? 0 : c.SearchedBy,
            StartsOrContains = string.IsNullOrEmpty(model.Search) ? 0 : c.StartsOrContains
        });

        IQueryable<CommunitySearchResult>? notFollowedUserItemQuery = null;
        IQueryable<CommunitySearchResult>? notFollowedCompanyItemQuery = null;
        if (model.OnlyFollowed != true)
        {
            IQueryable<User> notFollowedUserQuery = _context.Users
                .AsNoTracking()
                .Where(u => u.StatusId == UserStatus.Active)
                .Where(u => u.Id != model.UserId)
                .Where(u => !followedUserIdQuery.Any(id => id == u.Id));

            IQueryable<Company> notFollowedCompanyQuery = _context.Companies
                .AsNoTracking()
                .Where(c => c.StatusId == Enums.CompanyStatus.Active)
                .Where(c => !followedCompanyIdQuery.Any(id => id == c.Id));
            notFollowedUserQuery = FilterSearchCommunityUsers(notFollowedUserQuery, model.Search, model.Filter);
            notFollowedCompanyQuery = FilterSearchCommunityCompanies(notFollowedCompanyQuery, model.Search, model.Filter);
            notFollowedUserItemQuery = notFollowedUserQuery.Select(u => new CommunitySearchResult
            {
                Id = u.Id,
                Sort = Convert.ToString(u.LastName + " " + u.FirstName),
                Type = CommunityItemType.User,
                IsFollowed = false,
                Order = string.IsNullOrEmpty(model.Search) ? 0 : u.SearchedBy,
                StartsOrContains = string.IsNullOrEmpty(model.Search) ? 0 : u.StartsOrContains

            });
            notFollowedCompanyItemQuery = notFollowedCompanyQuery.Select(c => new CommunitySearchResult
            {
                Id = c.Id,
                Sort = Convert.ToString(c.Name),
                Type = CommunityItemType.Company,
                IsFollowed = false,
                Order = string.IsNullOrEmpty(model.Search) ? 0 : c.SearchedBy,
                StartsOrContains = string.IsNullOrEmpty(model.Search) ? 0 : c.StartsOrContains
            });
        }
        return CreateQuery(notFollowedUserItemQuery, followedUserItemQuery, notFollowedCompanyItemQuery, followedCompanyItemQuery, model.Type);
    }

    private IQueryable<CommunitySearchResult> CreateForYouCommunityQuery(CommunityCreateQueryModel model)
    {
        if (model.Filter != null)
        {
            model.Type = MatchFilterToType(model.Filter, model.Type);
        }
        IQueryable<int> followedUserIdQuery = _context.UserFollowers
            .AsNoTracking()
            .Where(uf => uf.FollowerId == model.UserId)
            .Select(uf => uf.FollowedId);
        IQueryable<int> followedCompanyIdQuery = _context.CompanyFollowers
            .AsNoTracking()
            .Where(cf => cf.FollowerId == model.UserId)
            .Select(cf => cf.CompanyId);

        IQueryable<int> userCategoryIdQuery = _context.UserProfileCategories
            .AsNoTracking()
            .Where(u => u.UserProfileId == model.UserId)
            .Select(u => u.CategoryId);

        IQueryable<int> userRegionIdQuery = _context.UserProfileRegions
            .AsNoTracking()
            .Where(u => u.UserProfileId == model.UserId)
            .Select(u => u.RegionId);

        IQueryable<UserProfileCategory> followedForYouUserQuery = _context.UserProfileCategories
            .AsNoTracking()
            .Include(u => u.UserProfile)
                .ThenInclude(u => u.User)
            .Include(u => u.UserProfile)
                .ThenInclude(u => u.Regions)
            .Where(x => x.UserProfile.User.StatusId == UserStatus.Active)
            .Where(x => userCategoryIdQuery.Contains(x.CategoryId))
            .Where(x => x.UserProfile.Regions.Any(r => userRegionIdQuery.Contains(r.RegionId)))
            .Where(x => x.UserProfileId != model.UserId)
            .Where(x => followedUserIdQuery.Any(id => id == x.UserProfileId));

        IQueryable<CompanyCategory> followedForYouCompanyQuery = _context.CompanyCategories
            .AsNoTracking()
            .Include(c => c.Company)
            .Where(c => c.Company.StatusId == Enums.CompanyStatus.Active)
            .Where(x => userCategoryIdQuery.Contains(x.CategoryId))
            .Where(cc => followedCompanyIdQuery.Any(id => id == cc.CompanyId));

        followedForYouUserQuery = FilterSearchForYouUsers(followedForYouUserQuery, model.Search, model.Filter);
        followedForYouCompanyQuery = FilterSearchForYouCompanies(followedForYouCompanyQuery, model.Search, model.Filter);
        IQueryable<CommunitySearchResult> followedUserItemQuery = followedForYouUserQuery
            .GroupBy(u => u.UserProfileId)
            .Select(g => new CommunitySearchResult
            {
                Id = g.Key,
                Sort = Convert.ToString(g.First().UserProfile.User.LastName + " " + g.First().UserProfile.User.FirstName),
                Type = CommunityItemType.User,
                IsFollowed = true
            });
        IQueryable<CommunitySearchResult> followedCompanyItemQuery = followedForYouCompanyQuery
            .GroupBy(c => c.CompanyId)
            .Select(g => new CommunitySearchResult
            {
                Id = g.Key,
                Sort = Convert.ToString(g.First().Company.Name),
                Type = CommunityItemType.Company,
                IsFollowed = true
            });
        IQueryable<CommunitySearchResult>? notFollowedUserItemQuery = null;
        IQueryable<CommunitySearchResult>? notFollowedCompanyItemQuery = null;

        if (model.OnlyFollowed != true)
        {
            IQueryable<UserProfileCategory> notFollowedForYouUserQuery = _context.UserProfileCategories
                .AsNoTracking()
                .Include(u => u.UserProfile)
                    .ThenInclude(up => up.User)
                .Include(u => u.UserProfile)
                    .ThenInclude(u => u.Regions)
                .Where(x => x.UserProfile.User.StatusId == UserStatus.Active)
                .Where(x => userCategoryIdQuery.Contains(x.CategoryId))
                .Where(x => x.UserProfile.Regions.Any(r => userRegionIdQuery.Contains(r.RegionId)))
                .Where(x => x.UserProfileId != model.UserId)
                .Where(x => followedUserIdQuery.Any(id => id == x.UserProfileId) == false);


            IQueryable<CompanyCategory> notFollowedForYouCompanyQuery = _context.CompanyCategories
                .AsNoTracking()
                .Include(cc => cc.Company)
                .Where(c => c.Company.StatusId == Enums.CompanyStatus.Active)
                .Where(x => userCategoryIdQuery.Contains(x.CategoryId))
                .Where(cc => followedCompanyIdQuery.Any(id => id == cc.CompanyId) == false);


            notFollowedForYouUserQuery = FilterSearchForYouUsers(notFollowedForYouUserQuery, model.Search, model.Filter);

            notFollowedForYouCompanyQuery = FilterSearchForYouCompanies(notFollowedForYouCompanyQuery, model.Search, model.Filter);

            notFollowedUserItemQuery = notFollowedForYouUserQuery
                .GroupBy(u => u.UserProfileId)
                .Select(g => new CommunitySearchResult
                {
                    Id = g.Key,
                    Sort = Convert.ToString(g.First().UserProfile.User.LastName + " " + g.First().UserProfile.User.FirstName),
                    Type = CommunityItemType.User,
                    IsFollowed = false
                });
            notFollowedCompanyItemQuery = notFollowedForYouCompanyQuery
                .GroupBy(c => c.CompanyId)
                .Select(g => new CommunitySearchResult
                {
                    Id = g.Key,
                    Sort = Convert.ToString(g.First().Company.Name),
                    Type = CommunityItemType.Company,
                    IsFollowed = false
                });
        }
        return CreateQuery(notFollowedUserItemQuery, followedUserItemQuery, notFollowedCompanyItemQuery, followedCompanyItemQuery, model.Type);
    }

    private IQueryable<CommunitySearchResult> CreateQuery(IQueryable<CommunitySearchResult>? notFollowedUserItemQuery,
        IQueryable<CommunitySearchResult> followedUserItemQuery, IQueryable<CommunitySearchResult>? notFollowedCompanyItemQuery,
        IQueryable<CommunitySearchResult> followedCompanyItemQuery, CommunityItemType? type)
    {
        IQueryable<CommunitySearchResult> userItemQuery = notFollowedUserItemQuery?.Union(followedUserItemQuery) ?? followedUserItemQuery;
        IQueryable<CommunitySearchResult> companyItemQuery = notFollowedCompanyItemQuery?.Union(followedCompanyItemQuery) ?? followedCompanyItemQuery;

        if (type == null)
            return userItemQuery.Union(companyItemQuery);
        return type switch
        {
            CommunityItemType.User => userItemQuery,
            CommunityItemType.Company => companyItemQuery,
            _ => throw new NotSupportedException()
        };
    }

    private IQueryable<User> FilterSearchCommunityUsers(IQueryable<User> userQuery, string? search, string? filter)
    {
        if (!string.IsNullOrEmpty(filter))
        {
            foreach (string property in filter.Split("&").ToList())
            {
                var ids = ParseFilterByField(property);
                if (ids != null && ids.Count > 0)
                {
                    if (property.Contains("regionids"))
                    {
                        List<int> regionIds = _commonService.ExpandRegionListForFiltration(ids, true, true);
                        userQuery = userQuery
                            .Include(u => u.UserProfile)
                                .ThenInclude(up => up.Regions)
                            .Where(u => u.UserProfile.Regions.Select(r => r.RegionId).Any(r => regionIds.Contains(r)));
                    }
                    if (property.Contains("categoryids"))
                    {
                        userQuery = userQuery.Where(u => u.UserProfile.Categories.Select(r => r.CategoryId).Any(r => ids.Contains(r)));
                    }
                    if (property.Contains("roletypes"))
                    {
                        userQuery = userQuery.Where(x => x.Roles.Any(y => ids.Contains(y.RoleId)));
                    }
                }
            }
        }
        if (!string.IsNullOrEmpty(search))
        {
            userQuery = userQuery.Include(x => x.Company).Include(x => x.Country).Include(x => x.UserProfile).ThenInclude(x => x.State).Include(x => x.UserProfile).
                                            ThenInclude(y => y.Categories).ThenInclude(z => z.Category);
            var searchParameters = Enum.GetNames(typeof(CommunityUserSearchyByFieldsEnum)).AsEnumerable();
            var users = GetSearchedCommunityData(userQuery, search, searchParameters, "user");
            userQuery = _mapper.Map<List<User>>(users).AsQueryable();

        }

        return userQuery;
    }

    private IQueryable<Company> FilterSearchCommunityCompanies(IQueryable<Company> companyQuery, string? search, string? filter)
    {
        if (!string.IsNullOrEmpty(filter))
        {
            foreach (string property in filter.Split("&").ToList())
            {
                var ids = ParseFilterByField(property);
                if (ids != null && ids.Count > 0)
                {
                    if (property.Contains("projectcapabilities"))
                    {
                        companyQuery = companyQuery.Where(c => c.Categories.Select(r => r.CategoryId).Any(r => ids.Contains(r)));
                    }
                    if (property.Contains("industryids"))
                    {
                        companyQuery = companyQuery
                            .Where(c => ids.Contains(c.IndustryId));
                    }
                    if (property.Contains("companytype"))
                    {
                        companyQuery = companyQuery
                            .Where(c => ids.Contains((int)c.TypeId));
                    }
                }
            }

        }
        if (!string.IsNullOrEmpty(search))
        {
            companyQuery = companyQuery.Include(x => x.Industry).Include(x => x.Categories).ThenInclude(y => y.Category);
            var searchParameters = Enum.GetNames(typeof(CommunityCompanySearchyByFieldsEnum)).AsEnumerable();
            var company = GetSearchedCommunityData(companyQuery, search, searchParameters, "company");
            companyQuery = _mapper.Map<List<Company>>(company).AsQueryable();
        }

        return companyQuery;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="companyQuery"></param>
    /// <param name="search"></param>
    /// <param name="filter"></param>
    /// <returns></returns>

    private IQueryable<CompanyCategory> FilterSearchForYouCompanies(IQueryable<CompanyCategory> companyQuery, string? search, string? filter)
    {
        if (!string.IsNullOrEmpty(search))
        {
            companyQuery = companyQuery.Where(c => c.Company.Name.Contains(search));
        }

        if (!string.IsNullOrEmpty(filter))
        {
            foreach (string property in filter.Split("&").ToList())
            {
                var ids = ParseFilterByField(property);
                if (ids != null && ids.Count > 0)
                {
                    if (property.Contains("industry"))
                    {
                        companyQuery = companyQuery
                            .Where(cc => ids.Contains(cc.Company.IndustryId));
                    }
                    if (property.Contains("companytype"))
                    {
                        companyQuery = companyQuery
                            .Where(cc => ids.Contains((int)cc.Company.TypeId));
                    }
                }
            }
        }
        return companyQuery;
    }

    private IQueryable<UserProfileCategory> FilterSearchForYouUsers(IQueryable<UserProfileCategory> userQuery, string? search, string? filter)
    {
        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            userQuery = userQuery.Where(upc => !string.IsNullOrEmpty(upc.UserProfile.User.FirstName) && upc.UserProfile.User.FirstName.StartsWith(search) ||
            (!string.IsNullOrEmpty(upc.UserProfile.User.LastName) && upc.UserProfile.User.LastName.StartsWith(search)) ||
            (upc.UserProfile.User.FirstName + " " + upc.UserProfile.User.LastName).StartsWith(search));
        }

        if (!string.IsNullOrEmpty(filter))
        {
            foreach (string property in filter.Split("&").ToList())
            {
                var ids = ParseFilterByField(property);
                if (ids != null && ids.Count > 0)
                {
                    if (property.Contains("region"))
                    {
                        userQuery = userQuery
                            .Include(u => u.UserProfile)
                                .ThenInclude(up => up.Regions)
                            .Where(u => u.UserProfile.Regions.Select(upr => upr.RegionId).Any(r => ids.Contains(r)));
                    }
                }
            }
        }
        return userQuery;
    }

    private CommunityItemType? MatchFilterToType(string filter, CommunityItemType? type)
    {
        if (filter.Contains("region"))
        {
            type = CommunityItemType.User;
            if (filter.Contains("industry"))
            {
                type = null;
            }
        }
        else if (filter.Contains("industry"))
        {
            type = CommunityItemType.Company;
        }
        return type;
    }
    #region Search by Text Community
    /// <summary>
    /// Dynamic Function to retieve data from community based on searched word. Each item in community data is serialized to  Json obect to convert to Dictionary
    /// Conversion to dictionary enables getting column name and comparing value against search term.
    /// The parameters that contain search term is obtained and is order by hierarchy, followed by starts with search term followed by contains search term
    /// </summary>
    /// <param name="CommunityData"></param>
    /// <param name="searchTerm"></param>
    /// <param name="searchByFields"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<dynamic> GetSearchedCommunityData(IQueryable<dynamic> CommunityData, string searchTerm, IEnumerable<string> searchByFields, string type)
    {
        List<dynamic> totalResults = new List<dynamic>();

        JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };
        try
        {
            Parallel.ForEach(CommunityData, (Action<dynamic>)(Data =>
            {
                IDictionary<string, object> result = ToDictionary(JObject.Parse(JsonConvert.SerializeObject(Data, _jsonSettings)));
                if (result != null && Data is User user)
                {
                    result.Add("FullName", user.FirstName + " " + user.LastName);

                }
                Dictionary<int, int> containsSearchTerm = CheckIfCommunityContainsSearch(result, searchByFields, searchTerm ?? searchTerm, type);
                //Dictionary key contains the search by Field and value contains if the field starts with or contains search term.
                containsSearchTerm = containsSearchTerm.OrderBy(x => x.Key).ThenBy(x => x.Value).ToDictionary(k => k.Key, v => v.Value);
                //First match, which is closest to search term is taken after ordering te items.
                if (containsSearchTerm.Count > 0)
                {
                    Data.SearchedBy = (containsSearchTerm.Keys.FirstOrDefault());
                    Data.StartsOrContains = (containsSearchTerm.Values.FirstOrDefault());
                    totalResults.Add(Data);
                }

            }));
        }
        catch (Exception ex)
        {
        }
        return totalResults;
    }
    /// <summary>
    /// Iteration is done on the data w.r.t properties specified in Enum. Here the field containin search term is retieved along with the position(starts with or contains)
    /// </summary>
    /// <param name="community"></param>
    /// <param name="searchProperties"></param>
    /// <param name="searchTerm"></param>
    /// /// <param name="type"></param>
    /// <returns>Fields containing search term and position(starts with or contains)of the search term</returns>
    public Dictionary<int, int> CheckIfCommunityContainsSearch(IDictionary<string, object> community, IEnumerable<string> searchProperties, string searchTerm, string type)
    {

        int startsorContains = 0;
        Dictionary<int, int> results = new Dictionary<int, int>();
        Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
        string NormalizedFormofSearchText = RemoveDiacritics(searchTerm);

        foreach (string key in community.Keys)
        {
            if (searchProperties.Contains(key))
            {
                object nextLevel = community[key];
                if (nextLevel != null)
                {
                    if (nextLevel is IDictionary)
                    {
                        results = RecursiveDictionaryCall((Dictionary<string, object>)nextLevel, searchProperties, NormalizedFormofSearchText, type + "_" + key);
                        if (results.Count > 0)
                        {
                            keyValuePairs = keyValuePairs.Union(results).ToDictionary(k => k.Key, v => v.Value);
                        }
                        continue;
                    }
                    else if (nextLevel is IList)
                    {

                        string json = JsonConvert.SerializeObject(nextLevel);
                        JArray jsonObject = JArray.Parse(json);
                        for (int i = 0; i < jsonObject.Count; i++)
                        {

                            var isToBeSearched = Convert.ToString(jsonObject[i]["Category"]["Name"]);
                            string text = RemoveDiacritics(Convert.ToString(isToBeSearched)) ?? "";
                            if (NormalizedFormofSearchText is not null)
                            {
                                startsorContains = text.StartsWith(NormalizedFormofSearchText, StringComparison.OrdinalIgnoreCase) ? 1 : text.Contains(NormalizedFormofSearchText, StringComparison.OrdinalIgnoreCase) ? 2 : 0;
                                if (startsorContains != 0)
                                {
                                    string FieldType = $"{type}_Categories";
                                    keyValuePairs.Add(EnumHelper.GetEnumValue<CommunitySearchOrderEnum>(FieldType), startsorContains);
                                    break;
                                }
                            }

                        }

                    }

                    else
                    {
                        string FieldValueNormalization = RemoveDiacritics(Convert.ToString(nextLevel));
                        string FieldType = $"{type}_{key}";
                        keyValuePairs = GetFieldandPosition(NormalizedFormofSearchText, FieldValueNormalization, FieldType, keyValuePairs);
                    }
                }
            }
        }
        return keyValuePairs;
    }
    /// <summary>
    /// Considering Level 2 .Recursive call to search for the word if Dictionary contains dictionary, IList or value
    /// </summary>
    /// <param name="nextLevel"></param>
    /// <param name="searchProperties"></param>
    /// <param name="searchTerm"></param>
    public Dictionary<int, int> RecursiveDictionaryCall(IDictionary<string, object> nextLevel, IEnumerable<string> searchProperties, string NormalizedFormofSearchText, string type)
    {
        int startsorContains = 0;
        Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
        Dictionary<int, int> results = new Dictionary<int, int>();
        foreach (string key in nextLevel.Keys)
        {
            if (searchProperties.Contains(key))
            {
                object nestedLevel = nextLevel[key];
                if (nestedLevel != null)
                {
                    if (nestedLevel is IDictionary)
                    {
                        results = RecursiveDictionaryCall((Dictionary<string, object>)nestedLevel, searchProperties, NormalizedFormofSearchText, type + "_" + key);
                        if (results.Count > 0)
                        {
                            keyValuePairs = keyValuePairs.Union(results).ToDictionary(k => k.Key, v => v.Value);
                        }
                    }

                    else if (nestedLevel is IList)
                    {

                        string json = JsonConvert.SerializeObject(nestedLevel);
                        JArray jsonObject = JArray.Parse(json);
                        for (int i = 0; i < jsonObject.Count; i++)
                        {

                            var isToBeSearched = Convert.ToString(jsonObject[i]["Category"]["Name"]);
                            string text = RemoveDiacritics(Convert.ToString(isToBeSearched)) ?? "";
                            if (NormalizedFormofSearchText is not null)
                            {
                                startsorContains = text.StartsWith(NormalizedFormofSearchText, StringComparison.OrdinalIgnoreCase) ? 1 : text.Contains(NormalizedFormofSearchText, StringComparison.OrdinalIgnoreCase) ? 2 : 0;
                                if (startsorContains != 0)
                                {
                                    string FieldType = $"{type}_Categories";
                                    keyValuePairs.Add(EnumHelper.GetEnumValue<CommunitySearchOrderEnum>(FieldType), startsorContains);
                                    break;
                                }
                            }

                        }

                    }
                    else if ((nestedLevel is not IDictionary) && (nestedLevel is not IList))
                    {
                        if (NormalizedFormofSearchText is not null)
                        {
                            string FieldValueNormalization = RemoveDiacritics(Convert.ToString(nestedLevel));
                            string FieldType = $"{type}_{key}";
                            keyValuePairs = GetFieldandPosition(NormalizedFormofSearchText, FieldValueNormalization, FieldType, keyValuePairs);

                        }

                    }
                }
            }
        }

        return keyValuePairs;
    }
    /// <summary>
    /// Function to ignore special characters(accens|Diacritics) during search
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string RemoveDiacritics(string text)
    {
        if (!string.IsNullOrEmpty(text))
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
        return string.Empty;
    }
    /// <summary>
    /// Function to convert JObject to dictionay.
    /// </summary>
    /// <param name="object"></param>
    /// <returns></returns>
    public IDictionary<string, object> ToDictionary(JObject @object)
    {

        try
        {
            var result = (Dictionary<string, object>?)JObjectExtension.ToDictionary(@object);
            return result;
        }
        catch (Exception ex)
        {
            throw;
        }


    }
    /// <summary>
    /// Function to add the get field and position of text
    /// </summary>
    /// <param name="SearchTextNormalizedForm"></param>
    /// <param name="FieldValueNormalizedForm"></param>
    /// <param name="Field"></param>
    /// <param name="SearchKeyValuePair"></param>
    /// <returns></returns>
    public Dictionary<int, int> GetFieldandPosition(string SearchTextNormalizedForm, string FieldValueNormalizedForm, string Field, Dictionary<int, int> SearchKeyValuePair)
    {
        int startsorContains = 0;
        if (SearchTextNormalizedForm is not null && FieldValueNormalizedForm is not null)
        {
            startsorContains = FieldValueNormalizedForm.StartsWith(SearchTextNormalizedForm, StringComparison.OrdinalIgnoreCase) ? 1 : FieldValueNormalizedForm.Contains(SearchTextNormalizedForm, StringComparison.OrdinalIgnoreCase) ? 2 : 0;
            if (startsorContains != 0)
            {
                SearchKeyValuePair.Add(EnumHelper.GetEnumValue<CommunitySearchOrderEnum>(Field), startsorContains);

            }
        }
        return SearchKeyValuePair;
    }
    #endregion
}
