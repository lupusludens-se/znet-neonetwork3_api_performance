using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Feedback
{
    public class FeedbackRequest
    {
        [Range(1, 5, ErrorMessage = "Rating must be within 1 to 5")]
        public int Rating { get; set; }

        [Required, ValidatePlainTextLength(1000)]
        public string Comments { get; set; }
    }
}
