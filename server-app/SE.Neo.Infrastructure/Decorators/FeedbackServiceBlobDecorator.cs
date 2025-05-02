using SE.Neo.Common.Models.Feedback;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class FeedbackServiceBlobDecorator : AbstractFeedbackServiceDecorator
    {
        private readonly IBlobServicesFacade _blobServicesFacade;

        public FeedbackServiceBlobDecorator(
            IFeedbackService feedbackService,
            IBlobServicesFacade blobServicesFacade)
            : base(feedbackService)
        {
            _blobServicesFacade = blobServicesFacade;
        }

        public override async Task<WrapperModel<FeedbackDTO>> GetFeedbacksAsync(BaseSearchFilterModel filter)
        {
            WrapperModel<FeedbackDTO> feedbacksResult = await base.GetFeedbacksAsync(filter);

            List<FeedbackDTO> FeedbackDtOs = feedbacksResult.DataList.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(FeedbackDtOs, dto => dto.User.Image, (dto, b) => dto.User.Image = b);
            feedbacksResult.DataList = FeedbackDtOs;

            return feedbacksResult;
        }

        public override async Task<int> CreateFeedbackAsync(CreateFeedbackDTO modelDTO)
        {
            return await base.CreateFeedbackAsync(modelDTO);

        }

        public override async Task<FeedbackDTO> GetFeedbackAsync(int feedbackId)
        {
            FeedbackDTO feedback = await base.GetFeedbackAsync(feedbackId);
            await _blobServicesFacade.PopulateWithBlobAsync(feedback, dto => dto?.User.Image, (dto, b) => { if (dto != null) dto.User.Image = b; });
            return feedback;

        }
    }
}
