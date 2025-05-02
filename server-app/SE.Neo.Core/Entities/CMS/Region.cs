using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Region")]
    public class Region : BaseCMSEntity
    {
        [Column("CMS_Region_Id")]
        public override int Id { get; set; }

        [Column("Region_Name")]
        [MaxLength(250)]
        public override string Name { get; set; }

        [Column("Region_Slug")]
        [MaxLength(250)]
        public override string Slug { get; set; }

        [Column("CMS_Parent_Region_Id")]
        public int? ParentId { get; set; }

        [Column("Is_Deleted")]
        public override bool IsDeleted { get; set; }

        [Column("Description")]
        public override string Description { get; set; }

        public ICollection<UserProfileRegion> UserProfiles { get; set; }

        public ICollection<DiscussionRegion> DiscussionRegions { get; set; }
    }
}