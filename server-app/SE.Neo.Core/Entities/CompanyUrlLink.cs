using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Company_Url_Link")]
    public class CompanyUrlLink : BaseIdEntity
    {
        [Column("Url_Link_Id")]
        public override int Id { get; set; }

        [Column("Url_Link")]
        [MaxLength(2048)]
        public string UrlLink { get; set; }

        [Column("Url_Name")]
        [MaxLength(250)]
        public string UrlName { get; set; }

        [Column("Company_Id")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}