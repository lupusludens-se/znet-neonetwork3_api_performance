namespace SE.Neo.WebAPI.Models.Feedback
{
    public class FeedbackExportResponse
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public string Rating { get; set; }
        public string CreatedOn { get; set; }
        public string Role { get; set; }
        public string Company { get; set; }
        public string User { get; set; }
    }
}
