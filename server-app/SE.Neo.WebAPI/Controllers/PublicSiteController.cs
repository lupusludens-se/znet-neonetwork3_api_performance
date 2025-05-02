using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Activity;
using SE.Neo.WebAPI.Models.Project;
using SE.Neo.WebAPI.Models.ScheduleDemo;
using SE.Neo.WebAPI.Models.User;
//using SE.Neo.WebAPI.Models.ScheduleDemo;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/public")]
    public class PublicSiteController : ControllerBase
    {
        private readonly IScheduleDemoApiService _scheduleDemoApiService;
        private readonly IActivityApiService _activityApiService;
        private readonly IPublicDashboardApiService _publicDashboardService;

        private readonly ILogger<UserController> _logger;
        private readonly IActionContextAccessor actionContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicSiteController"/> class.
        /// </summary>
        /// <param name="scheduleDemoApiService">The schedule demo API service.</param>
        /// <param name="activityApiService">The activity API service.</param>
        /// <param name="logger">The logger instance.</param>
        /// <param name="publicDashboardService">The public dashboard service.</param>
        /// <param name="actionContextAccessor">The action context accessor.</param>
        public PublicSiteController(IScheduleDemoApiService scheduleDemoApiService, IActivityApiService activityApiService, ILogger<UserController> logger,
            IPublicDashboardApiService publicDashboardService,
            IActionContextAccessor actionContextAccessor)
        {
            _scheduleDemoApiService = scheduleDemoApiService;
            _logger = logger;
            this.actionContextAccessor = actionContextAccessor;
            _activityApiService = activityApiService;
            _publicDashboardService = publicDashboardService;
        }

        /// <summary>
        /// Sends a message to schedule a demo.
        /// </summary>
        /// <param name="model">The schedule demo request model.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>
        [HttpPost("schedule-demo")]
        public async Task<IActionResult> SendScheduleDemoMessage(ScheduleDemoRequest model)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            if (currentUser == null)
            {
                await _scheduleDemoApiService.SendScheduleDemoMessageAsync(model, actionContextAccessor.ActionContext);
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// Tracks public activity.
        /// </summary>
        /// <param name="model">The activity request model.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>
        [HttpPost("track-public-activity")]
        public async Task<IActionResult> TrackPublicActivity(ActivityRequest model)
        {
            try
            {
                await _activityApiService.CreatePublicActivityAsync(model);
            }
            catch (Exception)
            {
            }

            return Ok();
        }

        /// <summary>
        /// Gets the trending projects.
        /// </summary>
        /// <returns>An IActionResult containing the trending projects.</returns>
        [HttpGet("projects-trending")]
        public async Task<IActionResult> GetProjects()
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<NewTrendingProjectResponse> response = new();
            if (currentUser == null)
            {
                response = await _publicDashboardService.GetProjectsDataForDiscoverability();
            }
            return Ok(response);
        }
    }
}
