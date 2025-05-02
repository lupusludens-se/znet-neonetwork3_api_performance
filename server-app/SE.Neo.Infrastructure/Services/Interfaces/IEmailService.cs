using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Core.Enums;
using SE.Neo.EmailTemplates.Models.BaseModel;

namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface IEmailService : IEmailSender
    {
        public Task SendTemplatedEmailAsync(string to, string subject, BaseTemplatedEmailModel model, ActionContext? context = null, bool includeCC = false, UnsubscribeEmailType summaryEmail = UnsubscribeEmailType.NA, int userId = 0);
        new public Task SendEmailAsync(string to, string subject, string htmlBody);
        public Task<Dictionary<string, byte[]>> GenerateMailImagesContent();
    }
}
