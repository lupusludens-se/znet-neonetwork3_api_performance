
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Feedback;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize, Active]
    [ApiController]
    [Route("api/feedbacks")]
    public class FeedbackController : ControllerBase
    {
        private readonly ILogger<FeedbackController> _logger;
        private readonly IFeedbackApiService _feedbackApiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="feedbackApiService">The feedback API service instance.</param>
        public FeedbackController(ILogger<FeedbackController> logger, IFeedbackApiService feedbackApiService)
        {
            _logger = logger;
            _feedbackApiService = feedbackApiService;
        }

        /// <summary>
        /// Get list of the feedbacks by filters.
        /// </summary>
        /// <param name="filter">Defines filter parameters.</param>
        /// <returns>List of the feedbacks.</returns>
        [HttpGet]
        [Permissions(PermissionType.AdminAll)]
        public async Task<IActionResult> GetFeedbacks([FromQuery] BaseSearchFilterModel filter)
        {
            var feedbacks = await _feedbackApiService.GetFeedbacksAsync(filter);
            return Ok(feedbacks);
        }

        /// <summary>
        /// Get feedback details by Id.
        /// </summary>
        /// <param name="id">Identification of the feedback.</param>
        /// <returns>Feedback details or NotFound if not found.</returns>
        [HttpGet("{id}")]
        [Permissions(PermissionType.AdminAll)]
        public async Task<IActionResult> GetFeedback(int id)
        {
            var feedback = await _feedbackApiService.GetFeedbackAsync(id);
            return feedback != null ? Ok(feedback) : NotFound();
        }

        /// <summary>
        /// Post the User Feedback.
        /// </summary>
        /// <param name="formData">The feedback request data.</param>
        /// <returns>Id of the created feedback.</returns>
        [HttpPost("submitfeedback")]
        public async Task<IActionResult> PostFeedback([FromBody] FeedbackRequest formData)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            int feedbackId = await _feedbackApiService.CreateFeedbackAsync(formData, currentUser);
            return Ok(feedbackId);
        }

        /// <summary>
        /// Export feedbacks based on filter parameters.
        /// </summary>
        /// <param name="filter">Defines filter parameters.</param>
        /// <returns>CSV file containing the feedbacks.</returns>
        [HttpGet("export")]
        [Permissions(PermissionType.AdminAll)]
        public async Task<IActionResult> ExportFeedbacks([FromQuery] BaseSearchFilterModel filter)
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            MemoryStream feedbacksStream = new MemoryStream();
            int feedbacksCount = await _feedbackApiService.ExportFeedbacksAsync(filter, currentUser, feedbacksStream);
            EntityTagHeaderValue feedbacksFoundCount = new EntityTagHeaderValue("\"" + $"Found {feedbacksCount} Feedbacks" + "\"");
            return File(feedbacksStream, "text/csv", $"Export file {DateTime.UtcNow.Month}_{DateTime.UtcNow.Day}_{DateTime.UtcNow.Year}.csv", null, feedbacksFoundCount);
        }
    }
}
