using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Models.AzureSearch;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Services
{
    public class AzureSearchService : IAzureSearchService
    {
        private readonly SearchClient _searchClient;
        private readonly AzureCognitiveSearchConfig _azureCognitiveSearchConfig;
        private readonly ILogger<AzureSearchService> _logger;

        public AzureSearchService(IOptions<AzureCognitiveSearchConfig> azureCognitiveSearchConfig, ILogger<AzureSearchService> logger)
        {
            _azureCognitiveSearchConfig = azureCognitiveSearchConfig.Value;
            _logger = logger;

            var serviceEndpoint = new Uri($"https://{_azureCognitiveSearchConfig.ServiceName}.search.windows.net/");
            _searchClient = new SearchClient(serviceEndpoint, _azureCognitiveSearchConfig.IndexName,
                new AzureKeyCredential(_azureCognitiveSearchConfig.ApiKey));
        }

        public async Task<SearchOutput> FindAsync(SearchRequest request)
        {
            var options = new SearchOptions()
            {
                Size = request.Size,
                Skip = request.Skip,
                IncludeTotalCount = request.IncludeCount,
                Filter = CreateFilterExpression(request.Filters),
                OrderBy = { request.OrderBy }
            };

            if (request.Facets != null && request.Facets.Any())
            {
                foreach (var facet in request.Facets)
                {
                    options.Facets.Add(facet);
                }
            }

            SearchResults<SearchDocument> response = await _searchClient.SearchAsync<SearchDocument>(request.SearchText, options);

            var facetOutput = new Dictionary<string, IList<FacetValue>>();
            if (response.Facets != null)
            {
                foreach (var facetResult in response.Facets)
                {
                    facetOutput[facetResult.Key] = facetResult.Value
                               .Select(x => new FacetValue() { Value = x.Value.ToString(), Count = x.Count })
                               .ToList();
                }
            }

            var results = response.GetResults().Select(x => Map(x.Document)).ToList();

            return new SearchOutput
            {
                Count = response.TotalCount,
                Results = results,
                Facets = facetOutput
            };
        }

        private static string CreateFilterExpression(List<Models.AzureSearch.SearchFilter>? filters)
        {
            if (filters == null || !filters.Any())
            {
                return string.Empty;
            }

            var filterExpressions = new List<string>();

            filterExpressions.AddRange(filters.Select(x => x.CreateODataFilter()));

            return string.Join(" and ", filterExpressions);
        }

        private static SearchEntity Map(SearchDocument document)
        {
            return new SearchEntity()
            {
                Id = document.ContainsKey("id") ? document.GetString("id") : null,
                OriginalId = document.ContainsKey("originalId") ? document.GetInt32("originalId") : null,
                EntityType = document.ContainsKey("entityType") ? document.GetInt32("entityType") : null,
                Description = document.ContainsKey("description") ? document.GetString("description") : null,
                Subject = document.ContainsKey("subject") ? document.GetString("subject") : null,
                IsDeleted = document.ContainsKey("isDeleted") ? document.GetBoolean("isDeleted") : null,
                UpdatedAt = document.ContainsKey("updatedAt") ? document.GetDateTimeOffset("updatedAt")?.DateTime : null,
                Categories = document.ContainsKey("categories") ? document.GetObjectCollection("categories").Select(x => new SearchEntityCategory
                {
                    CategoryId = x.ContainsKey("categoryId") ? x.GetInt32("categoryId") : null,
                    Name = x.ContainsKey("name") ? x.GetString("name") : null,
                }).ToList() : null,
                Technologies = document.ContainsKey("technologies") ? document.GetObjectCollection("technologies").Select(x => new SearchEntityTechnology
                {
                    TechnologyId = x.ContainsKey("technologyId") ? x.GetInt32("technologyId") : null,
                    Name = x.ContainsKey("name") ? x.GetString("name") : null,
                }).ToList() : null,
                Solutions = document.ContainsKey("solutions") ? document.GetObjectCollection("solutions").Select(x => new SearchEntitySolution
                {
                    SolutionId = x.ContainsKey("solutionId") ? x.GetInt32("solutionId") : null,
                    Name = x.ContainsKey("name") ? x.GetString("name") : null,
                }).ToList() : null,
                Regions = document.ContainsKey("regions") ? document.GetObjectCollection("regions").Select(x => new SearchEntityRegion
                {
                    RegionId = x.ContainsKey("regionId") ? x.GetInt32("regionId") : null,
                    Name = x.ContainsKey("name") ? x.GetString("name") : null,
                }).ToList() : null,
                ContentTags = document.ContainsKey("contentTags") ? document.GetObjectCollection("contentTags").Select(x => new SearchEntityContentTag
                {
                    ContentTagId = x.ContainsKey("contentTagId") ? x.GetInt32("contentTagId") : null,
                    Name = x.ContainsKey("name") ? x.GetString("name") : null
                }).ToList() : null
            };
        }
    }
}
