using Microsoft.EntityFrameworkCore;
using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Resource_Technology")]
    [Index(nameof(ResourceId), nameof(TechnologyId), IsUnique = true)]
    public class ResourceTechnology : BaseIdEntity
    {
        [Column("Project_Technology_Id")]
        public override int Id { get; set; }

        [Column("Resource_Id")]
        [ForeignKey("Resource")]
        public int ResourceId { get; set; }

        [Column("Technology_Id")]
        [ForeignKey("Technology")]
        public int TechnologyId { get; set; }

        public Technology Technology { get; set; }

        public Resource Resource { get; set; }
    }
}