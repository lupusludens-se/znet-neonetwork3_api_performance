using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.EmailAlert;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IEmailAlertApiService
    {
        Task<WrapperModel<EmailAlertResponse>> GetEmailAlertsAsync();

        Task UpdateEmailAlertsAsync(EmailAlertRequest model, int? userId = null);

        Task PatchEmailAlertAsync(int id, JsonPatchDocument patchDoc);

        Task<WrapperModel<EmailAlertResponse>> GetUserEmailAlertsAsync(int userId);


        Task PatchUserEmailAlertAsync(int userId, int id, JsonPatchDocument patchDoc);
    }
}
