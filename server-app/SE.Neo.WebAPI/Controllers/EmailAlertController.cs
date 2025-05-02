
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.EmailAlert;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize, Active]
    [ApiController]
    [Route("api/email-alerts")]
    public class EmailAlertController : ControllerBase
    {
        private readonly ILogger<EmailAlertController> _logger;
        private readonly IEmailAlertApiService _emailAlertApiService;

        /// <summary>  
        /// Initializes a new instance of the <see cref="EmailAlertController"/> class.  
        /// </summary>  
        /// <param name="logger">The logger instance.</param>  
        /// <param name="emailAlertApiService">The email alert API service instance.</param>  
        public EmailAlertController(ILogger<EmailAlertController> logger, IEmailAlertApiService emailAlertApiService)
        {
            _logger = logger;
            _emailAlertApiService = emailAlertApiService;
        }

        /// <summary>  
        /// Get list of the email alerts.  
        /// </summary>  
        /// <returns>List of the email alerts.</returns>  
        [Permissions(PermissionType.EmailAlertManagement)]
        [HttpGet]
        public async Task<IActionResult> GetEmailAlerts()
        {
            WrapperModel<EmailAlertResponse> alerts = await _emailAlertApiService.GetEmailAlertsAsync();

            return Ok(alerts);
        }

        /// <summary>  
        /// Update list of the email alerts.  
        /// </summary>  
        /// <param name="model">The email alert request model.</param>  
        /// <returns>List of the email alerts.</returns>  
        [Permissions(PermissionType.EmailAlertManagement)]
        [HttpPut]
        public async Task<IActionResult> UpdateEmailAlerts(EmailAlertRequest model)
        {
            await _emailAlertApiService.UpdateEmailAlertsAsync(model);
            return Ok();
        }

        /// <summary>  
        /// Partially update email alert settings.  
        /// </summary>  
        /// <param name="id">Unique identifier of the email alert.</param>  
        /// <param name="patchRequest">Partially updated email alert object.</param>  
        /// <returns>Ok result.</returns>  
        [Permissions(PermissionType.EmailAlertManagement)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmailAlert(int id, EmailAlertJsonPatchRequest patchRequest)
        {
            JsonPatchDocument patchDoc = patchRequest.JsonPatchDocument;
            await _emailAlertApiService.PatchEmailAlertAsync(id, patchDoc);

            return Ok();
        }

        /// <summary>  
        /// Get list of user's email alerts.  
        /// </summary>  
        /// <returns>List of the email alerts.</returns>  
        [HttpGet("/api/users/current/email-alerts")]
        public async Task<IActionResult> GetUserEmailAlerts()
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            WrapperModel<EmailAlertResponse> alerts = await _emailAlertApiService.GetUserEmailAlertsAsync(currentUser.Id);

            return Ok(alerts);
        }

        /// <summary>  
        /// Update list of user's email alerts.  
        /// </summary>  
        /// <param name="model">The email alert request model.</param>  
        /// <returns>List of the email alerts.</returns>  
        [HttpPut("/api/users/current/email-alerts")]
        public async Task<IActionResult> UpdateUserEmailAlerts(EmailAlertRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            await _emailAlertApiService.UpdateEmailAlertsAsync(model, currentUser.Id);

            return Ok();
        }

        /// <summary>  
        /// Partially update user's email alert settings.  
        /// </summary>  
        /// <param name="id">Unique identifier of the email alert.</param>  
        /// <param name="patchRequest">Partially updated email alert object.</param>  
        /// <returns>Ok result.</returns>  
        [HttpPatch("/api/users/current/email-alerts/{id}")]
        public async Task<IActionResult> PatchUserEmailAlert(int id, EmailAlertJsonPatchRequest patchRequest)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            JsonPatchDocument patchDoc = patchRequest.JsonPatchDocument;
            await _emailAlertApiService.PatchUserEmailAlertAsync(currentUser.Id, id, patchDoc);

            return Ok();
        }
    }
}
