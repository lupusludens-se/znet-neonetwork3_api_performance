using Microsoft.EntityFrameworkCore;
using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Discussion_Category")]
    [Index(nameof(DisscussionId), nameof(CategoryId), IsUnique = true)]
    public class DiscussionCategory : BaseIdEntity
    {
        [Column("Discussion_Category_Id")]
        public override int Id { get; set; }

        [Column("Disscussion_Id")]
        [ForeignKey("Discussion")]
        public int DisscussionId { get; set; }

        [Column("Category_Id")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public Discussion Discussion { get; set; }
    }
}
