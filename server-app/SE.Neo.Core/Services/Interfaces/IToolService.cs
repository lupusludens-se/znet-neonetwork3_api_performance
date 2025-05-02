using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.Tool;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IToolService
    {
        Task<WrapperModel<ToolDTO>> GetToolsAsync(BaseSearchFilterModel filter, int userId, bool isToolViewAll);

        Task<ToolDTO> GetToolAsync(int id, string? expand, int? userId, bool isToolViewAll);

        Task<int> CreateUpdateToolAsync(int id, ToolDTO modelDTO);

        bool IsToolTitleUnique(string title, int toolId = 0);

        Task<int> CreateSolarQuoteAsync(SolarQuoteDTO modelDTO);

        Task RemoveToolAsync(int id);

        Task PatchToolAsync(int id, JsonPatchDocument patchDoc);

        bool IsToolExist(int id, bool includeInactive = false);

        Task<IEnumerable<ToolDTO>> GetPinnedToolsAsync(BaseSearchFilterModel filter, int userId);

        Task CreatePinnedToolsAsync(int userId, IList<ToolPinnedDTO> model);
    }
}