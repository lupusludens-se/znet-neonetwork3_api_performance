using Microsoft.AspNetCore.Mvc;

namespace SE.Neo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class SendGridWebhookController : ControllerBase
    {
        private readonly ILogger<SendGridWebhookController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridWebhookController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance to log information.</param>
        public SendGridWebhookController(ILogger<SendGridWebhookController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Receives the webhook from SendGrid.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("sendgrid-response")]
        public async Task<IActionResult> ReceiveWebhook()
        {
            _logger.LogError($"Information the webhook is triggered {DateTime.UtcNow}");
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body))
                {
                    string jsonPayLoad = await reader.ReadToEndAsync();

                    _logger.LogError($"Details from the sendgrid API Response: - {DateTime.UtcNow} jsonPayLoad: {jsonPayLoad}");

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Step Error: - {DateTime.UtcNow} Ex: {ex.Message}. Detailed: {ex?.InnerException?.Message}");
                throw;
            }
        }
    }
}