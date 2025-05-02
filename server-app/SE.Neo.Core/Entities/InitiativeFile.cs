using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_File")]
    public class InitiativeFile : BaseIdEntity
    {
        [Column("Initiative_File_Id")]
        public override int Id { get; set; }

        [Column("Initiative_Id")]
        [ForeignKey("Initiative")]
        public int InitiativeId { get; set; }
        public Initiative Initiative { get; set; }

        [Column("File_Id")]
        [ForeignKey("File")]
        public int FileId { get; set; }

        public File File { get; set; }
    }
}
