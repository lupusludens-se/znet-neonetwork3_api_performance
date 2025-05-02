using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDataTypes
{
    [Table("Product_Type")]
    public class ProductType : BaseEntity
    {
        [Column("Product_Type_Id")]
        public Enums.ProductType Id { get; set; }

        [Column("Product_Type_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
