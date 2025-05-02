namespace SE.Neo.WebAPI.Models.Project
{
    public class BaseCommentedProjectDetailsResponse : BaseProjectDetailsResponse
    {
        public string TimeAndUrgencyConsiderations { get; set; }
        public string AdditionalComments { get; set; }
    }
}
