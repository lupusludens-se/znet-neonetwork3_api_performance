using SE.Neo.Common.Models.Activity;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IActivityService
    {
        Task<int> CreateActivityAsync(ActivityDTO model);

        Task<int> CreatePublicActivityAsync(PublicSiteActivityDTO model);

        bool IsValid(string serializedValue, Enums.ActivityType activityType, Enums.ActivityLocation activityLocation);
    }
}
