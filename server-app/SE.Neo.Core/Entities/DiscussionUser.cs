using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    /// <summary>
    /// Defines list of participants in the discussion
    /// </summary>
    [Table("Discussion_User")]
    public class DiscussionUser : BaseIdEntity
    {
        [Column("Discussion_User_Id")]
        public override int Id { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column("Discussion_Id")]
        [ForeignKey("Discussion")]
        public int DiscussionId { get; set; }

        public int UnreadCount { get; set; }

        public User User { get; set; }

        public Discussion Discussion { get; set; }
    }
}
