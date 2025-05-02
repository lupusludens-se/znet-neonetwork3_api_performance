using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("File")]
    public class File : BaseIdEntity
    {
        [Column("File_Id")]
        public override int Id { get; set; }

        [Column("File_Type")]
        public FileType Type { get; set; }

        [Column("File_Extension")]
        public FileExtension Extension { get; set; }

        public string Link { get; set; }

        [Column("File_Name")]
        [ForeignKey("Image")]
        public string Name { get; set; }

        [Column("Actual_File_Name")]
        public string ActualFileName { get; set; }

        [Column("Actual_File_Title")]
        public string ActualFileTitle { get; set; }

        public Blob? Image { get; set; }

        [Column("File_Size")]
        public int Size { get; set; }

        [Column("File_Version")]
        public int Version { get; set; }
    }
}