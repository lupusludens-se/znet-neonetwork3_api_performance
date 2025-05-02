using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.Tool;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Mapping;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public partial class ToolService : IToolService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<ToolService> _logger;
        private readonly IMapper _mapper;

        public ToolService(ApplicationContext context, ILogger<ToolService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<WrapperModel<ToolDTO>> GetToolsAsync(BaseSearchFilterModel filter, int userId, bool isToolViewAll)
        {
            var toolsQueryable = _context.Tools.AsNoTracking();
            if (userId > 0)
            {
                if (isToolViewAll)
                {
                    filter.Expand += ",pinned";
                }
                else
                {
                    filter.Expand = "roles,companies,icon,pinned"; // mandatory expand if tools need to match user roles and companies
                    toolsQueryable = FilterUserAllowedTools(userId, toolsQueryable);
                }
                toolsQueryable = ExpandSearchOrderTools(toolsQueryable, filter, userId);
            }
            else
            {
                filter.Expand = "roles,icon";
                toolsQueryable = ExpandSearchOrderTools(toolsQueryable, filter, userId);
                toolsQueryable = FilterUserAllowedTools(userId, toolsQueryable);
                toolsQueryable = toolsQueryable.OrderBy(x => x.Title);
            }

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await toolsQueryable.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<ToolDTO> { Count = count, DataList = new List<ToolDTO>() };
                }
            }
            if (filter.Skip.HasValue)
            {
                toolsQueryable = toolsQueryable.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                toolsQueryable = toolsQueryable.Take(filter.Take.Value);
            }
            var tools = await toolsQueryable.ToListAsync();
            List<ToolDTO>? toolDTOs = new List<ToolDTO>();

            if (userId == 0)
            {

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<PublicToolProfile>();

                });
                var mapper = config.CreateMapper();
                toolDTOs = tools.Select(mapper.Map<ToolDTO>).ToList();
            }
            else
            {
                toolDTOs = tools.Select(_mapper.Map<ToolDTO>).ToList();
                toolDTOs.ForEach(toolDTO => { toolDTO.IsPinned = toolDTO.Pinned.Any(df => df.UserId == userId); });
            }
            return new WrapperModel<ToolDTO> { Count = count, DataList = toolDTOs };
        }

        public async Task<ToolDTO> GetToolAsync(int id, string? expand, int? userId, bool isToolViewAll)
        {
            var toolsQueryable = _context.Tools
                .AsNoTracking()
                .Where(t => t.Id == id);

            if (isToolViewAll)
            {
                expand += ",icon,pinned";
            }
            else
            {
                if (userId.HasValue)
                {
                    expand = "roles,companies,icon,pinned";
                    toolsQueryable = FilterUserAllowedTools(userId.Value, toolsQueryable);
                }
                else
                {
                    return null;
                }
            }
            toolsQueryable = ExpandSearchOrderTools(toolsQueryable, new BaseSearchFilterModel { Expand = expand });
            var tool = await toolsQueryable.FirstOrDefaultAsync();
            var toolDTO = _mapper.Map<ToolDTO>(tool);
            if (toolDTO != null)
            {
                if (userId.HasValue)
                    toolDTO.IsPinned = toolDTO.Pinned.Any(df => df.UserId == userId.Value);
            }
            return toolDTO;
        }

        public async Task<int> CreateUpdateToolAsync(int id, ToolDTO modelDTO)
        {
            bool isEdit = id > 0;
            var model = new Tool();
            if (isEdit)
            {
                model = _context.Tools.SingleOrDefault(b => b.Id == id);
                if (model == null)
                    throw new CustomException("Error occurred on saving data. Tool does not exist.");
            }
            int toolId = isEdit ? id : 0;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _mapper.Map(modelDTO, model);
                    if (!isEdit)
                    {
                        _context.Tools.AddRange(model);
                        _context.SaveChanges();
                        toolId = model.Id;
                    }
                    //    Remove Roles and Companies
                    if (modelDTO.Roles != null)
                    {
                        _context.RemoveRange(_context.ToolRoles.Where(a => a.ToolId == toolId));
                        _context.ToolRoles.AddRange(modelDTO.Roles.Select(item => new ToolRole() { ToolId = toolId, RoleId = item.Id }));
                        if (modelDTO.Roles.Any(r => r.Id == (int)RoleType.SolutionProvider))
                        {
                            _context.ToolRoles.Add(new ToolRole { ToolId = toolId, RoleId = (int)RoleType.SPAdmin });
                        }
                    }
                    if (modelDTO.Companies != null)
                    {
                        _context.RemoveRange(_context.ToolCompanies.Where(a => a.ToolId == toolId));
                        _context.ToolCompanies.AddRange(modelDTO.Companies.Select(item => new ToolCompany() { ToolId = toolId, CompanyId = item.Id }));
                    }

                    //    Add new Roles and Companies
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            return toolId;
        }

        public async Task<int> CreateSolarQuoteAsync(SolarQuoteDTO solarQuoteDTO)
        {
            var model = new SolarQuote();
            _mapper.Map(solarQuoteDTO, model);
            int quoteId;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SolarQuotes.Add(model);
                    _context.SaveChanges();
                    quoteId = model.Id;
                    if (solarQuoteDTO.Interests != null)
                    {
                        _context.SolarQuoteValuesProvided.AddRange(solarQuoteDTO.Interests.Select(item => new SolarQuoteValueProvided { ValueProvidedId = (ValueProvidedType)item.Id, SolarQuoteId = quoteId }));
                    }
                    if (solarQuoteDTO.ContractStructures != null)
                    {
                        _context.SolarQuoteContractStructures.AddRange(solarQuoteDTO.ContractStructures.Select(item => new SolarQuoteContractStructure { ContractStructureId = (ContractStructureType)item.Id, SolarQuoteId = model.Id }));
                    }
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            return quoteId;
        }

        public bool IsToolTitleUnique(string title, int toolId = 0)
        {
            var tool = toolId != 0 ?
                _context.Tools.Where(t => t.Title == title && t.Id != toolId).ToList() :
                _context.Tools.Where(t => t.Title == title).ToList();
            return !tool.Any();
        }

        public async Task RemoveToolAsync(int id)
        {
            var model = await _context.Tools.SingleOrDefaultAsync(t => t.Id == id);
            if (model != null)
            {
                List<ToolRole> toolRoles = await _context.ToolRoles.AsQueryable().Where(tr => tr.ToolId == id).ToListAsync();
                List<ToolCompany> toolCompanies = await _context.ToolCompanies.AsQueryable().Where(tc => tc.ToolId == id).ToListAsync();
                List<ToolPinned> toolPinned = await _context.ToolsPinned.AsQueryable().Where(tp => tp.ToolId == id).ToListAsync();
                List<InitiativeTool> initiativeTools = await _context.InitiativeTool.AsQueryable().Where(tp => tp.ToolId == id).ToListAsync();
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.ToolsPinned.RemoveRange(toolPinned);
                        _context.ToolRoles.RemoveRange(toolRoles);
                        _context.ToolCompanies.RemoveRange(toolCompanies);
                        _context.InitiativeTool.RemoveRange(initiativeTools);
                        _context.Tools.Remove(model);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new CustomException(CoreErrorMessages.ErrorOnRemoving, ex);
                    }
                }
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }

        public async Task PatchToolAsync(int id, JsonPatchDocument patchDoc)
        {
            Tool tool = await _context.Tools.SingleOrDefaultAsync(t => t.Id == id);
            if (tool != null)
            {
                try
                {
                    patchDoc.ApplyTo(tool);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new CustomException("Could not apply patch to Tool.", ex);
                }
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }

        public bool IsToolExist(int id, bool includeInactive = false)
        {
            return _context.Tools.AsNoTracking().Any(t => t.Id == id && (includeInactive || t.IsActive));
        }

        public async Task<IEnumerable<ToolDTO>> GetPinnedToolsAsync(BaseSearchFilterModel filter, int userId)
        {
            List<int> toolIds = _context.ToolsPinned.Where(t => t.UserId.Equals(userId)).Select(s => s.ToolId).ToList();

            IQueryable<Tool> toolsQueryable = _context.Tools.Where(x => toolIds.Contains(x.Id)).AsNoTracking();

            toolsQueryable = ExpandSearchOrderTools(toolsQueryable, filter);

            List<Tool> tools = await toolsQueryable.ToListAsync();

            List<ToolDTO> toolDTOs = tools.Select(_mapper.Map<ToolDTO>).ToList();

            toolDTOs.ForEach(toolDTO => { toolDTO.IsPinned = true; });

            return toolDTOs;
        }

        public async Task CreatePinnedToolsAsync(int userId, IList<ToolPinnedDTO> modelDTO)
        {
            if (modelDTO.Count > 5)
                throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} Count should not be more than 5.");
            try
            {
                //Remove Pinned tools dependencies
                _context.RemoveRange(_context.ToolsPinned.Where(a => a.UserId == userId));
                //Add Pinned tools dependencies
                _context.ToolsPinned.AddRange(modelDTO.Select(item => new ToolPinned() { UserId = userId, ToolId = item.ToolId }));

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
        }
    }
}