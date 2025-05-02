namespace SE.Neo.Common.Models.Feedback
{
    public class BaseFeedbackDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
    }
}
