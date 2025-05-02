using SE.Neo.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Blob")]
    public class Blob : BaseEntity
    {
        [Key, Column("Blob_Name")]
        [MaxLength(1024), MinLength(1)]
        public string Name { get; set; }

        [Column("Blob_Container_Name")]
        [ForeignKey("Container")]
        public BlobType ContainerId { get; set; }

        public virtual BlobContainer Container { get; set; }
    }
}
