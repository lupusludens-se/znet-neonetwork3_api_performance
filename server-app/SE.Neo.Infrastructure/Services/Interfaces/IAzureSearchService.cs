using SE.Neo.Infrastructure.Models.AzureSearch;

namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface IAzureSearchService
    {
        Task<SearchOutput> FindAsync(SearchRequest request);
    }
}
