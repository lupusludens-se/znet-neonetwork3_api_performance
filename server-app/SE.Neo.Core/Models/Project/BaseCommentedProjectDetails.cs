using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Models.Project
{
    public abstract class BaseCommentedProjectDetails : BaseProjectDetails
    {
        [Column("Time_And_Urgency_Considerations")]
        [StringLength(200)]
        public string TimeAndUrgencyConsiderations { get; set; }

        [Column("Additional_Comments")]
        [StringLength(200)]
        public string AdditionalComments { get; set; }
    }
}
