using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.UserProfile;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [Route("api")]
    public class UnsubscribeController : ControllerBase
    {
        private readonly ILogger<UnsubscribeController> _logger;
        private readonly IUnsubscribeEmailApiService _unsubscribeEmailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsubscribeController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="unsubscribeEmailService">The unsubscribe email service instance.</param>
        public UnsubscribeController(ILogger<UnsubscribeController> logger, IUnsubscribeEmailApiService unsubscribeEmailService)
        {
            _logger = logger;
            _unsubscribeEmailService = unsubscribeEmailService;
        }

        /// <summary>
        /// Gets the email associated with the provided request token.
        /// </summary>
        /// <param name="model">The unsubscribe request model containing the token.</param>
        /// <returns>An <see cref="IActionResult"/> containing the email details or a bad request status.</returns>
        [HttpPost("unsubscribe-getdetails")]
        public async Task<IActionResult> GetEmailFromRequestToken([FromBody] UnsubscribeRequest model)
        {
            _logger.LogInformation($"GetEmailFromRequestToken: {model.Token}");
            var result = await _unsubscribeEmailService.GetEmailFromRequestToken(model);
            if (!string.IsNullOrEmpty(result?.Message))
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates the user's email preferences based on the provided request token.
        /// </summary>
        /// <param name="model">The unsubscribe request model containing the token.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
        [HttpPost("unsubscribe")]
        public async Task<IActionResult> UpdateUserEmailPreferences([FromBody] UnsubscribeRequest model)
        {
            _logger.LogInformation($"UpdateUserEmailPreferences: {model.Token}");
            if (!string.IsNullOrEmpty(model?.Token))
            {
                var result = await _unsubscribeEmailService.UpdateEmailFrequency(model, EmailAlertCategory.Summary);
                if (!string.IsNullOrEmpty(result?.Message))
                {
                    return Ok(result);
                }
            }
            return BadRequest();
        }
    }
}