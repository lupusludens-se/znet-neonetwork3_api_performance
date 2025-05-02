using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Tool;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IToolApiService
    {
        Task<WrapperModel<ToolResponse>> GetToolsAsync(BaseSearchFilterModel filter, int userId, bool isToolViewAll);

        Task<ToolResponse> GetToolAsync(int id, string? expand, int userId, bool isToolViewAll);

        Task<int> CreateUpdateToolAsync(int id, ToolRequest model);

        Task<int> CreateSolarQuoteAsync(SolarQuoteRequest model, int userId);

        Task<int> CreateSolarPortfolioReviewAsync(SolarPortfolioReviewRequest model, int userId);

        Task RemoveToolAsync(int id);

        Task PatchToolAsync(int id, JsonPatchDocument patchDoc);

        Task<IEnumerable<ToolResponse>> GetPinnedToolsAsync(BaseSearchFilterModel filter, int userId);

        Task CreatePinnedToolsAsync(int userId, IList<ToolPinnedRequest> model);
        bool IsToolExist(int id, bool includeInactive = false);
    }
}