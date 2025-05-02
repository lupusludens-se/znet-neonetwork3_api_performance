using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Discussion_Follower")]
    [Index(nameof(DisscussionId), nameof(UserId), IsUnique = true)]
    public class DiscussionFollower : BaseIdEntity
    {
        [Column("Discussion_Follower_Id")]
        public override int Id { get; set; }

        [Column("Discussion_Id")]
        [ForeignKey("Discussion")]
        public int DisscussionId { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        public Discussion Discussion { get; set; }
    }
}
