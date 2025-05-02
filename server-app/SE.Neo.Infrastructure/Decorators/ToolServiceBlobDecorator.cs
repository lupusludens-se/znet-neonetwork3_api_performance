using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.Tool;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class ToolServiceBlobDecorator : AbstractToolServiceDecorator
    {
        private readonly IBlobServicesFacade _blobServicesFacade;

        public ToolServiceBlobDecorator(
            IBlobServicesFacade blobServicesFacade,
            IToolService toolService) : base(toolService)
        {
            _blobServicesFacade = blobServicesFacade;
        }

        public override async Task<WrapperModel<ToolDTO>> GetToolsAsync(BaseSearchFilterModel filter, int userId, bool isToolViewAll)
        {
            WrapperModel<ToolDTO> toolsResult = await base.GetToolsAsync(filter, userId, isToolViewAll);

            List<ToolDTO> toolDTOs = toolsResult.DataList.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(toolDTOs, dto => dto.Icon, (dto, b) => dto.Icon = b);
            toolsResult.DataList = toolDTOs;

            return toolsResult;
        }

        public override async Task<ToolDTO> GetToolAsync(int id, string? expand, int? userId, bool isToolViewAll)
        {
            ToolDTO? toolDTO = await base.GetToolAsync(id, expand, userId, isToolViewAll);

            await _blobServicesFacade.PopulateWithBlobAsync(toolDTO, dto => dto?.Icon, (dto, b) => { if (dto?.Icon != null) dto.Icon = b; });

            return toolDTO;
        }

        public override async Task<int> CreateUpdateToolAsync(int id, ToolDTO modelDTO)
        {
            bool isUpdate = id > 0;
            string? oldToolIconName = isUpdate ? (await base.GetToolAsync(id, null, null, true))?.IconName : null;
            id = await _toolService.CreateUpdateToolAsync(id, modelDTO);
            if (isUpdate)
                if (!string.IsNullOrEmpty(oldToolIconName) && oldToolIconName != modelDTO.IconName)
                    await _blobServicesFacade.DeleteBlobAsync(new BlobBaseDTO() { Name = oldToolIconName, ContainerName = BlobType.Tools.ToString() });
            return id;
        }

        public override async Task<IEnumerable<ToolDTO>> GetPinnedToolsAsync(BaseSearchFilterModel filter, int userId)
        {
            IEnumerable<ToolDTO> toolsResult = await base.GetPinnedToolsAsync(filter, userId);

            List<ToolDTO> toolDTOsResult = toolsResult.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(toolDTOsResult, dto => dto.Icon, (dto, b) => dto.Icon = b);

            return toolDTOsResult;
        }
    }
}