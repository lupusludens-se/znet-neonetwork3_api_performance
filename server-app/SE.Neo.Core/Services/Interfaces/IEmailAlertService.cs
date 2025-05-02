using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.EmailAlert;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IEmailAlertService
    {
        Task<WrapperModel<EmailAlertDTO>> GetEmailAlertsAsync();

        Task UpdateEmailAlertsAsync(IEnumerable<EmailAlertDTO> dataToUpdate);

        Task PatchEmailAlertAsync(int id, JsonPatchDocument patchDoc);

        Task<WrapperModel<EmailAlertDTO>> GetUserEmailAlertsAsync(int userId);

        Task UpdateUserEmailAlertsAsync(int userId, IEnumerable<EmailAlertDTO> dataToUpdate);

        Task PatchUserEmailAlertAsync(int userId, int id, JsonPatchDocument patchDoc);
    }
}
