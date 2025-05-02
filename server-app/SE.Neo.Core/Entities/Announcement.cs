using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Announcement")]
    public class Announcement : BaseIdEntity
    {
        [Column("Announcement_Id")]
        public override int Id { get; set; }

        [Column("Background_Image_Name")]
        [ForeignKey("BackgroundImage")]
        public string? BackgroundImageName { get; set; }

        public Blob? BackgroundImage { get; set; }

        [Column("Name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column("Button_Text")]
        [MaxLength(21)]
        public string ButtonText { get; set; }

        [Column("Button_Url")]
        [MaxLength(2048)]
        public string ButtonUrl { get; set; }

        [Column("Is_Active")]
        public bool IsActive { get; set; }

        [Column("Audience_Id")]
        [ForeignKey("Audience")]
        public int AudienceId { get; set; }

        public virtual Role Audience { get; set; }
    }
}
