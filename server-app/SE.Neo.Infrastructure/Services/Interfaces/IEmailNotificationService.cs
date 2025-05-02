namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface IEmailNotificationService
    {
        public Task CompleteRegistrationAsync(string username, string firstName);
    }
}
