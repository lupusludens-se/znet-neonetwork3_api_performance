using SE.Neo.WebAPI.Models.Activity;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IActivityApiService
    {
        Task<int> CreateActivityAsync(ActivityRequest model);

        Task<int> CreatePublicActivityAsync(ActivityRequest model);
    }
}
