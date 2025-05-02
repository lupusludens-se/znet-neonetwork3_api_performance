using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Country;
using SE.Neo.WebAPI.Models.Permission;
using SE.Neo.WebAPI.Models.Role;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.State;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface ICommonApiService
    {
        Task<IEnumerable<CategoryResponse>> GetCategoriesAsync(string? expand = null, string? filterBy = null, UserModel? currentUser = null);

        Task<IEnumerable<TechnologyResponse>> GetTechnologiesAsync(string? expand = null, UserModel? currentUser = null);

        Task<IEnumerable<SolutionResponse>> GetSolutionsAsync(string? expand = null, string? filterBy = null, UserModel? currentUser = null);

        Task<IEnumerable<RegionResponse>> GetRegionsAsync(BaseSearchFilterModel filter);

        Task<IEnumerable<StateResponse>> GetStatesAsync(BaseSearchFilterModel filter);

        Task<IEnumerable<CountryResponse>> GetCountriesAsync(BaseSearchFilterModel filter);

        Task<IEnumerable<RoleResponse>> GetRolesAsync(string? expand);

        Task<IEnumerable<PermissionResponse>> GetPermissionsAsync(string? expand);

        Task<IEnumerable<BaseIdNameResponse>> GetHeardViaAsync();

        Task<IEnumerable<TimeZoneResponse>> GetTimeZonesAsync();

        Task PostContactUsMessageAsync(ContactUsRequest model);

        Task<IEnumerable<InitiativeScale>> GetRegionScaleTypesAsync();
    }
}