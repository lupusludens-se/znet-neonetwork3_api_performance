using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.Unsubscribe;
using SE.Neo.WebAPI.Models.UserProfile;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IUnsubscribeEmailApiService
    {
        Task<UnsubscribeResponse> GetEmailFromRequestToken(UnsubscribeRequest model);
        Task<UnsubscribeResponse> UpdateEmailFrequency(UnsubscribeRequest model, EmailAlertCategory emailAlertCategory);
    }
}
