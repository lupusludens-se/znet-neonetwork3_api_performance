using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Feedback;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IFeedbackApiService
    {
        Task<int> CreateFeedbackAsync(FeedbackRequest model, UserModel user);

        Task<WrapperModel<FeedbackResponse>> GetFeedbacksAsync(BaseSearchFilterModel filter);
        Task<FeedbackResponse> GetFeedbackAsync(int feedbackId);
        Task<int> ExportFeedbacksAsync(BaseSearchFilterModel filter, UserModel? currentuser, MemoryStream stream);
    }
}
