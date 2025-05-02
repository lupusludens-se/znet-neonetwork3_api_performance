using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SE.Neo.Core.Entities
{
    [Table("Tool")]
    public class Tool : BaseIdEntity
    {
        [Column("Tool_Id")]
        public override int Id { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [Column("Tool_Url")]
        public string ToolUrl { get; set; }

        [Column("Icon_Name")]
        [ForeignKey("Icon")]
        public string? IconName { get; set; }

        public Blob? Icon { get; set; }

        [Column("Is_Active")]
        public bool IsActive { get; set; }

        [Column("Tool_Type")]
        public ToolType ToolType { get; set; }

        [Column("Tool_Height")]
        public int ToolHeight { get; set; }

        public ICollection<ToolRole> Roles { get; set; }

        public ICollection<ToolCompany> Companies { get; set; }

        public ICollection<ToolPinned> Pinned { get; set; }
    }
}
