using SE.Neo.WebAPI.Models.Search;
namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface ISearchApiService
    {
        Task<FacetWrapperModel<SearchDocument>> SearchAsync(GlobalSearchFilter filter, int userId);
    }
}
