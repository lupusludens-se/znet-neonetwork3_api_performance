using SE.Neo.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Discussion_Source")]
    public class DiscussionSource : BaseEntity
    {
        [Column("Discussion_Source_Id")]
        public DiscussionSourceType Id { get; set; }

        [Column("Discussion_Source_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
