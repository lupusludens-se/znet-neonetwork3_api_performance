using Microsoft.AspNetCore.Mvc;

namespace SE.Neo.EmailAlertSender.Interfaces
{
    public interface IEmailAlertSendService
    {
        public Task SendEmailsAsync(ActionContext context);
    }
}