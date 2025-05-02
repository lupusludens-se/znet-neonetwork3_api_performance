using SE.Neo.Common.Models.User;

namespace SE.Neo.Common.Models.Feedback
{
    public class FeedbackDTO : BaseFeedbackDTO
    {
        public UserDTO User { get; set; }
        public DateTime? CreatedOn { get; set; }

    }
}
