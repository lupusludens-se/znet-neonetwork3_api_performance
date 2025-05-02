using SE.Neo.Common.Models.Feedback;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<int> CreateFeedbackAsync(CreateFeedbackDTO modelDTO);
        Task<WrapperModel<FeedbackDTO>> GetFeedbacksAsync(BaseSearchFilterModel filter);

        Task<FeedbackDTO> GetFeedbackAsync(int feedbackId);
    }
}
