using AutoMapper;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Country;
using SE.Neo.WebAPI.Models.Permission;
using SE.Neo.WebAPI.Models.Role;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.State;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class CommonApiService : ICommonApiService
    {
        private readonly ILogger<UserApiService> _logger;
        private readonly IMapper _mapper;
        private readonly ICommonService _commonService;
        private readonly IContactUsService _contactUsService;

        public CommonApiService(ILogger<UserApiService> logger, IMapper mapper,
            ICommonService commonService, IContactUsService contactUsService)
        {
            _logger = logger;
            _mapper = mapper;
            _commonService = commonService;
            _contactUsService = contactUsService;
        }

        public async Task<IEnumerable<CategoryResponse>> GetCategoriesAsync(string? expand = null, string? filterBy = null, UserModel? currentUser = null)
        {
            IEnumerable<CategoryDTO> categories = await _commonService.GetCategoriesAsync(expand, filterBy, currentUser?.RoleIds, currentUser?.CompanyId);
            return categories.Select(_mapper.Map<CategoryResponse>);
        }

        public async Task<IEnumerable<TechnologyResponse>> GetTechnologiesAsync(string? expand = null, UserModel? currentUser = null)
        {
            IEnumerable<TechnologyDTO> technologies = await _commonService.GetTechnologiesAsync(expand, currentUser?.RoleIds, currentUser?.CompanyId);
            return technologies.Select(_mapper.Map<TechnologyResponse>);
        }

        public async Task<IEnumerable<SolutionResponse>> GetSolutionsAsync(string? expand = null, string? filterBy = null, UserModel? currentUser = null)
        {
            IEnumerable<SolutionDTO> solutions = await _commonService.GetSolutionsAsync(expand, filterBy, currentUser?.RoleIds, currentUser?.CompanyId);
            return solutions.Select(_mapper.Map<SolutionResponse>);
        }

        public async Task<IEnumerable<RegionResponse>> GetRegionsAsync(BaseSearchFilterModel filter)
        {
            IEnumerable<RegionDTO> regions = await _commonService.GetRegionsAsync(filter);
            return regions.Select(_mapper.Map<RegionResponse>);
        }

        public async Task<IEnumerable<StateResponse>> GetStatesAsync(BaseSearchFilterModel filter)
        {
            IEnumerable<StateDTO> stateDTOs = await _commonService.GetStatesAsync(filter);
            return stateDTOs.Select(_mapper.Map<StateResponse>);
        }

        public async Task<IEnumerable<CountryResponse>> GetCountriesAsync(BaseSearchFilterModel filter)
        {
            IEnumerable<CountryDTO> countries = await _commonService.GetCountriesAsync(filter);
            return countries.Select(_mapper.Map<CountryResponse>);
        }

        public async Task<IEnumerable<RoleResponse>> GetRolesAsync(string? expand)
        {
            IEnumerable<RoleDTO> roles = await _commonService.GetRolesAsync(expand);
            return roles.Select(_mapper.Map<RoleResponse>);
        }

        public async Task<IEnumerable<PermissionResponse>> GetPermissionsAsync(string? expand)
        {
            IEnumerable<PermissionDTO> permissions = await _commonService.GetPermissionsAsync(expand);
            return permissions.Select(_mapper.Map<PermissionResponse>);
        }

        public async Task<IEnumerable<BaseIdNameResponse>> GetHeardViaAsync()
        {
            IEnumerable<BaseIdNameDTO> heardVia = await _commonService.GetHeardViaAsync();
            return heardVia.Select(_mapper.Map<BaseIdNameResponse>);
        }

        public async Task<IEnumerable<TimeZoneResponse>> GetTimeZonesAsync()
        {
            IEnumerable<TimeZoneDTO> timezones = await _commonService.GetTimeZonesAsync();
            return timezones.Select(_mapper.Map<TimeZoneResponse>);
        }

        public async Task<IEnumerable<InitiativeScale>> GetRegionScaleTypesAsync()
        {
            return await _commonService.GetRegionScaleTypesAsync();
        }

        public async Task PostContactUsMessageAsync(ContactUsRequest model)
        {
            string htmlBody = $"<b>First Name:</b> {model.FirstName}<br>" +
                $"<b>Last Name:</b> {model.LastName}<br>" +
                $"<b>Email:</b> {model.Email}<br>" +
                $"<b>Company:</b> {model.Company}<br>" +
                $"<b>Message:</b> {model.Message}";

            await _contactUsService.SendContactUsMessageAsync(htmlBody);
        }
    }
}