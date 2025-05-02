using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Company_File")]
    public class CompanyFile : BaseIdEntity
    {
        [Column("Company_File_Id")]
        public override int Id { get; set; }

        [Column("Company_Id")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [Column("File_Id")]
        [ForeignKey("File")]
        public int FileId { get; set; }

        public File File { get; set; }

        [Column("Is_Private")]
        public bool IsPrivate { get; set; }
    }
}
