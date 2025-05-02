using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface ICommonService
    {
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string? expand = null, string? filterBy = null, List<int>? userRoleIds = null, int? userCompanyId = null);

        Task<Category> GetCategory(int categoryId);

        Task<IEnumerable<TechnologyDTO>> GetTechnologiesAsync(string? expand = null, List<int>? userRoleIds = null, int? userCompanyId = null);

        Task<IEnumerable<SolutionDTO>> GetSolutionsAsync(string? expand = null, string? filterBy = null, List<int>? userRoleIds = null, int? userCompanyId = null);

        Task<IEnumerable<RegionDTO>> GetRegionsAsync(BaseSearchFilterModel filter);

        Task<IEnumerable<StateDTO>> GetStatesAsync(BaseSearchFilterModel filter);

        Task<IEnumerable<CountryDTO>> GetCountriesAsync(BaseSearchFilterModel filter);

        Task<IEnumerable<RoleDTO>> GetRolesAsync(string? expand);

        Task<IEnumerable<PermissionDTO>> GetPermissionsAsync(string? expand);

        Task<IEnumerable<BaseIdNameDTO>> GetHeardViaAsync();

        Task<IEnumerable<TimeZoneDTO>> GetTimeZonesAsync();

        Task<Entities.TimeZone> GetTimeZoneByClientIdOrDefault(string timeZoneId = "", string defaultStandardName = "Eastern Standard Time");

        Task<TimeZoneDTO> GetTimeZone(int timeZoneId);

        Task<List<int>> GetRegionListForUserProfile(int userId, bool parentInclude = false, bool childInclude = false, bool usaAllInclude = false);

        List<int> ExpandRegionListForFiltration(List<int> regionIds, bool childInclude = false, bool usaStatesInclude = false, bool usaAllInclude = true);

        Task<bool> IsRoleIdExistAsync(int id);

        Task<bool> IsCountryIdExistAsync(int id);

        Task<bool> IsStateIdExistAsync(int id);

        Task<bool> IsCategoryIdExistAsync(int id);

        Task<bool> IsSolutionIdExistAsync(int id);

        Task<bool> IsTechnologyIdExistAsync(int id);

        Task<bool> IsRegionIdExistAsync(int id);

        Task<bool> IsTimeZoneIdExistAsync(int id);

        Task<IEnumerable<InitiativeScale>> GetRegionScaleTypesAsync();

        Task<List<Region>> GetAllRegions();
    }
}