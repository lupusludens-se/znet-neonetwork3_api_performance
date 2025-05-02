using Microsoft.EntityFrameworkCore;
using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Resource_Category")]
    [Index(nameof(ResourceId), nameof(CategoryId), IsUnique = true)]
    public class ResourceCategory : BaseIdEntity
    {
        [Column("Resource_Category_Id")]
        public override int Id { get; set; }

        [Column("Resource_Id")]
        [ForeignKey("Resource")]
        public int ResourceId { get; set; }

        [Column("Category_Id")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Resource Resource { get; set; }
        public Category Category { get; set; }
    }
}