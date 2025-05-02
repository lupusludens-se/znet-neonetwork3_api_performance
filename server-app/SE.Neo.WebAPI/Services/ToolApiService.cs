using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.Tool;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;
using SE.Neo.WebAPI.Models.Tool;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class ToolApiService : IToolApiService
    {
        private readonly ILogger<ToolApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IToolService _toolService;
        private readonly IEmailService _emailService;

        public ToolApiService(ILogger<ToolApiService> logger, IMapper mapper, IToolService toolService, IEmailService emailService)
        {
            _logger = logger;
            _mapper = mapper;
            _toolService = toolService;
            _emailService = emailService;
        }

        public async Task<WrapperModel<ToolResponse>> GetToolsAsync(BaseSearchFilterModel filter, int userId, bool isToolViewAll)
        {
            WrapperModel<ToolDTO> tools = await _toolService.GetToolsAsync(filter, userId, isToolViewAll);
            var wrapper = new WrapperModel<ToolResponse>
            {
                Count = tools.Count,
                DataList = tools.DataList.Select(_mapper.Map<ToolResponse>)
            };
            return wrapper;
        }

        public async Task<ToolResponse> GetToolAsync(int id, string? expand, int userId, bool isToolViewAll)
        {
            ToolDTO modelDTO = await _toolService.GetToolAsync(id, expand, userId, isToolViewAll);
            return _mapper.Map<ToolResponse>(modelDTO);
        }

        public async Task<int> CreateUpdateToolAsync(int id, ToolRequest model)
        {
            ToolDTO modelDTO = _mapper.Map<ToolDTO>(model);
            if (id > 0)
                modelDTO.Id = id;
            int toolId = await _toolService.CreateUpdateToolAsync(id, modelDTO);
            return toolId;
        }

        public async Task<int> CreateSolarQuoteAsync(SolarQuoteRequest model, int userId)
        {
            SolarQuoteDTO modelDTO = _mapper.Map<SolarQuoteDTO>(model);
            modelDTO.RequestedByUserId = userId;
            int quoteId = await _toolService.CreateSolarQuoteAsync(modelDTO);
            return quoteId;
        }

        public async Task<int> CreateSolarPortfolioReviewAsync(SolarPortfolioReviewRequest model, int userId)
        {
            SolarQuoteDTO modelDTO = _mapper.Map<SolarQuoteDTO>(model);
            modelDTO.RequestedByUserId = userId;
            int quoteId = await _toolService.CreateSolarQuoteAsync(modelDTO);
            return quoteId;
        }

        public async Task RemoveToolAsync(int id)
        {
            await _toolService.RemoveToolAsync(id);
        }

        public async Task PatchToolAsync(int id, JsonPatchDocument patchDoc)
        {
            await _toolService.PatchToolAsync(id, patchDoc);
        }

        public async Task<IEnumerable<ToolResponse>> GetPinnedToolsAsync(BaseSearchFilterModel filter, int userId)
        {
            IEnumerable<ToolDTO> tools = await _toolService.GetPinnedToolsAsync(filter, userId);
            return tools.Select(_mapper.Map<ToolResponse>);
        }
        public async Task CreatePinnedToolsAsync(int userId, IList<ToolPinnedRequest> model)
        {
            List<ToolPinnedDTO> modelDTO = model.Select(_mapper.Map<ToolPinnedDTO>).ToList();
            await _toolService.CreatePinnedToolsAsync(userId, modelDTO);
        }

        public bool IsToolExist(int id, bool includeInactive)
        {
            return _toolService.IsToolExist(id, includeInactive);
        }
    }
}