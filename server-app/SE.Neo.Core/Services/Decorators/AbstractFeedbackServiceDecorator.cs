using SE.Neo.Common.Models.Feedback;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services.Decorators
{
    public abstract class AbstractFeedbackServiceDecorator : IFeedbackService
    {
        protected readonly IFeedbackService _feedbackService;

        public AbstractFeedbackServiceDecorator(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public virtual async Task<WrapperModel<FeedbackDTO>> GetFeedbacksAsync(BaseSearchFilterModel filter)
        {
            return await _feedbackService.GetFeedbacksAsync(filter);
        }

        public virtual async Task<int> CreateFeedbackAsync(CreateFeedbackDTO modelDTO)
        {
            return await _feedbackService.CreateFeedbackAsync(modelDTO);
        }

        public virtual async Task<FeedbackDTO> GetFeedbackAsync(int feedbackId)
        {
            return await _feedbackService.GetFeedbackAsync(feedbackId);
        }
    }
}
