namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface IGraphAPIService
    {
        public Task<string> AddUserAndResetPasswordAsync(string firstName, string lastName, string email, bool retry = true);
        public Task UpdateUserAccessAsync(string userId, bool isEnabled);
        public Task DeleteUserAsync(string azureId);
    }
}
