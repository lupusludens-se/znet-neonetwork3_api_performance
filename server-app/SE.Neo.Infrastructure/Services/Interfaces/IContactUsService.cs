namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface IContactUsService
    {
        Task SendContactUsMessageAsync(string htmlBody);
    }
}