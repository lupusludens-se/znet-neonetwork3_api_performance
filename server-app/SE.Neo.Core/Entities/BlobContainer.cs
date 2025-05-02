using SE.Neo.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Blob_Container")]
    public class BlobContainer : BaseEntity
    {
        [Column("Blob_Container_Id")]
        public BlobType Id { get; set; }

        [Column("Blob_Container_Name")]
        [MaxLength(63), MinLength(3)]
        public string Name { get; set; }
    }
}
