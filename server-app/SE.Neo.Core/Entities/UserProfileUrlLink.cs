using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User_Profile_Url_Link")]
    public class UserProfileUrlLink : BaseIdEntity
    {
        [Column("Url_Link_Id")]
        public override int Id { get; set; }

        [Column("Url_Link")]
        [MaxLength(2048)]
        public string UrlLink { get; set; }

        [Column("Url_Name")]
        [MaxLength(250)]
        public string UrlName { get; set; }

        [Column("User_Profile_Id")]
        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}