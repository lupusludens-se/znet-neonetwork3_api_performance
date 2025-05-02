using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Time_Zone")]
    public class TimeZone : BaseIdEntity
    {
        [Column("Time_Zone_Id")]
        public override int Id { get; set; }

        [Column("Display_Name")]
        [MaxLength(100)]
        public string DisplayName { get; set; }

        [Column("Standard_Name")]
        [MaxLength(100)]
        public string StandardName { get; set; }

        [Column("Has_DST")]
        public bool HasDST { get; set; }

        [Column("UTC_Offset")]
        public double UTCOffset { get; set; }

        [Column("Abbreviation")]
        public string Abbreviation { get; set; }

        [Column("Windows_Name")]
        [MaxLength(100)]
        public string WindowsName { get; set; }

        [Column("Daylight_Abbreviation")]
        [MaxLength(10)]
        public string? DaylightAbbreviation { get; set; }

        [Column("Daylight_Name")]
        [MaxLength(100)]
        public string? DaylightName { get; set; }
    }
}