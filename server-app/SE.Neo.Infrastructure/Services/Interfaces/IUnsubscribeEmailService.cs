namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface IUnsubscribeEmailService
    {
        Task<string> EncryptAsync(string userId);
        Task<string> DecryptAsync(byte[] hashedText);
    }
}
