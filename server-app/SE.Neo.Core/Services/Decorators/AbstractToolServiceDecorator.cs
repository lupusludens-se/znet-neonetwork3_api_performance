using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.Tool;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services.Decorators
{
    public abstract class AbstractToolServiceDecorator : IToolService
    {
        protected readonly IToolService _toolService;

        public AbstractToolServiceDecorator(IToolService toolService)
        {
            _toolService = toolService;
        }

        public virtual async Task<int> CreateSolarQuoteAsync(SolarQuoteDTO modelDTO)
        {
            return await _toolService.CreateSolarQuoteAsync(modelDTO);
        }

        public virtual async Task<int> CreateUpdateToolAsync(int id, ToolDTO modelDTO)
        {
            return await _toolService.CreateUpdateToolAsync(id, modelDTO);
        }

        public virtual async Task<ToolDTO> GetToolAsync(int id, string? expand, int? userId, bool isToolViewAll)
        {
            return await _toolService.GetToolAsync(id, expand, userId, isToolViewAll);
        }

        public virtual async Task<WrapperModel<ToolDTO>> GetToolsAsync(BaseSearchFilterModel filter, int userId, bool isToolViewAll)
        {
            return await _toolService.GetToolsAsync(filter, userId, isToolViewAll);
        }

        public virtual async Task<IEnumerable<ToolDTO>> GetPinnedToolsAsync(BaseSearchFilterModel filter, int userId)
        {
            return await _toolService.GetPinnedToolsAsync(filter, userId);
        }

        public virtual async Task CreatePinnedToolsAsync(int userId, IList<ToolPinnedDTO> model)
        {
            await _toolService.CreatePinnedToolsAsync(userId, model);
        }

        public bool IsToolExist(int id, bool includeInactive = false)
        {
            return _toolService.IsToolExist(id, includeInactive);
        }

        public virtual bool IsToolTitleUnique(string title, int toolId = 0)
        {
            return _toolService.IsToolTitleUnique(title, toolId);
        }

        public virtual async Task PatchToolAsync(int id, JsonPatchDocument patchDoc)
        {
            await _toolService.PatchToolAsync(id, patchDoc);
        }

        public virtual async Task RemoveToolAsync(int id)
        {
            await _toolService.RemoveToolAsync(id);
        }
    }
}