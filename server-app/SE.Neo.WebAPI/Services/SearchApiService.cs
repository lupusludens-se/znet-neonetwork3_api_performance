using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Models.AzureSearch;
using SE.Neo.Infrastructure.Services.Interfaces;
using SE.Neo.WebAPI.Models.Search;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class SearchApiService : ISearchApiService
    {
        private readonly IAzureSearchService _azureSearchService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        private const string EntityTypeFacetValue = "entityType";

        public SearchApiService(IAzureSearchService azureSearchService, IMapper mapper, IUserService userService)
        {
            _azureSearchService = azureSearchService;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<FacetWrapperModel<SearchDocument>> SearchAsync(GlobalSearchFilter filter, int userId)
        {
            if ((!filter.TaxonomyType.HasValue || !filter.TaxonomyId.HasValue) && (string.IsNullOrEmpty(filter.Search) || filter.Search.Length < 3))
            {
                return new FacetWrapperModel<SearchDocument>
                {
                    DataList = new List<SearchDocument>(),
                    Count = 0,
                    Counters = new EntityTypeCountersModel()
                };
            }
            UserDTO userDTO = await _userService.GetUserAsync(userId, "userprofile.categories,roles,userprofile.regions");
            // facets return count of results grouped by provided values
            // basically it gives us count of each entity in the result list
            var facets = new List<string>();
            if (filter.IncludeCount)
            {
                facets.Add(EntityTypeFacetValue);
            }

            var searchRequest = new SearchRequest
            {
                SearchText = filter.Search ?? string.Empty,
                Size = filter.Take,
                Skip = filter.Skip,
                IncludeCount = filter.IncludeCount,
                Facets = facets,
                Filters = CreateSeachFilters(filter.TaxonomyType, filter.TaxonomyId, filter.EntityType, userDTO)
            };

            searchRequest.OrderBy = "viewCount desc";

            SearchOutput result = await _azureSearchService.FindAsync(searchRequest);

            EntityTypeCountersModel counters = GetCounters(result);

            return new FacetWrapperModel<SearchDocument>
            {
                DataList = result.Results.Select(_mapper.Map<SearchDocument>),
                Count = (int)(result.Count ?? 0),
                Counters = counters
            };
        }

        private static EntityTypeCountersModel GetCounters(SearchOutput result)
        {
            var counters = new EntityTypeCountersModel();
            if (result.Facets != null && result.Facets.ContainsKey(EntityTypeFacetValue))
            {
                foreach (var facetValue in result.Facets[EntityTypeFacetValue])
                {
                    if (int.TryParse(facetValue.Value, out int type) && Enum.IsDefined(typeof(AzureSearchEntityType), type))
                    {
                        switch ((AzureSearchEntityType)type)
                        {
                            case AzureSearchEntityType.Forum:
                                counters.ForumsCount = facetValue.Count ?? 0;
                                break;
                            case AzureSearchEntityType.Article:
                                counters.ArticlesCount = facetValue.Count ?? 0;
                                break;
                            case AzureSearchEntityType.Project:
                                counters.ProjectsCount = facetValue.Count ?? 0;
                                break;
                            case AzureSearchEntityType.Company:
                                counters.CompaniesCount = facetValue.Count ?? 0;
                                break;
                            case AzureSearchEntityType.Event:
                                counters.EventsCount = facetValue.Count ?? 0;
                                break;
                            case AzureSearchEntityType.Tool:
                                counters.ToolsCount = facetValue.Count ?? 0;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return counters;
        }

        private static List<SearchFilter> CreateSeachFilters(AzureSearchTaxonomyType? type, int? id, AzureSearchEntityType? entityType, UserDTO userDTO)
        {
            var defaultFilters = new List<SearchFilter>
                {
                    new SearchFilter { Field = "isDeleted", Operator = "eq", Value = "false" }
                };

            if (type.HasValue && id.HasValue)
            {
                switch (type.Value)
                {
                    case AzureSearchTaxonomyType.Category:
                        defaultFilters.Add(new SearchFilter { Expression = $"categories/any(category: category/categoryId eq {id.Value})" });
                        break;
                    case AzureSearchTaxonomyType.Solution:
                        defaultFilters.Add(new SearchFilter { Expression = $"solutions/any(solution: solution/solutionId eq {id.Value})" });
                        break;
                    case AzureSearchTaxonomyType.Technology:
                        defaultFilters.Add(new SearchFilter { Expression = $"technologies/any(technology: technology/technologyId eq {id.Value})" });
                        break;
                    case AzureSearchTaxonomyType.Region:
                        defaultFilters.Add(new SearchFilter { Expression = $"regions/any(region: region/regionId eq {id.Value})" });
                        break;
                    case AzureSearchTaxonomyType.ContentTag:
                        defaultFilters.Add(new SearchFilter { Expression = $"contentTags/any(contentTag: contentTag/contentTagId eq {id.Value})" });
                        break;
                    default:
                        break;
                }
            }

            if (entityType.HasValue)
            {
                defaultFilters.Add(new SearchFilter { Field = "entityType", Operator = "eq", Value = entityType.Value.ToString("d") });
            }

            // admin can view all items
            return userDTO.Roles.Any(roleDTO => roleDTO.Id == (int)RoleType.Admin)
                ? defaultFilters
                : AddUserAllowedFilters(userDTO, defaultFilters);
        }

        private static List<SearchFilter> AddUserAllowedFilters(UserDTO userDTO, List<SearchFilter> defaultFilters)
        {
            string userRoleIds = string.Join(',', userDTO.Roles.Select(r => r.Id.ToString()));
            string userRegionIds = string.Join(',', userDTO.UserProfile.Regions.Select(r => r.Id.ToString()));
            string userCategoryIds = string.Join(',', userDTO.UserProfile.Categories.Select(c => c.Id.ToString()));
            var searchExpressions = new List<string>();

            // tool
            searchExpressions.Add($"(entityType eq {(int)AzureSearchEntityType.Tool} and allowedRoles/any(role: search.in(role/allowedRoleId, '{userRoleIds}')))");
            searchExpressions.Add($"(entityType eq {(int)AzureSearchEntityType.Tool} and allowedCompanies/any(company: company/allowedCompanyId eq '{userDTO.CompanyId}'))");

            // event
            var allowedRegionsFilter = string.IsNullOrEmpty(userRegionIds)
                ? "true"
                : $"allowedRegions/any(region: search.in(region/allowedRegionId, '{userRegionIds}'))";
            var allowedCategoriesFilter = string.IsNullOrEmpty(userCategoryIds)
                ? "true"
                : $"allowedCategories/any(category: search.in(category/allowedCategoryId, '{userCategoryIds}'))";
            searchExpressions.Add($"(entityType eq {(int)AzureSearchEntityType.Event} and " +
                $"(" +
                    $"(" +
                        $"allowedRoles/any(role: search.in(role/allowedRoleId, '{userRoleIds}')) and " +
                        $"(not allowedRegions/any() or {allowedRegionsFilter}) and " +
                        $"(not allowedCategories/any() or {allowedCategoriesFilter})" +
                    $") or " +
                    $"allowedUsers/any(user: user/allowedUserId eq '{userDTO.Id}')" +
                $"))");

            // forum
            searchExpressions.Add($"(entityType eq {(int)AzureSearchEntityType.Forum} and isPrivate eq false)");
            searchExpressions.Add($"(entityType eq {(int)AzureSearchEntityType.Forum} and isPrivate eq true and allowedUsers/any(user: user/allowedUserId eq '{userDTO.Id}'))");

            // projects
            if (userDTO.Roles.Select(r => r.Id).Any(id => id == (int)RoleType.SolutionProvider || id == (int)RoleType.SPAdmin) == false)
            {
                searchExpressions.Add($"(entityType eq {(int)AzureSearchEntityType.Project} and UserStatus eq {Convert.ToInt32(UserStatus.Active)})");
            }

            //companies
            searchExpressions.Add($"(entityType eq {(int)AzureSearchEntityType.Company})");

            // articles
            searchExpressions.Add($"(entityType eq {(int)AzureSearchEntityType.Article} and allowedRoles/any(role: search.in(role/allowedRoleId, '{userRoleIds}')))");

            defaultFilters.Add(new SearchFilter { Expression = "(" + string.Join(" or ", searchExpressions) + ")" });
            return defaultFilters;
        }
    }
}