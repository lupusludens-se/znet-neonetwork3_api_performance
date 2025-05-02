using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public abstract class BaseCommentedProjectDetailsRequest : BaseProjectDetailsRequest
    {
        [StringLength(200)]
        public string? TimeAndUrgencyConsiderations { get; set; }

        [StringLength(200)]
        public string? AdditionalComments { get; set; }
    }
}
