namespace SE.Neo.WebAPI.Models.Feedback
{
    public class FeedbackResponse
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedOn { get; set; }
        public FeedbackUserResponse FeedbackUser { get; set; }
    }
}
