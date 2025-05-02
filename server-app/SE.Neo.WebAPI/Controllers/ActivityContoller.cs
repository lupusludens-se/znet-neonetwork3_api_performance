using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Activity;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/activities")]
    public class ActivityContoller : ControllerBase
    {
        private readonly IActivityApiService _activityApiService;
        private readonly ILogger<ActivityContoller> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityContoller"/> class.
        /// </summary>
        /// <param name="activityApiService">The activity API service.</param>
        /// <param name="logger">The logger instance.</param>
        public ActivityContoller(
            IActivityApiService activityApiService,
            ILogger<ActivityContoller> logger
            )
        {
            _activityApiService = activityApiService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new activity.
        /// </summary>
        /// <param name="model">The activity request model.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        /// <remarks>
        /// Details has to be a stringified JSON object with required information for an activity depending on ActivityType.
        /// </remarks>
        [Active(skipAuthorize: true)]
        [HttpPost]
        public async Task<IActionResult> AddActivity(ActivityRequest model)
        {
            int activityId = await _activityApiService.CreateActivityAsync(model);
            return Ok(activityId);
        }
    }
}
