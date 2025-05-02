
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services;
using SE.Neo.WebAPI.Services.Interfaces;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class CommonController : ControllerBase
    {
        private readonly ICommonApiService _commonApiService;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonController"/> class.
        /// </summary>
        /// <param name="commonApiService">The common API service.</param>
        /// <param name="logger">The logger instance.</param>
        public CommonController(
            ICommonApiService commonApiService,
            ILogger<UserController> logger
            )
        {
            _commonApiService = commonApiService;
            _logger = logger;
        }

        /// <summary>
        /// Get List of CMS Categories.
        /// </summary>
        /// <param name="expand">resources</param>
        /// <param name="filterBy">solutionids</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of categories.</returns>
        [HttpGet("categories")]
        [Active(skipAuthorize: true)]
        public async Task<IActionResult> GetCategories(string? expand, string? filterBy)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            var categories = await _commonApiService.GetCategoriesAsync(expand, filterBy, currentUser);
            return Ok(categories);
        }

        /// <summary>
        /// Get List of CMS Solutions.
        /// </summary>
        /// <param name="expand">resources</param>
        /// <param name="filterBy">ids</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of solutions.</returns>
        [HttpGet("solutions")]
        [Active(skipAuthorize: true)]
        public async Task<IActionResult> GetSolutions(string? expand, string? filterBy)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            var solutions = await _commonApiService.GetSolutionsAsync(expand, filterBy, currentUser);
            return Ok(solutions);
        }

        /// <summary>
        /// Get List of CMS Technologies.
        /// </summary>
        /// <param name="expand">resources</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of technologies.</returns>
        [HttpGet("technologies")]
        public async Task<IActionResult> GetTechnologies(string? expand)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            var technology = await _commonApiService.GetTechnologiesAsync(expand, currentUser);
            return Ok(technology);
        }

        /// <summary>
        /// Get List of CMS Regions.
        /// </summary>
        /// <remarks>
        /// FilterBy -> parentids=0, parentids=1,2
        /// <br />
        /// OrderBy -> name.asc, name.desc
        /// </remarks>
        /// <param name="filter">The filter model for searching and filtering regions.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of regions.</returns>
        [HttpGet("regions")]
        public async Task<IActionResult> GetRegions([FromQuery] BaseSearchFilterModel filter)
        {
            var regions = await _commonApiService.GetRegionsAsync(filter);
            return Ok(regions);
        }

        /// <summary>
        /// Get List of States.
        /// </summary>
        /// <remarks>
        /// FilterBy -> countryids=1,2
        /// <br />
        /// OrderBy -> name.asc, name.desc
        /// <br />
        /// Expand -> country
        /// </remarks>
        /// <param name="filter">The filter model for searching and filtering states.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of states.</returns>
        [HttpGet("states")]
        public async Task<IActionResult> GetStates([FromQuery] BaseSearchFilterModel filter)
        {
            var states = await _commonApiService.GetStatesAsync(filter);
            return Ok(states);
        }

        /// <summary>
        /// Get List of Countries.
        /// </summary>
        /// <remarks>
        /// OrderBy -> name.asc, name.desc
        /// <br />
        /// Expand -> states
        /// </remarks>
        /// <param name="filter">The filter model for searching and filtering countries.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of countries.</returns>
        [AllowAnonymous]
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries([FromQuery] BaseSearchFilterModel filter)
        {
            var countries = await _commonApiService.GetCountriesAsync(filter);
            return Ok(countries);
        }

        /// <summary>
        /// Get List of Roles.
        /// </summary>
        /// <param name="expand">resources</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of roles.</returns>
        [Active]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles(string? expand)
        {
            var roles = await _commonApiService.GetRolesAsync(expand);
            return Ok(roles);
        }

        /// <summary>
        /// Get List of Permissions.
        /// </summary>
        /// <param name="expand">resources</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of permissions.</returns>
        [Active]
        [HttpGet("permissions")]
        public async Task<IActionResult> GetPermissions(string? expand)
        {
            var permissions = await _commonApiService.GetPermissionsAsync(expand);
            return Ok(permissions);
        }

        /// <summary>
        /// Get List of Heard Via options.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of heard via options.</returns>
        [AllowAnonymous]
        [HttpGet("heardvia")]
        public async Task<IActionResult> GetHeardVia()
        {
            var heardvia = await _commonApiService.GetHeardViaAsync();
            return Ok(heardvia);
        }

        /// <summary>
        /// Get list of available timezones.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of available timezones.</returns>
        [Active]
        [HttpGet("timezones")]
        public async Task<IActionResult> GetTimeZones()
        {
            IEnumerable<TimeZoneResponse> timezones = await _commonApiService.GetTimeZonesAsync();
            return Ok(timezones);
        }

        /// <summary>
        /// Health check endpoint.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing the health check result.</returns>
        [AllowAnonymous]
        [HttpGet("health")]
        [ExcludeLogging]
        public async Task<IActionResult> GetHealthCheck()
        {
            var categories = await _commonApiService.GetHeardViaAsync();
            return Ok("Success");
        }

        /// <summary>
        /// Send message to contact us.
        /// </summary>
        /// <param name="model">The contact us request model.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [AllowAnonymous]
        [HttpPost("contact-us")]
        public async Task<IActionResult> SendContactUsMessage(ContactUsRequest model)
        {
            await _commonApiService.PostContactUsMessageAsync(model);
            return Ok();
        }


        /// <summary>
        /// Get Region Scale Types
        /// </summary>
        /// <returns></returns>
        //[Roles(RoleType.Corporation)]
        [Route("get-region-scale-types")]
        [HttpGet]
        public async Task<IActionResult> GetRegionScaleTypes()
        {
            var items = await _commonApiService.GetRegionScaleTypesAsync();
            return Ok(items);
        }

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="logEntry">The log request model.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost("logs")]
        public IActionResult Log(LogRequest logEntry)
        {
            var message = logEntry.Message;

            var propertyEnrichers = new List<ILogEventEnricher>
            {
                new PropertyEnricher("Source", (int)logEntry.Source)
            };

            if (logEntry.Error is not null)
            {
                propertyEnrichers.Add(new PropertyEnricher("Client Error", logEntry.Error, true));

                message ??= logEntry.Error.Message;
            }

            if (logEntry.ExtraInfo?.Any() == true)
            {
                propertyEnrichers.Add(new PropertyEnricher("Extra Info", logEntry.ExtraInfo, true));
            }

            using (LogContext.Push(propertyEnrichers.ToArray()))
            {
                _logger.Log(logEntry.Level, message, new { logEntry.Error, logEntry.ExtraInfo, ClientDateTimeUtc = logEntry.DateTimeUtc });
            }

            return Ok();
        }
    }
}
