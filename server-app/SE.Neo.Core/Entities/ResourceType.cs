using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Resource_Type")]
    public class ResourceType : BaseEntity
    {
        [Column("Resource_Type_Id")]
        public Enums.ResourceType Id { get; set; }

        [Column("Resource_Type_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
