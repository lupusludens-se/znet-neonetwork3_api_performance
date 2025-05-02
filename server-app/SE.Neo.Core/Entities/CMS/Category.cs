using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Category")]
    public class Category : BaseCMSEntity
    {
        [Column("CMS_Category_Id")]
        public override int Id { get; set; }

        [Column("Category_Name")]
        [MaxLength(250)]
        public override string Name { get; set; }

        [Column("Category_Slug")]
        [MaxLength(250)]
        public override string Slug { get; set; }

        [Column("Is_Deleted")]
        public override bool IsDeleted { get; set; }

        [Column("CMS_Solution_Id")]
        [ForeignKey("Solution")]
        public int? SolutionId { get; set; }
        public Solution? Solution { get; set; }

        [Column("Description")]
        public override string Description { get; set; }

        public ICollection<ResourceCategory> CategoryResources { get; set; }

        public ICollection<UserProfileCategory> UserProfiles { get; set; }

        public ICollection<DiscussionCategory> DiscussionCategories { get; set; }

        public ICollection<CategoryTechnology> Technologies { get; set; }

        public ICollection<EventCategory> EventCategories { get; set; }
        public ICollection<SkillsByCategory> SkillsByCategory { get; set; }
    }
}