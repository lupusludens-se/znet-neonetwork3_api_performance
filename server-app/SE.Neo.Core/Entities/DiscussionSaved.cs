using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Discussion_Saved")]
    [Index(nameof(DiscussionId), nameof(UserId), IsUnique = true)]
    public class DiscussionSaved : BaseIdEntity
    {
        [Column("Discussion_Saved_Id")]
        public override int Id { get; set; }

        [Column("Discussion_Id")]
        [ForeignKey("Discussion")]
        public int DiscussionId { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        public Discussion Discussion { get; set; }
    }
}