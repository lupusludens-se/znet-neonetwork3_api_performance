using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Company_Follower")]
    public class CompanyFollower : BaseIdEntity
    {
        [Column("Company_Follower_Id")]
        public override int Id { get; set; }

        [Column("Follower_Id")]
        [ForeignKey("Follower")]
        public int FollowerId { get; set; }

        [Column("Company_Id")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public User Follower { get; set; }
    }
}