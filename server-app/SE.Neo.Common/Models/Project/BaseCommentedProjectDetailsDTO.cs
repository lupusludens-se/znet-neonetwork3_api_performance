using SE.Neo.Common.Attributes;

namespace SE.Neo.Common.Models.Project
{
    public class BaseCommentedProjectDetailsDTO : BaseProjectDetailsDTO
    {
        [PropertyComparation("Time & Urgency Considerations")]
        public string TimeAndUrgencyConsiderations { get; set; }

        [PropertyComparation("Additional Comments")]
        public string AdditionalComments { get; set; }
    }
}
