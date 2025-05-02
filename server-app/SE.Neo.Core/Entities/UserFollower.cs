using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User_Follower")]
    public class UserFollower : BaseIdEntity
    {
        [Column("User_Follower_Id")]
        public override int Id { get; set; }

        [Column("Follower_Id")]
        [ForeignKey("Follower")]
        public int FollowerId { get; set; }

        [Column("Followed_Id")]
        [ForeignKey("User")]
        public int FollowedId { get; set; }

        public User Followed { get; set; }

        public User Follower { get; set; }
    }
}