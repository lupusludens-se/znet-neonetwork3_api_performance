using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Company_Announcement_Region")]
    public class CompanyAnnouncementRegion : BaseIdEntity
    {
        [Column("Company_Announcement_Region_Id")]
        public override int Id { get; set; }

        [Column("Company_Announcement_Id")]
        [ForeignKey("CompanyAnnouncement")]
        public int CompanyAnnouncementId { get; set; }

        public CompanyAnnouncement CompanyAnnouncement { get; set; }

        [Column("CMS_Region_Id")]
        [ForeignKey("Region")]
        public int RegionId { get; set; }

        public Region Region { get; set; }
    }
}

