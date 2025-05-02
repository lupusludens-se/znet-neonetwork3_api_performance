using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Company_Announcement")]
    public class CompanyAnnouncement : BaseIdEntity
    {
        [Column("Company_Announcement_Id")]
        public override int Id { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Link")]
        public string Link { get; set; }

        [Column("Company_Id")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        [Column("Status_Id")]
        [ForeignKey("Company_Announcement_Status")]
        public CompanyAnnouncementStatus StatusId { get; set; } 

        [Column("Scale_Id")]
        [ForeignKey("Company_Announcement_Scale")]
        public int ScaleId { get; set; } 

        public ICollection<CompanyAnnouncementRegion> Regions { get; set; }
    }
}