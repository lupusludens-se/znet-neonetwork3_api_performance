using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User_Profile_Region")]
    public class UserProfileRegion : BaseIdEntity
    {
        [Column("User_Profile_Region_Id")]
        public override int Id { get; set; }

        [Column("User_Profile_Id")]
        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }

        [Column("Region_Id")]
        [ForeignKey("Region")]
        public int RegionId { get; set; }

        public UserProfile UserProfile { get; set; }
        public Region Region { get; set; }
    }
}